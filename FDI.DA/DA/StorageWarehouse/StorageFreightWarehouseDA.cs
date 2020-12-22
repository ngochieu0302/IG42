using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA.StorageWarehouse
{
    public class StorageFreightWarehouseDA : BaseDA
    {
        #region Contruction
        public StorageFreightWarehouseDA()
        {
        }
        public StorageFreightWarehouseDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public StorageFreightWarehouseDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 1)
        {
            var name = FomatString.Slug(keword);
            var date = DateTime.Now;
            var query = FDIDB.AutoSelectProductDetail(name, showLimit).Select(c => new SuggestionsProduct
            {
                ID = c.ID,
                name = "<span>Mã SP: <b>" + (c.Code ?? "") + "</b> " + "</span>",
                UrlImg = c.UrlImg ?? "/Content/Admin/images/auto-default.jpg",
                value = c.Code,
                QuantityDay = c.QuantityDay,
                DateS = date.ToString("yyyy-MM-dd"),
                DateE = date.AddMonths(c.QuantityDay ?? 0).ToString("yyyy-MM-dd"),
                code = c.Code,
                data = c.Price.ToString(),
                title = c.Name,
                PriceCost = c.Price,
                pricenew = c.Price,
                Type = type
            });
            return query.ToList();
        }
        public List<StorageFreightWarehouseItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDate().TotalSeconds() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDate().AddDays(1).TotalSeconds() : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.StorageFreightWarehouses
                        where o.AgencyId == agencyId && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new StorageFreightWarehouseItem()
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice ?? 0,
                            Payment = o.Payment ?? 0,
                            DateCreated = o.DateCreated,
                            DateImport = o.DateImport,
                            UserID = o.UserID,
                            AgencyReceiveID = o.AgencyReceiveID,
                            Note = o.Note,
                            IsActive = o.IsActive,
                            IsDelete = o.IsDelete,
                            Status = o.Status,
                            UsernameActive = o.DN_Users1.UserName,
                            DateActive = o.DateActive,
                            UsernameCreate = o.DN_Users.UserName,
                        };
            var status = httpRequest["IsDelete"];
            if (!string.IsNullOrEmpty(status))
            {
                var tus = int.Parse(status);
                query = query.Where(c => c.Status == tus);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<StorageFreightWarehouseItem> GetListSimpleByRequestRecive(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDate().TotalSeconds() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDate().AddDays(1).TotalSeconds() : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.StorageFreightWarehouses
                        where o.AgencyReceiveID == agencyId && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new StorageFreightWarehouseItem()
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice ?? 0,
                            Payment = o.Payment ?? 0,
                            DateCreated = o.DateCreated,
                            DateImport = o.DateImport,
                            UserID = o.UserID,
                            KeyGuid = o.keyreq,
                            AgencyReceiveID = o.AgencyReceiveID,
                            Note = o.Note,
                            IsActive = o.IsActive,
                            IsDelete = o.IsDelete,
                            Status = o.Status,
                            IsOrder = o.IsOrder,
                            UsernameActive = o.DN_Users1.UserName,
                            DateActive = o.DateActive,
                            UsernameCreate = o.DN_Users.UserName,
                        };
            var status = httpRequest["IsDelete"];
            if (!string.IsNullOrEmpty(status))
            {
                var tus = int.Parse(status);
                query = query.Where(c => c.Status == tus);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<StorageFreightWarehouseItem> GetListSimpleAllByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDate().TotalSeconds() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDate().AddDays(1).TotalSeconds() : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.StorageFreightWarehouses
                        where o.DateCreated >= fromDate && o.DateCreated <= toDate
                        && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new StorageFreightWarehouseItem()
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice ?? 0,
                            Payment = o.Payment ?? 0,
                            DateCreated = o.DateCreated,
                            DateImport = o.DateImport,
                            UserID = o.UserID,
                            Note = o.Note,
                            IsActive = o.IsActive,
                            IsDelete = o.IsDelete,
                            Status = o.Status,
                            UsernameActive = o.DN_Users1.UserName,
                            DateActive = o.DateActive,
                            UsernameCreate = o.DN_Users.UserName,
                        };
            var status = httpRequest["IsDelete"];
            if (!string.IsNullOrEmpty(status))
            {
                var tus = int.Parse(status);
                query = query.Where(c => c.Status == tus);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<StorageFreightWarehouseItem> GetListExcel(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.StorageFreightWarehouses
                        where o.IsDelete.HasValue || o.IsDelete == false && o.AgencyId == agencyId && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        orderby o.ID descending
                        select new StorageFreightWarehouseItem()
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice,
                            DateCreated = o.DateCreated,
                            UserID = o.UserID,
                            Note = o.Note
                        };
            var status = httpRequest["IsDelete"];
            if (!string.IsNullOrEmpty(status))
            {
                var tus = int.Parse(status);
                query = query.Where(c => c.Status == tus);
            }
            return query.ToList();
        }
        public StorageFreightWarehouseItem GetStorageFreightWarehousesItem(int id)
        {
            var query = from c in FDIDB.StorageFreightWarehouses
                        where c.ID == id
                        select new StorageFreightWarehouseItem()
                        {
                            ID = c.ID,
                            Note = c.Note,
                            DateCreated = c.DateCreated,
                            DateImport = c.DateImport,
                            Code = c.Code,
                            TotalPrice = c.TotalPrice,
                            Payment = c.Payment,
                            UserID = c.UserID,
                            UsernameCreate = c.DN_Users.UserName,
                            UsernameActive = c.DN_Users1.UserName,
                            AgencyReceiveID = c.AgencyReceiveID,
                            LstImport = from v in c.FreightWarehouses
                                        select new FreightWarehouseItem()
                                        {
                                            ID = v.ID,
                                            Code = v.Shop_Product_Detail.Code,
                                            Date = v.Date,
                                            ProductName = v.Shop_Product_Detail.Name,
                                            ProductID = v.ProductID,
                                            Quantity = v.Quantity,
                                            Price = v.Price,
                                            IsDelete = v.IsDelete,
                                        },
                            LstImportActive = from v in c.FreightWareHouse_Active
                                        select new FreightWarehouseActiveItem()
                                        {
                                            ID = v.ID,
                                            Code = v.Shop_Product.CodeSku,
                                            Date = v.Date,
                                            ProductName = v.DN_ImportProduct.Product_Value.Shop_Product_Detail.Name,
                                            ProductID = v.ProductID,
                                            Quantity = v.Quantity,
                                            Price = v.Price,
                                            IsDelete = v.IsDelete,
                                            TotalPrice = v.Price*v.Quantity*v.ValueWeight,
                                            Barcode = v.BarCode,
                                            DateE = v.DateEnd,
                                            ValueWeight = v.ValueWeight,
                                            Idimport = v.ImportProductGID
                                        },
                        };
            return query.FirstOrDefault();
        }
        public StorageFreightWarehouse GetById(int id)
        {
            var query = from o in FDIDB.StorageFreightWarehouses where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<DN_ImportProduct> GetListArrIdImport(List<Guid> lst)
        {
            var query = from o in FDIDB.DN_ImportProduct where lst.Contains(o.GID) select o;
            return query.ToList();
        }
        public StorageFreightWarehouse GetByKey(Guid keyGuid)
        {
            var query = from o in FDIDB.StorageFreightWarehouses where o.keyreq == keyGuid select o;
            return query.FirstOrDefault();
        }
        public List<StorageFreightWarehouseItemNew> ListItemsNotActive(bool active)
        {
            var query = from c in FDIDB.StorageFreightWarehouses
                        where (!c.IsActive.HasValue || c.IsActive == active)
                        select new StorageFreightWarehouseItemNew
                        {
                            ID = c.ID,
                            Note = c.Note,
                            Fullname = c.DN_Users.LoweredUserName,
                            DateCreate = c.DateCreated,
                            Url = "/StorageFreightWarehouse"
                        };
            return query.ToList();
        }
        public List<StorageFreightWarehouse> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.StorageFreightWarehouses where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(StorageFreightWarehouse item)
        {
            FDIDB.StorageFreightWarehouses.Add(item);
        }
        public void AddStoragePrudct(StorageProduct item)
        {
            FDIDB.StorageProducts.Add(item);
        }
        public void Delete(FreightWarehouse item)
        {
            FDIDB.FreightWarehouses.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
