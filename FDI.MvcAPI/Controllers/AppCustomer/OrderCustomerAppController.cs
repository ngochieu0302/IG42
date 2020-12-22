using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;

using FDI.DA;
using FDI.MvcAPI.Common;
using FDI.Simple;
using  FDI.Utils;
//using FDI.Utils;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    [CustomAuthorize]
    public class OrderCustomerAppController : BaseApiController
    {
        //
        // GET: /OrderCustomerApp/
        readonly OrderCustomerDA _da = new OrderCustomerDA("#");
        readonly ProductDA _productDa = new ProductDA("#");
        private readonly CustomerDA _customerDA = new CustomerDA("#");
        private readonly ContactOrderDA _orderDa = new ContactOrderDA();

        public ActionResult GetListAll(string key, int id)
        {
            var obj = key != Keyapi ? new List<CusContactItem>() : _da.GetListAll(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByCusId(string key, int id)
        {
            var obj = key != Keyapi ? new CusContactItem() : _da.GetListByCusId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="aid">agencyid</param>
        /// <param name="cid">customerid</param>
        /// <param name="a">Địa chỉ người nhận</param>
        /// <param name="m">SĐT người nhận</param>
        /// <param name="json">Danh sách sản phẩm đặt.</param>
        /// <param name="la"></param>
        /// <param name="lo"></param>
        /// <returns></returns>
        public ActionResult AddContactOrder(string key, int id, int aid, int cid, string a, string m, string json, string la, string lo, string cn)
        {
            if (key == Keyapi)
            {
                try
                {
                    if (!string.IsNullOrEmpty(json))
                    {
                        var datenow = ConvertDate.TotalSeconds(DateTime.Now);
                        var lst = GetObjJson<List<ContactDetaiAppItem>>(json);
                        var lstDetail = lst.Where(item => item.Q > 0).Select(item => new Shop_ContactOrder_Details
                        {
                            GID = Guid.NewGuid(),
                            Quantity = item.Q,
                            QuantityOld = 0,
                            Price = item.P,
                            DateCreated = datenow,
                            Status = (int)FDI.CORE.OrderStatus.Pending,
                            ProductID = item.PId,
                        }).ToList();
                        if (lstDetail.Any())
                        {
                            var total = lstDetail.Sum(c => c.Price * c.Quantity);
                            var totalQ = lstDetail.Sum(c => c.Quantity);
                            var order = new Shop_ContactOrder
                            {
                                AgencyId = aid,
                                Shop_ContactOrder_Details = lstDetail,
                                TotalPrice = total,
                                DateCreated = datenow,
                                Status = (int)FDI.CORE.OrderStatus.Pending,
                                IsDelete = false,
                                StartDate = datenow,
                                CustomerName = cn,
                                Latitute = la,
                                Longitude = lo,
                                EndDate = datenow + 3600,
                                CustomerID = cid,
                                Mobile = m,
                                Address = a,
                                Quantity = totalQ
                            };
                            if (id > 0)
                            {
                                order = _da.GetByID(id);
                                order.Shop_ContactOrder_Details.Clear();
                                order.Shop_ContactOrder_Details = lstDetail;
                                order.Address = a;
                                order.Mobile = m;
                            }
                            else _da.Add(order);
                            _da.Save();
                            return Json(1, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DoOrder(OrderGetItem model)
        {
            var customer = _customerDA.GetCustomerItem(CustomerId);
            var date = ConvertDate.TotalSeconds(DateTime.Now);
            
            var hour = 0;
            var orderType = model.ListProductModel.Select(m => (CORE.OrderType) m.OrderType).OrderBy(m => m.GetOrder()).FirstOrDefault();
            var receive = DateTime.Today;

            if (orderType == CORE.OrderType.TOMORROW)
            {
                hour = 12;
                receive = receive.AddDays(1);
            }

            if (orderType == CORE.OrderType.BEFORE12H)
            {
                hour = 12;
            }
            if (orderType == CORE.OrderType.BEFORE17H)
            {
                hour = 17;
            }

            var order = new Shop_ContactOrder()
            {
                Address = model.Address,
                CustomerID = CustomerId,
                CustomerName = model.CustomerName,
                Mobile = model.Mobile,
                AgencyId = customer.AgencyId,
                Status = (int)FDI.CORE.OrderStatus.Pending,
                ReceiveDate = ConvertDate.TotalSeconds(receive),
                ReceiveHour = hour,
                DateCreated = date,
                IsDelete = false
            };

            foreach (var productItem in model.ListProductModel)
            {
                var product = _productDa.GetProductDetailItem(productItem.ID);

                if (product == null)
                {
                    return Json(new JsonMessage(true, "Sản phẩm không tồn tại"));
                }

                var day = OrderExtensions.GetDayByOrderType((OrderType)productItem.OrderType);
                var hours = OrderExtensions.GetHourByOrderType((OrderType)productItem.OrderType);
                var todayCode = ConvertDate.TotalSeconds(DateTime.Today.AddDays(day));
                var receiveDate = ConvertDate.TotalSeconds(DateTime.Today.AddDays(day).AddHours(hours));

                var productitem = new Shop_ContactOrder_Details()
                {
                    Quantity = productItem.Q,
                    ProductID = productItem.ID,
                    Price = product.PriceUnit, //don gia
                    Weight = productItem.Weight,
                    DateCreated = date,
                    OrderType = productItem.OrderType,
                    TodayCode = todayCode,
                    ReceiveDate = receiveDate,
                    GID = Guid.NewGuid(),
                };

                switch (productitem.OrderType)
                {
                    case 1:
                        productitem.DateCreated = ConvertDate.TotalSeconds(DateTime.Today.AddHours(12));
                        break;
                }

                order.Shop_ContactOrder_Details.Add(productitem);
            }

            order.TotalPrice = order.Shop_ContactOrder_Details.Sum(m => m.Price * m.Quantity * m.Weight);

            _orderDa.Add(order);
            _orderDa.Save();

            return Json(new JsonMessage(false, "Đặt hàng thành công"));
        }


        [HttpPost]
        public ActionResult GetAll(int pageIndex, int? pageSize)
        {
            if (pageSize >= 25)
            {
                return Json(new JsonMessage(true, ""));
            }

            var lst = _orderDa.GetListByCustomer(pageIndex, pageSize??10, CustomerId, out var totalRecord);

            var response = new List<object>();
            foreach (var orderItem in lst)
            {
                if (orderItem.Status != null)
                    response.Add(new
                    {
                        // ReceiveDate = orderItem.ReceiveDateTxt,
                        StatusTxt = ((OrderStatus)orderItem.Status).GetDisplayName(),
                        Name = string.Join(", ", orderItem.LstOrderDetailItems.Select(m => m.ProductName).ToArray())
                            .SubString(30),
                        orderItem.ID,
                        orderItem.DateCreated
                    });
            }

            return Json(new BaseResponse<List<object>>() { Total = totalRecord, Data = response });
        }

        [HttpPost]
        public ActionResult GetById(int id)
        {
            var order = _orderDa.GetContactOrderItem(id, CustomerId);
            if (order == null)
            {
                return Json(new JsonMessage(true, "Đơn hàng không tồn tại"));
            }

            return Json(new BaseResponse<ContactOrderItem>() { Data = order });
        }

        [HttpPost]
        public ActionResult GetProductToDay()
        {
            var todayCodde = ConvertDate.TotalSeconds(DateTime.Today);
            var lst = _orderDa.GetProductToDay(todayCodde);
            return Json(lst);
        }
    }
}
