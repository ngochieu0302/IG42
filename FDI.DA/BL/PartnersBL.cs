using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class PartnersBL : BaseBL
    {
        private readonly PartnersDL _dl = new PartnersDL();

        public List<PartnerItem> GetList(int page, ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList(page, ref total);
            var key = "Partner_GetListGallery_" + page;
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<PartnerItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetList(page, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetList(page, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }

        public PartnerItem GetById(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetById(id);
            var key = "Partner_GetNewsId_" + id;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (PartnerItem)Cache.GetCache(key);
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

        public List<PartnerItem> GetPartnerOther(int id)
        {
            var key = "VideoBL_GetPartnerOther_" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetPartnerOther(id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<PartnerItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetPartnerOther(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetPartnerOther(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<PartnerItem> GetPartnerListnews()
        {
            var key = "PartnerBL_GetPartnerListnews";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetPartnerListnews();
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<PartnerItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetPartnerListnews();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetPartnerListnews();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<PartnerItem> GetListPartner(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListPartner(id);
            var key = "Partner_GetListPartner_" + id;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<PartnerItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListPartner(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListPartner(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<PartnerItem> GetListHot(int take)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListHot(take);
            var key = "Partner_GetListHot_" + take;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<PartnerItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListHot(take);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListHot(take);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}