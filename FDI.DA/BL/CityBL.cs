using System.Collections.Generic;
using System.Xml;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class CityBL:BaseBL
    {
        private  readonly CityDl Dl = new CityDl();

        public XmlDocument GetGetWertherByLocationId(string locationId)
        {
            var xdoc = new XmlDocument();
            var key = string.Format("GetGetWertherByLocationId{0}", locationId);
            var data = (XmlDocument)Cache.GetCache(key);
            if (data != null)
                return data;
            xdoc.Load("http://weather.yahooapis.com/forecastrss?w=" + locationId + "&u=c");
            Cache.Set(key, xdoc, ConfigCache.TimeExpire);
            return xdoc;
        }

        public  CityItem GetCityItemById(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return Dl.GetCityItemById(id);
            var key = "CityBLGetCityItemById_" + id;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (CityItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = Dl.GetCityItemById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = Dl.GetCityItemById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public  CityItem GetDistrictItemByCityId(int cityID)
        {
            var key = "CityBLGetDistrictItemByCityId" + cityID;
            if (ConfigCache.EnableCache != 1)
                return Dl.GetDistrictItemByCityId(cityID);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (CityItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = Dl.GetDistrictItemByCityId(cityID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = Dl.GetDistrictItemByCityId(cityID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public  DistrictItem GetListGoogleMapByDistrictID(int districtID)
        {
            var key = "CityBLGetListGoogleMapByDistrictID" + districtID;
            if (ConfigCache.EnableCache != 1)
                return Dl.GetListGoogleMapByDistrictID(districtID);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (DistrictItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = Dl.GetListGoogleMapByDistrictID(districtID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = Dl.GetListGoogleMapByDistrictID(districtID);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }


        public  List<CityItem> GetAllListByGoogleMap()
        {
            const string key = "CityBLGetAllListByGoogleMap";
            if (ConfigCache.EnableCache != 1)
                return Dl.GetAllListByGoogleMap();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CityItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = Dl.GetAllListByGoogleMap();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = Dl.GetAllListByGoogleMap();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public  List<CityItem> GetAllListByWeather()
        {
            const string key = "CityBLGetAllListByWeather";
            if (ConfigCache.EnableCache != 1)
                return Dl.GetAllListByWeather();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CityItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = Dl.GetAllListByWeather();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = Dl.GetAllListByWeather();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}