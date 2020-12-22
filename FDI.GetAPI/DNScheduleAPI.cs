using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNScheduleAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelScheduleItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNSchedule/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelScheduleItem>(urlJson);
        }

        public List<ScheduleItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNSchedule/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<ScheduleItem>>(urlJson);
        }

        public List<ScheduleItem> GetListByUserId(int agencyid)
        {
            var urlJson = string.Format("{0}DNSchedule/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<ScheduleItem>>(urlJson);
        }

        public ScheduleItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNSchedule/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<ScheduleItem>(urlJson);
        }

        public List<ScheduleItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNSchedule/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<ScheduleItem>>(urlJson);
        }

        public List<ScheduleItem> GetAllByUserId(int agencyid, string userId)
        {
            var urlJson = string.Format("{0}DNSchedule/GetAllByUserId?key={1}&agencyId={2}&userId={3}", _url, Keyapi,agencyid, userId);
            return GetObjJson<List<ScheduleItem>>(urlJson);
        }
        
        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNSchedule/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNSchedule/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

		public JsonMessage Delete(string lstArrId)
		{
			var urlJson = string.Format("{0}DNSchedule/Delete?key={1}&lstArrId={2}", _url, Keyapi, lstArrId);
			return GetObjJson<JsonMessage>(urlJson);
		}
        public JsonMessage Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}DNSchedule/Hide?key={1}&lstArrId={2}", _url, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Show(string lstArrId)
        {
            var urlJson = string.Format("{0}DNSchedule/Show?key={1}&lstArrId={2}", _url, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
