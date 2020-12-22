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

namespace FDI.DA.DA.DN_Sales
{
    public class DNPromotionDA : BaseDA
    {
        #region Constructer
        public DNPromotionDA()
        {
        }

        public DNPromotionDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNPromotionDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<DNPromotionItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Promotion
                        where c.AgencyId == agencyId && (!c.IsDeleted.HasValue || c.IsDeleted == false) && c.IsShow == true
                        orderby c.ID descending
                        select new DNPromotionItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            DateEnd = c.DateEnd,
                            DateStart = c.DateStart,
                            Note = c.Note,
                            Quantity = c.Quantity,
                            AgencyId = c.AgencyId,
                            Username = c.DN_Users.UserName,
                            IsAgency = c.IsAgency,
                            IsAll = c.IsAll,
                            IsEnd = c.IsEnd,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public DNPromotionItem GetDNPromotionItem(int id)
        {
            var query = from c in FDIDB.DN_Promotion
                        where c.ID == id && c.IsDeleted == false
                        orderby c.ID descending
                        select new DNPromotionItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            DateEnd = c.DateEnd,
                            DateStart = c.DateStart,
                            Note = c.Note,
                            Quantity = c.Quantity,
                            AgencyId = c.AgencyId,
                            Username = c.DN_Users.UserName,
                            IsAgency = c.IsAgency,
                            IsAll = c.IsAll,
                            IsEnd = c.IsEnd,
                            TotalOrder = c.TotalOrder,
                            IsOnly = c.IsOnly,
                            IsShow = c.IsShow,
                            Type = c.Type,
                            ListProductDetailItems = c.Product_Promotion.Where(a => (!a.Shop_Product_Detail.IsDelete.HasValue || a.Shop_Product_Detail.IsDelete == false) && a.Shop_Product_Detail.IsShow == true).Select(v => new ShopProductDetailItem
                            {
                                ID = v.Shop_Product_Detail.ID,
                                Name = v.Shop_Product_Detail.Name,
                                Code = v.Shop_Product_Detail.Code,
                                Price = v.Shop_Product_Detail.Price,
                                Quantity = v.Quantity,
                                UrlPicture = v.Shop_Product_Detail.Gallery_Picture.Folder + v.Shop_Product_Detail.Gallery_Picture.Url
                            }),
                            LstCategoryItems = c.Categories.Where(a => (!a.IsDeleted.HasValue || a.IsDeleted == false) && a.IsShow == true).Select(v => new CategoryItem
                            {
                                ID = v.Id,
                                Name = v.Name
                            }),
                            ListPromotionDetailItems = c.Promotion_Product.Where(b => (!b.Shop_Product.Shop_Product_Detail.IsDelete.HasValue || b.Shop_Product.Shop_Product_Detail.IsDelete == false) && b.Shop_Product.Shop_Product_Detail.IsShow == true).Select(v => new DNPromotionProductItem
                            {
                                ID = v.Shop_Product.ID,
                                Name = v.Shop_Product.Shop_Product_Detail.Name,
                                Code = v.Shop_Product.CodeSku,
                                Price = v.Price,
                                PriceProduct = (v.Shop_Product.Shop_Product_Detail.Price * v.Shop_Product.Product_Size.Value),
                                Quantity = v.Quantity,
                                IsEnd = v.IsEnd,
                                Percent = v.Percent,
                                Note = v.Note,
                                UrlPicture = v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder + v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url,
                            })
                        };
            return query.FirstOrDefault();
        }
        #region GetPromotion in Order
        public List<DNPromotionPItem> GetPromotionProduct(int id, int agencyid, int quantity)
        {
            const int type = (int)TypeEnumPromotion.Product;
            var date = DateTime.Now.TotalSeconds();
            var modelS = from o in FDIDB.DN_Promotion
                         where (!o.IsEnd.HasValue || !o.IsEnd.Value) && (!o.IsDeleted.HasValue || !o.IsDeleted.Value) &&
                             o.IsShow == true && o.Type == type
                             && (!o.IsAgency.HasValue || !o.IsAgency.Value || o.AgencyId == agencyid) && o.DateStart < date &&
                             o.DateEnd > date && (o.Quantity == 0 || o.QuantityUse == 0 || o.Quantity - (o.QuantityUse ?? 0) > 0)
                             && (o.IsAll == true || o.Product_Promotion.Any(k => k.Shop_Product_Detail.ID == id && k.Quantity <= quantity) ||
                              o.Categories.Any(k => k.Shop_Product_Detail.Any(p => p.ID == id))
                              || o.Categories.Any(k => k.Category1.Any(c => c.Shop_Product_Detail.Any(p => p.ID == id))))
                         select new DNPromotionPItem
                         {
                             ID = o.ID,
                             Title = o.Name,
                             Quantity = o.Product_Promotion.Where(k => k.Shop_Product_Detail.ID == id).Select(p => p.Quantity).FirstOrDefault() ?? 1,
                             PromotionSPItems = o.Promotion_Product.Where(p => p.Quantity > 0 && (!p.IsEnd.HasValue || !p.IsEnd.Value))
                                     .Select(p => new PromotionSPItem
                                     {
                                         ID = o.ID,
                                         Name = o.Name,
                                         ProductID = p.ProductID,
                                         UrlImg = p.Shop_Product.Shop_Product_Detail.PictureID.HasValue
                                                 ? "/Uploads/" + p.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder +
                                                   p.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url
                                                 : "/Content/Admin/images/auto-default.jpg",
                                         Quantity = p.Quantity,
                                         Code = p.Shop_Product.Shop_Product_Detail.Code,
                                         Title = p.Shop_Product.Shop_Product_Detail.Name,
                                         PriceSp = p.Shop_Product.Shop_Product_Detail.Price * p.Shop_Product.Product_Size.Value/1000,
                                         IsOnly = o.IsOnly,
                                         Percent = p.Percent,
                                         Price = p.Price,
                                         TotalPrice = (p.Shop_Product.Shop_Product_Detail.Price * p.Shop_Product.Product_Size.Value/1000 ?? 0) - ((p.Percent ?? 0) * (p.Shop_Product.Shop_Product_Detail.Price * p.Shop_Product.Product_Size.Value/1000 ?? 0) / 100) - (p.Price ?? 0)
                                     })

                         };
            return modelS.ToList();
        }
        public List<DNPromotionPItem> GetPromotionOrder(int agencyid, decimal totalorder)
        {
            const int type = (int)TypeEnumPromotion.Order;
            var date = DateTime.Now.TotalSeconds();
            var modelS = from o in FDIDB.DN_Promotion
                         where (!o.IsEnd.HasValue || !o.IsEnd.Value) && (!o.IsDeleted.HasValue || !o.IsDeleted.Value) && o.IsShow == true && o.Type == type
                         && (!o.IsAgency.HasValue || !o.IsAgency.Value || o.AgencyId == agencyid) && o.DateStart < date && o.DateEnd > date && (o.Quantity == 0 || o.QuantityUse == 0 || o.Quantity - o.QuantityUse > 0)
                         && (o.TotalOrder <= totalorder)
                         select new DNPromotionPItem
                         {
                             ID = o.ID,
                             Title = o.Name,
                             PromotionSPItems = o.Promotion_Product.Where(p => (!p.IsEnd.HasValue || !p.IsEnd.Value))
                                      .Select(p => new PromotionSPItem
                                      {
                                          ID = p.ID,
                                          Name = o.Name,
                                          ProductID = p.ProductID,
                                          UrlImg = p.Shop_Product.Shop_Product_Detail.PictureID.HasValue
                                                  ? "/Uploads/" + p.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder +
                                                    p.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url
                                                  : "/Content/Admin/images/auto-default.jpg",
                                          Quantity = p.Quantity,
                                          Code = p.Shop_Product.Shop_Product_Detail.Code,
                                          Title = p.Shop_Product.Shop_Product_Detail.Name,
                                          PriceSp = p.Shop_Product.Shop_Product_Detail.Price * p.Shop_Product.Product_Size.Value/1000,
                                          IsOnly = o.IsOnly,
                                          Percent = p.Percent,
                                          Price = p.Price,
                                          TotalPrice = (p.Shop_Product.Shop_Product_Detail.Price * p.Shop_Product.Product_Size.Value/1000 ?? 0) - ((p.Percent ?? 0) * (p.Shop_Product.Shop_Product_Detail.Price * p.Shop_Product.Product_Size.Value/1000 ?? 0) / 100) - (p.Price ?? 0)
                                      })
                         };
            return modelS.ToList();
        }
        #endregion
        public List<Shop_Product_Detail> GetListIntProductByArrId(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Product_Detail where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<DN_Promotion> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Promotion where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public DN_Promotion GetById(int promotion)
        {
            var query = from c in FDIDB.DN_Promotion where c.ID == promotion select c;
            return query.FirstOrDefault();
        }
        public List<Category> GetListCateByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Categories
                        where ltsArrId.Contains(o.Id)
                        select o;
            return query.ToList();
        }
        public void Add(DN_Promotion item)
        {
            FDIDB.DN_Promotion.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
