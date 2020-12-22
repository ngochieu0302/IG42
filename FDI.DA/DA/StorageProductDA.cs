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
    public class StorageProductDA : BaseDA
    {
        public StorageProductDA()
        {
        }
        public StorageProductDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public StorageProductDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<StorageProductItem> GetListSimple()
        {
            var query = from o in FDIDB.StorageProducts
                        where o.IsDelete == false
                        orderby o.ID descending
                        select new StorageProductItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            Suppliername = o.DN_Supplier.Name,
                            TotalPrice = o.TotalPrice,
                            DateCreated = o.DateCreated,
                            UserID = o.UserID,
                            Note = o.Note,
                            
                        };
            return query.ToList();
        }
        //public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 0)
        //{
        //    keword = FomatString.Slug(keword);
        //    var query = from c in FDIDB.DN_ImportProduct
        //                where c.Shop_Product.Shop_Product_Detail.NameAscii.Contains(keword) &&
        //                c.IsDelete == false && c.StorageProduct.IsDelete == false && c.Shop_Product.IsDelete == false
        //                select new SuggestionsProduct
        //                {
        //                    ID = c.ID,
        //                    value = c.Shop_Product.Shop_Product_Detail.NameAscii,
        //                    title = c.Shop_Product.Shop_Product_Detail.Name,
        //                    code = c.Shop_Product.CodeSku,
        //                    QuantityDay = c.Shop_Product.QuantityDay ?? 0,
        //                    Quantity = c.Quantity,
        //                    QuantityOut = c.Export_Product.Where(u => u.IsDelete == false).Sum(u => u.Quantity ?? 0),
        //                    Date = c.Date,
        //                    DateEnd = c.DateEnd,
        //                    Unit = c.Shop_Product.Shop_Product_Detail.DN_Unit.Name,
        //                    Type = type,
        //                    //Imei = c.Imei,
        //                    pricenew = c.Shop_Product.PriceNew
        //                };
        //    return query.Take(showLimit).ToList();
        //}
        public List<StorageProductItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDate().TotalSeconds() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDate().AddDays(1).TotalSeconds() : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.StorageProducts
                        where o.AgencyId == agencyId && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        orderby o.ID descending
                        select new StorageProductItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice ?? 0,
                            //Payment = o.Payment ?? 0,
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

        public List<ImportProductItem> GetListProductByRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out int? quantity)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_ImportProduct
                        where c.IsDelete == false && c.Quantity > c.QuantityOut
                        orderby c.Date descending
                        select new ImportProductItem
                        {
                            GID = c.GID,
                            Name = c.Product_Value.Shop_Product_Detail.Name,
                            //CodeSku = c.Shop_Product.CodeSku,
                            //SizeName = c.Shop_Product.Product_Size.Name,
                            //ColorName = c.Shop_Product.System_Color.Value,
                            Quantity = c.Quantity ?? 0,
                            QuantityOut = c.QuantityOut,
                            DateEnd = c.DateEnd,
                            BarCode = c.BarCode,
                            Date = c.Date,
                            PriceNew = c.PriceNew ?? 0,
                        };
            query = query.SelectByRequest(Request);
            total = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.PriceNew);
            quantity = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)));
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<ProductItem> GetListProductValueByRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalOld, out int? quantity)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Product_Value
                        where  c.IsDelete == false && c.Shop_Product_Detail.IsDelete == false
                         && c.Quantity > c.QuantityOut
                        orderby c.ID descending
                        select new ProductItem
                        {
                            ID = c.ID,
                            Name = c.Shop_Product_Detail.Name,
                            CodeSku = c.Shop_Product_Detail.Code,
                            Quantity = c.Quantity ?? 0,
                            IsShow = c.Shop_Product_Detail.IsShow,
                            QuantityOut = c.QuantityOut,
                            QuantityDay = c.Shop_Product_Detail.QuantityDay,
                            BarCode = c.Barcode,
                            CreateDate = c.DateCreated,
                            PriceNew = c.PriceNew ?? 0,
                            PriceOld = c.Shop_Product_Detail.Price ?? 0,
                            PriceCost = c.PriceCost
                        };
            query = query.SelectByRequest(Request);
            total = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.PriceNew);
            totalOld = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.PriceOld);
            quantity = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)));
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<CateValueItem> GetListCateValueByRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalOld, out decimal? quantity)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Cate_Value
                        where c.IsDelete == false && c.Category.IsDeleted == false
                         && c.Quantity > c.QuantityOut
                        orderby c.ID descending
                        select new CateValueItem()
                        {
                            ID = c.ID,
                            Name = c.Category.Name,
                            Code = c.Code,
                            Quantity = c.Quantity ?? 0,
                            QuantityOut = c.QuantityOut,
                            Barcode = c.Barcode,
                            DateImport = c.DateImport,
                            PriceNew = c.PriceNew ?? 0,
                            PriceCost = c.PriceCost
                        };
            query = query.SelectByRequest(Request);
            total = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.PriceNew);
            totalOld = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.PriceCost);
            quantity = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)));
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<ImportProductItem> GetListProductLater(HttpRequestBase httpRequest, int agencyid, out decimal? total, out int? quantity)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_ImportProduct
                        where c.AgencyId == agencyid && c.IsDelete == false  && c.DateEnd < now
                        orderby c.Date descending
                        select new ImportProductItem
                        {
                            GID = c.GID,
                            Name = c.Product_Value.Shop_Product_Detail.Name,
                            Code = c.Product_Value.Shop_Product_Detail.Code,
                            //SizeName = c.Shop_Product.Product_Size.Name,
                            //ColorName = c.Shop_Product.System_Color.Value,
                            Quantity = c.Quantity ?? 0,
                            QuantityOut = c.QuantityOut,
                            DateEnd = c.DateEnd,
                            Date = c.Date,
                            PriceNew = c.PriceNew ?? 0,
                        };
            query = query.SelectByRequest(Request);
            query = query.Where(c => c.Quantity > (c.QuantityOut ?? 0));
            total = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)) * m.PriceNew);
            quantity = query.Sum(m => (m.Quantity - (m.QuantityOut ?? 0)));
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<StorageProductItem> GetListExcel(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.StorageProducts
                        where o.IsDelete == false && o.AgencyId == agencyId && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        orderby o.ID descending
                        select new StorageProductItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice,
                            DateCreated = o.DateCreated,
                            UserID = o.UserID,
                            Note = o.Note
                        };
            return query.ToList();
        }

        public StorageProductItem GetStorageProductItem(int id)
        {
            var query = from c in FDIDB.StorageProducts
                        where c.ID == id
                        select new StorageProductItem
                        {
                            ID = c.ID,
                            Note = c.Note,
                            DateCreated = c.DateCreated,
                            DateImport = c.DateImport,
                            Code = c.Code,
                            TotalPrice = c.TotalPrice,
                            //Payment = c.Payment,
                            UserID = c.UserID,
                            UserName = c.DN_Users.UserName,
                            //LstImport = from v in c.DN_ImportProduct
                            //            select new ImportProductItem
                            //            {
                            //                ID = v.ID,
                            //                Code = v.Shop_Product.CodeSku,
                            //                DateEnd = v.DateEnd,
                            //                Date = v.Date,
                            //                ProductName = v.Shop_Product.Shop_Product_Detail.Name,
                            //                ProductID = v.ProductID,
                            //                Quantity = v.Quantity,
                            //                Price = v.Price,
                            //                IsDelete = v.IsDelete,
                            //                BarCode = v.BarCode,
                            //                Value = v.Value,
                            //                PriceNew = v.PriceNew,
                            //            },
                            //LstExport = from v in c.Export_Product_Detail
                            //            select new ExportProductDetailItem
                            //            {
                            //                ID = v.ID,
                            //                Code = v.Shop_Product.CodeSku,
                            //                ProductName = v.Shop_Product.Shop_Product_Detail.Name,
                            //                ValueName = v.Shop_Product_Value.Name,
                            //                Quantity = v.Quantity,
                            //                Price = v.Shop_Product_Value.DN_Import.Where(u => u.IsDelete == false).OrderByDescending(u => u.Storage.DateCreated).Select(u => u.Price).FirstOrDefault(),
                            //                IsDelete = v.IsDelete
                            //            }
                        };
            return query.FirstOrDefault();
        }
        public List<CateValueItem> GetCateProductValueItem(double value,int skip)
        {
            double val = 0;
            if (value == 0.5){val = 1;}else{val = value/0.5;}
            var query = from c in FDIDB.Cate_Value
                        where c.Quantity > c.QuantityOut
                        orderby c.DateCreated 
                        select new CateValueItem
                        {
                            ID = c.ID,
                            CateID = c.CateID,
                            Barcode = c.Barcode,
                            Name = c.Category.Name,
                            PriceNew = c.PriceNew,
                            PriceCost = c.PriceCost,
                            DateCreate = c.DateCreated,
                            DateImport = c.DateImport,
                            Quantity = c.Quantity,
                            QuantityOut = c.QuantityOut,
                            Unitname = c.DN_Unit.Name,
                            Code = c.Code,
                            ListProductValueItems = c.Product_Value.Where(a=>a.Quantity > a.QuantityOut).Select(z=> new ProductValueItem
                            {
                                Name = z.Shop_Product_Detail.Name,
                                Code = z.Shop_Product_Detail.Code,
                                PriceCost = z.PriceCost,
                                PriceNew = z.PriceNew,
                                DateImport = z.DateImport,
                                Barcode = z.Barcode,
                                Value = z.Value,
                                Unitname = z.DN_Unit.Name,
                                ListImportProductItems = z.DN_ImportProduct.Where(a=>a.Quantity > a.QuantityOut).Select(v=> new ImportProductItem
                                {
                                    GID = v.GID,
                                    //ProductID = v.ProductID,
                                    Date = v.Date,
                                    DateEnd = v.DateEnd,
                                    BarCode = v.BarCode,
                                    ProductValueID = z.ID,
                                    CateValueID = c.ID,
                                    Code = v.Product_Value.Shop_Product_Detail.Code,
                                    Name = z.Shop_Product_Detail.Name,
                                    Quantity = v.Quantity,
                                    Value = v.Value,
                                    Price = v.Price,
                                    PriceNew = v.PriceNew,
                                })
                            })
                        };
            return query.Skip(skip).Take(int.Parse(val.ToString())).ToList();
        }
        public CateValueItem GetCateValueItem(int id)
        {
            var query = from c in FDIDB.Cate_Value
                        where c.ID == id
                        orderby c.ID
                        select new CateValueItem
                        {
                            ID = c.ID,
                            CateID = c.CateID,
                            Barcode = c.Barcode,
                            Name = c.Category.Name,
                            PriceNew = c.PriceNew,
                            PriceCost = c.PriceCost,
                            DateCreate = c.DateCreated,
                            DateImport = c.DateImport,
                            Quantity = c.Quantity,
                            QuantityOut = c.QuantityOut,
                            Unitname = c.DN_Unit.Name,
                            Code = c.Code,
                            ListProductValueItems = c.Product_Value.Where(a => a.Quantity > a.QuantityOut).Select(z => new ProductValueItem
                            {
                                ID = z.ID,
                                Name = z.Shop_Product_Detail.Name,
                                Code = z.Shop_Product_Detail.Code,
                                PriceCost = z.PriceCost,
                                PriceNew = z.PriceNew,
                                DateImport = z.DateImport,
                                Barcode = z.Barcode,
                                Value = z.Value,
                                Unitname = z.DN_Unit.Name,
                            })
                        };
            return query.FirstOrDefault();
        }
        public ProductValueItem GetStorageProductValueItem(int id)
        {
            var query = from c in FDIDB.Product_Value
                        where c.ID == id
                        select new ProductValueItem
                        {
                            Code = c.Shop_Product_Detail.Code,
                            Name = c.Shop_Product_Detail.Name,
                            PriceCost = c.Shop_Product_Detail.Price,
                            PriceNew = c.PriceNew,
                            Value = c.Value,
                            ListImportProductItems = from v in c.DN_ImportProduct
                                        select new ImportProductItem
                                        {
                                            GID = v.GID,
                                            Code = c.Shop_Product_Detail.Code,
                                            DateEnd = v.DateEnd,
                                            Date = v.Date,
                                            Name = c.Shop_Product_Detail.Name,
                                            //ProductID = v.ProductID,
                                            Quantity = v.Quantity,
                                            Price = v.Price,
                                            IsDelete = v.IsDelete,
                                            BarCode = v.BarCode,
                                            Value = v.Value,
                                            PriceNew = v.PriceNew,
                                        },
                        };
            return query.FirstOrDefault();
        }
        //public List<DN_ImportProduct> GetListProductImport(List<int> lst)
        //{
        //    var query = from o in FDIDB.Shop_Product
        //                where lst.Contains(o.ID) && o.IsDelete == false
        //                select new DN_ImportProduct
        //                {
        //                    ID = o.ID,
        //                    Date = o.CreateDate,
        //                    DateEnd = o.EndDate,
        //                    Price = o.PriceNew
        //                };
        //    return query.ToList();

        //}
        public List<Product_Value> GetListImportbyProductValue(List<int> lst)
        {
            var query = from o in FDIDB.Product_Value
                where lst.Contains(o.ID) && o.IsDelete == false
                select o;
            return query.ToList();

        }

        public StorageProduct GetById(int id)
        {
            var query = from o in FDIDB.StorageProducts where o.ID == id select o;
            return query.FirstOrDefault();
        }

        public List<StorageProduct> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.StorageProducts where lst.Contains(o.ID) select o;
            return query.ToList();
        }

        //public List<RecipeItem> GetListProduct(List<int?> lst)
        //{
        //    var query = from o in FDIDB.Product_Recipe
        //                where lst.Contains(o.ProductId) && o.IsDelete == false
        //                select new RecipeItem
        //                {
        //                    ProductID = o.ProductId,
        //                    ValueId = o.ValueId,
        //                    Quantity = o.Quantity
        //                };
        //    return query.ToList();
        //}

        public void Add(StorageProduct item)
        {
            FDIDB.StorageProducts.Add(item);
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
