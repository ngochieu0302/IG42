using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class OrderAPI : BaseAPI
    {
        public ModelOrderItem ListItems(int agencyid, string url, Guid userId, bool isadmin = true)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/ListItems{1}&key={2}&agencyId={3}&isadmin={4}&UserId={5}", Domain, url, Keyapi, agencyid, isadmin, userId);
            return GetObjJson<ModelOrderItem>(urlJson);
        }
        public ModelOrderItem ListItemsAll(int agencyid, string url, Guid userId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/ListItemsAll{1}&key={2}", Domain, url, Keyapi);
            return GetObjJson<ModelOrderItem>(urlJson);
        }
        public ModelOrderItem GetListbyDate(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/GetListbyDate{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelOrderItem>(urlJson);
        }
        public ModelEnterprisesItem OrderByEnterprisesRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/OrderByEnterprisesRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            var key = string.Format("OrderAPI_OrderByEnterprisesRequest_{0}-{1}", agencyid, url);
            return GetCache<ModelEnterprisesItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        public ModelMonthItem OrderByAgencyRequest(int id, int year)
        {
            var urlJson = string.Format("{0}Order/OrderByAgencyRequest?id={1}&year={2}&key={3}", Domain, id, year, Keyapi);
            var key = string.Format("OrderAPI_OrderByAgencyRequest_{0}-{1}", id, year);
            return GetCache<ModelMonthItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        public ModelDNUserItem OrderByUserRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/OrderByUserRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            var key = string.Format("OrderAPI_OrderByUserRequest_{0}-{1}", agencyid, url);
            return GetCache<ModelDNUserItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        public ModelDNUserItem OrderByUserPVRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/OrderByUserPVRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDNUserItem>( urlJson);
        }
        
        public ModelDNUserItem OrderByUser1Request(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/OrderByUser1Request{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            var key = string.Format("OrderAPI_OrderByUser1Request_{0}-{1}", agencyid, url);
            return GetCache<ModelDNUserItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        public ModelDNLevelRoomItem OrderByLevelRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/OrderByLevelRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            var key = string.Format("OrderAPI_OrderByLevelRequest_{0}-{1}", agencyid, url);
            return GetCache<ModelDNLevelRoomItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        public OrderItem GetMassageItemById(int id)
        {
            var urlJson = string.Format("{0}Order/GetMassageItemById?key={1}&id={2}", Domain, Keyapi, id);
            var key = string.Format("OrderAPI_GetMassageItemById_{0}", id);
            return GetCache<OrderItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        
        public ModelDNRoomItem OrderByRoomRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/OrderByRoomRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            var key = string.Format("OrderAPI_OrderByRoomRequest_{0}-{1}", agencyid, url);
            return GetCache<ModelDNRoomItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        public ModelBedDeskItem OrderByBedDeskRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/OrderByBedDeskRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            var key = string.Format("OrderAPI_OrderByBedDeskRequest_{0}-{1}", agencyid, url);
            return GetCache<ModelBedDeskItem>(key, urlJson, ConfigCache.TimeExpire);
        }
        public ModelOrderItem ListOrderFashion(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/ListOrderFashion{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelOrderItem>(urlJson);
        }
        public ModelOrderDetailItem ListOrderFashionDetail(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/ListOrderFashionDetail{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelOrderDetailItem>(urlJson);
        }
        public ModelOrderItem ListUserItems(string url, Guid guid)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/ListUserItems{1}&key={2}&guid={3}", Domain, url, Keyapi, guid);
            return GetObjJson<ModelOrderItem>(urlJson);
        }
        public ModelOrderItem GetListByCustomer(string url, int id)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Order/GetListByCustomer{1}&key={2}&id={3}", Domain, url, Keyapi, id);
            return GetObjJson<ModelOrderItem>(urlJson);
        }
        public List<OrderItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}Order/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<OrderItem>>(urlJson);
        }
        public OrderItem GetOrderItem(int id)
        {
            var urlJson = string.Format("{0}Order/GetOrderItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<OrderItem>(urlJson);
        }
        public OrderDetailItem GetOrderDetailItem(int id)
        {
            var urlJson = string.Format("{0}Order/GetOrderDetailItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<OrderDetailItem>(urlJson);
        }
        public OrderItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}Order/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<OrderItem>(urlJson);
        }
        public bool CheckOrder(string listId, decimal? sdate, decimal? eDate,int? timeearly)
        {
            var urlJson = string.Format("{0}Order/CheckOrder?key={1}&listid={2}&sdate={3}&eDate={4}&timedo={5}", Domain, Keyapi, listId, sdate, eDate, timeearly);
            return GetObjJson<bool>(urlJson);
        }
        public OrderGetItem OrderByBedIdContactId(int agencyid, int id)
        {
            var urlJson = string.Format("{0}Order/OrderByBedIdContactId?key={1}&agencyId={2}&bedid={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<OrderGetItem>(urlJson);
        }
        public PacketItem ProductDefaultbyBedid(int agencyid, int id,int packet)
        {
            var urlJson = string.Format("{0}Order/ProductDefaultbyBedid?key={1}&agencyId={2}&bedid={3}&packetId={4}", Domain, Keyapi, agencyid, id,packet);
            return GetObjJson<PacketItem>(urlJson);
        }
        public OrderGetItem OrderByBedIdContactIdSpa(int agencyid, int id)
        {
            var urlJson = string.Format("{0}Order/OrderByBedIdContactIdSpa?key={1}&agencyId={2}&bedid={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<OrderGetItem>(urlJson);
        }
        public OrderGetItem OrderByBedId(int agencyid, int id)
        {
            var urlJson = string.Format("{0}Order/OrderByBedId?key={1}&agencyId={2}&bedid={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<OrderGetItem>(urlJson);
        }
        public List<PriceAgencyItem> ListPriceAgencyByAgencyId(int agencyid)
        {
            var urlJson = string.Format("{0}Order/ListPriceAgencyByAgencyId?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<PriceAgencyItem>>(urlJson);
        }
        public List<OrderProcessItem> ListOrderByAgency()
        {
            var urlJson = string.Format("{0}Order/ListOrderByAgency?key={1}", Domain, Keyapi);
            return GetObjJson<List<OrderProcessItem>>(urlJson);
        }
        public List<OrderItem> GetOrder()
        {
            var urlJson = string.Format("{0}Order/GetOrder?key={1}", Domain, Keyapi);
            return GetObjJson<List<OrderItem>>(urlJson);
        }
        public int Delete(int agencyid, int orderId)
        {
            var urlJson = string.Format("{0}Order/Delete?key={1}&agencyId={2}&orderId={3}", Domain, Keyapi, agencyid, orderId);
            return GetObjJson<int>(urlJson);
        }
        public int Active(int id)
        {
            var urlJson = string.Format("{0}Order/Active?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<int>(urlJson);
        }

        public JsonMessage AddSales(int agencyid, string json, Guid UserId, string code, string port)
        {
            var urlJson = string.Format("{0}Order/AddSales?key={1}&agencyId={2}&json={3}&UserId={4}&code={5}&port={6}", Domain, Keyapi, agencyid, json, UserId, code, port);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage AddSalesApp(int agencyid, string listIdp, Guid UserId, string code, string port)
        {
            var urlJson = string.Format("{0}Order/AddSalesApp?key={1}&agencyId={2}&listIdp={3}&UserId={4}&code={5}&port={6}", Domain, Keyapi, agencyid, listIdp, UserId, code, port);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public int Add(int agencyid, string json, Guid UserId, string code, string port)
        {
            var urlJson = string.Format("{0}Order/Add?key={1}&agencyId={2}&json={3}&UserId={4}&code={5}&port={6}", Domain, Keyapi, agencyid, json, UserId, code, port);
            return GetObjJson<int>(urlJson);
        }
        
        public int Update(string json)
        {
            var urlJson = string.Format("{0}Order/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int UpdateMassage(string json)
        {
            var urlJson = string.Format("{0}Order/UpdateMassage?key={1}&json={2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int AddFashion(int agencyid, string json, Guid UserId, string code, string port)
        {
            var urlJson = string.Format("{0}Order/AddFashion?key={1}&agencyId={2}&UserId={3}&json={4}&code={5}", Domain, Keyapi, agencyid, UserId, json,code);
            return GetObjJson<int>(urlJson);
        }
        public int AddRestaurant(int agencyid, string json, Guid UserId, string code, string port)
        {
            var urlJson = string.Format("{0}Order/AddRestaurant?key={1}&agencyId={2}&json={3}&UserId={4}&code={5}&port={6}", Domain, Keyapi, agencyid, json, UserId, code, port);
            return GetObjJson<int>(urlJson);
        }
        public int AddNext(int agencyid, int id, string port)
        {
            var urlJson = string.Format("{0}Order/AddNext?key={1}&agencyId={2}&id={3}&port={4}", Domain, Keyapi, agencyid, id, port);
            return GetObjJson<int>(urlJson);
        }
        public int StopOrder(int agencyid, int id)
        {
            var urlJson = string.Format("{0}Order/StopOrder?key={1}&agencyId={2}&id={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<int>(urlJson);
        }
        public OrderItem ContactToOrder(int agencyid, int id, Guid UserId)
        {
            var urlJson = string.Format("{0}Order/ContactToOrder?key={1}&agencyId={2}&id={3}&UserId={4}", Domain, Keyapi, agencyid, id, UserId);
            return GetObjJson<OrderItem>(urlJson);
        }

        public int ProcessingRestaurant(int agencyid, int id, Guid UserId)
        {
            var urlJson = string.Format("{0}Order/ProcessingRestaurant?key={1}&agencyId={2}&id={3}&UserId={4}", Domain, Keyapi, agencyid, id, UserId);
            return GetObjJson<int>(urlJson);
        }
        public int CompleteRestaurant(int agencyid, string json, Guid UserId)
        {
            var urlJson = string.Format("{0}Order/CompleteRestaurant?key={1}&agencyId={2}&json={3}&UserId={4}", Domain, Keyapi, agencyid, json, UserId);
            return GetObjJson<int>(urlJson);
        }
        public int CopyRestaurant(int agencyid, string json, string code, Guid UserId)
        {
            var urlJson = string.Format("{0}Order/CopyRestaurant?key={1}&agencyId={2}&code={3}&json={4}&UserId={5}", Domain, Keyapi, agencyid, code, json, UserId);
            return GetObjJson<int>(urlJson);
        }
        public int AddMassage(string json, int agencyid, Guid UserId)
        {
            var urlJson = string.Format("{0}Order/AddMassage?key={1}&json={2}&agencyId={3}&UserId={4}", Domain, Keyapi, json, agencyid, UserId);
            return GetObjJson<int>(urlJson);
        }
        public int AddSpa(string json, int agencyid, Guid UserId)
        {
            var urlJson = string.Format("{0}Order/AddSpa?key={1}&json={2}&agencyId={3}&UserId={4}", Domain, Keyapi, json, agencyid, UserId);
            return GetObjJson<int>(urlJson);
        }
    }
}
