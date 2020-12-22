using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNDocumentRoomAPI : BaseAPI
    {
        public List<DNDocumentRoomItem> GetListSimple()
        {
            var urlJson = string.Format("{0}DNDocumentRoom/GetListSimple?key={1}", Domain, Keyapi);
            return GetObjJson<List<DNDocumentRoomItem>>(urlJson);
        }

        public List<DNDocumentRoomItem> GetListNotInBedDesk(int agencyid)
        {
            var urlJson = string.Format("{0}DNDocumentRoom/GetListNotInBedDesk?key={1}&agencyId={2}", Domain, Keyapi,agencyid);
            return GetObjJson<List<DNDocumentRoomItem>>(urlJson);
        }

        public ModelDNDocumentRoomItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDocumentRoom/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNDocumentRoomItem>(urlJson);
        }

        public DNDocumentRoomItem GetItemsByID(int id)
        {
            var urlJson = string.Format("{0}DNDocumentRoom/GetItemsByID?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNDocumentRoomItem>(urlJson);
        }

        public List<DNDocumentRoomItem> GetRoomByLevelId(int id)
        {
            var urlJson = string.Format("{0}DNDocumentRoom/GetRoomByLevelId?key={1}&levelId={2}", Domain, Keyapi, id);
            return GetObjJson<List<DNDocumentRoomItem>>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNDocumentRoom/Add?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNDocumentRoom/Update?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNDocumentRoom/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
