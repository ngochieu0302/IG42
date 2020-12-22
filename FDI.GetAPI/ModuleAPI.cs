using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ModuleAPI : BaseAPI
    {
        public ModuleSettingItem GetKey(string url,int id)
        {
            var urlJson = string.Format("{0}Module/GetKey?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<ModuleSettingItem>(urlJson);
        }
        public List<RouterItem> GetRouter()
        {
            var urlJson = string.Format("{0}DNModule/GetRouter?key={1}", Domain, Keyapi);
            return GetObjJson<List<RouterItem>>(urlJson);
        }
        public List<ModuleSettingItem> GetAll(string url)
        {
            var urlJson = string.Format("{0}Module/GetAll?key={1}", url, Keyapi);
            return GetObjJson<List<ModuleSettingItem>>(urlJson);
        }

        public SysPageItem GetBykey(string url, string name)
        {
            var urlJson = string.Format("{0}Module/GetBykey?key={1}&name={2}", url, Keyapi, name);
            return GetObjJson<SysPageItem>(urlJson);
        }

        public SysPageItem GetById(string url, int id)
        {
            var urlJson = string.Format("{0}Module/GetById?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<SysPageItem>(urlJson);
        }
    }
}
