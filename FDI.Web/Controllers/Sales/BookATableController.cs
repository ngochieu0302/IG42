using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using FDI.CORE;

namespace FDI.Web.Controllers
{
    public class BookATableController : BaseController
    {
        private readonly ProductAPI _productApi = new ProductAPI();
        private readonly DNBedDeskAPI _deskApi = new DNBedDeskAPI();
        private readonly ContactOrderAPI _contactOrderApi = new ContactOrderAPI();
        private readonly OrderAPI _ordersApi = new OrderAPI();
        private readonly CustomerAPI _customerApi = new CustomerAPI();
        readonly DiscountAPI _discountAPI = new DiscountAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = _deskApi.GetListBedItemByDateNow(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = new ModelBedDeskItem
            {
                ListItem = _deskApi.GetListNow(UserItem.AgencyID),
                Listid = ArrId
            };
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var obj = _contactOrderApi.GetContactOrderItem(ArrId.FirstOrDefault());
            return View(obj);
        }
        public ActionResult AjaxContactToOrder()
        {
            var obj = _ordersApi.ContactToOrder(UserItem.AgencyID, ArrId.FirstOrDefault(), UserItem.UserId);
            var model = new ModelBedDeskItem
            {
                ListItem = _deskApi.GetListNow(UserItem.AgencyID),
                OrderItem = obj,
                Listid = obj.DN_Bed_Desk1.Select(m => m.ID),
                CategoryItems = _productApi.GetList(UserItem.AgencyID)
            };
            return View("AjaxOrders", model);
        }
        public ActionResult AjaxOrders()
        {
            var id = Request["id"];
            var obj = new OrderItem();
            var listint = ArrId;
            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                obj = _ordersApi.GetItemById(Convert.ToInt32(id));
                if (obj != null)
                    listint = obj.DN_Bed_Desk1.Select(m => m.ID).ToList();
            }
            var model = new ModelBedDeskItem
            {
                ListItem = _deskApi.GetListNow(UserItem.AgencyID),
                OrderItem = obj,
                Listid = listint,
                CategoryItems = _productApi.GetList(UserItem.AgencyID)
            };
            return View(model);
        }
        public ActionResult AjaxPrint()
        {
            var model = new ModelBedDeskItem
            {
                OrderItem = _ordersApi.GetItemById(ArrId.FirstOrDefault()),
                NameAgency = UserItem.AgencyName,
                AddressAgency = UserItem.AgencyAddress,
                UserName = UserName,
                DiscountItems = _discountAPI.GetDiscountItem(1, UserItem.AgencyID)
            };
            return View(model);
        }
        public ActionResult StopOrder()
        {
            var i = _contactOrderApi.StopOrder(UserItem.AgencyID, ArrId.FirstOrDefault());
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Đã hủy đơn đặt hàng thành công."
            };
            if (i == 0)
            {
                msg.Erros = true;
                msg.Message = "Error.";
            }
            if (i == 1)
            {
                msg.Erros = false;
                msg.Message = "Đã hủy đơn hàng thành công.";
            }
            if (i == 3)
            {
                msg.Erros = true;
                msg.Message = "không có đơn hàng và đơn đặt hàng nào cần hủy!";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProcessingRestaurant(int id)
        {
            _ordersApi.ProcessingRestaurant(UserItem.AgencyID, id, UserItem.UserId);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxNoteCate()
        {
            var url = Request.Form.ToString();
            var msg = _customerApi.UpdateCustomerCare(url, UserItem.AgencyID);
            return Json(msg);
        }
        public ActionResult Auto()
        {
            var query = Request["query"];
            var type = Request["type"] ?? "0";
            var ltsResults = _productApi.GetListAuto(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults.Select(c => new SuggestionsProduct
                {
                    ID = c.ID,
                    IsCombo = 0,
                    value = c.value,
                    title = c.title,
                    data = "Giá: " + c.pricenew.Money(),
                    name = "Mã SP: " + c.value + " | Màu: " + c.Color + " | Size: " + c.Size,
                    pricenew = c.pricenew,
                    Unit = c.Unit,
                    Type = c.Type
                })
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoCustomer()
        {
            var query = Request["query"];
            query = query.Replace("%", "");
            query = query.Replace("?", "");
            var ltsResults = _customerApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoCustomerItem()
        {
            var query = Request["query"];
            var ltsResults = _customerApi.GetListAuto(query, 10, UserItem.AgencyID);
            var item = ltsResults.FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Actions()
        //{
        //    var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
        //    var objitem = new OrderGetItem();
        //    var listid = Request["itemId"];
        //    switch (DoAction)
        //    {
        //        case ActionType.Add:
        //            try
        //            {
        //                UpdateModel(objitem);
        //                objitem.Value = 60;
        //                objitem.list = ArrId;
        //                var orderItem = _ordersApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate);
        //                if (orderItem)
        //                {
        //                    msg.Message = "Bàn đang được sử dụng.";
        //                    msg.Erros = true;
        //                    return Json(msg);
        //                }
        //                var contactitem = _contactOrderApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate);
        //                if (contactitem)
        //                {
        //                    msg.Message = string.Format("Thời gian {0} - {1} đã có người đặt.",
        //                        objitem.StartDate.DecimalToString("HH:mm dd/MM/yyyy"), objitem.EndDate.DecimalToString("HH:mm dd/MM/yyyy"));
        //                    msg.Erros = true;
        //                    return Json(msg);
        //                }
        //                var json = new JavaScriptSerializer().Serialize(objitem);
        //                msg.ID = _contactOrderApi.AddRestaurant(json, UserItem.AgencyID, UserItem.UserId, ":4000").ToString();
        //                if (msg.ID == "0")
        //                {
        //                    msg.Message = "Có lỗi xảy ra!";
        //                    msg.Erros = true;
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                msg.Message = "Có lỗi xảy ra!";
        //                msg.Erros = true;
        //            }
        //            break;
        //        case ActionType.Order:
        //            try
        //            {
        //                UpdateModel(objitem);
        //                objitem.list = ArrId;
        //                if (objitem.ID == 0)
        //                {
        //                    // Đơn hàng nhà hàng ko cho đặt bàn khi có khách hàng.
        //                    var date = DateTime.Now;
        //                    var dateend = DateTime.Today.AddDays(1);
        //                    objitem.StartDate = date.TotalSeconds();
        //                    objitem.EndDate = dateend.TotalSeconds();
        //                    objitem.Value = (int)((objitem.EndDate - objitem.StartDate) / 60);

        //                    var orderItem = _ordersApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate);
        //                    if (orderItem)
        //                    {
        //                        msg.Message = "Bàn đang được sử dụng.";
        //                        msg.Erros = true;
        //                        return Json(msg);
        //                    }
        //                    var contactitem = _contactOrderApi.CheckOrder(listid, objitem.StartDate, objitem.EndDate);
        //                    if (contactitem)
        //                    {
        //                        msg.Message = string.Format("Thời gian {0} - {1} đã có người đặt.",
        //                            objitem.StartDate.DecimalToString("HH:mm dd/MM/yyyy"),
        //                            objitem.EndDate.DecimalToString("HH:mm dd/MM/yyyy"));
        //                        msg.Erros = true;
        //                        return Json(msg);
        //                    }
        //                }
        //                var json = new JavaScriptSerializer().Serialize(objitem);
        //                msg.ID = _ordersApi.AddRestaurant(UserItem.AgencyID, json, UserItem.UserId, CodeLogin(), ":4000").ToString();
        //                if (msg.ID == "0")
        //                {
        //                    msg.Erros = true;
        //                    msg.Message = "Có lỗi xảy ra!";
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                msg.Message = "Có lỗi xảy ra!";
        //                msg.Erros = true;
        //            }
        //            break;
        //        case ActionType.Edit:
        //            try
        //            {
        //                UpdateModel(objitem);
        //                objitem.list = ArrId;
        //                var json = new JavaScriptSerializer().Serialize(objitem);
        //                msg.ID = _ordersApi.CopyRestaurant(UserItem.AgencyID, json, CodeLogin(), UserItem.UserId).ToString();
        //                if (msg.ID == "0")
        //                {
        //                    msg.Erros = true;
        //                    msg.Message = "Có lỗi xảy ra!";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                msg.Message = "Có lỗi xảy ra!";
        //                msg.Erros = true;
        //            }
        //            break;
        //        case ActionType.Delete:
        //            var i = _ordersApi.StopOrder(UserItem.AgencyID, ArrId.FirstOrDefault());
        //            msg = new JsonMessage
        //            {
        //                Erros = false,
        //                Message = "Đã hủy đơn đặt hàng thành công."
        //            };
        //            if (i == 0)
        //            {
        //                msg.Erros = true;
        //                msg.Message = "Error.";
        //            }
        //            if (i == 1)
        //            {
        //                msg.Erros = false;
        //                msg.Message = "Đã hủy đơn hàng thành công.";
        //            }
        //            if (i == 3)
        //            {
        //                msg.Erros = true;
        //                msg.Message = "không có đơn hàng và đơn đặt hàng nào cần hủy!";
        //            }
        //            break;
        //        case ActionType.Complete:
        //            try
        //            {
        //                UpdateModel(objitem);
        //                var json = new JavaScriptSerializer().Serialize(objitem);
        //                msg.Type = _ordersApi.CompleteRestaurant(UserItem.AgencyID, json, UserItem.UserId);
        //                if (msg.Type == 0)
        //                {
        //                    msg.Erros = true;
        //                    msg.Message = "Có lỗi xảy ra!";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                msg.Message = "Có lỗi xảy ra!";
        //                msg.Erros = true;
        //            }
        //            break;
        //        default:
        //            msg.Message = "Bạn không có quyền thực hiện chức năng này.";
        //            msg.Erros = true;
        //            break;

        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}
    }
}