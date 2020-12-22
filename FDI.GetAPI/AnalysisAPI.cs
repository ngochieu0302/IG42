using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class AnalysisAPI : BaseAPI
    {
        public AnalysisItem GetAllAnalysis(int agencyId, decimal start, decimal end)
        {
            var urlJson = string.Format("{0}Analysis/GetAllAnalysis?key={1}&agencyId={2}&start={3}&end={4}", Domain, Keyapi, agencyId, start, end);
            return GetObjJson<AnalysisItem>(urlJson);
        }

        public List<AnalysisItem> AnalysisByEnterprise(int enterprisesId, decimal start, decimal end)
        {
            var urlJson = string.Format("{0}Analysis/AnalysisByEnterprise?key={1}&agencyId={2}&start={3}&end={4}", Domain, Keyapi, enterprisesId, start, end);
            return GetObjJson<List<AnalysisItem>>(urlJson);
        }

        public List<ProductExportItem> ProductTop(int agencyId, decimal start, decimal end)
        {
            var urlJson = string.Format("{0}Analysis/ProductTop?key={1}&agencyId={2}&start={3}&end={4}", Domain, Keyapi, agencyId, start, end);
            return GetObjJson<List<ProductExportItem>>(urlJson);
        }
    }
}
