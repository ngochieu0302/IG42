using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNWeeklyAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<WeeklyItem> GetListSimpleByRequest(int agencyid)
        {
            var urlJson = string.Format("{0}DNWeekly/GetListSimpleByRequest?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<WeeklyItem>>(urlJson);
        }

        public List<WeeklyItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNWeekly/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<WeeklyItem>>(urlJson);
        }

        public WeeklyItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNWeekly/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<WeeklyItem>(urlJson);
        }

        public List<WeeklyItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNWeekly/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<WeeklyItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNWeekly/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNWeekly/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        
    }
}
