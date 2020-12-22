using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CityAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelCityItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}City/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelCityItem>(urlJson);
        }
        public CityItem GetCityItem(int id)
        {
            var urlJson = string.Format("{0}City/GetCityItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<CityItem>(urlJson);
        }
        public List<CityItem> GetAll()
        {
            var urlJson = string.Format("{0}City/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<CityItem>>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}City/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}City/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}City/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
