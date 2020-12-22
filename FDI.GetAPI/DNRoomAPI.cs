using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNRoomAPI : BaseAPI
    {
        public DNRoomItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}DNRoom/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNRoomItem>(urlJson);
        }

        public ModelDNRoomItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNRoom/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNRoomItem>(urlJson);
        }
        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNRoom/Add?key={1}&agencyId={2}&{3}", Domain, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNRoom/Update?key={1}&agencyId={2}&{3}", Domain, Keyapi, agencyid, json);
            return GetObjJson<int>(urlJson);
        }
        
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNRoom/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}