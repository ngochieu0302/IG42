using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNUserAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public int UpdatePass(string code, string passold, string passnew)
        {
            var urlJson = string.Format("{0}DNUser/UpdatePass?key={1}&code={2}&passold={3}&passnew={4}", Domain, Keyapi, code, passold, passnew);
            return GetObjJson<int>(urlJson);
        }
        public int UpdateCusPass(string code, string passold, string passnew)
        {
            var urlJson = string.Format("{0}DNUser/UpdateCusPass?key={1}&code={2}&passold={3}&passnew={4}", Domain, Keyapi, code, passold, passnew);
            return GetObjJson<int>(urlJson);
        }

        public List<DNUserItem> GetListByAgency()
        {
            var urlJson = string.Format("{0}DNUser/GetListByAgency?key={1}", _url, Keyapi);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var urlJson = string.Format("{0}DNUser/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}", Domain, Keyapi, keword, showLimit, agencyId);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }

        public List<DNUserCalendarItem> GetListCalendarUser(int agencyid, decimal dates, decimal datee, Guid userid)
        {
            var urlJson = string.Format("{0}DNUser/GetListCalendar?key={1}&agencyId={2}&dates={3}&datee={4}&userid={5}", _url, Keyapi, agencyid, dates, datee, userid);
            return GetObjJson<List<DNUserCalendarItem>>(urlJson);
        }
        public List<DNUserCalendarItem> GetListCalendar(int agencyid, decimal dates, decimal datee)
        {
            var urlJson = string.Format("{0}DNUser/GetListCalendar?key={1}&agencyId={2}&dates={3}&datee={4}", _url, Keyapi, agencyid, dates, datee);
            return GetObjJson<List<DNUserCalendarItem>>(urlJson);
        }
        public List<DNUserCalendarItem> GetListTotalMonth(int agencyid, decimal dates, decimal datee)
        {
            var urlJson = string.Format("{0}DNUser/GetListTotalMonth?key={1}&agencyId={2}&dates={3}&datee={4}", _url, Keyapi, agencyid, dates, datee);
            return GetObjJson<List<DNUserCalendarItem>>(urlJson);
        }

        public List<SalaryMonthDetailItem> GetListUserRolesMonth(int agencyid, int month, int year)
        {
            var urlJson = string.Format("{0}DNUser/GetListUserRolesMonth?agencyId={1}&key={2}&agencyId={3}&month={4}&year={5}", _url, agencyid, Keyapi, agencyid, month, year);
            return GetObjJson<List<SalaryMonthDetailItem>>(urlJson);
        }

        public ModelDNUserItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNUser/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelDNUserItem>(urlJson);
        }

        public List<DNUserCalendarItem> ListUserNotSId(int agencyid, Guid id, int sid, decimal date)
        {
            var urlJson = string.Format("{0}DNUser/ListUserNotSId?key={1}&agencyId={2}&id={3}&sid={4}&date={5}", _url, Keyapi, agencyid, id, sid, date);
            return GetObjJson<List<DNUserCalendarItem>>(urlJson);
        }
        public List<DNUserItem> GetAll(int agencyId)
        {
            var urlJson = string.Format("{0}DNUser/GetAll?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }
        public List<DNUserItem> FindByName(string name)
        {
            var urlJson = string.Format("{0}DNUser/FindByName?key={1}&name={2}", _url, Keyapi, name);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }
        public List<DNUserItem> GetListAllAgency(int agencyId)
        {
            var urlJson = string.Format("{0}DNUser/GetListAllAgency?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }
        public List<DNUserItem> GetListAllChoose(int agencyId)
        {
            var urlJson = string.Format("{0}DNUser/GetListAllChoose?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }
        public List<DNUserItem> GetListAllSevice(int agencyId, string json)
        {
            var urlJson = string.Format("{0}DNUser/GetListAllSevice?key={1}&agencyId={2}&json={3}", _url, Keyapi, agencyId, json);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }

        public List<DNUserItem> GetListAllKt(int agencyId)
        {
            var urlJson = string.Format("{0}DNUser/GetListAllKt?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }
        public List<DNUserSimpleItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}DNUser/GetListSimple?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNUserSimpleItem>>(urlJson);
        }

        public DNUserItem GetUserIdByCodeCheckIn(string codecheckin, int agencyid)
        {
            var urlJson = string.Format("{0}DNUser/GetUserIdByCodeCheckIn?key={1}&codecheckin={2}&agencyId={3}", _url, Keyapi, codecheckin, agencyid);
            return GetObjJson<DNUserItem>(urlJson);
        }

        public DNUserItem GetItemModuleById(Guid id)
        {
            var urlJson = string.Format("{0}DNUser/GetItemModuleById?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<DNUserItem>(urlJson);
        }

        public DNUserItem GetItemById(int agencyid, Guid id)
        {
            var urlJson = string.Format("{0}DNUser/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi, agencyid, id);
            return GetObjJson<DNUserItem>(urlJson);
        }

        public bool CheckUserName(string txt, Guid id, int agencyid)
        {
            var urlJson = string.Format("{0}DNUser/CheckUserName?txt={1}&id={2}&agencyId={3}", Domain, txt, id, agencyid);
            return GetObjJson<bool>(urlJson);
        }

        public DNUserItem GetScheduleById(int agencyid, string id)
        {
            var urlJson = string.Format("{0}DNUser/GetScheduleById?key={1}&agencyId={2}&id={3}", _url, Keyapi, agencyid, id);
            return GetObjJson<DNUserItem>(urlJson);
        }

        public JsonMessage Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNUser/AddUser?key={1}&agencyId={2}&json={3}", _url, Keyapi, agencyid, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNUser/Update?key={1}&agencyId={2}&json={3}", _url, Keyapi, agencyid, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage AddModuleUser(string listInt, string userId, int agencyid)
        {
            var urlJson = string.Format("{0}DNUser/AddModuleUser?key={1}&agencyId={2}&listInt={3}&userId={4}", _url, Keyapi, agencyid, listInt, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage UpdateModuleActive(Guid userid, string ltrInts)
        {
            var urlJson = string.Format("{0}DNUser/UpdateModuleActive?key={1}&userid={2}&ltrInts={3}", _url, Keyapi, userid, ltrInts);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage ShowHide(Guid userId, bool isOut)
        {
            var urlJson = string.Format("{0}DNUser/Delete?key={1}&userId={2}&isOut={3}", _url, Keyapi, userId, isOut);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public int CheckinUser(int codecheckin, int agencyid)
        {
            var urlJson = string.Format("http://localhost:2160/Utility/CheckIn?key=checkinfdi&codeon={0}&agencyid={1}", codecheckin, agencyid);
            //var urlJson = string.Format("http://124.158.4.87:96/Utility/CheckIn?key=checkinfdi&codeon={0}&agencyid={1}", codecheckin, agencyid);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Delete(string agencyid, string listint)
        {
            var urlJson = string.Format("{0}DNUser/Delete?key={1}&agencyId={2}&listint={3}", _url, Keyapi, agencyid, listint);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public List<ModelDNUserAddItem> GetListByArrId(string lstId)
        {
            ;
            var urlJson = string.Format("{0}DNUser/Update?key={1}&id={2}", _url, Keyapi, lstId);
            return GetObjJson<List<ModelDNUserAddItem>>(urlJson);
        }
    }
}
