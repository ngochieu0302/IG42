using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CompanyAPI:BaseAPI
    {
        public ModelCompanyItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Company/ListItems{1}&key={2}", Domain, url, Keyapi);
            return GetObjJson<ModelCompanyItem>(urlJson);
        }
        public CompanyItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}Company/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CompanyItem>(urlJson);
        }
        public List<CompanyItem> GetAll()
        {
            var urlJson = string.Format("{0}Company/GetAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<CompanyItem>>(urlJson);
        }
        public JsonMessage Add(string json,Guid userId)
        {
            var urlJson = string.Format("{0}Company/Add?key={1}&userId={2}&{3}", Domain, Keyapi, userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json,Guid userId)
        {
            var urlJson = string.Format("{0}Company/Update?key={1}&userId={2}&{3}", Domain, Keyapi,userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Company/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
