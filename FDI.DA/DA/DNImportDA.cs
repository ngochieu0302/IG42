using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNImportDA : BaseDA
    {
        public DNImportDA()
        {
        }
        public DNImportDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNImportDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<StorageItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.Storages
                        where o.AgencyId == agencyId && o.IsDelete == false
                        orderby o.ID descending
                        select new StorageItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice,
                            DateCreated = o.DateCreated,
                            DateImport = o.DateImport,
                            UserID = o.UserID,
                            Note = o.Note
                        };
            return query.ToList();
        }

        public List<StorageItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from)
            ? from.StringToDate().TotalSeconds() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDate().AddDays(1).TotalSeconds() : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.Storages
                        where o.AgencyId == agencyId && o.DateImport >= fromDate && o.DateImport <= toDate
                        orderby o.ID descending
                        select new StorageItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice,
                            Payment = o.Payment ?? 0,
                            DateCreated = o.DateCreated,
                            DateImport = o.DateImport,
                            UserID = o.UserID,
                            Note = o.Note,
                            IsDelete = o.IsDelete
                        };
            var isdelete = httpRequest["IsDelete"];
            if (isdelete == "1")
            {
                query = query.Where(c => c.IsDelete == true);
            }
            else if (isdelete == "0")
            {
                query = query.Where(c => c.IsDelete == false);
            }
            else if (isdelete == "2")
            {
                query = query.Where(c => c.IsDelete == false && c.Payment < c.TotalPrice);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public ModuleShopProductValueItem GetListValueByRequest(HttpRequestBase httpRequest, int agecncy, out decimal? total, out decimal? quantity)
        {
            Request = new ParramRequest(httpRequest);
            var isValue = httpRequest["IsValue"] ?? "1";
            var query = (from o in FDIDB.sp_GetListInventoryValue(int.Parse(isValue), agecncy)
                         orderby o.ID descending
                         select new ShopProductValueItem
                         {
                             ID = o.ID,
                             Name = o.Name,
                             UnitName = o.Unitname,
                             Quantity = o.slNhap,
                             QuantityOut = o.slXuat,
                             QuantityInv = o.tonkho
                         }).ToList();
            var model = new ModuleShopProductValueItem
            {
                ListItems = query.ToList(),
            };
            //query = query.SelectByRequest(Request);
            //total = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.Price);
            total = 0;
            quantity = query.Sum(m => m.QuantityInv);
            //query = query.SelectPageByRequest(Request, ref TotalRecord);
            return model;
        }

        public List<ShopProductValueItem> GetListValueView(HttpRequestBase httpRequest, int agecncy, int id)
        {
            Request = new ParramRequest(httpRequest);
            var isValue = httpRequest["IsValue"] ?? "1";
            var query = from o in FDIDB.DN_Import
                        where o.Shop_Product_Value.AgencyId == agecncy && o.IsDelete == false && o.Shop_Product_Value.IsDeleted == false
                        && o.Storage.IsDelete == false
                        && (isValue == "1" && o.Quantity > o.QuantityOut || isValue == "0" && o.Quantity <= o.QuantityOut) && o.Shop_Product_Value.ID == id
                        orderby o.ID descending
                        select new ShopProductValueItem
                        {
                            ID = o.Shop_Product_Value.ID,
                            Name = o.Shop_Product_Value.Name,
                            UnitName = o.Shop_Product_Value.DN_Unit.Name,
                            Date = o.Storage.DateImport,
                            DateEnd = o.DateEnd,
                            Quantity = o.Quantity ?? 0,
                            QuantityOut = o.QuantityOut,
                            Price = o.Price
                        };
            return query.ToList();
        }
        public List<ShopProductValueItem> GetListValueLater(HttpRequestBase httpRequest, int agecncy, out decimal? total, out decimal? quantity)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Import
                        where o.Shop_Product_Value.AgencyId == agecncy && o.IsDelete == false && o.Shop_Product_Value.IsDeleted == false && o.Storage.IsDelete == false && o.DateEnd < now
                        orderby o.ID descending
                        select new ShopProductValueItem
                        {
                            ID = o.Shop_Product_Value.ID,
                            Name = o.Shop_Product_Value.Name,
                            UnitName = o.Shop_Product_Value.DN_Unit.Name,
                            Date = o.Storage.DateImport,
                            DateEnd = o.DateEnd,
                            Quantity = o.Quantity ?? 0,
                            QuantityOut = o.Export_Product_Value.Where(c => c.IsDelete == false).Sum(c => c.Quantity ?? 0),
                            Price = o.Price
                        };
            query = query.SelectByRequest(Request);
            query = query.Where(c => c.Quantity > (c.QuantityOut ?? 0));
            total = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.Price);
            quantity = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)));
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 0)
        {
            keword = FomatString.Slug(keword);
            var query = from c in FDIDB.DN_Import
                        where c.Shop_Product_Value.NameAscii.Contains(keword)
                        && c.Shop_Product_Value.AgencyId == agencyId && c.IsDelete == false && c.Shop_Product_Value.IsDeleted == false && c.Storage.IsDelete == false && c.Quantity > 0
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            value = c.Shop_Product_Value.NameAscii,
                            title = c.Shop_Product_Value.Name,
                            code = c.Shop_Product_Value.Code,
                            QuantityDay = c.Shop_Product_Value.QuantityDay ?? 0,
                            Quantity = c.Quantity,
                            QuantityOut = c.Export_Product_Value.Where(u => u.IsDelete == false).Sum(u => u.Quantity ?? 0),
                            Date = c.Storage.DateImport,
                            DateEnd = c.DateEnd,
                            Unit = c.Shop_Product_Value.DN_Unit.Name,
                            Type = type,
                            pricenew = c.Price
                        };
            return query.Take(showLimit).ToList();
        }

        public StorageItem GetStorageItem(int id)
        {
            var query = from c in FDIDB.Storages
                        where c.ID == id
                        select new StorageItem
                        {
                            ID = c.ID,
                            Note = c.Note,
                            DateCreated = c.DateCreated,
                            DateImport = c.DateImport,
                            Code = c.Code,
                            UserName = c.DN_Users.UserName,
                            TotalPrice = c.TotalPrice,
                            UserID = c.UserID,
                            Payment = c.Payment,
                            LstImport = from v in c.DN_Import
                                        where v.IsDelete == false
                                        select new DNImportItem
                                        {
                                            ID = v.ID,
                                            ValueId = v.ValueId,
                                            ValueName = v.Shop_Product_Value.Name,
                                            Quantity = v.Quantity,
                                            QuantityDay = v.Shop_Product_Value.QuantityDay,
                                            DateEnd = v.DateEnd,
                                            Price = v.Price,
                                            IsDelete = v.IsDelete,
                                            UnitName = v.Shop_Product_Value.DN_Unit.Name
                                        }
                        };
            return query.FirstOrDefault();
        }
        public Storage GetById(int id)
        {
            var query = from o in FDIDB.Storages where o.ID == id && o.IsDelete == false select o;
            return query.FirstOrDefault();
        }
        public BarcodeSouceItem GetByBarcode(string barcode, int cusId)
        {
            var query = from o in FDIDB.GetBySouceByBarcode(barcode, cusId)
                        select new BarcodeSouceItem
                            {
                                GID = o.GID,
                                BarCode = o.BarCode,
                                NameAgency = o.NameAgency,
                                AddressAgency = o.AddressAgency,
                                PhoneAgency = o.PhoneAgency,
                                Price = o.Price,
                                PriceNew = o.PriceNew,
                                IDStorage = o.IDStorage,
                                NameSupplier = o.NameSupplier,
                                AddressSupplier = o.AddressSupplier,
                                Value = o.Value,
                                IsCheck = o.Ischeck,
                                FullName = o.FullName,
                                UserName = o.UserName,
                                CountSource1 = o.CountSource1,
                                CountSource2 = o.CountSource2,
                                CountSource3 = o.CountSource3,
                                CountSource4 = o.CountSource4,
                                DatePack = o.DatePack,
                                DateImport = o.DateImport,
                                DateOrder = o.DateOrder,
                                Status = 1
                            };
            return query.FirstOrDefault();
        }
        public List<SourceItem> GetListSourcesByStorageID(int id, int type)
        {
            var _dimg = Utility._dimg;
            var query = from o in FDIDB.Sources
                        where o.StorageProductID == id && o.Type == type
                        orderby o.ID
                        select new SourceItem
                            {
                                ID = o.ID,
                                PictureUrl = _dimg + o.Gallery_Picture.Folder + o.Gallery_Picture.Url,
                                Description = o.Description,
                                Name = o.Name
                            };
            return query.ToList();
        }
        public SourceItem GetSourceByID(int id)
        {
            var _dimg = Utility._dimg;
            var _dvideo = Utility._dvideo;
            var query = from o in FDIDB.Sources
                        where o.ID == id
                        select new SourceItem
                        {
                            ID = o.ID,
                            PictureUrl = _dimg + o.Gallery_Picture.Folder + o.Gallery_Picture.Url,
                            Description = o.Description,
                            Name = o.Name,
                            Details = o.Details,
                            SourceVideo = o.Gallery_Video.Select(m => new GalleryVideoItem
                            {
                                Name = m.Name,
                                PictureUrl = _dimg + m.Gallery_Picture.Folder + m.Gallery_Picture.Url,
                                Url = _dvideo + m.Url,
                                UrlYoutobe = m.UrlYoutube
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<Shop_Product_Value> GetListProductValue(List<int> lst)
        {
            var query = from o in FDIDB.Shop_Product_Value where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public List<Storage> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.Storages where lst.Contains(o.ID) && o.IsDelete == false select o;
            return query.ToList();
        }

        public List<DN_Import> GetListDnImportArrId(List<int> lst)
        {
            var query = from o in FDIDB.DN_Import
                        where lst.Contains(o.ID) && o.IsDelete == false
                        select o;
            return query.ToList();
        }

        public void Add(Storage item)
        {
            FDIDB.Storages.Add(item);
        }
        public void Delete(DN_Import item)
        {
            FDIDB.DN_Import.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
