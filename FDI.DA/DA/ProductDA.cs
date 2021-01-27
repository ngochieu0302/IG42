using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class ProductDA : BaseDA
    {
        #region Constructer

        public ProductDA()
        {
        }

        public ProductDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ProductDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion

        public List<ProductItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.Shop_Product_Detail.IsDelete != true
                        orderby c.ID descending
                        select new ProductItem
                        {
                            ID = c.ID,
                            Name = c.Shop_Product_Detail.Name,
                            CodeSku = c.CodeSku,
                            SizeName = c.Product_Size.Name,
                            ColorName = c.System_Color.Value,
                            UrlPicture = c.Shop_Product_Detail.Gallery_Picture.Folder + c.Shop_Product_Detail.Gallery_Picture.Url,
                            IsShow = c.IsShow,
                            PriceNew = (c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000) ?? 0,
                            //PriceOld = c.PriceOld,
                            //Percent = c.Percent,
                            ProductDetailID = c.ProductDetailID,
                            IsDelete = c.IsDelete,
                            CreateBy = c.CreateBy,
                            CreateDate = c.CreateDate
                        };
            var productDetailId = httpRequest.QueryString["productDetailId"];
            if (!string.IsNullOrEmpty(productDetailId))
            {
                var pId = int.Parse(productDetailId);
                query = query.Where(m => m.ProductDetailID == pId);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<ProductValueAddItem> GetListSimpleProductDetail()
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.IsDelete == false
                        && c.IsShow == true
                        orderby c.ID descending
                        select new ProductValueAddItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Price = c.Price,
                        };
            return query.ToList();
        }
        public List<ProductItem> GetListSimple(int agencyId)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new ProductItem
                        {
                            ID = c.ID,
                            IsShow = c.IsShow,
                            PriceNew = (c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000) ?? 0,
                            IsDelete = c.IsDelete,
                            CreateBy = c.CreateBy,
                            CreateDate = c.CreateDate,
                        };
            return query.ToList();
        }

        public List<CategoryItem> GetList(int agencyId)
        {
            var query = from c in FDIDB.Categories
                        where c.IsDeleted == false
                        orderby c.Sort
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Icon = c.Icon,
                            ListProductItem = c.Shop_Product_Detail.Where(m => m.IsDelete == false && m.IsShow == true).OrderByDescending(m => m.ID).Select(m => new ShopProductDetailItem
                            {
                                UrlPicture = m.Gallery_Picture.Folder + m.Gallery_Picture.Url,
                                ListProductItem = m.Shop_Product.OrderByDescending(p => p.ID).Where(p => p.IsDelete == false).Select(p => new ProductItem
                                {
                                    ID = p.ID,
                                    Name = m.Name + " " + (p.Product_Size != null ? p.Product_Size.Name : ""),
                                    CodeSku = p.CodeSku,
                                    PriceNew = (p.Shop_Product_Detail.Price * (p.Product_Size == null ? 1000 : p.Product_Size.Value) / 1000) ?? 0,
                                })
                            })
                        };
            return query.ToList();
        }

        public List<ProductItem> GetLisByCategoryId(int categoryId, int agencyId)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.IsShow == true
                        orderby c.ID descending
                        select new ProductItem
                        {
                            ID = c.ID,
                        };
            return query.ToList();
        }

        public List<ProductItem> GetListByAgency(int agencyId)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsShow == true && c.IsDelete == false
                        select new ProductItem
                        {
                            ID = c.ID,
                            //Value = c.Value,
                            PriceNew = (c.Shop_Product_Detail.Price * c.Product_Size.Value / 1000) ?? 0,
                            Name = c.Shop_Product_Detail.Name
                        };
            return query.ToList();
        }
        public ProductItem ChangeProductSize(int dId, int sId, int cId)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.ProductDetailID == dId && (sId == 0 || c.SizeID == sId) && (cId == 0 || c.ColorID == cId)
                        select new ProductItem
                        {
                            ID = c.ID,
                            PriceNew = ((c.Shop_Product_Detail.Price) * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000) ?? 0,
                            //PriceOld = c.PriceOld,
                        };
            return query.FirstOrDefault();
        }
        public List<ProductItem> GetListByProductDetailsId(int agencyId, int productDetailId)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsShow == true && c.IsDelete == false && c.ProductDetailID == productDetailId
                        select new ProductItem
                        {
                            ID = c.ID,
                            //Value = c.Value,
                            PriceNew = (c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000) ?? 0,
                            //PriceOld = c.PriceOld,
                            SizeName = c.Product_Size.Name,
                            CodeSku = c.CodeSku,
                            ColorName = c.System_Color.Name,
                            Quantity = c.Quantity,
                        };
            return query.ToList();
        }

        public List<PacketItem> GetListByPacket(int agencyId, int beddeskId)
        {
            var query = from c in FDIDB.DN_Packet
                        where c.AgencyID == agencyId && c.DN_Bed_Desk.Any(u => u.ID == beddeskId)
                        select new PacketItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            IsEarly = c.IsEarly.HasValue && c.IsEarly.Value,
                            LstProduct = from v in c.DN_Product_Packet
                                         select new ProductItem
                                         {
                                             ID = v.Shop_Product.ID,
                                             Value = v.Shop_Product.ID,
                                             PriceNew = (v.Shop_Product.Shop_Product_Detail.Price * (v.Shop_Product.Product_Size == null ? 1000 : v.Shop_Product.Product_Size.Value) / 1000) ?? 0,
                                         }
                        };
            return query.ToList();
        }
        public List<ProductPacketItem> ListByPacket(int agencyId)
        {
            var query = from c in FDIDB.DN_Packet
                        where c.AgencyID == agencyId && c.DN_Bed_Desk.Any()
                        select new ProductPacketItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            //Value = c.DN_Product_Packet.Select(m => m.Shop_Product.Value).FirstOrDefault()
                        };
            return query.ToList();
        }
        public ModelOrderGetItem ListProductByDeddeskId(int agencyId, int beddeskId)
        {
            var query = (from c in FDIDB.DN_Packet
                         where c.AgencyID == agencyId && c.DN_Bed_Desk.Any(u => u.ID == beddeskId)
                         select new ModelOrderGetItem
                         {
                             ID = 0,
                             BedDeskID = beddeskId,
                             Time = c.Time,
                             Price = c.Price,
                             IsEarly = c.IsEarly.HasValue && c.IsEarly.Value,
                             ListItem = c.DN_Product_Packet.Select(m => new ProductItem
                             {
                                 ID = m.Shop_Product.ID,
                                 //Value = m.Shop_Product.Value,
                                 PriceNew = (m.Shop_Product.Shop_Product_Detail.Price * (m.Shop_Product.Product_Size == null ? 1000 : m.Shop_Product.Product_Size.Value) / 1000) ?? 0,
                             })
                         }).FirstOrDefault();
            return query;
        }
        public ModelOrderGetItem ListProductByDeddeskIdSpa(int agencyId, int beddeskId)
        {
            var query = (from c in FDIDB.Shop_Product_Detail
                         select new ModelOrderGetItem
                         {
                             ID = c.ID,
                             ListItem = c.Shop_Product.Where(m => m.IsDelete != true).Select(m => new ProductItem
                             {
                                 ID = m.ID,
                                 Name = m.Shop_Product_Detail.Name,
                                 //Value = m.Value,
                                 PriceNew = (m.Shop_Product_Detail.Price * (m.Product_Size == null ? 1000 : m.Product_Size.Value) / 1000) ?? 0,
                             })
                         }).FirstOrDefault();
            return query;
        }
        public List<ShopStatusItem> GetStatus(int agencyId)
        {
            var query = from c in FDIDB.DN_Status
                        where c.AgencyID == agencyId
                        select new ShopStatusItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Color = c.Color
                        };
            return query.ToList();
        }
        public bool CheckExitCode(string code, int id, int agencyId)
        {
            return FDIDB.Shop_Product.Any(c => c.ID != id && c.CodeSku == code && c.IsDelete == false);

        }
        public List<Category> GetListCategories(List<int> lstInts)
        {
            var query = from c in FDIDB.Categories where lstInts.Contains(c.Id) select c;
            return query.ToList();
        }
        public List<Gallery_Picture> GetListPicture(List<int> lstInts)
        {
            var query = from c in FDIDB.Gallery_Picture where lstInts.Contains(c.ID) select c;
            return query.ToList();
        }
        protected SqlConnection GetConnect(string str)
        {
            try
            {
                return new SqlConnection(str);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 1)
        {
            var name = FomatString.Slug(keword);
            var date = DateTime.Now;
            var query = FDIDB.AutoSelectProduct(name, showLimit).Select(c => new SuggestionsProduct
            {
                ID = c.ID,
                IdDetail = c.IdDetail,
                IsCombo = 0,
                name = "<span>BarCode:  <b>" + (c.BarCode ?? "") + "</b>- SL:  <b>" + (c.Quantity ?? 0) + "</b></span>",
                UrlImg = c.UrlImg ?? "/Content/Admin/images/auto-default.jpg",
                value = c.BarCode,
                BarCode = c.BarCode,
                QuantityDay = c.QuantityDay,
                Quantity = c.Quantity,
                DateS = date.ToString("yyyy-MM-dd"),
                DateE = date.AddMonths(c.QuantityDay ?? 0).ToString("yyyy-MM-dd"),
                data = c.PriceNew.ToString(),
                title = c.Name,
                pricenew = c.PriceNew,
                Type = type,

            });
            return query.ToList();
        }

        public List<SuggestionsProduct> GetListAutoOne(string keword, int showLimit, int agencyId, int type = 1)
        {
            var name = FomatString.Slug(keword);
            var query = FDIDB.AutoSelectProductOne(name, showLimit, agencyId).Select(c => new SuggestionsProduct
            {
                ID = c.ID,
                IsCombo = 0,
                name = "<span>BarCode:  <b>" + (c.BarCode ?? "") + "</b>" + "</span>",
                UrlImg = c.UrlImg ?? "/Content/Admin/images/auto-default.jpg",
                value = c.BarCode,
                data = c.PriceNew.ToString(),
                title = c.Name,
                PriceCost = c.Price,
                DateS = c.Date.DecimalToString("dd/MM/yyyy"),
                DateE = c.DateEnd.DecimalToString("dd/MM/yyyy"),
                Quantity = 1,
                pricenew = c.PriceNew,
                Type = type,
                Idimport = c.IdImport,
                BarCode = c.BarCode,
                Valueweight = c.Value
            }).ToList();
            return query.ToList();
        }
        public List<SuggestionsProduct> GetListAutoFull(string keword, int showLimit, int agencyId, int type = 1)
        {
            var name = FomatString.Slug(keword);
            var date = DateTime.Now;
            var query = FDIDB.AutoSelectProductFull(name, showLimit).Select(c => new SuggestionsProduct
            {
                ID = c.ID,
                IsCombo = 0,
                //name = "<span>Mã SP: <b>" + (c.CodeSku ?? "") + "</b> - BarCode:  <b>" + (c.BarCode ?? "") + "</b>- SL:  <b>" + (c.Quantity ?? 0) + "</b> - Màu: <b> " + (c.namecolor ?? "") + "</b> - Size: <b>" + (c.namesize ?? "") + "</b>" + "</span>",
                UrlImg = c.UrlImg ?? "/Content/Admin/images/auto-default.jpg",

                QuantityDay = c.QuantityDay,
                //Quantity = c.Quantity,
                DateS = date.ToString("yyyy-MM-dd"),
                DateE = date.AddMonths(c.QuantityDay ?? 0).ToString("yyyy-MM-dd"),
                code = c.CodeSku,
                data = c.PriceNew.ToString(),
                title = c.Name,
                //PriceCost = c.PriceCost,
                pricenew = c.PriceNew,
                Type = type
            });
            return query.ToList();
        }
        public List<SuggestionsProduct> GetListCommentAuto(string keword, int showLimit, int agencyId)
        {
            var name = FomatString.Slug(keword);
            var query = from c in FDIDB.Shop_Product
                        where (!c.IsDelete.HasValue || c.IsDelete == false) && !string.IsNullOrEmpty(c.Shop_Product_Detail.Code) && c.IsShow == true &&
                        (c.Shop_Product_Detail.NameAscii.Contains(name) || c.Shop_Product_Detail.Code.ToLower().Contains(name))
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            IsCombo = 0,
                            value = c.Shop_Product_Detail.Code + " - " + c.Shop_Product_Detail.Name,
                            title = c.Shop_Product_Detail.Name,
                            data = "Giá: " + SqlFunctions.StringConvert(c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000),
                            name = "",
                            Color = c.System_Color.Name ?? "",
                            Size = c.Product_Size.Name ?? "",
                            pricenew = c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000,
                            Unit = c.Shop_Product_Detail.DN_Unit.Name ?? "",
                        };
            return query.Take(showLimit).ToList();
        }

        public ProductItem GetCostProduceItem(int id)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.ID == id
                        select new ProductItem
                        {
                            ID = c.ID,
                            LstCostProducts = from u in c.Cost_Product
                                              select new CostProductItem
                                              {
                                                  BiasProduceID = u.BiasProduceID,
                                                  Percent = u.Percent,
                                                  SetupProductID = u.SetupProductId,
                                                  SetupProductName = u.SetupProduction.Name
                                              },
                            LstCostProductUserItems = from u in c.Cost_Product_User
                                                      select new CostProductUserItem
                                                      {
                                                          BiasProduceID = u.BiasProduceID,
                                                          Percent = u.Percent,
                                                          UserName = u.DN_Users.UserName,
                                                          SetupProductID = u.SetupProductId,
                                                          SetupProductName = u.SetupProduction.Name,
                                                          UserId = u.UserId
                                                      }
                        };
            return query.FirstOrDefault();
        }

        public List<SuggestionsProduct> GetListAutoComplete(string keword, int showLimit, int agencyId, int type = 0)
        {
            var name = FomatString.Slug(keword);
            var query = from c in FDIDB.Shop_Product
                        where (!c.IsDelete.HasValue || c.IsDelete == false) && !string.IsNullOrEmpty(c.Shop_Product_Detail.Code) && c.IsShow == true
                        && (c.Shop_Product_Detail.NameAscii.Contains(name) || c.CodeSku.ToLower().Contains(name))
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            IsCombo = 0,
                            value = c.CodeSku,
                            QuantityDay = c.QuantityDay ?? 0,
                            title = c.Shop_Product_Detail.Name,
                            data = "Giá: " + SqlFunctions.StringConvert(c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000),
                            name = "Mã SP: " + c.CodeSku,
                            pricenew = c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000,
                            Unit = c.Shop_Product_Detail.DN_Unit.Name,
                            Type = type
                        };
            return query.ToList();
        }

        public ProductItem GetProductItem(int id)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.ID == id
                        select new ProductItem
                        {
                            ID = c.ID,
                            PriceUnit = c.Shop_Product_Detail.Price,// dơn giá
                            Name = c.Shop_Product_Detail.Name,
                            SizeName = c.Product_Size.Name,
                            ColorName = c.System_Color.Name,
                            CodeSku = c.CodeSku,
                            Percent = c.Shop_Product_Detail.Percent,
                            Quantity = c.Quantity,
                            TypeID = c.TypeID,
                            QuantityDay = c.QuantityDay,
                            ProductDetailID = c.ProductDetailID,
                            PriceNew = (c.Shop_Product_Detail.Price * (c.Product_Size == null ? 1000 : c.Product_Size.Value) / 1000) ?? 0,
                            UnitID = c.Shop_Product_Detail.UnitID,
                            //PriceOld = c.PriceOld,
                            //PriceCost = c.PriceCost,
                            IsShow = c.IsShow,
                            ProductionCostID = c.ProductionCostID,
                            SizeID = c.SizeID,
                            ColorID = c.ColorID,
                            //LstRecipeItems = from v in c.Product_Recipe
                            //                 select new RecipeItem
                            //                 {
                            //                     ID = v.ID,
                            //                     ProductID = v.ProductId,
                            //                     ValueName = v.Shop_Product_Value.Name,
                            //                     ValueId = v.ValueId,
                            //                     Quantity = v.Quantity,
                            //                     UnitName = v.Shop_Product_Value.DN_Unit.Name,
                            //                     Price = v.Price,
                            //                     IsDelete = v.IsDelete,
                            //                     ProductName = v.Shop_Product_Detail.Name,
                            //                     ProductDetailID = v.ProductDetailId,
                            //                 },
                            LstShopProductPictureItem = c.Shop_Product_Picture.Where(m => m.ProductID == c.ID).Select(n => new ShopProductPictureItem
                            {
                                UrlPicture = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                                PictureID = n.PictureID,
                                Sort = n.Sort,
                                ProductID = n.ProductID
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<Shop_Product> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Shop_Product
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public List<DN_ImportProduct> GetListImportProductArrId(List<Guid> ltsArrId)
        {
            var query = from c in FDIDB.DN_ImportProduct
                        where ltsArrId.Contains(c.GID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public ShopProductCartItem GetProductCart(int id, int sizeId)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.ID == id
                        select new ShopProductCartItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Price = c.Shop_Product.Where(a => a.SizeID == sizeId).Select(a => a.Shop_Product_Detail.Price * a.Product_Size.Value / 1000).FirstOrDefault(),
                            ColorName = c.Shop_Product.Where(a => a.SizeID == sizeId).Select(a => a.System_Color.Name).FirstOrDefault(),
                            Size = c.Shop_Product.Where(a => a.SizeID == sizeId).Select(a => a.Product_Size.Name).FirstOrDefault(),
                        };
            return query.FirstOrDefault();
        }
        public Shop_Product GetById(int id)
        {
            var query = from c in FDIDB.Shop_Product where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public ProductItem GetProductDetailItem(int id)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                where c.ID == id 
                select new ProductItem
                {
                    ID = c.ID,
                    Name = c.Name,
                    NameAscii = c.NameAscii,
                    UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                    PriceUnit = c.Price
                };
            return query.FirstOrDefault();
        }

        public void Add(Shop_Product item)
        {
            FDIDB.Shop_Product.Add(item);
        }

        public void Delete(Cost_Product item)
        {
            FDIDB.Cost_Product.Remove(item);
        }
        public void Delete(Cost_Product_User item)
        {
            FDIDB.Cost_Product_User.Remove(item);
        }
    }
}
