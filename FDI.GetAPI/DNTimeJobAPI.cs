using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;
namespace FDI.GetAPI
{
    public class DNTimeJobAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelDNTimeJobItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNTimeJob/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelDNTimeJobItem>(urlJson);
        }

        public List<DNTimeJobItem> GetAll()
        {
            var urlJson = string.Format("{0}DNTimeJob/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<DNTimeJobItem>>(urlJson);
        }

        public List<DNUserTimeJobItem> GetAllByMonth(int month, int year, int agencyid)
        {
            var urlJson = string.Format("{0}DNTimeJob/GetAllByMonth?key={1}&month={2}&year={3}&agencyId={4}", _url, Keyapi, month, year, agencyid);
            return GetObjJson<List<DNUserTimeJobItem>>(urlJson);
        }

        public List<UserViewItem> GetAllOnlineByToday(int agencyid)
        {
            var urlJson = string.Format("{0}DNTimeJob/GetAllOnlineByToday?key={1}&agencyId={2}", _url, Keyapi, agencyid);
            return GetObjJson<List<UserViewItem>>(urlJson);
        }

        public DNTimeJobItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNTimeJob/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi, agencyid, id);
            return GetObjJson<DNTimeJobItem>(urlJson);
        }

        public List<DNTimeJobItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNTimeJob/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi, agencyid, lstId);
            return GetObjJson<List<DNTimeJobItem>>(urlJson);
        }

        public JsonMessage  Add(string code, string json, int agencyid)
        {
            var urlJson = string.Format("{0}DNTimeJob/Add?key={1}&code={2}&json={3}&agencyId={3}", _url, Keyapi, code, json);
            var key = string.Format("DNBedDeskAPI_GetListItemByDateNow_{0}", code);
            if (Cache.KeyExistsCache(key))
            {
                Cache.DeleteCache(key);
            }
            return GetObjJson<JsonMessage>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNTimeJob/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi, agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }
    }
}
