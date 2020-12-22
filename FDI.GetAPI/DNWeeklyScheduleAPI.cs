using System;
using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNWeeklyScheduleAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<WeeklyScheduleItem> GetListSimpleByRequest(int agencyid)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/GetListSimpleByRequest?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<WeeklyScheduleItem>>(urlJson);
        }

        public List<WeeklyScheduleItem> GetAll(int agencyid, int caledarid)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/GetAll?key={1}&agencyId={2}&caledarid={3}", _url, Keyapi,agencyid, caledarid);
            var key = string.Format("DNWeeklyScheduleAPIGetAll_{0}-{1}", agencyid, caledarid);
            return GetObjJson<List<WeeklyScheduleItem>>(urlJson);
            //return GetCache<List<WeeklyScheduleItem>>(key, urlJson, ConfigCache.TimeExpire);
        }

        public WeeklyScheduleItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<WeeklyScheduleItem>(urlJson);
        }
        
        public List<WeeklyScheduleItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<WeeklyScheduleItem>>(urlJson);
        }

        public List<WeeklyScheduleItem> GetAllByAgencyId(Guid userid, int agencyid = 0)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/GetAllByAgencyId?key={1}&agencyId={2}&userid={3}", _url, Keyapi,agencyid, userid);
            return GetObjJson<List<WeeklyScheduleItem>>(urlJson);
        }

        public List<WeeklyScheduleItem> GetExitsAllByAgencyId(int agencyid, string lstScheduleId)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/GetExitsAllByAgencyId?key={1}&agencyId={2}&lstScheduleId={3}", _url, Keyapi,agencyid, lstScheduleId);
            return GetObjJson<List<WeeklyScheduleItem>>(urlJson);
        }

        public List<DNUserItem> GetAllUserSchedule(int agencyid, Guid userId, decimal date, int scheduleId)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/GetAllUserSchedule?key={1}&agencyId={2}&userId={3}&scheduleId={4}&date={5}", _url, Keyapi,agencyid, userId, scheduleId, date);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNWeeklySchedule/Update?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        
    }
}
