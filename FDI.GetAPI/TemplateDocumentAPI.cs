using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class TemplateDocumentAPI:BaseAPI
    {
        public ModelTemplateDocumentItem GetListSimpleByRequest(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}TemplateDocument/GetListSimpleByRequest{1}&key={2}", Domain, url, Keyapi);
            return GetObjJson<ModelTemplateDocumentItem>(urlJson);
        }
        public List<TemplateDocumentItem> GetList(int id)
        {
            var urlJson = string.Format("{0}TemplateDocument/GetList?id={1}&key={2}", Domain, id, Keyapi);
            return GetObjJson<List<TemplateDocumentItem>>(urlJson);
        }

        public TemplateDocumentItem GetTemplateDocItem(int id)
        {
            var urlJson = string.Format("{0}TemplateDocument/GetTemplateDocItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<TemplateDocumentItem>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}TemplateDocument/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public async Task<JsonMessage> Update(TemplateDocument request)
        {
            var urlJson = string.Format("{0}TemplateDocument/Update?key={1}", Domain, Keyapi);
            return await PostDataAsync<JsonMessage>(urlJson, request);
        }

        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}TemplateDocument/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public TemplateDocumentItem GetItemByIDAndIDDoc(int id, int iddoc)
        {
            var urlJson = string.Format("{0}TemplateDocument/GetItemByIDAndIDDoc?key={1}&id={2}&iddoc={3}", Domain, Keyapi, id, iddoc);
            return GetObjJson<TemplateDocumentItem>(urlJson);
        }
    }
}
