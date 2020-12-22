using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNDocumentFilesAPI : BaseAPI
    {
        public List<DocumentFilesItem> GetListSimple()
        {
            var urlJson = string.Format("{0}DNDocumentFilesFiles/GetListSimple?key={1}", Domain, Keyapi);
            return GetObjJson<List<DocumentFilesItem>>(urlJson);
        }

        public ModelDocumentFilesItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDocumentFiles/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDocumentFilesItem>(urlJson);
        }

        public DocumentFilesItem GetItemsByID(int id)
        {
            var urlJson = string.Format("{0}DNDocumentFiles/GetItemsByID?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DocumentFilesItem>(urlJson);
        }
        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNDocumentFiles/Add?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public List<int> AddList(string json)
        {
            var urlJson = string.Format("{0}DNDocumentFiles/AddList?key={1}&json={2}", Domain, Keyapi, json);
            return GetObjJson<List<int>>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNDocumentFiles/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNDocumentFiles/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
