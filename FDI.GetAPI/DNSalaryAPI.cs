using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNSalaryAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelDNSalaryItem GetListSimpleByRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNSalary/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelDNSalaryItem>(urlJson);
        }

        public List<DNSalaryItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNSalary/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNSalaryItem>>(urlJson);
        }

        public DNSalaryItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNSalary/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<DNSalaryItem>(urlJson);
        }

        public List<DNSalaryItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNSalary/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<DNSalaryItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNSalary/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNSalary/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        
    }
}
