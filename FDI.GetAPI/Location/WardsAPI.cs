using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class WardsAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelWardsItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Wards/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelWardsItem>(urlJson);
        }
        public WardsItem GetWardsItem(int id)
        {
            var urlJson = string.Format("{0}Wards/GetWardsItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<WardsItem>(urlJson);
        }
        public List<WardsItem> GetAll()
        {
            var urlJson = string.Format("{0}Wards/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<WardsItem>>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}Wards/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}Wards/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Wards/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }

}
