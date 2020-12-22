using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNCabinetDocumentAPI : BaseAPI
    {
        public List<DNCabinetDocumentItem> GetListSimple()
        {
            var urlJson = string.Format("{0}DNCabinetDocument/GetListSimple?key={1}", Domain, Keyapi);
            return GetObjJson<List<DNCabinetDocumentItem>>(urlJson);
        }

        public List<DNCabinetDocumentItem> GetListNotInBedDesk(int agencyid)
        {
            var urlJson = string.Format("{0}DNCabinetDocument/GetListNotInBedDesk?key={1}&agencyId={2}", Domain, Keyapi,agencyid);
            return GetObjJson<List<DNCabinetDocumentItem>>(urlJson);
        }

        public ModelDNCabinetDocumentItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNCabinetDocument/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNCabinetDocumentItem>(urlJson);
        }

        public DNCabinetDocumentItem GetItemsByID(int id)
        {
            var urlJson = string.Format("{0}DNCabinetDocument/GetItemsByID?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNCabinetDocumentItem>(urlJson);
        }

        public List<DNCabinetDocumentItem> GetItemsByRoomID(int roomId)
        {
            var urlJson = string.Format("{0}DNCabinetDocument/GetItemsByRoomID?key={1}&roomId={2}", Domain, Keyapi, roomId);
            return GetObjJson<List<DNCabinetDocumentItem>>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNCabinetDocument/Add?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNCabinetDocument/Update?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNCabinetDocument/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
