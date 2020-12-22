using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ContactBL:BaseBL
    {
        private readonly ContactDL _dl = new ContactDL();

        public List<SystemConfigItem> SysConfig()
        {
            const string key = "ContactBLSysConfig";
            if (ConfigCache.EnableCache != 1)
                return _dl.SysConfig();
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<SystemConfigItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.SysConfig();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.SysConfig();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
public SystemConfigItem SysConfigItems()
        {
            const string key = "ContactBLSysConfigItems";
            if (ConfigCache.EnableCache != 1)
                return _dl.SysConfigItems();
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SystemConfigItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.SysConfigItems();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.SysConfigItems();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<GoogleMapItem> GetGoogleMap()
        {
            const string key = "ContactBLGetGoogleMap";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetGoogleMap();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<GoogleMapItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetGoogleMap();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetGoogleMap();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<CityItem> GetCity()
        {
            var key = string.Format("ContactBLGetCity");
            if (ConfigCache.EnableCache != 1)
                return _dl.GetCity();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CityItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetCity();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetCity();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}