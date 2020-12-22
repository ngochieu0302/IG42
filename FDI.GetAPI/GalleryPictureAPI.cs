using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class GalleryPictureAPI : BaseAPI
    {        
        public ModelPictureItem ListItems(int agencyId,string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}GalleryPicture/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelPictureItem>(urlJson);
        }        

        public PictureItem GetPictureItem(int id)
        {
            var urlJson = string.Format("{0}GalleryPicture/GetPictureItem?key={1}&id={2}", Domain,Keyapi, id);
            return GetObjJson<PictureItem>(urlJson);
        }

        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}GalleryPicture/Add?key={1}&json={2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}GalleryPicture/Update?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}GalleryPicture/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage ShowHide(string lstArrId, bool showhide)
        {
            var urlJson = string.Format("{0}GalleryPicture/ShowHide?key={1}&lstArrId={2}&showhide={3}", Domain, Keyapi, lstArrId, showhide);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
