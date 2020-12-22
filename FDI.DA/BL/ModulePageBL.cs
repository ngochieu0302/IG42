using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ModulePageBL :BaseBL
    {
        private readonly ModulePageDA _dl = new ModulePageDA();
      
        public SysPageItem GetBykey(string name)
        {
            var key = string.Format("Module_GetBykey_{0}", name);
            if (ConfigCache.EnableCache != 1)
                return _dl.GetBykey(name);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (SysPageItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetBykey(name);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetBykey(name);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public SysPageItem GetById(int id)
        {
            var key = string.Format("Module_GetById_{0}", id);
            if (ConfigCache.EnableCache != 1)
                return _dl.GetById(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (SysPageItem)Cache.GetCache(key);
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
    }
}
