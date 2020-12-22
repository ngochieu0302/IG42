using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class AreaAPI:BaseAPI
    {
        private readonly string _url = Domain;
        public ModelAreaItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Area/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelAreaItem>(urlJson);
        }
        public ModelAreaStaticItem ListItemsStatic(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Area/ListItemsStatic{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelAreaStaticItem>(urlJson);
        }
        public AreaItem GetAreaItem(int id)
        {
            var urlJson = string.Format("{0}Area/GetAreaItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<AreaItem>(urlJson);
        }
        public List<AreaItem> GetAll()
        {
            var urlJson = string.Format("{0}Area/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<AreaItem>>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}Area/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}Area/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Area/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }

}
