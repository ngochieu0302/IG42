using System.Collections.Generic;
using System.Linq;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class VideoBL : BaseBL
    {
        private readonly VideoDL _dl = new VideoDL();
        public List<VideoItem> GetList(int cateId,int page, ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList(cateId,page, ref total);
            var key = string.Format("VideoDL_GetList_{0}-{1}",cateId, page);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<VideoItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetList(cateId, page, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetList(cateId, page, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }

        public List<VideoItem> GetList()
        {           
            var key = "VideoDL_GetList";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList();
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<VideoItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public VideoItem GetById(int id)
        {
            var key = "VideoBL_GetById" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetById(id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (VideoItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public VideoItem GetVideoHot()
        {
            var key = "VideoBL_GetVideoHot";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetVideoHot();
            if (Cache.KeyExistsCache(key))
            {
                var lst = (VideoItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetVideoHot();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetVideoHot();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<VideoItem> GetVideoOther(int id)
        {
            var key = "VideoBL_GetVideoOther" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetVideoOther(id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<VideoItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetVideoOther(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetVideoOther(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}