using System.Collections.Generic;
using FDI.Utils;
using FDI.Simple;
using System.Linq;

namespace FDI.DA
{
    public class MenuDL : BaseDA
    {
        public List<MenusItem> GetListMenus(int groupId)
        {
            var query = from c in FDIDB.Menus
                        where c.IsShow == true && c.IsDeleted == false && c.GroupId == groupId && c.AgencyID == Utility.AgencyId && c.LanguageId == LanguageId
                        orderby c.Sort
                        select new MenusItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Url = c.Url,
                            Sort = c.Sort,
                            CateId = c.CateId,
                            ParentId = c.ParentId,
                            GroupId = c.GroupId,
                            IsNewTab = c.IsNewTab,
                            Icon = c.Icon,
                            Icolor = c.Icolor
                        };
            return query.ToList();
        }

        public List<MenusItem> GetListMenusById(int id)
        {
            var query = from c in FDIDB.Menus
                        where c.LanguageId == LanguageId && c.IsShow == true
                        && c.IsDeleted == false && c.ParentId == id
                        orderby c.Sort
                        select new MenusItem
                        {
                            ID = c.Id,
                            Url = c.Url,
                            Name = c.Name
                        };
            return query.ToList();
        }

        public CategoryItem CategoryItem(int id)
        {
            var query = from c in FDIDB.Categories
                        where c.Id == id                        
                        select new CategoryItem
                        {        
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description
                        };
            return query.FirstOrDefault();
        }
        public CategoryItem CategoryItem(string slug)
        {
            var query = from c in FDIDB.Categories
                        where c.Slug == slug
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Description = c.Description
                        };
            return query.FirstOrDefault();
        }

       

        public List<CategoryItem> GetCategories(int id)
        {
            var query = from c in FDIDB.Categories
                        where c.IsDeleted == false/* && c.IsShow == true*/ && (c.Id == id || c.ParentId == id)
                        orderby c.Sort
                        select new CategoryItem
                        {
                            ID = c.Id,
                            ParentId = c.ParentId,
                            Name = c.Name,
                            Description = c.Description,
                            Slug = c.Slug,
                        };
            return query.ToList();
        }

        public List<MenuGroupsItem> GetMenusGroup()
        {
            var query = from c in FDIDB.MenuGroups
                        where c.IsShow == true && c.AgencyID == Utility.AgencyId
                        orderby c.Sort
                        select new MenuGroupsItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Sort = c.Sort,
                            IsShow = c.IsShow,
                        };
            return query.ToList();
        }

        public List<CategoryItem> GetCategorieType(int type)
        {
            var query = from c in FDIDB.Categories
                        where c.IsDeleted == false && c.IsShow == true && c.Type == type
                        orderby c.Sort
                        select new CategoryItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId
                        };
            return query.ToList();
        }

        public List<CategoryItem> GetChildCategories(int type)
        {
            var query = from c in FDIDB.Category_GetList(LanguageId)
                        orderby c.Id descending 
                        select new CategoryItem
                        {
                            ID = c.Id,
                            ParentId = c.ParentId,
                            Name = c.Name,
                            Slug = c.NameRoot,
                            Type = c.Type,
                            IsLevel = c.IsLevel,
                        };
            if (type != 0)
            {
                query = query.Where(c => c.Type == type);
            }
            return query.ToList();
        }
    }
}