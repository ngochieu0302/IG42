using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNEDITScheduleAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelEditScheduleItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNEditSchedule/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelEditScheduleItem>(urlJson);
        }
        public List<EditScheduleItem> GetAll(decimal dates, decimal datee, int agencyid)
        {
            var urlJson = string.Format("{0}DNEditSchedule/GetAll?key={1}&dates={2}&datee={3}&agencyId={4}", _url, Keyapi, dates, datee, agencyid);
            return GetObjJson<List<EditScheduleItem>>(urlJson);
        }
        public List<EditScheduleItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNEditSchedule/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<EditScheduleItem>>(urlJson);
        }

        public EditScheduleItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNEditSchedule/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<EditScheduleItem>(urlJson);
        }
        public List<EditScheduleItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNEditSchedule/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<EditScheduleItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNEditSchedule/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNEditSchedule/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        
    }
}
