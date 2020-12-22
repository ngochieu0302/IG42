using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNComboAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<DNComboItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}DNCombo/GetListSimple?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNComboItem>>(urlJson);
        }

        public ModelDNComboItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNCombo/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNComboItem>(urlJson);
        }

        public DNComboItem GetComboItem(int id)
        {
            var urlJson = string.Format("{0}DNCombo/GetComboItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<DNComboItem>(urlJson);
        }
        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNCombo/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNCombo/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNCombo/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
