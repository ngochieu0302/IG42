using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;
using System;
using FDI.Utils;

namespace FDI.DA
{
    public class CategoryAppIG4DA : BaseDA
    {
        #region Constructer
        public CategoryAppIG4DA()
        {
        }

        public CategoryAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CategoryAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CategoryAppIG4Item> GetChildByParentId(bool setTitle, int type = 0)
        {
            var cate = from c in FDIDB.Category_GetList(LanguageId)
                       select new CategoryAppIG4Item
                       {
                           ID = c.Id,
                           ParentId = c.ParentId,
                           Name = c.Name,
                           Slug = c.NameRoot,
                           Type = c.Type,
                           IsLevel = c.IsLevel
                       };
            if (type != 0)
            {
                cate = cate.Where(c => c.Type == type);
            }
            var re = cate.ToList();
            if (setTitle)
                re.Insert(0, new CategoryAppIG4Item { ID = 0, Name = "-- Chọn chuyên mục --" });
            return re;
        }

        public List<CategoryAppIG4Item> GetChildByParentId(bool setTitle, bool IsAdmin, Guid UserID, int type = 0)
        {
            var cate = from c in FDIDB.Category_GetListByUser(LanguageId, IsAdmin, UserID, type)
                       select new CategoryAppIG4Item
                       {
                           ID = c.Id,
                           ParentId = c.ParentId,
                           Name = c.Name,
                           Slug = c.NameRoot,
                           Type = c.Type,
                           IsLevel = c.IsLevel,
                           Step = c.IsLevel
                       };
            var re = cate.ToList();
            if (setTitle)
                re.Insert(0, new CategoryAppIG4Item { ID = 0, Name = "-- Chọn chuyên mục --" });
            return re;
        }
        public List<TreeViewAppIG4Item> GetListTree(int type)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 0 && (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.LanguageId == LanguageId && (type == 0 || c.Type == type)
                        orderby c.IsLevel, c.Sort
                        select new TreeViewAppIG4Item
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            Count = FDIDB.Categories.Count(m => m.ParentId == c.Id && (!m.IsDeleted.HasValue || !m.IsDeleted.Value))
                        };
            return query.ToList();
        }
        public List<TreeViewAppIG4Item> GetListTreeByType(int type)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 0 && c.IsDeleted == false && c.LanguageId == LanguageId && c.Type == type && c.IsShow == true
                        orderby c.IsLevel, c.Sort
                        select new TreeViewAppIG4Item
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            Count = FDIDB.Categories.Count(m => m.ParentId == c.Id)
                        };
            return query.ToList();
        }
        public List<TreeViewAppIG4Item> GetListTreeByType(int type, List<int> listid, bool isadmin, Guid UserId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 0 && c.IsDeleted == false && c.LanguageId == LanguageId && (type == 0 || c.Type == type) && c.IsShow == true && (isadmin || c.Steps.Any(m => m.UserID == UserId))
                        orderby c.IsLevel, c.Sort
                        select new TreeViewAppIG4Item
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = listid.Any(m => m == c.Id),
                            Count = FDIDB.Categories.Count(m => m.ParentId == c.Id)
                        };
            return query.ToList();
        }
        public List<TreeViewAppIG4Item> GetListTreeByTypeStruct(int type, List<int> listid, bool isadmin, Guid UserId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id == (int)ModuleType.Struct || c.ParentId == (int)ModuleType.Struct && c.IsDeleted == false && c.LanguageId == LanguageId && c.IsShow == true && (isadmin || c.Steps.Any(m => m.UserID == UserId))
                        orderby c.IsLevel, c.Sort
                        select new TreeViewAppIG4Item
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = listid.Any(m => m == c.Id),
                            Count = FDIDB.Categories.Count(m => m.ParentId == c.Id)
                        };
            return query.ToList();
        }
        public List<CategoryAppIG4Item> GetAllListSimpleByParentId(int parentId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 1 && (c.ParentId == parentId) && (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.LanguageId == LanguageId
                        orderby c.Sort
                        select new CategoryAppIG4Item
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
                            UserCreate = c.UserCreate,
                            lstInt = FDIDB.Categories.Where(a=>a.ParentId == c.Id).Select(a=>a.Id).ToList(),
                        };
            return query.ToList();
        }
        
        public List<CategoryAppIG4Item> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 1
                        orderby c.Name
                        where c.IsShow == isShow && c.Name.StartsWith(keyword) && c.LanguageId == LanguageId
                        select new CategoryAppIG4Item
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
        public Category GetById(int categoryId)
        {
            var query = from c in FDIDB.Categories where c.Id == categoryId select c;
            return query.FirstOrDefault();
        }
        public CategoryAppIG4Item GetCategoryById(int categoryId)
        {
            var query = from c in FDIDB.Categories
                        where c.Id == categoryId
                        select new CategoryAppIG4Item
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Step = c.Step,
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
        public List<Category> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Categories where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }
        public void updatenameasi()
        {
            var query = (from c in FDIDB.Categories select c).ToList();
            foreach (var itemCategory in query)
            {
                itemCategory.Slug = FDIUtils.Slug(FDIUtils.NewUnicodeToAscii(itemCategory.Name));
            }
            Save();
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

        public List<CategoryAppIG4Item> GetAllByType(int type)
        {
            var query = from c in FDIDB.Categories
                        where c.Id > 1 && c.Type == type && (c.IsShow.HasValue && c.IsShow.Value) && (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                        orderby c.Sort
                        select new CategoryAppIG4Item
                        {
                            ID = c.Id,
                            Name = c.Name.Trim(),
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            Slug = c.Slug,
                        };
            return query.ToList();
        }
    }
}
