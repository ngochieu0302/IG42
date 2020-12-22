using System.Collections.Generic;
using System.Linq;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class HtmlSettingBL :BaseBL
    {
        private readonly HtmlSettingDL _dl = new HtmlSettingDL();

        public List<HtmlMapItem> GetList()
        {
            var key = string.Format("Html_MapGetList");
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList();

            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<HtmlMapItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<HtmlMapItem> GetByKey(int idModule)
        {
            var model = GetList().Where(c => c.IdModule == idModule);
            return model.ToList();
        }
    }
}