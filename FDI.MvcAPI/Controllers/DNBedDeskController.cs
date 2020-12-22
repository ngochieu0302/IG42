using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class DNBedDeskController : BaseApiController
    {
        //
        // GET: /DNBedDesk/

        readonly DNLevelRoomDA _dnLevelRoomBl = new DNLevelRoomDA();
        readonly DNBedDeskDA _da = new DNBedDeskDA();
        readonly OrdersDA _ordersDa = new OrdersDA();
        readonly ContactOrderDA _contactOrderDa = new ContactOrderDA();
        public ActionResult GetListSimple(string key, int agencyid)
        {
            var obj = Request["key"] != Keyapi ? new List<BedDeskItem>() : _da.GetListSimple(agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListInPacket(string key, int agencyid, int packetid)
        {
            var obj = Request["key"] != Keyapi ? new List<BedDeskItem>() : _da.GetListInPacket(agencyid, packetid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult SortNameBed(string key, string code)
        {
            if (key == Keyapi)
            {
                _da.SortNameBed(Agencyid());
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelBedDeskItem()
                : new ModelBedDeskItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListNow(string key, string code)
        {
            var obj = key != Keyapi ? new List<BedDeskItem>() : _da.GetListNow(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(string key, int agencyid)
        {
            var obj = key != Keyapi ? new List<BedDeskItem>() : _da.GetList(agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBedDeskItem(string key, int id)
        {
            var obj = key != Keyapi ? new BedDeskItem() : _da.GetBedDeskItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByRoomId(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new List<BedDeskItem>() : _da.GetListByRoomId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListBedItemByDateNow(string key, int agencyid)
        {
            var obj = Request["key"] != Keyapi ? new ModelBedDeskItem() : new ModelBedDeskItem { AgencyId = agencyid, ListItem = _da.GetListBedItemByDateNow(agencyid), ListRoomItem = _dnLevelRoomBl.GetAll(agencyid) };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListItemByDateNow(string key, int agencyid)
        {
            var obj = Request["key"] != Keyapi ? new ModelBedDeskItem() : new ModelBedDeskItem { AgencyId = agencyid, ListItem = _da.GetListItemByDateNow(agencyid), ListRoomItem = _dnLevelRoomBl.GetAll(agencyid) };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListItemByMWSID(string key, string code, int mwsid, int id)
        {
            var obj = Request["key"] != Keyapi ? new List<BedDeskItem>() : _da.GetListItemByMWSID(Agencyid(), mwsid, id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StopOrder(string key, int bedId, string port)
        {
            if (key == Keyapi)
            {
                var item = _da.GetOrderOrContactByBedId(bedId);
                var date = DateTime.Now.TotalSeconds();
                if (item.ContactOrders.Any())
                {
                    var obj = _contactOrderDa.GetById(item.ContactOrders.Select(m => m).FirstOrDefault());
                    if (obj != null)
                    {
                        if (date < obj.StartDate)
                        {
                            foreach (var items in obj.Shop_ContactOrder_Details)
                            {
                                items.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                            }
                            obj.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                        }
                        else obj.Status = (int)FDI.CORE.OrderStatus.Complete;
                        obj.EndDate = DateTime.Now.TotalSeconds();
                        _contactOrderDa.Save();
                        var url = port + "/updatecontact/" + obj.ID;
                        Node(url);
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (item.Shop_Orders.Any())
                {
                    var obj = _ordersDa.GetById(item.Shop_Orders.Select(m => m).FirstOrDefault());
                    if (obj != null)
                    {
                        
                        if (date < obj.StartDate)
                        {
                            foreach (var items in obj.Shop_Order_Details)
                            {
                                items.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                            }
                            foreach (var itemOrder in obj.WalletOrder_History)
                            {
                                itemOrder.IsDelete = true;
                            }
                            foreach (var item1 in obj.RewardHistories)
                            {
                                item1.IsDeleted = true;
                            }
                            foreach (var item2 in obj.ReceiveHistories)
                            {
                                item2.IsDeleted = true;
                            }
                            obj.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                        }
                        else obj.Status = (int)FDI.CORE.OrderStatus.Complete;
                        obj.EndDate = DateTime.Now.TotalSeconds();
                        _ordersDa.Save();
                        var url = port + "/updateorder/" + obj.ID;
                        Node(url);
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(3, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StopOrderSpa(string key, int bedId, string port)
        {
            if (key == Keyapi)
            {
                var item = _da.GetOrderOrContactByBedId(bedId);
                var date = DateTime.Now.TotalSeconds();
                if (item.ContactOrders.Any())
                {
                    var obj = _contactOrderDa.GetById(item.ContactOrders.Select(m => m).FirstOrDefault());
                    if (obj != null)
                    {
                        if (date < obj.StartDate)
                        {
                            foreach (var items in obj.Shop_ContactOrder_Details)
                            {
                                items.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                            }
                            obj.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                        }
                        else
                        {
                            obj.Status = (int)FDI.CORE.OrderStatus.Complete;
                        }
                        obj.EndDate = DateTime.Now.TotalSeconds();
                        _contactOrderDa.Save();
                        var url = port + "/updatecontact/" + obj.ID;
                        Node(url);
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (item.Shop_Orders.Any())
                {
                    var obj = _ordersDa.GetById(item.Shop_Orders.Select(m => m).FirstOrDefault());
                    if (obj != null)
                    {

                        if (date < obj.StartDate)
                        {
                            foreach (var items in obj.Shop_Order_Details)
                            {
                                items.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                            }
                            obj.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                        }
                        else
                        {
                            obj.Status = (int)FDI.CORE.OrderStatus.Complete;
                        }
                        obj.EndDate = DateTime.Now.TotalSeconds();
                        _ordersDa.Save();
                        var url = port + "/updateorder/" + obj.ID;
                        Node(url);
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(3, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var model = new DN_Bed_Desk();
            try
            {
                if (key == Keyapi)
                {
                    UpdateModel(model);
                    model.Name = HttpUtility.UrlDecode(model.Name);
                    _da.Add(model);
                    _da.Save();
                    return Json(model.ID, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key, string json)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Hide(string key, string code, int id)
        {
            if (key == Keyapi)
            {
                var lstInt = GetListShowDeskItem(code);
                var lst = _da.ListItemArrId(lstInt.Select(m => m.ID).ToList(), id);
                foreach (var item in lst)
                {
                    var obj = lstInt.FirstOrDefault(m => m.ID == item.ID);
                    if (obj != null)
                    {
                        item.IsShow = obj.S;
                        item.Quantity = obj.Q;
                    }
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        #region Xử lý chi tiết bàn
        public List<ShowDeskItem> GetListShowDeskItem(string code)
        {
            const string url = "Utility/GetListShowDeskItem?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ShowDeskItem>>(urlJson);
            return list;
        }
        #endregion
    }
}