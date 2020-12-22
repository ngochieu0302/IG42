using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class MenuBL : BaseBL
    {
        private readonly MenuDL _dl = new MenuDL();
        public List<MenusItem> GetListMenus(int groupId)
        {
            var key = "MenuBLGetListMenus_" + groupId;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListMenus(groupId);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<MenusItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListMenus(groupId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListMenus(groupId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<MenusItem> GetListMenusById(int id)
        {
            var key = "MenuBLGetListMenusById_" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListMenusById(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<MenusItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListMenusById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListMenusById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public CategoryItem CategoryItem(int id)
        {
            var key = "MenuBLCategoryItem_" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.CategoryItem(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (CategoryItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.CategoryItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.CategoryItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public CategoryItem CategoryItem(string slug)
        {
            var key = "MenuBLCategoryItem_" + slug;
            if (ConfigCache.EnableCache != 1)
                return _dl.CategoryItem(slug);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (CategoryItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.CategoryItem(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.CategoryItem(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        
        public List<CategoryItem> GetCategories(int id)
        {
            var key = "MenuBL_GetCategories_"+ id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetCategories(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetCategories(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetCategories(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<CategoryItem> GetCategorieType(int type)
        {
            var key = "MenuBL_GetCategorieType_" + type;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetCategorieType(type);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetCategorieType(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetCategorieType(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<CategoryItem> GetChildCategories(int type = 0)
        {
            var key = string.Format("MenuBL_GetChildCategories_{0}", type);
            if (ConfigCache.EnableCache != 1)
                return _dl.GetChildCategories(type);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetChildCategories(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetChildCategories(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<MenuGroupsItem> GetMenusGroup()
        {
            const string key = "MenuBL_GetMenusGroup";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetMenusGroup();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<MenuGroupsItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetMenusGroup();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetMenusGroup();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}