using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CustomerGroupAPI : BaseAPI
    {
        private readonly string _url = Domain;

        public ModelCustomerGroupItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CustomerGroup/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelCustomerGroupItem>(urlJson);
        }

        public List<CustomerGroupItem> GetList(int agencyid)
        {
            var urlJson = string.Format("{0}CustomerGroup/GetList?key={1}&agencyId={2}", _url, Keyapi, agencyid);
            return GetObjJson<List<CustomerGroupItem>>(urlJson);
        }
        public CustomerGroupItem GetCustomerGroupItem(int id)
        {
            var urlJson = string.Format("{0}CustomerGroup/GetCustomerGroupItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<CustomerGroupItem>(urlJson);
        }
        public JsonMessage Add(string json, int agencyid)
        {
            var urlJson = string.Format("{0}CustomerGroup/Add?key={1}&{2}&agencyId={3}", Domain, Keyapi, json, agencyid);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, int agencyid)
        {
            var urlJson = string.Format("{0}CustomerGroup/Update?key={1}&{2}&agencyId={3}", Domain, Keyapi, json, agencyid);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}CustomerGroup/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
