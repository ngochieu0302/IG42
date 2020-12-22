using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNExportAPI : BaseAPI
    {
        public List<DNExportItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}DNExport/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNExportItem>>(urlJson);
        }

        public ModelDNExportItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNExport/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNExportItem>(urlJson);
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type)
        {
            var urlJson = string.Format("{0}DNImport/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }

        public DNExportItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}DNExport/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNExportItem>(urlJson);
        }
        public JsonMessage Add(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}DNExport/Add?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}DNExport/Update?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNExport/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
