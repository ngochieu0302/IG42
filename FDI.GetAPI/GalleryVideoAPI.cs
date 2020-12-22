using FDI.Simple;

namespace FDI.GetAPI
{
    public class GalleryVideoAPI : BaseAPI
    {
        public ModelVideoItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}GalleryVideo/ListItems{1}&key={2}", Domain, url, Keyapi);
            return GetObjJson<ModelVideoItem>(urlJson);
        }

        public VideoItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}GalleryVideo/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<VideoItem>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}GalleryVideo/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}GalleryVideo/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }

        public int Show(string lstArrId)
        {
            var urlJson = string.Format("{0}GalleryVideo/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        public int Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}GalleryVideo/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}GalleryVideo/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}