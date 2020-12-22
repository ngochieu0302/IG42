using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNDocumentLevelAPI : BaseAPI
    {
        public List<DNDocumentLevelItem> GetListSimple()
        {
            var urlJson = string.Format("{0}DNDocumentLevel/GetListSimple?key={1}", Domain, Keyapi);
            return GetObjJson<List<DNDocumentLevelItem>>(urlJson);
        }

        public List<DNDocumentLevelItem> GetListNotInBedDesk(int agencyid)
        {
            var urlJson = string.Format("{0}DNDocumentLevel/GetListNotInBedDesk?key={1}&agencyId={2}", Domain, Keyapi,agencyid);
            return GetObjJson<List<DNDocumentLevelItem>>(urlJson);
        }

        public List<DNDocumentLevelItem> GetListItemByParentID(int agencyid)
        {
            var urlJson = string.Format("{0}DNDocumentLevel/GetListItemByParentID?key={1}&agencyId={2}", Domain, Keyapi,agencyid);
            return GetObjJson<List<DNDocumentLevelItem>>(urlJson);
        }

        public ModelDNDocumentLevelItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDocumentLevel/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNDocumentLevelItem>(urlJson);
        }

        public DNDocumentLevelItem GetItemsByID(int id)
        {
            var urlJson = string.Format("{0}DNDocumentLevel/GetItemsByID?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNDocumentLevelItem>(urlJson);
        }
        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNDocumentLevel/Add?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNDocumentLevel/Update?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNDocumentLevel/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
