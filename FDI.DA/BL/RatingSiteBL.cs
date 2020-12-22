using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using FDI.DA.DL;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.BL
{
    public class RatingSiteBL : BaseBL
    {
        private readonly RatingSiteDL _dl = new RatingSiteDL();
        public List<RatingSiteItem> GetListHome()
        {
            var key = "RatingSiteBLGetListHome";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListHome();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<RatingSiteItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListHome();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListHome();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<RatingSiteItem> GetListbyNewsId(int newsId)
        {
            var key = "RatingSiteBLGetListbyNewsId-" + newsId;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListbyNewsId(newsId);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<RatingSiteItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListbyNewsId(newsId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListbyNewsId(newsId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<RatingSiteItem> GetListbyProductId(int productId)
        {
            var key = "RatingSiteBLGetListbyProductId-" + productId;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListbyProductId(productId);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<RatingSiteItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListbyProductId(productId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListbyProductId(productId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}
