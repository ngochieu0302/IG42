using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class GoogleMapBL :BaseBL
    {
        private readonly GoogleMapDl _dl = new GoogleMapDl();
        public List<GoogleMapItem> GetGoogleMapsItemByDistrictID(int districtID)
        {
            var key = "GoogleMapBLGetGoogleMapsItemByDistrictID" + districtID;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetGoogleMapsItemByDistrictID(districtID);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<GoogleMapItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetGoogleMapsItemByDistrictID(districtID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetGoogleMapsItemByDistrictID(districtID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<GoogleMapItem> GetGoogleMapsItemByCityId(int cityID)
        {
            var key = "GoogleMapBLGetGoogleMapsItemByCityId" + cityID;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetGoogleMapsItemByCityId(cityID);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<GoogleMapItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetGoogleMapsItemByCityId(cityID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetGoogleMapsItemByCityId(cityID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<GoogleMapItem> GetAllListSimple()
        {
            const string key = "GoogleMapBL_GetAllListSimple";
            if (ConfigCache.EnableCache != 1)
                return _dl.GetAllListSimple();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<GoogleMapItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetAllListSimple();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetAllListSimple();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}