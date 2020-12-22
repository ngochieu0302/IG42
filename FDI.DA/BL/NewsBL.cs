using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class NewsBL : BaseBL
    {
        private readonly NewsDL _dl = new NewsDL();

        public List<NewsItem> GetList(int homeId = 0, int cateId = 0)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList(homeId, cateId);
            var key = "News_GetList_" + homeId + "_" + cateId;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetList(homeId, cateId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetList(homeId, cateId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<NewsItem> GetListPDF(int slug, int page, int rowPage, ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListPDF(slug, page, rowPage, ref total);
            var key = string.Format("News_GetListPDF_{0}_{1}", page, slug);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetListPDF(slug, page, rowPage, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetListPDF(slug, page, rowPage, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }
        public List<NewsItem> GetListHot()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListHot();
            var key = "News_GetListHot_";
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListHot();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListHot();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<NewsItem> GetList(int slug, int page, int rowPage, ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList(slug, page, rowPage, ref total);
            var key = string.Format("News_GetList_{0}_{1}", page, slug);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetList(slug, page, rowPage, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetList(slug, page, rowPage, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }

        public List<NewsItem> GetNewByTag(string slug, int cateId, int page, int rowPage, ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetNewByTag(slug, cateId, page, rowPage, ref total);
            var key = string.Format("News_GetNewTags_{0}_{1}", slug, page);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetNewByTag(slug, cateId, page, rowPage, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetNewByTag(slug, cateId, page, rowPage, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data ?? new List<NewsItem>();
        }

        public string GetName(int id)
        {
            var key = "NewsBLGetName_" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetName(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (string)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetName(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetName(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<NewsItem> GetNewKeyword(string keyword, int page, int rowPage, ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetNewKeyword(keyword, page, rowPage, ref total);
            var key = string.Format("News_GetNewKeyword_{0}_{1}", keyword, page);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetNewKeyword(keyword, page, rowPage, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetNewKeyword(keyword, page, rowPage, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }

        public List<NewsItem> GetListByCateId(int id, int take = 0)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListByCateId(id, take);
            var key = string.Format("News_GetListByCateId_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListByCateId(id, take);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListByCateId(id, take);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<NewsItem> GetListByDate(int take = 0)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListByDate(take);
            const string key = "News_GetListByDate";
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListByDate(take);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListByDate(take);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        
        public List<CategoryItem> GetListCateId(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListCateId(id);
            var key = string.Format("News_GetListCateId_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CategoryItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<NewsItem> GetListOther(int id, int ortherId)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListOther(id, ortherId);
            var key = string.Format("NewsBLGetListOther_{0}_{1}", id, ortherId);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<NewsItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListOther(id, ortherId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListOther(id, ortherId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public NewsItem GetNewsId(string slug, int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetNewsId(id, slug);
            var key = "News_GetNewsId_" + slug;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (NewsItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetNewsId(id, slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetNewsId(id, slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public NewsItem GetNewsTitleAssci(string slug)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetNewsTitleAssci(slug);
            var key = "News_GetNewsTitleAssci_" + slug;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (NewsItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetNewsTitleAssci(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetNewsTitleAssci(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public NewsItem GetNewsItem(string name)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetNewsItem(name);
            var key = "News_GetNewsItem_" + name;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (NewsItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetNewsItem(name);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetNewsItem(name);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}