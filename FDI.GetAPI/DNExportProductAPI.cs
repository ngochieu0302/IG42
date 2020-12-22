using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNExportProductAPI : BaseAPI
    {
        public List<DNExportProductItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}DNExportProduct/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNExportProductItem>>(urlJson);
        }

        public ModelDNExportProductItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNExportProduct/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDNExportProductItem>(urlJson);
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type)
        {
            var urlJson = string.Format("{0}StorageProduct/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }

        public DNExportProductItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}DNExportProduct/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNExportProductItem>(urlJson);
        }
        public int Add(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}DNExportProduct/Add?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}DNExportProduct/Update?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }

        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNExportProduct/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
