using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class CategoryDL : BaseDA
    {
        public CategoryItem GetBySlug(string slug)
        {
            var query = from c in FDIDB.Categories
                        where c.Slug.ToLower().Equals(slug.ToLower()) && c.IsDeleted == false
                        orderby c.Id descending
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Slug = c.Slug,
                            Description = c.Description,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            ParentId = c.ParentId,
                            SeoTitle = c.SEOTitle,
                            SeoDescription = c.SEODescription,
                            SeoKeyword = c.SEOKeyword
                        };
            return query.FirstOrDefault();
        }
        public List<CategoryItem> GetlistCate()
        {
            var query = from c in FDIDB.Categories 
                        where c.IsDeleted == false && c.LanguageId == LanguageId && c.IsShow == true
                        orderby c.Id descending
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Slug = c.Slug,
                            Description = c.Description,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            ParentId = c.ParentId,
                            SeoTitle = c.SEOTitle,
                            SeoDescription = c.SEODescription,
                            SeoKeyword = c.SEOKeyword
                        };
            return query.ToList();
        }
        public List<CategoryItem> GetlistCatebyParent(int id)
        {
            var query = from c in FDIDB.Categories
                        where c.IsDeleted == false && c.LanguageId == LanguageId && c.IsShow == true && c.ParentId == id
                        orderby c.Sort ascending 
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            Slug = c.Slug,
                            Type = c.Type,
                            Count = c.Shop_Product_Detail.Count,
                            CountNews = c.News_News.Count
                        };
            return query.ToList();
        }
        public List<CategoryItem> GetlistCateById(int id,int type)
        {
            var query = from c in FDIDB.Categories
                        where c.IsDeleted == false && c.LanguageId == LanguageId && c.IsShow == true && c.Type == type && (c.ParentId == 83)
                        && c.IsShowhome == true
                        orderby c.Id descending
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Slug = c.Slug,
                            Description = c.Description,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            ParentId = c.ParentId,
                            SeoTitle = c.SEOTitle,
                            SeoDescription = c.SEODescription,
                            SeoKeyword = c.SEOKeyword,
                            ListProductItem = c.Shop_Product_Detail.Where(a=>a.IsDelete == false && a.IsShow == true).Select(z=> new ShopProductDetailItem
                            {
                                ID = z.ID,
                                Name = z.Name,
                                NameAscii = z.NameAscii,
                                Description = z.Description,
                                Price = z.Price,
                                UrlPicture = z.Gallery_Picture.Folder + z.Gallery_Picture.Url,
                            })
                        };
            return query.ToList();
        }
        public List<CategoryItem> GetlistCateShowhome(int type)
        {
            var query = from c in FDIDB.Categories
                        where c.IsDeleted == false && c.LanguageId == LanguageId && c.IsShow == true && c.IsShowhome == true && c.Type== type
                        orderby c.Id 
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Slug = c.Slug,
                            Description = c.Description,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            ParentId = c.ParentId,
                            ListProductItem = c.Shop_Product_Detail.Where(a => a.IsShow == true && a.IsDelete != true  && a.IsHot == true).Select(z => new ShopProductDetailItem
                            {
                                ID = z.ID,
                                Name = z.Name,
                                NameAscii = z.NameAscii,
                                UrlPicture = z.Gallery_Picture.Folder + z.Gallery_Picture.Url,
                                Description = z.Description,
                                UrlPictureMap = z.Gallery_Picture1.Folder + z.Gallery_Picture1.Url,
                                Sale = z.Sale,
                                DateCreate = z.DateCreate,
                                ListProductItem = z.Shop_Product.Where(a=>a.IsDelete != true).Select(v=> new ProductItem
                                {
                                   ID = v.ID,
                                    PriceNew = (v.Shop_Product_Detail.Price * (decimal)v.Product_Size.Value / 1000) ?? 0,
                                    //PriceOld = v.PriceOld,
                                    
                                }),
                                //ListColorProductItem = from a in z.Shop_Product
                                //                       group a by a.ColorID into g
                                //                       select new ColorItem
                                //                       {
                                //                           //ID = g.FirstOrDefault().System_Color.ID,
                                //                           Name = g.FirstOrDefault().System_Color.Name,
                                //                           Value = g.FirstOrDefault().System_Color.Value
                                //                       },
                                ListSizeProductItem = from a in z.Shop_Product
                                                      where a.IsDelete != true
                                                      group a by a.SizeID into g
                                                      select new ProductSizeItem
                                                      {
                                                          ID = g.FirstOrDefault().Product_Size.ID,
                                                          Name = g.FirstOrDefault().Product_Size.Name,                                                          
                                                      }
                            })
                        };
            return query.ToList();
        }
        public CategoryItem GetCateHot(int type)
        {
            var query = from c in FDIDB.Categories
                        where c.IsDeleted == false && c.LanguageId == LanguageId && c.IsShow == true && c.IsMenu == true && c.Type == type
                        orderby c.Id
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Slug = c.Slug,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            ParentId = c.ParentId,
                            Description = c.Description,
                            ListProductItem = c.Shop_Product_Detail.Where(a => a.IsShow == true && a.IsDelete == false).Select(z => new ShopProductDetailItem
                            {
                                ID = z.ID,
                                Name = z.Name,
                                NameAscii = z.NameAscii,
                                UrlPicture = z.Gallery_Picture.Folder + z.Gallery_Picture.Url,
                                Description = z.Description,
                                UrlPictureMap = z.Gallery_Picture1.Folder + z.Gallery_Picture1.Url,
                                Sale = z.Sale,
                                ListProductItem = z.Shop_Product.Where(a => a.IsDelete != true).Select(v => new ProductItem
                                {
                                    ID = v.ID,
                                    PriceNew = (v.Shop_Product_Detail.Price * (decimal)v.Product_Size.Value / 1000) ?? 0,
                                    //PriceOld = v.PriceOld
                                }),
                                //ListColorProductItem = from a in z.Shop_Product
                                //                       group a by a.ColorID into g
                                //                       select new ColorItem
                                //                       {
                                //                           ID = g.FirstOrDefault().System_Color.ID,
                                //                           Name = g.FirstOrDefault().System_Color.Name,
                                //                           Value = g.FirstOrDefault().System_Color.Value
                                //                       },
                                //ListSizeProductItem = from a in z.Shop_Product
                                //                      group a by a.SizeID into g
                                //                      select new ProductSizeItem
                                //                      {
                                //                          ID = g.FirstOrDefault().Product_Size.ID,
                                //                          Name = g.FirstOrDefault().Product_Size.Name,
                                //                          Value = g.FirstOrDefault().Product_Size.Value
                                //                      }
                            })
                        };
            return query.FirstOrDefault();
        }
        public CategoryItem GetByid(int id)
        {
            var query = from c in FDIDB.Categories
                        where c.Id == id
                        orderby c.Id descending
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Slug = c.Slug,
                            Description = c.Description,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            ParentId = c.ParentId,
                            SeoTitle = c.SEOTitle,
                            SeoDescription = c.SEODescription,
                            SeoKeyword = c.SEOKeyword,
                            ListNewsItem = c.News_News.Where(a => a.IsShow == true && a.IsDeleted == false && c.LanguageId == LanguageId).Select(z => new NewsItem
                            {
                                Title = z.Title,
                                TitleAscii = z.TitleAscii,
                                ID = z.ID,
                                IsHot = z.IsHot,
                                DisplayOrder = z.DisplayOrder,
                                Description = z.Description,
                                
                                PictureUrl = z.Gallery_Picture.Folder + z.Gallery_Picture.Url,
                            }),
                            ListProductItem = c.Shop_Product_Detail.Where(a=>a.IsDelete == false && a.IsShow == true && a.IsHot == true).Select(s=> new ShopProductDetailItem
                            {
                                UrlPicture = s.Gallery_Picture.Folder + s.Gallery_Picture.Url,
                                Name = s.Name,
                                NameAscii = s.NameAscii,
                                DateCreate = s.DateCreate,
                                ID = s.ID,
                                
                                ListGalleryPictureItems = s.Gallery_Picture2.Where(a => a.IsDeleted == false && a.IsShow == true).Select(z => new GalleryPictureItem
                                {
                                    Name = z.Name,
                                    Url = z.Folder + z.Url
                                })
                            }),
                            
                        };
            return query.FirstOrDefault();
        }

    }
}