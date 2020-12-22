using FDI.Simple;

namespace FDI.GetAPI
{
    public class AdvertisingAPI : BaseAPI
    {
        public ModelAdvertisingItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Advertising/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelAdvertisingItem>(urlJson);
        }

        public AdvertisingItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}Advertising/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<AdvertisingItem>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}Advertising/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}Advertising/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }

        public int Show(string lstArrId)
        {
            var urlJson = string.Format("{0}Advertising/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        public int Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}Advertising/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Advertising/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}