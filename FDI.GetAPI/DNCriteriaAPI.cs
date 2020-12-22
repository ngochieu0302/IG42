using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNCriteriaAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelDNCriteriaItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNCriteria/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelDNCriteriaItem>(urlJson);
        }

        public List<DNCriteriaItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNCriteria/GetAll?key={1}&agencyId={2}", _url, Keyapi, agencyid);
            return GetObjJson<List<DNCriteriaItem>>(urlJson);
        }
        public List<TypeCriteriaItem> GetTypeAll()
        {
            var urlJson = string.Format("{0}DNCriteria/GetTypeAll?key={1}", _url, Keyapi);
            return GetObjJson<List<TypeCriteriaItem>>(urlJson);
        }

        public DNCriteriaItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNCriteria/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi, agencyid, id);
            return GetObjJson<DNCriteriaItem>(urlJson);
        }

        public List<DNCriteriaItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNCriteria/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi, agencyid, lstId);
            return GetObjJson<List<DNCriteriaItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNCriteria/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi, agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNCriteria/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi, agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNCriteria/Delete?key={1}&lstArrId={2}", _url, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
