using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Simple.QLCN;

namespace FDI.GetAPI.QLCN
{
    public class NguyenlieuCNAPI:BaseAPI
    {
        private readonly string _url = Domain;
        public ModelNguyenlieuCNItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}NguyenlieuCN/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelNguyenlieuCNItem>(urlJson);
        }

        public List<NguyenlieuCNItem> GetList()
        {
            var urlJson = string.Format("{0}NguyenlieuCN/GetList?key={1}", _url, Keyapi);
            return GetObjJson<List<NguyenlieuCNItem>>(urlJson);
        }
        public NguyenlieuCNItem GetNguyenlieuCNItem(int id)
        {
            var urlJson = string.Format("{0}NguyenlieuCN/GetNguyenlieuCNItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<NguyenlieuCNItem>(urlJson);
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var urlJson = string.Format("{0}NguyenlieuCN/GetListAuto?key={1}&keword={2}&showLimit={3}", _url, Keyapi, keword, showLimit);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public int Add(string json)
        {
            var urlJson = string.Format("{0}NguyenlieuCN/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}NguyenlieuCN/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string json)
        {
            var urlJson = string.Format("{0}NguyenlieuCN/Delete?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
    }
}
