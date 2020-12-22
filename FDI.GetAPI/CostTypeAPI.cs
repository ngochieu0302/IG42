using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class CostTypeAPI : BaseAPI
    {
        public ModelCostType GetListSimple(string url, int agencyId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CostType/GetListSimple{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelCostType>(urlJson);
        }
        public List<CostTypeItem> GetList(int agencyId, int type)
        {
            var urlJson = string.Format("{0}CostType/GetList?key={1}&agencyId={2}&type={3}", Domain, Keyapi, agencyId, type);
            return GetObjJson<List<CostTypeItem>>(urlJson);
        }
        public CostTypeItem GetCostTypeItem(int id)
        {
            var urlJson = string.Format("{0}CostType/GetCostTypeItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CostTypeItem>(urlJson);
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type)
        {
            var urlJson = string.Format("{0}CostType/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public int Add(string json)
        {
            var urlJson = string.Format("{0}CostType/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}CostType/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}CostType/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
