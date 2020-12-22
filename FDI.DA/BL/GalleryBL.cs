using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class GalleryBL :BaseBL
    {
        readonly GalleryDL _dl = new GalleryDL();

        public List<AdvertisingItem> GetListAdvertising(int id)
        {
            var key = "Banner_GetListAdvertising_" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListAdvertising(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<AdvertisingItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListAdvertising(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListAdvertising(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public AdvertisingItem GetAdvertisingItem(int id)
        {
            var key = "Banner_AdvertisingItem_" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetAdvertisingItem(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (AdvertisingItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetAdvertisingItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetAdvertisingItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<GalleryPictureItem> GetListPictureByCateId(int id)
        {
            var key = "GetListPictureByCateId_" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListPictureByCateId(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<GalleryPictureItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListPictureByCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListPictureByCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<AdvertisingPositionItem> GetAdvertising()
        {
            const string key = "GalleryDL_ GetAdvertising";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetAdvertising();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<AdvertisingPositionItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetAdvertising();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetAdvertising();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}