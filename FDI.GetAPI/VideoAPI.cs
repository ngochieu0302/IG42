using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class VideoAPI : BaseAPI
    {
       public List<VideoItem> GetList(string url)
        {
            var urlJson = string.Format("{0}Video/GetList?key={1}", url, Keyapi);
            return GetObjJson<List<VideoItem>>(urlJson);
        }
    }
}
