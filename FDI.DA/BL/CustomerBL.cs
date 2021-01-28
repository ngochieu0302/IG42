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
        public List<CustomerAppIG4Item> ListByMap(int km, float la, float lo)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.ListByMap(km, la, lo);
            var key = string.Format("CustomerBL_ListByMap_{0}_{1}_{2}", km, la, lo);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<CustomerAppIG4Item>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.ListByMap(km, la, lo);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.ListByMap(km, la, lo);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}