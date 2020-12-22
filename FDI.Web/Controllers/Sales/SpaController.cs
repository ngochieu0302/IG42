using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.Web.Controllers.Sales
{
    public class SpaController : BaseController
    {
        //
        // GET: /Spa/
        private readonly ProductAPI _productApi = new ProductAPI();
        private readonly PacketAPI _packetApi = new PacketAPI();
        private readonly DNBedDeskAPI _api = new DNBedDeskAPI();
        private readonly ContactOrderAPI _contactOrderApi = new ContactOrderAPI();
        private readonly OrderAPI _ordersApi = new OrderAPI();
        private readonly ExchangeAPI _exchangeApi = new ExchangeAPI();
        readonly DiscountAPI _discountAPI = new DiscountAPI();
        public ActionResult Index()
        {
            return View(_packetApi.GetListNotInBedDesk(UserItem.AgencyID));
        }
        public ActionResult ListItems()
        {
            //var da = ConvertDate.TotalSeconds(DateTime.Today.AddMinutes(1));
            var a = _api.GetListItemByDateNow(UserItem.AgencyID);
            var list = _exchangeApi.GetListByNow(UserItem.AgencyID);
            foreach (var item in list)
            {
                var obj1 = a.ListItem.FirstOrDefault(m => m.ID == item.BedDeskID);
                var obj2 = a.ListItem.FirstOrDefault(m => m.ID == item.BedDeskExID);
                if (obj1 != null && obj2 != null)
                {
                    var obj = obj1.DN_User_BedDesk;
                    obj1.DN_User_BedDesk = obj2.DN_User_BedDesk;
                    obj2.DN_User_BedDesk = obj;
                }
            }
            return View(a);
        }
        public ActionResult AjaxOrders()
        {

            var obj = _ordersApi.OrderByBedIdContactIdSpa(UserItem.AgencyID, ArrId.FirstOrDefault());
            var model = new ModelOrderGetItem
            {
                ID = obj.ID,
                UserName = UserItem.UserName,
                CustomerName = obj.CustomerName,
                Mobile = obj.Mobile,
                Address = obj.Address,
                Time = obj.Time,
                StartDate = obj.StartDate,
                ProductID = obj.ProductID,
                Listproduct = obj.Listproduct,
                Price = obj.Price,
                ListItem = obj.ListItem,
                DiscountItems = _discountAPI.GetDiscountItem(1, UserItem.AgencyID)
            };
            return View(model);
        }
        public ActionResult AjaxExchange(int id, decimal end)
        {
            var obj = new DNExchangeItem
            {
                BedDeskID = id,
                EndDate = end,
                url = WebConfig.UrlNode + ":5000"
            };
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
        public ActionResult AjaxView()
        {
            var obj = _productApi.ListProductByDeddeskIdSpa(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewBag.lstproduct = _productApi.GetListByAgency(UserItem.AgencyID);
            return View(obj);
        }
        public ActionResult KeyOrder(int agencyid)
        {
            _api.KeyGetListItemByDateNow(agencyid);
            var msg = new JsonMessage { Erros = false };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CompleteOrder(int idOd)
        {
            var msg = new JsonMessage { Erros = false };
            var i = _api.StopOrderSpa(idOd, ":5000");
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
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var objitem = new ModelOrderGetItem();
            var listid = Request["itemId"];
            var listp = Request["LstProductID"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(objitem);
                        objitem.EndDate = objitem.StartDate + objitem.Value * 60;
                        
                        //var obj = JsonConvert.DeserializeObject<List<ListBebDesk>>(temp);
                        //objitem.list = obj;
                        var orderItem = _ordersApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate,objitem.TimeWait);
                        if (orderItem)
                        {
                            msg.Message = "Giường đang được sử dụng.";
                            msg.Erros = true;
                            return Json(msg);
                        }
                        var contactitem = _contactOrderApi.CheckOrder(listid, objitem.StartDate,
                            objitem.EndDate, objitem.TimeWait);
                        if (contactitem)
                        {
                            msg.Message = string.Format("Thời gian {0} - {1} đã có người đặt.",
                                objitem.StartDate.DecimalToDate().Format("HH:mm dd/MM/yyyy"),
                                objitem.EndDate.DecimalToDate().Format("HH:mm dd/MM/yyyy"));
                            msg.Erros = true;
                            return Json(msg);
                        }
                        objitem.Lstproduct = listp;
                        var json = new JavaScriptSerializer().Serialize(objitem);
                        msg.ID = _contactOrderApi.AddSpa(json, UserItem.AgencyID, UserItem.UserId, ":5000").ToString();
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
                            //var obj = JsonConvert.DeserializeObject<List<ListBebDesk>>(listid);
                            //objitem.list = obj;
                            objitem.Lstproduct = listp;
                            objitem.IsEarly = true;
                            var jsons = new JavaScriptSerializer().Serialize(objitem);
                            msg.ID = _ordersApi.AddSpa(jsons, UserItem.AgencyID, UserItem.UserId).ToString();
                            msg.Message = "Thanh toán thành công.";
                            if (msg.ID == "0")
                            {
                                msg.Erros = true;
                                msg.Message = "Có lỗi xảy ra!";
                            }
                        }
                        else
                        {
                            var orderItem = _ordersApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate, objitem.TimeWait);
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
                            //var obj = JsonConvert.DeserializeObject<List<ListBebDesk>>(listid);
                            //objitem.list = obj;
                            objitem.Lstproduct = listp;
                            objitem.IsEarly = true;
                            var json = new JavaScriptSerializer().Serialize(objitem);
                            msg.ID = _ordersApi.AddSpa(json, UserItem.AgencyID, UserItem.UserId).ToString();
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
                    var i = _api.StopOrderSpa(ArrId.FirstOrDefault(), ":5000");
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
    }
}
