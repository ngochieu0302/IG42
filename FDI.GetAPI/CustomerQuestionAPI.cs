using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CustomerQuestionAPI:BaseAPI
    {
        public ModelCustomerQuestionItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CustomerQuestion/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelCustomerQuestionItem>(urlJson);
        }
        public CustomerQuestionItem GetCustomerQuestionItem(int id)
        {
            var urlJson = string.Format("{0}CustomerQuestion/GetCustomerQuestionItem?key={1}&Id={2}", Domain, Keyapi, id);
            return GetObjJson<CustomerQuestionItem>(urlJson);
        }
        public JsonMessage Add(string json, int agencyid)
        {
            var urlJson = string.Format("{0}CustomerQuestion/Add?key={1}&{2}&agencyId={3}", Domain, Keyapi, json, agencyid);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}CustomerQuestion/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}CustomerQuestion/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
