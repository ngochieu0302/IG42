using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
using System;
using System.Data.Entity.Infrastructure;

namespace FDI.DA
{
    public class ProductDL : BaseDA
    {
        public List<OrderItem> GetListOrder()
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            DateCreated = c.DateCreated,
                            
                        };
            return query.Take(6).ToList();
        }
       
        public List<ProductItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false
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
                            PriceNew = c.Shop_Product_Detail.Price * c.Product_Size.Value / 1000,
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
        public List<ProductItem> GetList()
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.LanguageId == LanguageId && c.IsShow == true
                        orderby c.ID descending 
                        select new ProductItem
                        {
                            ID = c.ID
                        };
            return query.ToList();
        }
        public List<ProductItem> GetListHome()
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.LanguageId == LanguageId && c.IsShow == true && c.IsHot == true
                        orderby c.ID descending 
                        select new ProductItem
                        {
                            ID = c.ID,
                            Name = c.Shop_Product_Detail.Name,
                            //NameAscii = c.NameAscii,
                            //UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Folder,
                            //Description = c.Description
                        };
            return query.ToList();
        }
        public List<ShopProductDetailItem> GetListProductDetails(int cate, int page, int rowPage, ref int total)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.IsDelete == false  && c.IsShow == true
                         && c.CateID == cate
                        orderby c.Sort 
                        select new ShopProductDetailItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Description = c.Description,
                            Sort = c.Sort,
                            ListGalleryPictureItems = c.Gallery_Picture2.Where(a => a.IsDeleted == false && a.IsShow == true).Select(z => new GalleryPictureItem
                            {
                                Name = z.Name,
                                Url = z.Folder + z.Url,
                            }),
                        };
            query = query.Paging(page, rowPage, ref total);
            return query.ToList();
        }
        

        public ShopProductDetailItem GetProductId(int id)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.IsDelete == false 
                        && c.IsShow == true && c.ID == id
                        select new ShopProductDetailItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            Description = c.Description,
                            Details = c.Details,
                           CategoryID = c.CateID,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            UrlPictureMap = c.Gallery_Picture1.Folder + c.Gallery_Picture1.Url,
                            ListCategoryItem = c.Categories.Where(a=>a.IsDeleted != true).Select(v=> new CategoryItem
                            {
                                ID = v.Id,
                            }),
                            ListGalleryPictureItems = c.Gallery_Picture2.Where(a => a.IsDeleted == false && a.IsShow == true).Select(z => new GalleryPictureItem
                            {
                                Name = z.Name,
                                Url = z.Folder + z.Url,
                            }),
                            ListProductItem = c.Shop_Product.Where(a=>a.IsDelete != true).Select(v=> new ProductItem
                            {
                                PriceNew = v.Shop_Product_Detail.Price * v.Product_Size.Value / 1000,
                                //PriceOld = v.PriceOld,
                                ID = v.ID,
                            }),
                            //ListColorProductItem = from a in c.Shop_Product group a by a.ColorID  into g select new ColorItem
                            //{
                            //    ID = g.FirstOrDefault().System_Color.ID,
                            //    Name = g.FirstOrDefault().System_Color.Name,
                            //    Value = g.FirstOrDefault().System_Color.Value
                            //},
                            ListSizeProductItem = from a in c.Shop_Product
                                                   group a by a.SizeID into g
                                                   select new ProductSizeItem
                                                   {
                                                       ID = g.FirstOrDefault().Product_Size.ID,
                                                       Name = g.FirstOrDefault().Product_Size.Name,
                                                       Value = g.FirstOrDefault().Product_Size.Value
                                                   }
                        };
            return query.FirstOrDefault();
        }
        public Shop_Product GetbyId(int id)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false
                              && c.IsShow == true && c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public TagItem GetListTag(int id)
        {
            var query = from c in FDIDB.System_Tag
                        where c.ID == id && c.IsDelete == false && c.LanguageId == LanguageId
                        select new TagItem
                        {
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            ID = c.ID

                        };
            return query.FirstOrDefault();
        }
        public CategoryItem CategoryItem(int id)
        {
            var query = from n in FDIDB.Categories
                        where n.IsDeleted == false && n.LanguageId == LanguageId && n.Id == id
                        select new CategoryItem
                        {
                            ID = n.Id,
                            Name = n.Name,
                            Description = n.Description,
                            Slug = n.Slug
                        };
            return query.FirstOrDefault();
        }
        public CategoryItem GetCateId(int id)
        {
            var query = from n in FDIDB.Categories
                        where n.IsDeleted == false && n.LanguageId == LanguageId && n.Id == id
                        select new CategoryItem
                        {
                            ID = n.Id,
                            Name = n.Name,
                            Slug = n.Slug,
                            Icon = n.Icon,
                        };
            return query.FirstOrDefault();
        }
        public List<ShopProductDetailItem> GetListCateId(List<int> lst,int id)
        {
            var query = from n in FDIDB.Shop_Product_Detail
                        where n.IsDelete == false && n.Categories.Any(c => lst.Contains(c.Id)) && n.ID != id
                        select new ShopProductDetailItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            NameAscii = n.NameAscii,
                            Description = n.Description,
                            UrlPicture = n.Gallery_Picture.Folder + n.Gallery_Picture.Url
                        };

            return query.Take(16).ToList();
        }
        public List<Category> GetListCategoryByArrId(List<int> lst)
        {
            var query = from n in FDIDB.Categories
                        where n.IsDeleted == false && lst.Contains(n.Id)
                        select n;
            return query.ToList();
        }
        public List<Gallery_Picture> GetListPictureByArrId(List<int> lst)
        {
            var query = from n in FDIDB.Gallery_Picture
                        where n.IsDeleted == false && lst.Contains(n.ID)
                        select n;
            return query.ToList();
        }
        public List<System_Tag> GetListIntTagByArrId(List<int> lst)
        {
            var query = from n in FDIDB.System_Tag
                        where n.IsDelete == false && lst.Contains(n.ID)
                        select n;
            return query.ToList();
        }
        public List<Shop_Product> GetListByArrId(List<int> lst)
        {
            var query = from n in FDIDB.Shop_Product
                        where n.IsDelete == false && lst.Contains(n.ID)
                        select n;
            return query.ToList();
        }
        public List<ProductItem> GetListOther(int cateId, int ortherId)
        {
            var query = from n in FDIDB.Shop_Product
                        where n.IsDelete == false && n.IsShow == true
                        && n.LanguageId == LanguageId && n.ID != ortherId
                        orderby n.ID descending
                        select new ProductItem
                        {
                            ID = n.ID
                        };
            return query.Take(9).ToList();
        }

        public void Add(Shop_Product item)
        {
            FDIDB.Shop_Product.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
        
        //public bool CheckNameAscii(string name, int id)
        //{
        //    return FDIDB.Shop_Product_Detail.Any(c => c.ID != id && c.NameAscii == name && c.IsDelete == false );

        //}
        /// <summary>
        /// dongdt 08/12/2017
        /// </summary>
        /// <param name="keyword">keyword</param>
        /// <param name="agencyid"></param>
        /// <param name="h">HomeId</param>
        /// <param name="s">SupplierId</param>
        /// <param name="stt"></param>
        /// <param name="ps">Price Start</param>
        /// <param name="pe">Price End</param>
        /// <param name="listn">List not included</param>
        /// <returns>List<ID></returns>
        
    }
}