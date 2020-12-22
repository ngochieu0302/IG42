using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class HtmlSettingAPI : BaseAPI
    {
        public List<HtmlMapItem> GetByModuleId(string url, int idModule)
        {
            var urlJson = string.Format("{0}HtmlSetting/GetByModuleId?key={1}&idModule={2}", url, Keyapi, idModule);
            return GetObjJson<List<HtmlMapItem>>(urlJson);
        }

        public List<HtmlMapItem> GetAll(string url)
        {
            var urlJson = string.Format("{0}HtmlSetting/GetAll?key={1}", url, Keyapi);
            return GetObjJson<List<HtmlMapItem>>(urlJson);
        }
    }
}
