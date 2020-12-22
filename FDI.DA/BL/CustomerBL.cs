using FDI.Simple;
using FDI.Utils;
using System.Collections.Generic;

namespace FDI.DA
{
    public class CustomerBL : BaseBL
    {
        private readonly CustomerDL _dl = new CustomerDL();
        public CustomerItem GetByid(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetByid(id);
            var key = string.Format("CustomerBL_GetByid_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (CustomerItem)Cache.GetCache(key);
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