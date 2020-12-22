
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNBedDeskAPI : BaseAPI
    {
        public List<BedDeskItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<BedDeskItem>>(urlJson);
        }
        public List<BedDeskItem> GetListInPacket(int agencyId, int packetid)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetListInPacket?key={1}&agencyId={2}&packetid={3}", Domain, Keyapi, agencyId, packetid);
            return GetObjJson<List<BedDeskItem>>(urlJson);
        }

        public ModelBedDeskItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNBedDesk/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelBedDeskItem>(urlJson);
        }
        public void SortNameBed(int agencyid)
        {
            var urlJson = string.Format("{0}DNBedDesk/SortNameBed?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            GetObjJson<int>(urlJson);
        }
        public List<BedDeskItem> GetListNow(int agencyid)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetListNow?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<BedDeskItem>>(urlJson);
        }
        public List<BedDeskItem> GetList(int agencyid)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetList?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<BedDeskItem>>(urlJson);
        }
        public List<BedDeskItem> GetListByRoomId(int id)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetListByRoomId?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<List<BedDeskItem>>(urlJson);
        }
        public BedDeskItem GetBedDeskItem(int id)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetBedDeskItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<BedDeskItem>(urlJson);
        }
        public ModelBedDeskItem GetListBedItemByDateNow(int agencyid)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetListBedItemByDateNow?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            var key = string.Format("DNBedDeskAPI_GetListBedItemByDateNow_{0}", agencyid);
            return GetCache<ModelBedDeskItem>(key, urlJson, ConfigCache.TimeExpire360);
        }
        public ModelBedDeskItem GetListItemByDateNow(int agencyid)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetListItemByDateNow?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            var key = string.Format("DNBedDeskAPI_GetListItemByDateNow_{0}", agencyid);
            return GetCache<ModelBedDeskItem>(key, urlJson, ConfigCache.TimeExpire360);
        }
        public void KeyGetListItemByDateNow(int agencyid)
        {
            var key = string.Format("DNBedDeskAPI_GetListItemByDateNow_{0}", agencyid);
            if (Cache.KeyExistsCache(key))
            {
                Cache.DeleteCache(key);
            }
        }
        public List<BedDeskItem> GetListItemByMWSID(int agencyid, int mwsid, int id = 0)
        {
            var urlJson = string.Format("{0}DNBedDesk/GetListItemByMWSID?key={1}&agencyId={2}&mwsid={3}&id={4}", Domain, Keyapi, agencyid, mwsid, id);
            return GetObjJson<List<BedDeskItem>>(urlJson);
        }
        public int ChangeStatus(int bedId, int statusId)
        {
            var urlJson = string.Format("{0}DNBedDesk/ChangeStatus?key={1}&bedId={2}&statusId={3}", Domain, Keyapi, bedId, statusId);
            return GetObjJson<int>(urlJson);
        }
        public int StopOrder(int bedId, string port)
        {
            var urlJson = string.Format("{0}DNBedDesk/StopOrder?key={1}&bedId={2}&port={3}", Domain, Keyapi, bedId, port);
            return GetObjJson<int>(urlJson);
        }

        public int StopOrderSpa(int bedId, string port)
        {
            var urlJson = string.Format("{0}DNBedDesk/StopOrderSpa?key={1}&bedId={2}&port={3}", Domain, Keyapi, bedId, port);
            return GetObjJson<int>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNBedDesk/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNBedDesk/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNBedDesk/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Hide(string id, int agencyid, string code)
        {
            var urlJson = string.Format("{0}DNBedDesk/Hide?key={1}&agencyId={2}&id={3}&code={4}", Domain, Keyapi, agencyid, id, code);
            return GetObjJson<int>(urlJson);
        }
    }
}