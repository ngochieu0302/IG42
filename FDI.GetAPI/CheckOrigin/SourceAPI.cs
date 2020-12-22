using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI.CheckOrigin
{
    public class SourceAPI:BaseAPI
    {
        public ModelSourceItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Source/ListItems{1}&key={2}", Domain, url, Keyapi);
            return GetObjJson<ModelSourceItem>(urlJson);
        }
        public SourceItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}Source/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<SourceItem>(urlJson);
        }
        public List<SourceItem> GetList()
        {
            var urlJson = string.Format("{0}Source/GetList?key={1}", Domain, Keyapi);
            return GetObjJson<List<SourceItem>>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}Source/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}Source/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Source/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
