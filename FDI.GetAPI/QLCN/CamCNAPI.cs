using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple.QLCN;

namespace FDI.GetAPI.QLCN
{
    public class CamCNAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelCamCNItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CamCN/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelCamCNItem>(urlJson);
        }

        public List<CamCNItem> GetList()
        {
            var urlJson = string.Format("{0}CamCN/GetList?key={1}", _url, Keyapi);
            return GetObjJson<List<CamCNItem>>(urlJson);
        }
        public CamCNItem GetCamCNItem(int id)
        {
            var urlJson = string.Format("{0}CamCN/GetCamCNItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<CamCNItem>(urlJson);
        }
        public int Add(string json, string codelogin)
        {
            var urlJson = string.Format("{0}CamCN/Add?key={1}&code={2}&{3}", Domain, Keyapi, codelogin, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json, string codelogin)
        {
            var urlJson = string.Format("{0}CamCN/Update?key={1}&code={2}&{3}", Domain, Keyapi, codelogin, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string json)
        {
            var urlJson = string.Format("{0}CamCN/Delete?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
    }
}
