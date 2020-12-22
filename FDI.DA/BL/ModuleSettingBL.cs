using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Utils;

namespace FDI.DA
{
    public class ModuleSettingBL : BaseBL
    {
        private readonly ModuleSettingDl _da = new ModuleSettingDl();
        public ModuleSetting GetKey(int id)
        {
            var data = GetAll().FirstOrDefault(s => s.ModuleId == id);
            return data;
        }

        public IEnumerable<ModuleSetting> GetAll()
        {
            const string key = "Setting_GetAll";
            if (ConfigCache.EnableCache != 1)
                return _da.GetAll();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ModuleSetting>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _da.GetAll();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _da.GetAll();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

    }
}