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
    public class CategoryDA : BaseDA
    {
        #region Constructer
        public CategoryDA()
        {
        }

        public CategoryDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CategoryDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CategoryItem> GetChildByParentId(bool setTitle)
        {
            var cate = from c in FDIDB.Category_GetList(LanguageId)
                       select new CategoryItem
                       {
                           ID = c.Id,
                           ParentId = c.ParentId,
                           Name = c.Name,
                           Slug = c.NameRoot,
                           Type = c.Type,
                           IsLevel = c.IsLevel,
                           Price = c.Price,
                           PriceRecipeFinal = c.PriceRecipeFinal,
                           PriceFinal = c.PriceFinal,
                           PercentLoss = c.PercentLoss,
                           WeightDefault = c.WeightDefault,
                       };

            var re = cate.ToList();
            if (setTitle)
                re.Insert(0, new CategoryItem { ID = 0, Name = "-- Chọn chuyên mục --" });
            return re;
        }
        public List<CategoryItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int type)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Categories
                        where (!o.IsDeleted.HasValue || o.IsDeleted == false) && o.IsShow == true && o.Type == type
                        orderby o.Id descending
                        select new CategoryItem
                        {
                            ID = o.Id,
                            Name = o.Name,
                            Slug = o.Slug,
                            Price = o.Price,
                            PriceCost = o.PriceCost,
                            Percent = o.Percent,
                            PercentLoss = o.PercentLoss,
                            PriceRecipeFinal = o.PriceRecipeFinal,
                            TotalIncurredFinal = o.TotalIncurredFinal,
                            Incurred = o.Incurred,
                            PriceFinal = o.PriceFinal,
                            WeightDefault = o.WeightDefault,
                            TotalRecipe = o.Category_Recipe.Count,
                            PriceOld = o.PriceOld,
                            Profit = o.Profit,
                            PriceRecipe = o.PriceRecipe,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<TreeViewItem> GetListTree(int agencyId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 1 && c.IsDeleted == false
                        orderby c.IsLevel, c.Sort
                        select new TreeViewItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            Count = FDIDB.Categories.Where(m => m.IsDeleted == false).Count(m => m.ParentId == c.Id)
                        };
            return query.ToList();
        }
        public List<TreeViewItem> GetListTreeByType(int type, string lstId, int agencyId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from c in FDIDB.Categories
                        where c.Id > 0 && c.IsDeleted == false && c.LanguageId == LanguageId && (type == 0 || c.Type == type) && c.IsShow == true
                        orderby c.IsLevel, c.Sort
                        select new TreeViewItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = ltsArrId.Any(m => m == c.Id),
                            Count = FDIDB.Categories.Where(m => m.IsDeleted == false).Count(m => m.ParentId == c.Id)
                        };
            return query.ToList();
        }

        public List<TreeViewItem> GetListTreeByTypeListId(int type, string lstId, int agencyId = 0)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from c in FDIDB.Categories
                        where c.Id > 0 && c.IsDeleted == false && c.LanguageId == LanguageId && c.IsShow == true && (c.Type == type || type == 0)
                        orderby c.IsLevel, c.Sort
                        select new TreeViewItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = ltsArrId.Any(m => m == c.Id),
                            Count = FDIDB.Categories.Where(m => m.IsDeleted == false).Count(m => m.ParentId == c.Id)
                        };
            return query.ToList();
        }
        public List<CategoryItem> GetAllListSimpleByParentId(int parentId, int agencyId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 1 && c.ParentId == parentId && c.IsDeleted == false && c.LanguageId == LanguageId
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            LanguageId = c.LanguageId,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            IsLevel = c.IsLevel,
                            SeoTitle = c.SEOTitle,
                            SeoKeyword = c.SEOKeyword,
                            SeoDescription = c.SEODescription,
                            Slug = c.Slug,
                            CreatedDate = c.CreatedDate,
                            UserCreate = c.UserCreate
                        };
            return query.ToList();
        }

        public CategoryItem GetItemById(int id)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 1 && c.Id == id && c.IsDeleted == false && c.LanguageId == LanguageId
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            LanguageId = c.LanguageId,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            Type = c.Type,
                            IsShow = c.IsShow,
                            Price = c.Price,
                            PriceFinal = c.PriceFinal,
                            PriceCost = c.PriceCost,
                            Incurred = c.Incurred,
                            PriceOld = c.PriceOld,
                            Profit = c.Profit,
                            PriceRecipe = c.PriceRecipe,
                            WeightDefault = c.WeightDefault,
                            PercentLoss = c.PercentLoss,
                            Percent = c.Percent,
                            PriceRecipeFinal = c.PriceRecipeFinal,
                            TotalIncurredFinal = c.TotalIncurredFinal,
                            IsLevel = c.IsLevel,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            PictureID = c.PictureID,
                            Icon = c.Icon,
                            UnitName = c.DN_Unit.Name,
                            UnitID = c.UnitID,
                            CostPrice = c.PriceCost
                        };
            return query.FirstOrDefault();
        }

        public List<CategoryItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow, int agencyId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 1
                        orderby c.Name
                        where c.IsShow == isShow && c.Name.StartsWith(keyword) && c.LanguageId == LanguageId
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            LanguageId = c.LanguageId,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            IsLevel = c.IsLevel,
                            SeoTitle = c.SEOTitle,
                            SeoKeyword = c.SEOKeyword,
                            SeoDescription = c.SEODescription,
                            Slug = c.Slug,
                            CreatedDate = c.CreatedDate,
                            UserCreate = c.UserCreate
                        };
            return query.Take(showLimit).ToList();
        }
        public bool CheckTitleAsciiExits(string slug, int id)
        {
            var query = (from c in FDIDB.Categories
                         where c.Slug == slug && c.Id != id && c.ParentId == 1 && c.IsDeleted == false && c.LanguageId == LanguageId
                         select c).Count();
            return query > 0;
        }

        public List<DN_AttributeDynamic> GetListAttr(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = (from c in FDIDB.DN_AttributeDynamic
                         where ltsArrId.Contains(c.ID)
                         select c);
            return query.ToList();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="categoryId">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Category GetById(int categoryId)
        {
            var query = from c in FDIDB.Categories where c.Id == categoryId select c;
            return query.FirstOrDefault();
        }
        public CategoryItem GetCategoryById(int categoryId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id == categoryId
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Type = c.Type,
                            LanguageId = c.LanguageId,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            IsLevel = c.IsLevel,
                            SeoTitle = c.SEOTitle,
                            SeoKeyword = c.SEOKeyword,
                            SeoDescription = c.SEODescription,
                            Slug = c.Slug,
                            CreatedDate = c.CreatedDate,
                            UserCreate = c.UserCreate
                        };
            return query.FirstOrDefault();
        }

        public List<CategoryItem> GetAll()
        {
            var query = from c in FDIDB.Categories
                        where c.IsShow == true && c.IsDeleted == false && c.Id > 1
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Type = c.Type,
                            LanguageId = c.LanguageId,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            Price = c.Price,
                            IsShow = c.IsShow,
                            IsLevel = c.IsLevel,
                            SeoTitle = c.SEOTitle,
                            SeoKeyword = c.SEOKeyword,
                            SeoDescription = c.SEODescription,
                            Slug = c.Slug,
                            CreatedDate = c.CreatedDate,
                            UserCreate = c.UserCreate
                        };
            return query.ToList();
        }

        public CategoryItem GetCategoryParentId(int categoryId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id == categoryId
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Type = c.Type,
                            LanguageId = c.LanguageId,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            IsLevel = c.IsLevel,
                            SeoTitle = c.SEOTitle,
                            SeoKeyword = c.SEOKeyword,
                            SeoDescription = c.SEODescription,
                            Slug = c.Slug,
                            CreatedDate = c.CreatedDate,
                            UserCreate = c.UserCreate
                        };
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrId">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Category> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from c in FDIDB.Categories where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }
        public List<Category> GetListByArrId1(List<int> lstId)
        {
            var query = from c in FDIDB.Categories where lstId.Contains(c.Id) select c;
            return query.ToList();
        }
        public bool CheckExits(Category category)
        {
            var query = from c in FDIDB.Categories where ((c.Name == category.Name) && (c.Id != category.Id)) select c;
            return query.Any();
        }
        public Category GetByName(string name, int agencyId)
        {
            var query = from c in FDIDB.Categories where (c.Name == name) select c;
            return query.FirstOrDefault();
        }
        public void Add(Category category)
        {
            FDIDB.Categories.Add(category);
        }
        public void Delete(Category category)
        {
            FDIDB.Categories.Remove(category);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion

        public List<CategoryItem> GetCategoryForPolicy()
        {
            var query = from c in FDIDB.Categories
                where c.Type == (int) CategoryModule.Product
                select new CategoryItem()
                {
                    ID = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Price = c.Price,
                };

            return query.ToList();
        }
    }
}
