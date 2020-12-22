using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ProductionCostAPI : BaseAPI
    {
        private readonly string _url = Domain;

        public ModelProductionCostItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ProductionCost/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelProductionCostItem>(urlJson);
        }

        public List<ProductionCostItem> GetList(int agencyId)
        {
            var urlJson = string.Format("{0}ProductionCost/GetList?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<ProductionCostItem>>(urlJson);
        }
        public ProductionCostItem GetUnitItem(int id)
        {
            var urlJson = string.Format("{0}ProductionCost/GetUnitItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<ProductionCostItem>(urlJson);
        }
        public int AddUnit(string json)
        {
            var urlJson = string.Format("{0}ProductionCost/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int UpdateUnit(string json)
        {
            var urlJson = string.Format("{0}ProductionCost/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
    }
}
