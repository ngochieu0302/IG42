using System;
using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNDocumentAPI : BaseAPI
    {
        public List<DocumentItem> GetListSimple()
        {
            var urlJson = string.Format("{0}DNDocument/GetListSimple?key={1}", Domain, Keyapi);
            return GetObjJson<List<DocumentItem>>(urlJson);
        }

        public ModelDocumentItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDocument/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDocumentItem>(urlJson);
        }
        public ModelDocumentItem ListItemsWarning(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDocument/ListItemsWarning{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDocumentItem>(urlJson);
        }
        public ModelDocumentItem ExcelDocument(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDocument/ExcelDocument{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDocumentItem>(urlJson);
        }

        public DocumentItem GetItemsByID(int id)
        {
            var urlJson = string.Format("{0}DNDocument/GetItemsByID?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DocumentItem>(urlJson);
        }
        public int Add(string json, string url)
        {
            var urlJson = string.Format("{0}DNDocument/Add?key={1}&json={2}&{3}", Domain, Keyapi, json, url);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json, string url)
        {
            var urlJson = string.Format("{0}DNDocument/Update?key={1}&json={2}&{3}", Domain, Keyapi, json, url);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId, string usercan)
        {
            var urlJson = string.Format("{0}DNDocument/Delete?key={1}&lstArrId={2}&usercan={3}", Domain, Keyapi, lstArrId, usercan);
            return GetObjJson<int>(urlJson);
        }
        public int Active(string lstArrId, Guid userId)
        {
            var urlJson = string.Format("{0}DNDocument/Active?key={1}&userId={2}&lstArrId={3}", Domain, Keyapi, userId, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public List<StaticDocumentItem> GeneralListStatic(int year, int areaId)
        {
            var urlJson = string.Format("{0}DNDocument/GeneralListStatic?key={1}&year={2}&areaId={3}", Domain, Keyapi, year, areaId);
            return GetObjJson<List<StaticDocumentItem>>(urlJson);
        }
    }
}
