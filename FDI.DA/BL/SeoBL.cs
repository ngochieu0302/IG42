using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class SeoBL : BaseBL
    {
        private readonly SeoDL _dl = new SeoDL();
        public SEOItem GetSeoTag(string url)
        {

            if (ConfigCache.EnableCache != 1)
                return _dl.GetSeoTag(url);
            var key = string.Format("SeoHomeWorks_{0}", url);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SEOItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetSeoTag(url);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetSeoTag(url);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
       
        public SEOItem GetSeoNews(string url)
        {

            if (ConfigCache.EnableCache != 1)
                return _dl.GetSeoNews(url);
            var key = string.Format("SeoNews_{0}", url);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SEOItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetSeoNews(url);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetSeoNews(url);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public SEOItem GetSeoProduct(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetSeoProduct(id);
            var key = string.Format("Seo_GetSeoProduct{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SEOItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetSeoProduct(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetSeoProduct(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public SEOItem GetSeoPartner(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetSeoPartner(id);
            var key = string.Format("Seo_GetSeoPartner{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SEOItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetSeoPartner(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetSeoPartner(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public SEOItem GetSeoCategory(string id, int type)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetSeoCategory(id, type);
            var key = string.Format("SeoCate_{0}_{1}", id, type);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SEOItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetSeoCategory(id, type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetSeoCategory(id, type);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public SEOItem GetSeoPage(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetSeoPage(id);
            var key = string.Format("GetSeoPage_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SEOItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetSeoPage(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetSeoPage(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}