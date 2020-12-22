using System.Collections.Generic;
using System.Xml;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class HomeNetworkDL : BaseBL
    {
        private readonly HomeNetworkDA _da = new HomeNetworkDA();

        public List<HomeNetworkItem> GetList()
        {
            const string key = "CityBLGetList";
            if (ConfigCache.EnableCache != 1)
                return _da.GetList();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<HomeNetworkItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _da.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _da.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}