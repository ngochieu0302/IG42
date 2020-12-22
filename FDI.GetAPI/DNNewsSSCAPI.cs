using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNNewsSSCAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<DNNewsSSCItem> GetListSimpleByRequest(int agencyid)
        {
            var urlJson = string.Format("{0}DNNewsSSC/GetListSimpleByRequest?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNNewsSSCItem>>(urlJson);
        }

        public List<DNNewsSSCItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNNewsSSC/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNNewsSSCItem>>(urlJson);
        }       

        public DNNewsSSCItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNNewsSSC/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<DNNewsSSCItem>(urlJson);
        }

        public List<DNNewsSSCItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNNewsSSC/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<DNNewsSSCItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNNewsSSC/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNNewsSSC/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        
    }
}
