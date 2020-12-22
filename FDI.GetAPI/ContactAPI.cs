using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ContactAPI : BaseAPI
    {
        public SystemConfigItem GetSysConfig(string url)
        {
            var urlJson = string.Format("{0}Contact/GetSysConfig?key={1}", url, Keyapi);
            return GetObjJson<SystemConfigItem>(urlJson);
        }

        public List<GoogleMapItem> GetGoogleMap(string url)
        {
            var urlJson = string.Format("{0}Contact/GetGoogleMap?key={1}", url, Keyapi);
            return GetObjJson<List<GoogleMapItem>>(urlJson);
        }
    }
}
