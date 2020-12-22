using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class GalleryAPI : BaseAPI
    {
        
        public List<AdvertisingItem> GetListAdvertising(string url, int id)
        {
            var urlJson = string.Format("{0}Gallery/GetListAdvertising?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<List<AdvertisingItem>>(urlJson);
        }
    }
}
