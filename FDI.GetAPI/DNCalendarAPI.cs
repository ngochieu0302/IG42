using System;
using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNCalendarAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelDNCalendarItem ListItems(int agencyid, string url)
        {
             url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNCalendar/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelDNCalendarItem>(urlJson);
        }

        public ModelDNCalendarItem ListItemsByUser(int agencyid, string url, Guid UserId)
        {
             url = string.IsNullOrEmpty(url) ? "?" : url;
             var urlJson = string.Format("{0}DNCalendar/ListItemsByUser{1}&key={2}&agencyId={3}&UserId={4}", _url, url, Keyapi, agencyid, UserId);
            return GetObjJson<ModelDNCalendarItem>(urlJson);
        }
       
        public List<DNCalendarItem> GetItemByUserId(Guid userid, int agencyid = 0)
        {
            var urlJson = string.Format("{0}DNCalendar/GetItemByUserId?key={1}&userid={2}&agencyId={3}", _url, Keyapi,userid, agencyid);
            return GetObjJson<List<DNCalendarItem>>(urlJson);
        }

        public List<CheckInItem> GetItemByUserIdDate(Guid userid, int agencyid, decimal date)
        {
            var urlJson = string.Format("{0}DNCalendar/GetItemByUserIdDate?key={1}&userid={2}&agencyid={3}&date={4}", _url, Keyapi, userid, agencyid, date);
            return GetObjJson<List<CheckInItem>>(urlJson);
        }

        public List<DNCalendarItem> GetItemByRoleId(int agencyid, Guid roleId)
        {
            var urlJson = string.Format("{0}DNCalendar/GetItemByRoleId?key={1}&agencyId={2}&roleId={3}", _url, Keyapi,agencyid, roleId);
            return GetObjJson<List<DNCalendarItem>>(urlJson);
        }

        public List<DNCalendarItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNCalendar/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNCalendarItem>>(urlJson);
        }        

        public DNCalendarItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNCalendar/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<DNCalendarItem>(urlJson);
        }

        public List<DNCalendarItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNCalendar/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<DNCalendarItem>>(urlJson);
        }

        public List<BedDeskItem> GetLisBedDesk(int agencyid)
        {
            var urlJson = string.Format("{0}DNCalendar/GetLisBedDesk?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<BedDeskItem>>(urlJson);
        }
        public int Add(int agencyid, string json,string code)
        {
            var urlJson = string.Format("{0}DNCalendar/Add?key={1}&agencyId={2}&json={3}&code={4}", _url, Keyapi,agencyid, json, code);
            return GetObjJson<int>(urlJson);
        }
        public int AddUserCalendar(int agencyid, string json, int id)
        {
            var urlJson = string.Format("{0}DNCalendar/AddUserCalendar?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        public int AddRolesCalendar(int agencyid, string json, int id)
        {
            var urlJson = string.Format("{0}DNCalendar/AddRolesCalendar?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        public int AddCalendarWeeklySchedule(int agencyid, int calenderId, string weeklySchedule)
        {
            var urlJson = string.Format("{0}DNCalendar/AddCalendarWeeklySchedule?key={1}&agencyId={2}&calenderId={3}&weeklySchedule={4}", _url, Keyapi,agencyid, calenderId, weeklySchedule);
            return GetObjJson<int>(urlJson);
        }
        public int Update(int agencyid, string json,string code, int id = 0)
        {
            var urlJson = string.Format("{0}DNCalendar/Update?key={1}&agencyId={2}&json={3}&id={4}&code={5}", _url, Keyapi,agencyid, json, id, code);
            return GetObjJson<int>(urlJson);
        }        
    }
}
