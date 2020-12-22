using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class ContactOrderController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly ContactOrderDA _da = new ContactOrderDA();
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelContactOrderItem()
                : new ModelContactOrderItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContactOrderItem(int id)
        {
            var obj = Request["key"] != Keyapi ? new ContactOrderItem() : _da.GetContactOrderItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItemByDay(string key, string code)
        {
            var obj = key != Keyapi ? new List<ContactOrderItem>() : _da.ListItemByDay(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListContactOrderByDateNow(string key)
        {
            var obj = key != Keyapi ? new List<OrderProcessItem>() : _da.ListContactOrderByDateNow();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListRestaurantByDateNow(string key)
        {
            var obj = key != Keyapi ? new List<OrderProcessItem>() : _da.ListRestaurantByDateNow();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckOrder(string key, string listid, decimal? sdate, decimal? eDate, int timedo)
        {
            var obj = key == Keyapi && _da.CheckOrder(listid, sdate, eDate, timedo);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json, string code, Guid UserId, string port = ":3000")
        {
            try
            {
                if (key == Keyapi)
                {

                    var obj = JsonConvert.DeserializeObject<ModelOrderGetItem>(json);
                    var listcontact = new List<Shop_ContactOrder>();
                    
                    foreach (var item in obj.list)
                    {
                        decimal totalprice = 0;
                        var order = new Shop_ContactOrder
                        {
                            StartDate = obj.StartDate,
                            UserID = UserId,
                            EndDate = obj.EndDate,
                            BedDeskID = item.idbed,
                            TotalMinute = obj.Value,
                            AgencyId = Agencyid(),
                            IsDelete = false,
                            DateCreated = DateTime.Now.TotalSeconds(),
                            CustomerID = obj.CustomerID,
                            CustomerName = obj.CustomerName,
                            Address = obj.Address,
                            Mobile = obj.Mobile,
                            IsEarly = obj.IsEarly,
                            Status = (int)(int)FDI.CORE.OrderStatus.Pending,
                            Content = HttpUtility.UrlDecode(obj.Note)
                        };
                        
                        if (obj.Lstproduct != null)
                        {
                            var orderDetail = new Shop_ContactOrder_Details();
                            var lstp = obj.Lstproduct.Split(',');
                            for (int i = 0; i < lstp.Length; i++)
                            {
                                var product = _da.GetProductItem(int.Parse(lstp[i]));
                                totalprice += product.PriceNew ?? 0;
                                orderDetail = new Shop_ContactOrder_Details
                                {
                                    ProductID = int.Parse(lstp[i]),
                                    Quantity = 1,
                                    Status = (int)(int)FDI.CORE.OrderStatus.Complete,
                                    Price = product.PriceNew,
                                    DateCreated = DateTime.Now.TotalSeconds(),
                                };
                                order.Shop_ContactOrder_Details.Add(orderDetail);
                            }
                            order.TotalPrice = totalprice;
                        }
                        listcontact.Add(order);
                    }
                    foreach (var item in listcontact)
                    {
                        _da.Add(item);
                    }
                    _da.Save();
                    foreach (var jsonnew in listcontact.Select(item => new OrderProcessItem
                    {
                        ID = item.ID,
                        BedDeskID = item.BedDeskID,
                        Minute = item.TotalMinute,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsEarly = obj.IsEarly.HasValue && obj.IsEarly.Value,
                        AgencyId = Agencyid(),
                        Status = 0
                    }))
                    {
                        json = new JavaScriptSerializer().Serialize(jsonnew);
                        Node(port + "/addcontactorder/" + json);
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddSpa(string key, string json, string code, Guid UserId, string port = ":5000")
        {
            try
            {
                if (key == Keyapi)
                {

                    var obj = JsonConvert.DeserializeObject<ModelOrderGetItem>(json);
                    var listcontact = new List<Shop_ContactOrder>();
                    
                    foreach (var item in obj.list)
                    {
                        decimal totalprice = 0;
                        var order = new Shop_ContactOrder
                        {
                            StartDate = obj.StartDate,
                            UserID = UserId,
                            EndDate = obj.EndDate,
                            BedDeskID = item.idbed,
                            TotalMinute = obj.Value,
                            AgencyId = Agencyid(),
                            IsDelete = false,
                            DateCreated = DateTime.Now.TotalSeconds(),
                            CustomerID = obj.CustomerID,
                            CustomerName = obj.CustomerName,
                            Address = obj.Address,
                            Mobile = obj.Mobile,
                            IsEarly = obj.IsEarly,
                            Status = (int)(int)FDI.CORE.OrderStatus.Pending,
                            Content = HttpUtility.UrlDecode(obj.Note)
                        };

                        if (obj.Lstproduct != null)
                        {
                            var orderDetail = new Shop_ContactOrder_Details();
                            var lstp = obj.Lstproduct.Split(',');
                            for (int i = 0; i < lstp.Length; i++)
                            {
                                var product = _da.GetProductItem(int.Parse(lstp[i]));
                                totalprice += product.PriceNew ?? 0;
                                orderDetail = new Shop_ContactOrder_Details
                                {
                                    ProductID = int.Parse(lstp[i]),
                                    Quantity = 1,
                                    Status = (int)(int)FDI.CORE.OrderStatus.Complete,
                                    Price = product.PriceNew,
                                    DateCreated = DateTime.Now.TotalSeconds(),
                                };
                                order.Shop_ContactOrder_Details.Add(orderDetail);
                            }
                            order.TotalPrice = totalprice;
                        }
                        listcontact.Add(order);
                    }
                    foreach (var item in listcontact)
                    {
                        _da.Add(item);
                    }
                    _da.Save();
                    foreach (var jsonnew in listcontact.Select(item => new OrderProcessItem
                    {
                        ID = item.ID,
                        BedDeskID = item.BedDeskID,
                        Minute = item.TotalMinute,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsEarly = obj.IsEarly.HasValue && obj.IsEarly.Value,
                        AgencyId = Agencyid(),
                        Status = 0
                    }))
                    {
                        json = new JavaScriptSerializer().Serialize(jsonnew);
                        Node(port + "/addcontactorder/" + json);
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddRestaurant(string key, string json, string code, Guid UserId, string port = ":4000")
        {
            try
            {
                if (key == Keyapi)
                {
                    var obj = JsonConvert.DeserializeObject<OrderGetItem>(json);
                    if (obj.list.Any())
                    {
                        var listbed = _da.GetListArrId(obj.list);
                        var order = new Shop_ContactOrder
                        {
                            StartDate = obj.StartDate,
                            UserID = UserId,
                            EndDate = obj.EndDate,
                            DN_Bed_Desk1 = listbed,
                            TotalMinute = obj.Value,
                            AgencyId = Agencyid(),
                            IsDelete = false,
                            DateCreated = DateTime.Now.TotalSeconds(),
                            CustomerID = obj.CustomerID,
                            CustomerName = obj.CustomerName,
                            Address = obj.Address,
                            Discount = obj.Deposits,
                            Mobile = obj.Mobile,
                            IsEarly = obj.IsEarly,
                            Status = (int)(int)FDI.CORE.OrderStatus.Pending,
                            Content = HttpUtility.UrlDecode(obj.Note)
                        };
                        _da.Add(order);
                        _da.Save();
                        foreach (var jsonnew in obj.list.Select(item => new OrderProcessItem
                        {
                            ID = order.ID,
                            BedDeskID = item,
                            Minute = order.TotalMinute,
                            StartDate = order.StartDate,
                            EndDate = order.EndDate,
                            AgencyId = Agencyid(),
                            IsEarly = obj.IsEarly.HasValue && obj.IsEarly.Value,
                            Status = 0
                        }))
                        {
                            json = new JavaScriptSerializer().Serialize(jsonnew);
                            Node(port + "/addcontactorder/" + json);
                        }
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult StopOrder(string key, string code, int id)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetById(id);
                if (obj != null && Agencyid() == obj.AgencyId)
                {
                    obj.Status = (int)(int)FDI.CORE.OrderStatus.Cancelled;
                    obj.EndDate = DateTime.Now.TotalSeconds();
                    _da.Save();
                    var url = ":4000/updatecontact/" + obj.ID;
                    Node(url);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}