using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ShopProductDetailDA : BaseDA
    {
        #region Constructer
        public ShopProductDetailDA()
        {
        }

        public ShopProductDetailDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ShopProductDetailDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<ShopProductDetailItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Shop_Product_Detail
                        where o.IsDelete == false
                        orderby o.ID descending
                        select new ShopProductDetailItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            NameAscii = o.NameAscii,
                            Price = o.Price ?? 0,
                            Name = o.Name,
                            UnitID = o.UnitID,
                            PictureID = o.PictureID,
                            IsShow = o.IsShow,
                            UnitName = o.DN_Unit.Name,
                            StartDate = o.StartDate,
                            UrlPicture = o.Gallery_Picture.Folder + o.Gallery_Picture.Url,
                            CategoryID = o.CateID,
                            Catename = o.Category.Name,
                            PriceCost = o.PriceCost ?? 0,
                            PriceOld = o.PriceOld ?? 0,
                            Value = o.Value ?? 0,
                            DetailRecipeItem = o.ProductDetail_Recipe.Where(a=>a.IsDeleted == false).Select(v=> new ProductDetailRecipeItem
                            {
                                ID = v.ID,
                                Incurred = v.Incurred,
                                LstRecipeProductValues = v.ProductValue_Recipe.Where(a => (a.IsDeleted == false || !a.IsDeleted.HasValue)).Select(c => new RecipeProductValue
                                {
                                    ValueName = c.Shop_Product_Value.Name,
                                    ValuePrice = c.Shop_Product_Value.Price,
                                    ID = c.ID,
                                    Quantity = c.Quantity,
                                    ProductValueId = c.ProductValueId,
                                    RecipeID = c.RecipeID,
                                    UnitName = c.Shop_Product_Value.DN_Unit.Name,
                                    DateCreate = c.DateCreate,
                                    TotalPrice = c.Shop_Product_Value.Price * c.Quantity,
                                }),
                                LstRecipeProductDetails = v.Mapping_ProductDetail_Recipe.Where(a => (a.IsDeleted == false || !a.IsDeleted.HasValue)).Select(c => new RecipeProductDetail
                                {
                                    ProductName = c.Shop_Product_Detail.Name,
                                    ID = c.ID,
                                    ProductPrice = c.Shop_Product_Detail.PriceCost,
                                    Quantity = c.Quantity,
                                    UnitName = c.Shop_Product_Detail.DN_Unit.Name,
                                    DetailID = c.DetailID,
                                    DateCreate = c.DateCreate,
                                    TotalPrice = c.Quantity * c.Shop_Product_Detail.PriceCost,
                                })
                            }),

                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        //public ModelProductExportItem GetListOrderDetail(int agencyid, string date)
        //{
        //    var datetime = date.StringToDate();
        //    var sdate = datetime.TotalSeconds();
        //    var edate = datetime.AddDays(1).TotalSeconds();
        //    var query = (from o in FDIDB.sp_GetListOrderDetail(sdate, edate)
        //                 select new ProductExportItem
        //                 {
        //                     ID = o.ID,
        //                     Name = o.Name,
        //                     UnitName = o.UnitName,
        //                     Price = o.Price,
        //                     Quantity = o.Quantity,
        //                     ColorName = o.Color,
        //                     SizeName = o.Size,
        //                     CodeSku = o.CodeSku
        //                 }).ToList();
        //    var lst = query.Select(c => c.ID).ToList();

        //    var query1 = from c in FDIDB.Export_Product
        //                 where lst.Contains(c.DN_ImportProduct.ProductID ?? 0) && c.IsDelete == false && c.DN_ExportProduct.IsDeleted == false
        //                 && c.DN_ExportProduct.DateExport == sdate
        //                 select new ExportProductItem
        //                 {
        //                     ProductID = c.DN_ImportProduct.ProductID,
        //                     Quantity = c.Quantity
        //                 };

        //    var model = new ModelProductExportItem
        //    {
        //        ListItem = query.ToList(),
        //        LstExportItem = query1.ToList()
        //    };
        //    return model;
        //}

        //public ModelProductExportItem GetOrderDetailExport(int agencyid, string date)
        //{
        //    var datetime = date.StringToDate();
        //    var sdate = datetime.TotalSeconds();
        //    var edate = datetime.AddDays(1).TotalSeconds();
        //    var query = (from o in FDIDB.sp_GetListOrderDetail(sdate, edate)
        //                 select new ProductExportItem
        //                 {
        //                     ID = o.ID,
        //                     Name = o.Name,
        //                     UnitName = o.UnitName,
        //                     Price = o.Price,
        //                     Quantity = o.Quantity,
        //                     ColorName = o.Color,
        //                     SizeName = o.Size,
        //                     CodeSku = o.CodeSku
        //                 }).ToList();
        //    var lst = query.Select(c => c.ID).ToList();
        //    //var query1 = from c in FDIDB.Export_Product
        //    //             where lst.Contains(c.DN_ImportProduct.ProductID ?? 0) && c.IsDelete == false && c.DN_ExportProduct.IsDeleted == false
        //    //             && c.DN_ExportProduct.DateExport == sdate
        //    //             select new ExportProductItem
        //    //             {
        //    //                 ID = c.ID,
        //    //                 ProductID = c.DN_ImportProduct.ProductID,
        //    //                 Quantity = c.Quantity - c.DN_ImportProduct.QuantityOut,
        //    //                 QuantityOut = c.DN_ImportProduct.QuantityOut,
        //    //                 Price = c.Price,
        //    //                 Date = c.Date,
        //    //                 DateEnd = c.DN_ImportProduct.DateEnd
        //    //             };

        //    var query1 = from c in FDIDB.DN_ImportProduct
        //                 where lst.Contains(c.ProductID ?? 0) && c.Quantity > c.QuantityOut && c.IsDelete == false 
        //                 orderby c.DateEnd
        //                 select new ExportProductItem
        //                 {
        //                     ID = c.ID,
        //                     ProductID = c.ProductID,
        //                     Quantity = c.Quantity - c.QuantityOut,
        //                     QuantityOut = c.QuantityOut,
        //                     Price = c.Price,
        //                     Date = c.Date,
        //                     DateEnd = c.DateEnd
        //                 };
        //    var query2 = from c in FDIDB.Export_Product
        //                 where sdate < c.Date && c.Date < edate
        //                 select new ExportProductItem
        //                 {
        //                     Quantity = c.Quantity,
        //                     ID = c.ID,
        //                     ImportID = c.InportProductID,
        //                     ProductID = c.DN_ImportProduct.Product_Value.Shop_Product_Detail.ID,
        //                 };
        //    var model = new ModelProductExportItem
        //    {
        //        ListItem = query.ToList(),
        //        LstExportItem = query1.ToList(),
        //        LstExportProductItem = query2.ToList(),
        //    };
        //    return model;
        //}

        //public ModuleShopProductValueItem GetListValueDetail(int agencyid, string date)
        //{
        //    var datetime = date.StringToDate();
        //    var sdate = datetime.TotalSeconds();
        //    var edate = datetime.AddDays(1).TotalSeconds();
        //    var query = (from o in FDIDB.sp_GetListValueDetail(sdate, edate, agencyid)
        //                 select new ShopProductValueItem
        //                 {
        //                     ID = o.ID,
        //                     Name = o.Name,
        //                     Quantity = o.Quantity,
        //                     UnitName = o.Unit
        //                 }).ToList();
        //    var lst = query.Select(c => c.ID).ToList();
        //    var query1 = from c in FDIDB.Export_Product_Value
        //                 where lst.Contains(c.DN_Import.ValueId ?? 0) && c.IsDelete == false && c.DN_Export.IsDeleted == false
        //                 && c.DN_Export.DateExport == sdate
        //                 select new DNImportItem
        //                 {
        //                     ValueId = c.DN_Import.ValueId,
        //                     Quantity = c.Quantity,
        //                     QuantityOut = c.DN_Import.QuantityOut,
        //                 };
        //    var model = new ModuleShopProductValueItem
        //    {
        //        ListItems = query.ToList(),
        //        LstImportItem = query1.ToList()
        //    };
        //    return model;
        //}

        public bool CheckValueProduct()
        {

            return false;
        }

        //public ModuleShopProductValueItem GetValueDetailExport(int agencyId, string date)
        //{
        //    var datetime = date.StringToDate();
        //    var sdate = datetime.TotalSeconds();
        //    var edate = datetime.AddDays(1).TotalSeconds();
        //    var query = (from o in FDIDB.sp_GetListValueDetail(sdate, edate, agencyId)
        //                 select new ShopProductValueItem
        //                 {
        //                     ID = o.ID,
        //                     Name = o.Name,
        //                     Quantity = o.Quantity,
        //                     UnitName = o.Unit,
        //                 }).ToList();
        //    var lst = query.Select(c => c.ID);
        //    var query1 = from c in FDIDB.DN_Import
        //                 where lst.Contains(c.ValueId ?? 0) && c.Quantity > c.QuantityOut && c.IsDelete == false && c.Storage.IsDelete == false
        //                 orderby c.DateEnd
        //                 select new DNImportItem
        //                 {
        //                     ID = c.ID,
        //                     ValueId = c.ValueId,
        //                     Quantity = c.Quantity - c.QuantityOut,
        //                     QuantityOut = c.QuantityOut,
        //                     Price = c.Price,
        //                     Date = c.Date,
        //                     DateEnd = c.DateEnd,
        //                 };

        //    var query2 = from c in FDIDB.Export_Product_Value
        //                 where c.DN_Export.DateExport == sdate
        //                 && lst.Contains(c.DN_Import.ValueId ?? 0) && c.DN_Export.IsDeleted == false
        //                 select new ExportProductValueItem
        //                 {
        //                     Quantity = c.Quantity,
        //                     ID = c.ID,
        //                     ImportID = c.ImportID,
        //                     ValueID = c.DN_Import.Shop_Product_Value.ID,
        //                 };

        //    var model = new ModuleShopProductValueItem
        //    {
        //        ListItems = query.ToList(),
        //        LstImportItem = query1.ToList(),
        //        LstExportValueItem = query2,
        //    };
        //    return model;
        //}

        public Shop_Product_Detail GetById(int id)
        {
            var query = from c in FDIDB.Shop_Product_Detail where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<Shop_Product> GetProductById(int id)
        {
            var query = from c in FDIDB.Shop_Product where c.ProductDetailID == id select c;
            return query.ToList();
        }
        public ShopProductDetailItem GetItemById(int id)
        {
            var query = from o in FDIDB.Shop_Product_Detail
                        where o.ID == id
                        select new ShopProductDetailItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            NameAscii = o.NameAscii,
                            PictureID = o.PictureID,
                            Details = o.Details,
                            Sort = o.Sort,
                            UnitID = o.UnitID,
                            Value = o.Value,
                            Description = o.Description,
                            UnitName = o.DN_Unit.Name,
                            Price = o.Price,
                            Name = o.Name.Trim(),
                            PriceCost = o.PriceCost,
                            PriceOld = o.PriceOld,
                            Percent = o.Percent,
                            QuantityDay = o.QuantityDay,
                            IsShow = o.IsShow,
                            NamePicture = o.Gallery_Picture.Name,
                            UrlPicture = o.Gallery_Picture.Folder + o.Gallery_Picture.Url,
                            CategoryID = o.CateID,

                            ListProductItem = o.Shop_Product.Where(a => a.IsDelete != true && a.IsShow == true).Select(c => new ProductItem
                            {
                                Name = o.Name,

                                ID = c.ID,
                                SizeID = c.SizeID,
                            }),
                            ListGalleryPictureItems = o.Gallery_Picture2.Where(a => a.IsDeleted != true && a.IsShow == true).Select(c => new GalleryPictureItem
                            {
                                ID = c.ID,
                                Folder = c.Folder,
                                Url = c.Url,
                                Name = c.Name,
                            })
                        };
            return query.FirstOrDefault();
        }
        public ProductDetailRecipeItem GetRecipeItemByDetailId(int id)
        {
            var query = from o in FDIDB.ProductDetail_Recipe
                        where o.ProductDetailId == id
                        select new ProductDetailRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            Quantity = o.Quantity,
                            ProductPrice = o.ProductPrice,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            ProductDetailId = o.ProductDetailId,
                            ProductName = o.Shop_Product_Detail.Name,
                            IsDeleted = o.IsDeleted,
                            ValuePrice = o.ValuePrice,
                            Username = o.DN_Users.UserName,
                            TimeM = o.TimeM,
                            Incurred = o.Incurred,
                            Note = o.Note,
                            LstRecipeProductValues = o.ProductValue_Recipe.Where(a => (a.IsDeleted == false || !a.IsDeleted.HasValue)).Select(c => new RecipeProductValue
                            {
                                ValueName = c.Shop_Product_Value.Name,
                                ValuePrice = c.Shop_Product_Value.Price,
                                ID = c.ID,
                                Quantity = c.Quantity,
                                ProductValueId = c.ProductValueId,
                                RecipeID = c.RecipeID,
                                UnitName = c.Shop_Product_Value.DN_Unit.Name,
                                DateCreate = c.DateCreate,
                                TotalPrice = c.Shop_Product_Value.Price * c.Quantity,
                            }),
                            LstRecipeProductDetails = o.Mapping_ProductDetail_Recipe.Where(a => (a.IsDeleted == false || !a.IsDeleted.HasValue)).Select(c => new RecipeProductDetail
                            {
                                ProductName = c.Shop_Product_Detail.Name,
                                ID = c.ID,
                                ProductPrice = c.Shop_Product_Detail.PriceCost,
                                Quantity = c.Quantity,
                                UnitName = c.Shop_Product_Detail.DN_Unit.Name,
                                DetailID = c.DetailID,
                                DateCreate = c.DateCreate,
                                TotalPrice = c.Quantity * c.Shop_Product_Detail.PriceCost,
                            })
                        };
            return query.FirstOrDefault();
        }
        public ProductDetailRecipeItem GetRecipeItemById(int id)
        {
            var query = from o in FDIDB.ProductDetail_Recipe
                        where o.ID == id
                        select new ProductDetailRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            Quantity = o.Quantity,
                            ProductPrice = o.ProductPrice,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            ProductDetailId = o.ProductDetailId,
                            ProductName = o.Shop_Product_Detail.Name,
                            IsDeleted = o.IsDeleted,
                            ValuePrice = o.ValuePrice,
                            Username = o.DN_Users.UserName,
                            TimeM = o.TimeM,
                            Incurred = o.Incurred,
                            Note = o.Note,
                            LstRecipeProductValues = o.ProductValue_Recipe.Where(a => (a.IsDeleted == false || !a.IsDeleted.HasValue)).Select(c => new RecipeProductValue
                            {
                                ValueName = c.Shop_Product_Value.Name,
                                ValuePrice = c.Shop_Product_Value.Price,
                                ID = c.ID,
                                Quantity = c.Quantity,
                                ProductValueId = c.ProductValueId,
                                RecipeID = c.RecipeID,
                                UnitName = c.Shop_Product_Value.DN_Unit.Name,
                                DateCreate = c.DateCreate,
                                TotalPrice = c.Shop_Product_Value.Price * c.Quantity,
                            }),
                            LstRecipeProductDetails = o.Mapping_ProductDetail_Recipe.Where(a => (a.IsDeleted == false || !a.IsDeleted.HasValue)).Select(c => new RecipeProductDetail
                            {
                                ProductName = c.Shop_Product_Detail.Name,
                                ID = c.ID,
                                ProductPrice = c.Shop_Product_Detail.PriceCost,
                                Quantity = c.Quantity,
                                UnitName = c.Shop_Product_Detail.DN_Unit.Name,
                                DetailID = c.DetailID,
                                DateCreate = c.DateCreate,
                                TotalPrice = c.Quantity * c.Shop_Product_Detail.PriceCost,
                            })
                        };
            return query.FirstOrDefault();
        }

        public ProductDetail_Recipe GetbyIdRecipe(int id)
        {
            var query = from o in FDIDB.ProductDetail_Recipe
                        where o.ID == id
                        select o;
            return query.FirstOrDefault();
        }
        public List<ShopProductDetailItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.Shop_Product_Detail
                        where o.IsShow == true && (!o.IsDelete.HasValue || o.IsDelete == false)
                        select new ShopProductDetailItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            NameAscii = o.NameAscii,
                            Price = o.Price,
                            Name = o.Name.Trim(),
                            QuantityDay = o.QuantityDay,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public bool CheckExitCode(string code, int id, int agencyId)
        {
            var query = FDIDB.Shop_Product_Detail.Any(c => c.ID != id && c.Code == code && c.IsDelete == false);
            return query;
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.Name.Contains(keword) && c.IsDelete == false
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            value = c.Name,
                            title = c.Name,
                            code = c.Code,
                            QuantityDay = c.QuantityDay ?? 0,
                            data = c.DN_Unit.Name,
                            pricenew = c.PriceCost
                        };
            return query.Take(showLimit).ToList();
        }
        public List<SuggestionsProduct> GetListAutoCate(string keword, int showLimit, int agencyId, int cateId=0)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                where c.Name.Contains(keword) && c.IsDelete == false && (cateId == 0 || c.CateID == cateId)
                select new SuggestionsProduct
                {
                    ID = c.ID,
                    value = c.Name,
                    title = c.Name,
                    code = c.Code,
                    QuantityDay = c.QuantityDay ?? 0,
                    data = c.DN_Unit.Name,
                    pricenew = c.PriceCost,
                    PercentProduct = c.Category.Percent,
                    Incurred = c.Incurred,
                };
            return query.Take(showLimit).ToList();
        }
        public List<SuggestionsProduct> GetListCateAuto(string keword, int showLimit, int agencyId, int cateId = 0)
        {
            var query = from c in FDIDB.Categories
                where c.Name.Contains(keword) && c.IsDeleted == false && (c.ParentId == cateId)
                select new SuggestionsProduct
                {
                    ID = c.Id,
                    value = c.Name,
                    title = c.Name,
                    data = c.DN_Unit.Name,
                    pricenew = c.Price,
                    PercentProduct = c.Percent,
                    Incurred = c.Incurred,
                    Quantity = c.WeightDefault,
                };
            return query.Take(showLimit).ToList();
        }

        public List<ShopProductDetailItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Shop_Product_Detail
                        where ltsArrId.Contains(o.ID)
                        select new ShopProductDetailItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            NameAscii = o.NameAscii,
                            Price = o.Price,
                            Name = o.Name.Trim(),
                            QuantityDay = o.QuantityDay,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }
        public List<Shop_Product_Detail> GetListProductDetailByArrId(List<int> lstId)
        {
            var query = from o in FDIDB.Shop_Product_Detail
                        where lstId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public List<Category> GetListCateByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Categories
                        where ltsArrId.Contains(o.Id)
                        select o;
            return query.ToList();
        }

        public List<Shop_Product_Detail> GetListProductByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Shop_Product_Detail
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public List<Gallery_Picture> GetListPictureByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Gallery_Picture
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public bool CheckName(string name, int id)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.Name == name && c.ID != id && c.IsDelete == false
                        select c;
            return query.Any();
        }
        public bool CheckNameAscii(string name, int id)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.NameAscii == name && c.ID != id
                        select c;
            return query.Any();
        }
        public List<System_Tag> GetListIntTagByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.System_Tag
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public void Add(Shop_Product_Detail shopProductDetail)
        {
            FDIDB.Shop_Product_Detail.Add(shopProductDetail);
        }
        public void AddRecipe(ProductDetail_Recipe shopProductDetail)
        {
            FDIDB.ProductDetail_Recipe.Add(shopProductDetail);
        }
        public void Delete(Shop_Product_Detail shopProductDetail)
        {
            FDIDB.Shop_Product_Detail.Remove(shopProductDetail);
        }
        public void DeleteProduct(Shop_Product shopProductDetail)
        {
            FDIDB.Shop_Product.Remove(shopProductDetail);
        }
        public void AddProduct(Shop_Product shopProductDetail)
        {
            FDIDB.Shop_Product.Add(shopProductDetail);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
