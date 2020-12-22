using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;
namespace FDI.GetAPI
{
    public class SetupProductionAPI : BaseAPI
    {
        public ModelSetupProductionItem GetListSimple(string url, int agencyId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}SetupProduction/GetListSimple{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelSetupProductionItem>(urlJson);
        }
        public List<SetupProductionItem> GetList(int agencyId)
        {
            var urlJson = string.Format("{0}SetupProduction/GetList?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<SetupProductionItem>>(urlJson);
        }
        public SetupProductionItem GetSetupProductionItem(int id)
        {
            var urlJson = string.Format("{0}SetupProduction/GetSetupProductionItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<SetupProductionItem>(urlJson);
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type)
        {
            var urlJson = string.Format("{0}SetupProduction/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}SetupProduction/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}SetupProduction/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}SetupProduction/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
