using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Simple.QLCN;

namespace FDI.GetAPI.QLCN
{
    public class CateCNAPI : BaseAPI
    {
        private readonly string _url = Domain;

        public ModelCateCNItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CateCN/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelCateCNItem>(urlJson);
        }

        public List<CateCNItem> GetList()
        {
            var urlJson = string.Format("{0}CateCN/GetList?key={1}", _url, Keyapi);
            return GetObjJson<List<CateCNItem>>(urlJson);
        }
        public CateCNItem GetCateCNItem(int id)
        {
            var urlJson = string.Format("{0}CateCN/GetCateCNItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<CateCNItem>(urlJson);
        }
        public int Add(string json)
        {
            var urlJson = string.Format("{0}CateCN/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}CateCN/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string json)
        {
            var urlJson = string.Format("{0}CateCN/Delete?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
    }
}
