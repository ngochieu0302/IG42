using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.DA.DL;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.BL
{
    public class SimvipBL : BaseBL
    {
        private readonly SimvipDL _dl = new SimvipDL();
        public List<SimvipItem> GetListHome()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListHome();
            var key = string.Format("Simvip_GetListHome");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<SimvipItem>)Cache.GetCache(key);
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
        public SimvipItem GetByid(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetByid(id);
            var key = string.Format("Simvip_GetByid");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (SimvipItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetByid(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetByid(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }

}
