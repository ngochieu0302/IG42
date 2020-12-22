using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class LinkWebsiteAPI : BaseAPI
    {
       public WebsiteItem GetById(string url, int id)
        {
            var urlJson = string.Format("{0}LinkWebsite/GetById?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<WebsiteItem>(urlJson);
        }

        public List<WebsiteItem> GetAll(string url)
        {
            var urlJson = string.Format("{0}LinkWebsite/GetAll?key={1}", url, Keyapi);
            return GetObjJson<List<WebsiteItem>>(urlJson);
        }
    }
}
