using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNDayOffAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelDayOffItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDayOff/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelDayOffItem>(urlJson);
        }

        public List<DayOffItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNDayOff/GetAll?key={1}&agencyId={2}", _url, Keyapi, agencyid);
            var key = string.Format("DNDayOffAPIGetAll_{0}", agencyid);
            return GetCache<List<DayOffItem>>(key, urlJson, ConfigCache.TimeExpire);
        }

        public DayOffItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNDayOff/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi, agencyid, id);
            return GetObjJson<DayOffItem>(urlJson);
        }

        public List<DayOffItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNDayOff/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi, agencyid, lstId);
            return GetObjJson<List<DayOffItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNDayOff/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi, agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNDayOff/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi, agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }
    }
}
