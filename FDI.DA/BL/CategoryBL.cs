using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;
namespace FDI.DA
{
    public class CategoryBL:BaseBL
    {
        private readonly CategoryDL _dl = new CategoryDL();
        
        public CategoryItem GetBySlug(string slug)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetBySlug(slug);
            var key = string.Format("Category_GetBySlug-{0}", slug);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (CategoryItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetBySlug(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetBySlug(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<CategoryItem> GetlistCate()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetlistCate();
            var key = string.Format("Category_GetlistCate");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetlistCate();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetlistCate();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<CategoryItem> GetlistCatebyParent(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetlistCatebyParent(id);
            var key = string.Format("Category_GetlistCatebyParent-{0}",id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetlistCatebyParent(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetlistCatebyParent(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<CategoryItem> GetlistCateById(int id,int type)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetlistCateById(id,type);
            var key = string.Format("Category_GetlistCateById-{0}-{1}",id,type);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetlistCateById(id, type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetlistCateById(id, type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<CategoryItem> GetlistCateShowhome(int type)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetlistCateShowhome(type);
            var key = string.Format("Category_GetlistCateShowhome-{0}", type);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetlistCateShowhome(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetlistCateShowhome(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public CategoryItem GetCateHot(int type)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetCateHot(type);
            var key = string.Format("Category_GetCateHot-{0}", type);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (CategoryItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetCateHot(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetCateHot(type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public CategoryItem GetByid(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetByid(id);
            var key = string.Format("Category_GetByid-{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (CategoryItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetByid(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetByid(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

    }
}