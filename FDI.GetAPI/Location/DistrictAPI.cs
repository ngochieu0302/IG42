using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DistrictAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelDistrictItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}District/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelDistrictItem>(urlJson);
        }
        public DistrictItem GetDistrictItem(int id)
        {
            var urlJson = string.Format("{0}District/GetDistrictItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<DistrictItem>(urlJson);
        }
        public List<DistrictItem> GetAll()
        {
            var urlJson = string.Format("{0}District/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<DistrictItem>>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}District/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}District/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}District/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
