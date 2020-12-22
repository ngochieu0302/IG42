using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple.QLCN;

namespace FDI.GetAPI.QLCN
{
    public class MenCNAPI:BaseAPI
    {
        private readonly string _url = Domain;
        public ModelMenCNItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}MenCN/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelMenCNItem>(urlJson);
        }

        public List<MenCNItem> GetList()
        {
            var urlJson = string.Format("{0}MenCN/GetList?key={1}", _url, Keyapi);
            return GetObjJson<List<MenCNItem>>(urlJson);
        }
        public MenCNItem GetMenCNItem(int id)
        {
            var urlJson = string.Format("{0}MenCN/GetMenCNItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<MenCNItem>(urlJson);
        }
        public int Add(string json,string codelogin)
        {
            var urlJson = string.Format("{0}MenCN/Add?key={1}&code={2}&{3}", Domain, Keyapi, codelogin,json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json,string codelogin)
        {
            var urlJson = string.Format("{0}MenCN/Update?key={1}&code={2}&{3}", Domain, Keyapi, codelogin,json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string json)
        {
            var urlJson = string.Format("{0}MenCN/Delete?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
    }
}
