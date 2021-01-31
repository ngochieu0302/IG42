using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.Web.Controllers
{
    public class MassageController : BaseController
    {
        private readonly ProductAPI _productApi = new ProductAPI();
        private readonly PacketAPI _packetApi = new PacketAPI();
        private readonly DNLevelRoomAPI _levelRoomApi = new DNLevelRoomAPI();
        private readonly DNBedDeskAPI _api = new DNBedDeskAPI();
        private readonly ContactOrderAPI _contactOrderApi = new ContactOrderAPI();
        private readonly OrderAPI _ordersApi = new OrderAPI();
        private readonly AddMinuteAPI _addMinuteAPI = new AddMinuteAPI();
        private readonly ExchangeAPI _exchangeApi = new ExchangeAPI();
        private readonly CustomerAPI _customerApi = new CustomerAPI();
        readonly DiscountAPI _discountAPI = new DiscountAPI();
        readonly DNUserAPI _dnUserApi = new DNUserAPI();
        public ActionResult Index()
        {
            return View(_packetApi.GetListNotInBedDesk(UserItem.AgencyID));
        }
        public ActionResult ListItems()
        {
            var a = _api.GetListItemByDateNow(UserItem.AgencyID);
            a.lstPacket = _packetApi.GetListSimple(UserItem.AgencyID);
            a.AgencyId = UserItem.AgencyID;
            return View(a);
        }
        public ActionResult ViewContactOrder()
        {
            var model = new ModelDNLevelRoomItem
            {
                ListItems = _levelRoomApi.GetListBed(UserItem.AgencyID),
                ListContacOrder = _contactOrderApi.ListItemByDay(UserItem.AgencyID),
            };
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var obj = _productApi.ListProductByDeddeskId(UserItem.AgencyID, ArrId.FirstOrDefault());
            return View(obj);
        }
        public ActionResult AjaxExchange(int id, decimal end)
        {
            var obj = new DNExchangeItem
            {
                BedDeskID = id,
                EndDate = end,
                url = WebConfig.UrlNode + ":3000"
            };
            return View(obj);
        }
        public ActionResult AjaxFormOrder()
        {
            var model = new ModelOrderItem
            {
                OrderItem = _ordersApi.GetMassageItemById(ArrId.FirstOrDefault()),
                ListAddMinuteItem = _addMinuteAPI.ListAllItems()
            };
            
            return View(model);
        }
        public ActionResult KeyOrder(int agencyid)
        {
            _api.KeyGetListItemByDateNow(agencyid);
            var msg = new JsonMessage { Erros = false };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxView()
        {
            var obj = _productApi.ListProductByDeddeskId(UserItem.AgencyID, ArrId.FirstOrDefault());
            return View(obj);
        }
        public ActionResult AjaxPrint()
        {
            var model = new DNAgencyItem
            {
                AddressAgency = UserItem.AgencyAddress,
                NameAgency = UserItem.AgencyName,
            };
            return View(model);
        }
        public ActionResult AjaxOrders()
        {
            var obj = _ordersApi.OrderByBedIdContactId(UserItem.AgencyID, ArrId.FirstOrDefault());
            var packetId = Request["packetId"] ?? "0";
            var lstProductDefault = _ordersApi.ProductDefaultbyBedid(UserItem.AgencyID, ArrId.FirstOrDefault(), int.Parse(packetId));
            var model = new ModelOrderGetItem
            {
                ID = obj.ID,
                UserName = UserItem.UserName,
                CustomerName = obj.CustomerName,
                Mobile = obj.Mobile,
                Address = obj.Address,
                AgencyID = UserItem.AgencyID,
                StartDate = obj.StartDate,
                ProductID = obj.ProductID,
                IsEarly = lstProductDefault.IsEarly,
                Listproduct = obj.Listproduct,
                LstProductPacketItems = lstProductDefault.ListProductPacketItems,
                TimeEarly = lstProductDefault.TimeEarly,
                TimeWait = lstProductDefault.TimeWait,
                ListItem = obj.ListItem,
                DiscountItems = _discountAPI.GetDiscountItem(1, UserItem.AgencyID)
            };
            if (obj.Listproduct != null && obj.Listproduct.Any())
            {
                model.Time = 0;
                //model.Price = obj.Listproduct.Sum(c => c.Shop_Product.Shop_Product_Detail.Price * c.Shop_Product.Product_Size.Value / 1000);
            }
            else if (lstProductDefault.ListProductPacketItems != null && lstProductDefault.ListProductPacketItems.Any())
            {
                model.Time = lstProductDefault.ListProductPacketItems.Sum(c => c.Time);
                model.Price = lstProductDefault.ListProductPacketItems.Sum(c => c.Price);
            }
            else
            {
                model.Time = 0;
                model.Price = 0;
            }
            return View(model);
        }
        public ActionResult AjaxOrdersSpeed()
        {
            var obj = _ordersApi.OrderByBedIdContactId(UserItem.AgencyID, ArrId.FirstOrDefault());
            var model = new ModelOrderGetItem
            {
                ID = obj.ID,
                UserName = UserItem.UserName,
                CustomerName = obj.CustomerName,
                Mobile = obj.Mobile,
                Address = obj.Address,
                Time = obj.Time,
                Price = obj.Price,
                ListItem = obj.ListItem,
                DiscountItems = _discountAPI.GetDiscountItem(1, UserItem.AgencyID)
            };
            return View(model);
        }
        public ActionResult AjaxStatus()
        {
            ViewBag.ID = ArrId.FirstOrDefault();
            var model = _productApi.GetStatus(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult ReturnView()
        {
            return View();
        }
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var objitem = new ModelOrderGetItem();
            var listid = Request["jsonBedUser"];
            var listp = Request["LstProductID"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(objitem);
                        objitem.EndDate = objitem.StartDate + objitem.Value * 60;
                        var obj = JsonConvert.DeserializeObject<List<ListBebDesk>>(listid);
                        objitem.list = obj;
                        var orderItem = _ordersApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate,objitem.TimeWait);
                        if (orderItem)
                        {
                            msg.Message = "Giường đang được sử dụng.";
                            msg.Erros = true;
                            return Json(msg);
                        }
                        var contactitem = _contactOrderApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate, objitem.TimeWait);
                        if (contactitem)
                        {
                            msg.Message = string.Format("Thời gian {0} - {1} đã có người đặt.", objitem.StartDate.DecimalToDate().Format("HH:mm dd/MM/yyyy"), objitem.EndDate.DecimalToDate().Format("HH:mm dd/MM/yyyy"));
                            msg.Erros = true;
                            return Json(msg);
                        }
                        objitem.Lstproduct = listp;
                        var json = new JavaScriptSerializer().Serialize(objitem);
                        msg.ID = _contactOrderApi.Add(json, UserItem.AgencyID, UserItem.UserId, ":3000").ToString();
                        msg.Message = "Đặt giường thành công.";
                        if (msg.ID == "0")
                        {
                            msg.Erros = true;
                            msg.Message = "Có lỗi xảy ra!";
                        }
                        if (msg.ID == "3")
                        {
                            msg.Erros = true;
                            msg.Message = "Thời gian này đã có người đặt";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg.Message = "Có lỗi xảy ra!";
                        msg.Erros = true;
                    }
                    break;
                case ActionType.Edit:
                    try
                    {
                        UpdateModel(objitem);
                        objitem.EndDate = objitem.StartDate + objitem.Value * 60;
                        if (!string.IsNullOrEmpty(objitem.ContactId) && int.Parse(objitem.ContactId) > 0)
                        {
                            var obj = JsonConvert.DeserializeObject<List<ListBebDesk>>(listid);
                            objitem.list = obj;
                            objitem.Lstproduct = listp;
                            objitem.IsEarly = true;
                            var jsons = new JavaScriptSerializer().Serialize(objitem);
                            msg.ID = _ordersApi.AddMassage(jsons, UserItem.AgencyID, UserItem.UserId).ToString();
                            msg.Message = "Thanh toán thành công.";
                            if (msg.ID == "0")
                            {
                                msg.Erros = true;
                                msg.Message = "Có lỗi xảy ra!";
                            }
                        }
                        else
                        {
                            var orderItem = _ordersApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate,objitem.TimeWait);
                            if (orderItem)
                            {
                                msg.Message = "Giường đang được sử dụng.";
                                msg.Erros = true;
                                return Json(msg);
                            }
                            var contactitem = _contactOrderApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate, objitem.TimeWait);
                            if (contactitem)
                            {
                                msg.Message = string.Format("Thời gian {0} - {1} đã có người đặt.",
                                    objitem.StartDate.DecimalToDate().Format("HH:mm dd/MM/yyyy"),
                                    objitem.EndDate.DecimalToDate().Format("HH:mm dd/MM/yyyy"));
                                msg.Erros = true;
                                return Json(msg);
                            }
                            var obj = JsonConvert.DeserializeObject<List<ListBebDesk>>(listid);
                            objitem.list = obj;
                            objitem.Lstproduct = listp;
                            var json = new JavaScriptSerializer().Serialize(objitem);
                            msg.ID = _ordersApi.AddMassage(json, UserItem.AgencyID, UserItem.UserId).ToString();
                            msg.Message = "Thanh toán thành công.";
                            if (msg.ID == "0")
                            {
                                msg.Erros = true;
                                msg.Message = "Có lỗi xảy ra!";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        msg.Message = "Có lỗi xảy ra!";
                        msg.Erros = true;
                    }
                    break;
                case ActionType.Order:
                    try
                    {
                        UpdateModel(objitem);
                        objitem.ID = ArrId.FirstOrDefault();
                        var json = new JavaScriptSerializer().Serialize(objitem);
                        msg.ID = _ordersApi.UpdateMassage(json).ToString();
                        msg.Message = "Thanh toán thành công.";
                        if (msg.ID == "0")
                        {
                            msg.Erros = true;
                            msg.Message = "Có lỗi xảy ra!";
                        }
                    }
                    catch (Exception)
                    {
                        msg.Message = "Có lỗi xảy ra!";
                        msg.Erros = true;
                    }
                    break;
                case ActionType.Complete:
                    var id = _exchangeApi.Add(UserItem.AgencyID, Request["BedDeskID"], Request["NameBedDes"], Request["EndDate"]);
                    if (id == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Error.";
                    }
                    else
                    {
                        msg.ID = id.ToString();
                        msg.Message = "Đã đổi giường thành công.";
                    }
                    break;
                case ActionType.Delete:
                    var i = _api.StopOrder(ArrId.FirstOrDefault(), ":3000");
                    msg = new JsonMessage
                    {
                        Erros = false,
                        Message = "Đơn đặt hàng đã hoàn thành."
                    };
                    if (i == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Error.";
                    }
                    if (i == 1)
                    {
                        msg.Erros = false;
                        msg.Message = "Đơn đặt hàng đã hoàn thành.";
                    }
                    if (i == 3)
                    {
                        msg.Erros = true;
                        msg.Message = "không có đơn hàng và đơn đặt hàng nào cần hủy!";
                    }
                    break;
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CompleteOrder(int idOd)
        {
            var i = _api.StopOrder(idOd, ":3000");
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Đơn đặt hàng đã hoàn thành."
            };
            if (i == 0)
            {
                msg.Erros = true;
                msg.Message = "Error.";
            }
            if (i == 1)
            {
                msg.Erros = false;
                msg.Message = "Đơn đặt hàng đã hoàn thành.";
            }
            if (i == 3)
            {
                msg.Erros = true;
                msg.Message = "không có đơn hàng và đơn đặt hàng nào cần hủy!";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoCustomerItem()
        {
            var query = Request["query"];
            var ltsResults = _customerApi.GetListAuto(query, 10, UserItem.AgencyID);
            var item = ltsResults.FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChooseUser()
        {
            var model = _dnUserApi.GetListAllChoose(UserItem.AgencyID);
            return View(model);
        } 
    }
}