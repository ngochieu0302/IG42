using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNUnitAPI : BaseAPI
    {
        private readonly string _url = Domain;

        public ModelDNUnitItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Unit/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelDNUnitItem>(urlJson);
        }

        public List<DNUnitItem> GetListUnit(int agencyid)
        {
            var urlJson = string.Format("{0}Unit/GetList?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNUnitItem>>(urlJson);
        }
        public List<DNUnitItem> GetAllList()
        {
            var urlJson = string.Format("{0}Unit/GetAllList?key={1}", _url, Keyapi);
            return GetObjJson<List<DNUnitItem>>(urlJson);
        }
        public DNUnitItem GetUnitItem(int id)
        {
            var urlJson = string.Format("{0}Unit/GetUnitItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<DNUnitItem>(urlJson);
        }

        public int CheckByName(string name, int id, int agencyId)
        {
            var urlJson = string.Format("{0}Unit/CheckByName?key={1}&name={2}&id={3}&agencyId={4}", Domain, Keyapi, name, id, agencyId);
            return GetObjJson<int>(urlJson);
        }

        public int AddUnit(string json)
        {
            var urlJson = string.Format("{0}Unit/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int UpdateUnit(string json)
        {
            var urlJson = string.Format("{0}Unit/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
	    public int DeleteUnit(string json)
	    {
		    var urlJson = string.Format("{0}Unit/Delete?key={1}&{2}", Domain, Keyapi, json);
		    return GetObjJson<int>(urlJson);
	    }
	}
}
