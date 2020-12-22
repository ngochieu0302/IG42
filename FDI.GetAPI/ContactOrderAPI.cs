using System;
using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ContactOrderAPI : BaseAPI
    {
        public ModelContactOrderItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ContactOrder/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelContactOrderItem>(urlJson);
        }

        public List<ContactOrderItem> ListItemByDay(int agencyid)
        {
            var urlJson = string.Format("{0}ContactOrder/ListItemByDay?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<ContactOrderItem>>(urlJson);
        }

        public ContactOrderItem GetContactOrderItem(int id)
        {
            var urlJson = string.Format("{0}ContactOrder/GetContactOrderItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<ContactOrderItem>(urlJson);
        }
        public bool CheckOrder(string listId, decimal? sdate, decimal? eDate,int? time)
        {
            var urlJson = string.Format("{0}ContactOrder/CheckOrder?key={1}&listid={2}&sdate={3}&eDate={4}&timedo={5}", Domain, Keyapi, listId, sdate, eDate,time);
            return GetObjJson<bool>(urlJson);
        }
        
        public int AddRestaurant(string json, int agencyid, Guid UserId, string port)
        {
            var urlJson = string.Format("{0}ContactOrder/AddRestaurant?key={1}&json={2}&agencyId={3}&UserId={4}&port={5}", Domain, Keyapi, json, agencyid, UserId, port);
            return GetObjJson<int>(urlJson);
        }

        public int StopOrder(int agencyid, int id)
        {
            var urlJson = string.Format("{0}ContactOrder/StopOrder?key={1}&agencyId={2}&id={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<int>(urlJson);
        }
        public int Add(string json, int agencyid, Guid UserId, string port)
        {
            var urlJson = string.Format("{0}ContactOrder/Add?key={1}&json={2}&agencyId={3}&UserId={4}&port={5}", Domain, Keyapi, json, agencyid, UserId, port);
            return GetObjJson<int>(urlJson);
        }
        public int AddSpa(string json, int agencyid, Guid UserId, string port)
        {
            var urlJson = string.Format("{0}ContactOrder/AddSpa?key={1}&json={2}&agencyId={3}&UserId={4}&port={5}", Domain, Keyapi, json, agencyid, UserId, port);
            return GetObjJson<int>(urlJson);
        }
    }
}
