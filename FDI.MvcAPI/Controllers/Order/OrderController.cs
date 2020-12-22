using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.DN_Sales;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    
    public class OrderController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly OrdersDA _da = new OrdersDA();
        private readonly BonusTypeDA _bonusTypeDa = new BonusTypeDA();
        private readonly ContactOrderDA _contactOrderDa = new ContactOrderDA();
        private readonly DNBedDeskDA _bedDeskDeskDa = new DNBedDeskDA();
        private readonly EnterprisesDA _d = new EnterprisesDA();
        readonly OrderDetailDA _orderDetailDa = new OrderDetailDA("#");
        readonly DNSalesDA _dnSalesDa = new DNSalesDA("#");
        readonly ProductDA _productDa = new ProductDA("#");
        private readonly CustomerDA _customerDA = new CustomerDA("#");
        private readonly OrdersDA _orderDa = new OrdersDA();

        public ActionResult ListItems()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelOrderItem()
                : new ModelOrderItem { ListOrderItem = _da.GetListSimpleByRequest(Request, Agencyid(), out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDiscount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsAll()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelOrderItem()
                : new ModelOrderItem { ListOrderItem = _da.GetListSimpleByRequestAll(Request, out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDiscount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListbyDate()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelOrderItem()
                : new ModelOrderItem { ListOrderItem = _da.GetListSimpleByRequestDate(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByAgencyRequest(int id, int year, string key)
        {
            decimal? total, totalDiscount, totalPay;
            var obj = Request["key"] != Keyapi
                ? new ModelMonthItem()
                : new ModelMonthItem
                {
                    BonusTypeItem = _bonusTypeDa.GetItemTop(),
                    ListItem = _da.OrderByAgencyRequest(id, year, out total, out totalPay, out totalDiscount),
                    Item = _d.GetItemByAgencyID(id),
                    Total = total,
                    TotalDiscount = totalDiscount,
                    TotalPay = totalPay
                };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByUserRequest()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelDNUserItem()
                : new ModelDNUserItem { ListItem = _da.OrderByUserRequest(Request, Agencyid(), out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDiscount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByUserPVRequest()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelDNUserItem()
                : new ModelDNUserItem { ListItem = _da.OrderByUserPVRequest(Request, Agencyid(), out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDiscount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByUser1Request()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelDNUserItem()
                : new ModelDNUserItem { ListItem = _da.OrderByUser1Request(Request, Agencyid(), out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDiscount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByLevelRequest()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelDNLevelRoomItem()
                : new ModelDNLevelRoomItem { ListItems = _da.OrderByLevelRequest(Request, Agencyid(), out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDisCount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByRoomRequest()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelDNRoomItem()
                : new ModelDNRoomItem { ListItems = _da.OrderByRoomRequest(Request, Agencyid(), out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDisCount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByBedDeskRequest()
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelBedDeskItem()
                : new ModelBedDeskItem { ListItem = _da.OrderByBedDeskRequest(Request, Agencyid(), out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDisCount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListOrderFashion()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelOrderItem()
                : new ModelOrderItem { ListOrderItem = _da.GetListOrderFashion(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListOrderFashionDetail()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelOrderDetailItem()
                : new ModelOrderDetailItem { ListItems = _da.GetListOrderFashionDetail(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListUserItems(Guid guid)
        {
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var obj = Request["key"] != Keyapi
                ? new ModelOrderItem()
                : new ModelOrderItem { ListOrderItem = _da.GetListSimpleByUser(Request, Agencyid(), guid, out total, out totalpay, out totaldiscount), TotalPay = totalpay, Total = total, TotalDiscount = totaldiscount, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByCustomer(int id)
        {
            decimal? totalPrice;
            int count;
            var obj = Request["key"] != Keyapi
               ? new ModelOrderItem()
               : new ModelOrderItem { ListOrderItem = _da.GetListByCustomer(Request, id, out totalPrice, out count), Total = totalPrice, Count = count, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListSimple(int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<OrderItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrderItem(int id)
        {
            var obj = Request["key"] != Keyapi ? new OrderItem() : _da.GetOrderItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrderDetailItem(Guid id)
        {
            var obj = Request["key"] != Keyapi ? new OrderDetailItem() : _da.GetOrderDetailItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMassageItemById(int id)
        {
            var obj = Request["key"] != Keyapi ? new OrderItem() : _da.GetMassageItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckOrder(string key, string listid, decimal sdate, decimal eDate, int timedo)
        {
            var obj = key == Keyapi && _da.CheckOrder(listid, sdate, eDate, timedo);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrder()
        {
            var obj = Request["key"] != Keyapi ? new List<OrderItem>() : _da.GetOrder();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListPriceAgencyByAgencyId(string key, string code)
        {
            var obj = key != Keyapi ? new List<PriceAgencyItem>() : _da.ListPriceAgencyByAgencyId(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByBedIdContactId(string key, string code, int bedid)
        {
            var obj = key != Keyapi ? new OrderGetItem() : _da.OrderByBedIdContactId(bedid, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductDefaultbyBedid(string key, string code, int bedid, int packetId)
        {
            var obj = key != Keyapi ? new PacketItem() : _da.ProductDefaultbyBedid(bedid, Agencyid(), packetId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByBedIdContactIdSpa(string key, string code, int bedid)
        {
            var obj = key != Keyapi ? new OrderGetItem() : _da.OrderByBedIdContactIdSpa(bedid, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderByBedId(string key, string code, int bedid)
        {
            var obj = key != Keyapi ? new OrderGetItem() : _da.OrderByBedId(bedid, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, string code, int id)
        {
            var obj = key != Keyapi ? new OrderItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListOrderByDateNow(string key)
        {
            var obj = key != Keyapi ? new List<OrderProcessItem>() : _da.ListOrderByDateNow();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListRestaurantByDateNow(string key)
        {
            var obj = key != Keyapi ? new List<OrderProcessItem>() : _da.ListRestaurantByDateNow();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Active(string key, int id)
        {
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetById(id);
                    model.IsActive = true;
                    _da.Save();
                    return Json(1, JsonRequestBehavior.AllowGet);

                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Bán hàng truyền thống
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <param name="json"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public ActionResult AddSales(string key, string code, string json, Guid UserId, string port)
        {
            var msg = new JsonMessage(false, "Hóa đơn đã được thêm mới.");
            var order = new Shop_Orders { AgencyId = Agencyid(), UserCreate = UserId, UserId = UserId };
            try
            {
                if (key == Keyapi)
                {
                    var dateCreated = DateTime.Now.TotalSeconds();
                    var keyorder = Request["KeyOrder"];
                    var lstOrder = (List<ModelSaleItem>)Session["AddSale"] ?? new List<ModelSaleItem>();
                    var model = lstOrder.FirstOrDefault(c => c.Key == Guid.Parse(keyorder));
                    if (model != null)
                    {
                        var lstM = model.SaleItems;
                        var lstDetail = new List<Shop_Order_Details>();
                        foreach (var saleItem in lstM)
                        {
                            var shopOrderDetails = new Shop_Order_Details
                            {
                                ProductID = saleItem.ProductID,
                                Quantity = saleItem.Quantity,
                                Status = (int)FDI.CORE.OrderStatus.Complete,
                                QuantityOld = 0,
                                Price = saleItem.Price,
                                DateCreated = dateCreated,
                                Discount = saleItem.PriceSale > 0
                                    ? saleItem.PriceSale
                                    : saleItem.PercentSale > 0 ? (saleItem.PercentSale * saleItem.Price / 100) : 0
                            };
                            if (saleItem.PromotionPs.Any())
                            {
                                shopOrderDetails.ContentPromotion = string.Join(",",
                                    saleItem.PromotionPs.Select(c => c.PromotionSPItems.Select(a => a.Title)));

                                lstDetail.AddRange(from shopOrderDetailse in saleItem.PromotionPs
                                                   from orderDetailse in shopOrderDetailse.PromotionSPItems
                                                   select new Shop_Order_Details
                                                   {
                                                       ProductID = orderDetailse.ProductID,
                                                       Quantity = orderDetailse.Quantity,
                                                       Status = (int)FDI.CORE.OrderStatus.Complete,
                                                       QuantityOld = 0,
                                                       Price = orderDetailse.Price,
                                                       DateCreated = dateCreated,
                                                       IsPromotion = true,
                                                       //PromotionID = shopOrderDetailse.ID,
                                                   });
                            }
                            lstDetail.Add(shopOrderDetails);
                        }
                        var lstO = model.PromotionOrder;
                        lstDetail.AddRange(from itemP in lstO
                                           from items in itemP.PromotionSPItems
                                           select new Shop_Order_Details
                                           {
                                               ProductID = items.ProductID,
                                               Quantity = items.Quantity,
                                               Status = (int)FDI.CORE.OrderStatus.Complete,
                                               QuantityOld = 0,
                                               Price = items.Price,
                                               DateCreated = dateCreated,
                                               IsPromotion = true,
                                               //PromotionID = itemP.ID,
                                           });
                        if (lstDetail.Any())
                        {
                            UpdateModel(order);
                            var dateOfSale = Request["DateOfSale_"];
                            order.StartDate = dateOfSale.StringToDate().TotalSeconds();
                            order.DateCreated = DateTime.Now.TotalSeconds();
                            order.TotalPrice = model.TotalPrice;
                            order.Status = (int)FDI.CORE.OrderStatus.Complete;
                            order.IsDelete = false;
                            //order.SalePercent = decimal.Parse(saleP ?? "0");
                            order.Shop_Order_Details = lstDetail;

                            if (!string.IsNullOrEmpty(order.SaleCode))
                            {
                                var temp = _dnSalesDa.GetSaleCodebyCode(order.SaleCode);
                                temp.IsUse = true;
                                temp.DateUse = DateTime.Now.TotalSeconds();
                                order.SalePercent = temp.DN_Sale.Price > 0
                                    ? temp.DN_Sale.Price
                                    : (temp.DN_Sale.Percent * order.TotalPrice / 100);
                                _dnSalesDa.Save();
                            }
                            var payment = model.Total - (order.PrizeMoney ?? 0) - (order.Discount * order.TotalPrice / 100) - (order.SalePercent ?? 0);
                            order.Payments = payment;
                            order.PriceReceipt = order.Payments;
                            if (!string.IsNullOrEmpty(order.Company) && !string.IsNullOrEmpty(order.CodeCompany))
                            {
                                order.Isinvoice = true;
                            }
                            _da.Add(order);
                            _da.Save();
                            if (payment.HasValue && payment > 0 && order.AgencyId.HasValue)
                                InsertSalary(order.ID, payment.Value, order.UserIdBedDeskID, order.BedDeskID ?? 0, order.AgencyId.Value);
                            if (order.CustomerID.HasValue && payment.HasValue && payment > 0)
                                InsertReward(Agencyid(), order.CustomerID.Value, payment, order.ID, dateCreated, order.PrizeMoney ?? 0);
                        }
                        else
                        {
                            msg.Erros = true;
                            msg.Message = "Bạn chưa chọn sản phẩm nào.";
                        }
                    }
                    else
                    {
                        msg.Erros = true;
                        msg.Message = "Bạn chưa chọn sản phẩm nào.";
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Hóa đơn chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSalesApp(string key, string listIdp, Guid UserId, string port, string code = "")
        {
            var msg = new JsonMessage(false, "Hóa đơn đã được thêm mới.");
            var order = new Shop_Orders { AgencyId = Agencyid(), UserCreate = UserId, UserId = UserId };
            try
            {
                if (key == Keyapi)
                {
                    var datenow = DateTime.Now.TotalSeconds();
                    var lstArr = FDIUtils.StringToListGuid(listIdp);
                    var lst = _productDa.GetListImportProductArrId(lstArr);
                    var lstDetail = (from item in lst
                                     let saleProducts = _dnSalesDa.GetSaleProduct(0, Agencyid())
                                     let pricesale = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.Price) : 0
                                     let percent = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.PercentSale) : 0
                                     let discount = pricesale + (item.Price * percent / 100)
                                     select new Shop_Order_Details
                                     {
                                         Barcode = item.BarCode,
                                         //ProductID = item.ProductID,
                                         Quantity = item.Quantity,
                                         QuantityOld = 0,
                                         Price = item.Price,
                                         TotalPrice = item.PriceNew - discount,
                                         Total = item.PriceNew,
                                         Discount = discount,
                                         DateCreated = datenow,
                                         Status = (int)FDI.CORE.OrderStatus.Complete,
                                         Value = item.Value,
                                     }).ToList();
                    if (lstDetail.Any())
                    {
                        var total = lstDetail.Sum(c => c.Total);
                        order.Shop_Order_Details = lstDetail;
                        order.TotalSaleSP = lstDetail.Sum(c => c.Discount);
                        order.DateCreated = datenow;
                        order.Status = (int)FDI.CORE.OrderStatus.Complete;
                        order.IsDelete = false;
                        order.StartDate = datenow;
                        decimal? priceCode = 0;
                        decimal? percentCode = 0;

                        if (!string.IsNullOrEmpty(code))
                        {
                            var temp = _dnSalesDa.GetSaleCodebyCode(code);
                            if (temp != null)
                            {
                                temp.IsUse = true;
                                temp.DateUse = DateTime.Now.TotalSeconds();
                                priceCode = (temp.DN_Sale.Price ?? 0);
                                percentCode = (((temp.DN_Sale.Percent ?? 0) * total / 100) ?? 0);
                                _dnSalesDa.Save();
                            }
                        }
                        var sale = _dnSalesDa.GetSaleByTotalOrder(total ?? 0, Agencyid());
                        var discsl = (((total * sale.Sum(p => p.PercentSale) / 100) ?? 0) + (sale.Sum(p => p.Price) ?? 0));
                        order.SalePercent = percentCode + ((total * sale.Sum(c => c.PercentSale) / 100) ?? 0);
                        order.SalePrice = priceCode + sale.Sum(c => c.Price);
                        var totalSale = percentCode + discsl + priceCode;
                        order.Total = total;
                        order.TotalPrice = total - totalSale; // tong gia chua co tru tich luy va chiet khau
                        order.Payments = order.TotalPrice;
                        order.PriceReceipt = order.Payments;
                        order.Type = (int)TypeOrder.Banle;
                        _da.Add(order);
                        _da.Save();
                    }
                    else
                    {
                        msg.Erros = true;
                        msg.Message = "Bạn chưa chọn sản phẩm nào.";
                    }


                }
            }
            catch (Exception)
            {

                msg.Erros = true;
                msg.Message = "Hóa đơn chưa được thêm mới.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string code, string json, Guid UserId, string port)
        {
            try
            {
                if (key == Keyapi)
                {
                    var dateCreated = DateTime.Now.TotalSeconds();
                    var lstDetail = GetListOrderDetailNewItem(code, dateCreated);
                    if (lstDetail.Any())
                    {
                        var obj = JsonConvert.DeserializeObject<OrderGetItem>(json);
                        var order = new Shop_Orders
                        {
                            StartDate = obj.StartDate,
                            BedDeskID = obj.BedDeskID,
                            EndDate = obj.EndDate,
                            TotalMinute = obj.Value,
                            AgencyId = Agencyid(),
                            IsDelete = false,
                            UserId = UserId,
                            DateCreated = dateCreated,
                            CustomerID = obj.CustomerID,
                            CustomerName = obj.CustomerName,
                            Address = obj.Address,
                            Mobile = obj.Mobile,
                            Discount = obj.Discount,
                            PrizeMoney = obj.PrizeMoney,
                            Status = (int)FDI.CORE.OrderStatus.Pending,
                            Note = obj.Note,
                            Shop_Order_Details = lstDetail,
                            TotalPrice = lstDetail.Sum(c => c.Quantity * c.Price)
                        };
                        var payment = order.TotalPrice - (order.PrizeMoney ?? 0);
                        order.Payments = obj.Payments > payment ? order.TotalPrice : payment;
                        order.PriceReceipt = order.Payments;
                        _da.Add(order);
                        _da.Save();
                        if (payment.HasValue && payment > 0 && order.AgencyId.HasValue)
                            InsertSalary(order.ID, payment.Value, order.UserIdBedDeskID, order.BedDeskID ?? 0, order.AgencyId.Value);
                        if (order.CustomerID.HasValue && payment.HasValue && payment > 0)
                            InsertReward(Agencyid(), order.CustomerID.Value, payment, order.ID, dateCreated, obj.PrizeMoney ?? 0);
                        return Json(order.ID, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Đăt hàng thời trang
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public ActionResult AddFashion(string key, string code, Guid UserId, string json)
        {
            try
            {
                if (key == Keyapi)
                {
                    var dateCreated = DateTime.Now.TotalSeconds();
                    var lstDetail = GetListOrderDetailNewItem(code, dateCreated);
                    if (lstDetail.Any())
                    {
                        var obj = JsonConvert.DeserializeObject<OrderGetItem>(json);
                        var order = new Shop_Orders
                        {
                            StartDate = dateCreated,
                            DateCreated = dateCreated,
                            EndDate = obj.EndDate,
                            AgencyId = Agencyid(),
                            IsDelete = false,
                            IsActive = false,
                            UserId = UserId,
                            CustomerID = obj.CustomerID,
                            CustomerName = obj.CustomerName,
                            Address = obj.Address,
                            Mobile = obj.Mobile,
                            Discount = obj.Discount,
                            PrizeMoney = obj.PrizeMoney,
                            Status = (int)FDI.CORE.OrderStatus.Pending,
                            Note = obj.Note,
                            Payments = obj.Payments,
                            Shop_Order_Details = lstDetail,
                            TotalPrice = lstDetail.Sum(c => c.Quantity * c.Price)
                        };
                        _da.Add(order);
                        var payment = order.TotalPrice - (order.PrizeMoney ?? 0);
                        order.Payments = order.Payments > payment ? payment : order.Payments;
                        order.PriceReceipt = order.Payments;
                        _da.Save();
                        return Json(order.ID, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Bán hàng massage
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <param name="code"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public ActionResult AddMassage(string key, string json, string code, Guid UserId, string port = ":3000")
        {
            try
            {
                if (key == Keyapi)
                {
                    var obj = JsonConvert.DeserializeObject<ModelOrderGetItem>(json);
                    var prizemoney = obj.PrizeMoney ?? 0;
                    if (obj.Lstproduct != null)
                    {
                        var dateCreated = DateTime.Now.TotalSeconds();
                        var idproduct = obj.Lstproduct.Split(',');
                        var listorder = new List<Shop_Orders>();

                        foreach (var item in obj.list)
                        {
                            decimal totalprice = 0;
                            var order = new Shop_Orders
                            {
                                StartDate = obj.StartDate,
                                BedDeskID = item.idbed,
                                EndDate = obj.EndDate,
                                TotalMinute = obj.Value,
                                UserIdBedDeskID = item.userid,
                                AgencyId = Agencyid(),
                                IsDelete = false,
                                Discount = obj.Discount,
                                UserId = UserId,
                                DateCreated = dateCreated,
                                CustomerID = obj.CustomerID,
                                CustomerName = obj.CustomerName,
                                Address = obj.Address,
                                PrizeMoney = obj.PrizeMoney,
                                Mobile = obj.Mobile,
                                IsEarly = obj.IsEarly,
                                Status = (int)FDI.CORE.OrderStatus.Complete,
                                Note = obj.Note,

                            };
                            if (!string.IsNullOrEmpty(obj.ContactId) && int.Parse(obj.ContactId) > 0) order.ContactOrderID = int.Parse(obj.ContactId);
                            order.PrizeMoney = prizemoney > order.Payments ? order.Payments : prizemoney;
                            foreach (string t in idproduct)
                            {
                                var product = _da.GetProductItem(int.Parse(t));
                                totalprice += product.PriceNew ?? 0;
                                var orderDetail = new Shop_Order_Details
                                {
                                    ProductID = int.Parse(t),
                                    Quantity = 1,
                                    Status = (int)FDI.CORE.OrderStatus.Complete,
                                    Price = product.PriceNew,
                                    DateCreated = dateCreated
                                };
                                order.Shop_Order_Details.Add(orderDetail);
                            }
                            order.TotalPrice = totalprice;
                            order.Payments = totalprice - (totalprice * obj.Discount / 100) - order.PrizeMoney;
                            order.PriceReceipt = order.Payments;
                            listorder.Add(order);

                        }
                        foreach (var shopOrderse in listorder)
                        {
                            _da.Add(shopOrderse);
                        }
                        _da.Save();
                        foreach (var shopOrderse in listorder)
                        {
                            if (shopOrderse.TotalPrice.HasValue && Agencyid() > 0)
                            {
                                InsertSalary(shopOrderse.ID, shopOrderse.TotalPrice.Value, shopOrderse.UserIdBedDeskID, shopOrderse.BedDeskID ?? 0, Agencyid());
                                if (shopOrderse.CustomerID != null) InsertReward(Agencyid(), shopOrderse.CustomerID.Value, shopOrderse.TotalPrice.Value, shopOrderse.ID, dateCreated, shopOrderse.PrizeMoney ?? 0);
                            }
                            if (!string.IsNullOrEmpty(obj.ContactId) && int.Parse(obj.ContactId) > 0)
                            {
                                var contact = _contactOrderDa.GetById(shopOrderse.ContactOrderID ?? 0);
                                contact.Status = (int)FDI.CORE.OrderStatus.Complete;
                                _contactOrderDa.Save();
                                var url = port + "/updatecontact/" + obj.ContactId;
                                Node(url);
                            }
                            if (shopOrderse.StartDate != null)
                            {
                                var jsonnew = new OrderProcessItem
                                {
                                    ID = shopOrderse.ID,
                                    BedDeskID = shopOrderse.BedDeskID,
                                    Minute = shopOrderse.TotalMinute,
                                    StartDate = (int)shopOrderse.StartDate,
                                    EndDate = (int)shopOrderse.EndDate,
                                    AgencyId = Agencyid(),
                                    UserName = obj.list.Where(m => m.userid == shopOrderse.UserIdBedDeskID).Select(m => m.UserName).FirstOrDefault(),
                                    Status = 0,
                                    TimeWait = obj.TimeWait ?? 0,
                                };
                                json = new JavaScriptSerializer().Serialize(jsonnew);
                            }
                            Node(port + "/addorder/" + json);
                        }
                        // Reward Customer
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateMassage(string key, string json, string port = ":3000")
        {
            try
            {
                if (key == Keyapi)
                {
                    var obj = JsonConvert.DeserializeObject<ModelOrderGetItem>(json);
                    var model = _da.GetById(obj.ID);
                    model.AddMinuteID = obj.AddMinuteID;
                    model.EndDate = obj.EndDate;
                    model.StartDate = obj.StartDate;
                    model.Payments = obj.Payments;
                    model.TotalPrice = obj.TotalPrice;
                    model.Note = obj.Note;
                    _da.Save();
                    Node(port + "/updateorderAdd/" + json);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
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
                    var prizemoney = obj.PrizeMoney ?? 0;
                    //var discount = obj.Discount ?? 0;
                    if (obj.Lstproduct != null)
                    {
                        var dateCreated = DateTime.Now.TotalSeconds();
                        var idproduct = obj.Lstproduct.Split(',');
                        var listorder = new List<Shop_Orders>();

                        foreach (var item in obj.list)
                        {
                            decimal totalprice = 0;
                            var order = new Shop_Orders
                            {
                                StartDate = obj.StartDate,
                                BedDeskID = item.idbed,
                                EndDate = obj.EndDate,
                                TotalMinute = obj.Value,
                                AgencyId = Agencyid(),
                                IsDelete = false,
                                Discount = obj.Discount,
                                UserId = UserId,
                                DateCreated = dateCreated,
                                CustomerID = obj.CustomerID,
                                CustomerName = obj.CustomerName,
                                Address = obj.Address,
                                PrizeMoney = obj.PrizeMoney,
                                Mobile = obj.Mobile,
                                IsEarly = obj.IsEarly,
                                Status = (int)FDI.CORE.OrderStatus.Complete,
                                Note = obj.Note,

                            };
                            if (!string.IsNullOrEmpty(obj.ContactId) && int.Parse(obj.ContactId) > 0)
                            {
                                order.ContactOrderID = int.Parse(obj.ContactId);
                            }
                            if (prizemoney > order.Payments)
                            {
                                order.PrizeMoney = order.Payments;
                                prizemoney = prizemoney - order.Payments ?? 0;
                                order.Payments = 0;
                            }
                            else
                            {
                                order.PrizeMoney = prizemoney;
                                prizemoney = 0;
                                order.Payments = order.Payments - prizemoney;
                            }

                            //if (order.BedDeskID != null)
                            //{
                            //    var userid = _bedDeskDeskDa.UserIdByBedDate(order.StartDate, item);
                            //    order.UserIdBedDeskID = userid;
                            //}
                            //add list orderdetail
                            var orderDetail = new Shop_Order_Details();
                            //foreach (var items in obj.ListItem)
                            //{
                            //    orderDetail = new Shop_Order_Details
                            //    {
                            //        ProductID = items.ID,
                            //        Quantity = 1,
                            //        Status = (int)FDI.CORE.OrderStatus.Complete,
                            //        Price = items.PriceNew,
                            //        DateCreated = dateCreated
                            //    };
                            //    order.Shop_Order_Details.Add(orderDetail);
                            //}
                            foreach (string t in idproduct)
                            {
                                var product = _da.GetProductItem(int.Parse(t));
                                totalprice += product.PriceNew ?? 0;
                                orderDetail = new Shop_Order_Details
                                {
                                    ProductID = int.Parse(t),
                                    Quantity = 1,
                                    Status = (int)FDI.CORE.OrderStatus.Complete,
                                    Price = product.PriceNew,
                                    DateCreated = dateCreated
                                };
                                order.Shop_Order_Details.Add(orderDetail);
                            }
                            order.TotalPrice = totalprice;
                            order.Payments = totalprice - (totalprice * obj.Discount / 100);
                            listorder.Add(order);

                        }
                        foreach (var shopOrderse in listorder)
                        {
                            _da.Add(shopOrderse);
                        }
                        _da.Save();
                        foreach (var shopOrderse in listorder)
                        {
                            if (shopOrderse.TotalPrice.HasValue && Agencyid() > 0)
                            {
                                InsertSalary(shopOrderse.ID, shopOrderse.TotalPrice.Value, shopOrderse.UserIdBedDeskID, shopOrderse.BedDeskID ?? 0, Agencyid());
                                if (shopOrderse.CustomerID != null) InsertReward(Agencyid(), shopOrderse.CustomerID.Value, shopOrderse.TotalPrice.Value, shopOrderse.ID, dateCreated, shopOrderse.PrizeMoney ?? 0);
                            }
                            if (!string.IsNullOrEmpty(obj.ContactId) && int.Parse(obj.ContactId) > 0)
                            {
                                var contact = _contactOrderDa.GetById(shopOrderse.ContactOrderID ?? 0);
                                contact.Status = (int)FDI.CORE.OrderStatus.Complete;
                                _contactOrderDa.Save();
                                var url = port + "/updatecontact/" + obj.ContactId;
                                Node(url);
                            }
                            var jsonnew = new OrderProcessItem
                            {
                                ID = shopOrderse.ID,
                                BedDeskID = shopOrderse.BedDeskID,
                                Minute = shopOrderse.TotalMinute,
                                StartDate = (int)shopOrderse.StartDate,
                                EndDate = (int)shopOrderse.EndDate,
                                AgencyId = Agencyid(),
                                Status = 0
                            };
                            json = new JavaScriptSerializer().Serialize(jsonnew);
                            Node(port + "/addorder/" + json);
                        }
                        // Reward Customer
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key)
        {
            try
            {
                if (key == Keyapi)
                {
                    var prizeMoney = Request["PrizeMoney"] ?? "0";
                    var discount = Request["Discount"] ?? "0";
                    var payments = Request["Payments"] ?? "0";
                    var model = _da.GetById(ItemId);
                    model.PrizeMoney = decimal.Parse(prizeMoney);
                    model.Discount = decimal.Parse(discount);
                    model.IsActive = true;
                    var total = model.Payments + decimal.Parse(payments);
                    var money = model.TotalPrice - decimal.Parse(prizeMoney);
                    model.Payments = total >= money ? money : total;
                    _da.Save();
                    if (model.Payments.HasValue && model.AgencyId.HasValue)
                        InsertSalary(model.ID, model.Payments.Value, model.UserIdBedDeskID, model.BedDeskID ?? 0, model.AgencyId.Value);
                    if (model.CustomerID.HasValue && model.Payments.HasValue)
                    {
                        var dateCreated = DateTime.Now.TotalSeconds();
                        InsertReward(Agencyid(), model.CustomerID.Value, model.Payments, model.ID, dateCreated, model.PrizeMoney ?? 0);
                    }
                    return Json(model.ID, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #region Xử lý đơn hàng nhà hàng

        /// <summary>
        /// Đơn hàng gọi món từ nhà hàng
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <param name="json"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public ActionResult AddRestaurant(string key, string code, string json, Guid UserId, string port)
        {
            try
            {
                if (key == Keyapi)
                {
                    var dateCreated = DateTime.Now.TotalSeconds();
                    var lstDetail = GetListOrderDetailNewItem(code, dateCreated);
                    if (lstDetail.Any())
                    {
                        var obj = JsonConvert.DeserializeObject<OrderGetItem>(json);
                        if (obj.list.Any())
                        {
                            var listbed = _da.GetListArrId(obj.list);
                            Shop_Orders order;
                            var check = false;
                            var listid = new List<int>();
                            if (obj.ID == 0)
                            {
                                order = new Shop_Orders
                                {
                                    StartDate = obj.StartDate,
                                    DN_Bed_Desk1 = listbed,
                                    BedDeskID = obj.list.FirstOrDefault(),
                                    EndDate = obj.EndDate,
                                    TotalMinute = obj.Value,
                                    AgencyId = Agencyid(),
                                    IsDelete = false,
                                    Discount = obj.Discount ?? 0,
                                    PrizeMoney = obj.PrizeMoney ?? 0,
                                    UserCreate = UserId,
                                    UserId = UserId,
                                    Payments = lstDetail.Sum(c => c.Quantity * c.Price) - (lstDetail.Sum(c => c.Quantity * c.Price) * obj.Discount / 100),
                                    DateCreated = dateCreated,
                                    UserIdBedDeskID = UserId,
                                    Status = (int)FDI.CORE.OrderStatus.Pending,
                                    Note = obj.Note,
                                    Shop_Order_Details = lstDetail,
                                    TotalPrice = lstDetail.Sum(c => c.Quantity * c.Price)
                                };
                                _da.Add(order);
                            }
                            else
                            {
                                order = _da.GetById(obj.ID);
                                listid = order.DN_Bed_Desk1.Select(m => m.ID).ToList();
                                order.DN_Bed_Desk1.Clear();
                                order.DN_Bed_Desk1 = listbed;
                                order.BedDeskID = obj.list.FirstOrDefault();
                                order.UserId = UserId;
                                order.Note = obj.Note;
                                var listold = _orderDetailDa.GetListByArrId(order.Shop_Order_Details.Where(o => o.Status < (int)FDI.CORE.OrderStatus.Cancelled).Select(m => m.GID).ToList());
                                // sửa
                                foreach (var item in lstDetail.Where(m => listold.Any(o => o.ProductID == m.ProductID)))
                                {
                                    var objorder = listold.FirstOrDefault(o => o.ProductID == item.ProductID);
                                    if (objorder != null && item.Quantity != objorder.Quantity)
                                    {
                                        if (objorder.Status == (int)FDI.CORE.OrderStatus.Pending) objorder.Quantity = item.Quantity;
                                        else
                                        {
                                            //objorder.QuantityOld = objorder.Quantity + objorder.QuantityOld;
                                            objorder.Quantity = item.Quantity;
                                            objorder.Status = (int)FDI.CORE.OrderStatus.Pending;
                                        }
                                        check = true;
                                    }
                                }
                                // hủy
                                foreach (var item in listold.Where(m => lstDetail.All(o => o.ProductID != m.ProductID)))
                                {
                                    item.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                                    check = true;
                                }
                                // thêm
                                foreach (var item in lstDetail.Where(m => listold.All(o => o.ProductID != m.ProductID)))
                                {
                                    item.OrderID = obj.ID;
                                    _orderDetailDa.Add(item);
                                    check = true;
                                }
                                _orderDetailDa.Save();
                                if (check)
                                {
                                    order.Status = (int)FDI.CORE.OrderStatus.Pending;
                                    order.TotalPrice = lstDetail.Where(m => m.Status < 4).Sum(c => c.Quantity * c.Price);
                                }
                            }
                            _da.Save();
                            HandlingNode(port, listid, obj.list, order, true, check);
                            return Json(order.ID, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Thanh toán đơn hàng nhà hàng
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public ActionResult CompleteRestaurant(string key, string code, string json, Guid UserId)
        {
            try
            {
                if (key == Keyapi)
                {
                    var obj = JsonConvert.DeserializeObject<OrderGetItem>(json);
                    var order = _da.GetById(obj.ID);
                    var dateCreated = DateTime.Now.TotalSeconds();
                    order.UserId = UserId;
                    order.CustomerID = obj.CustomerID;
                    order.EndDate = dateCreated;
                    order.CustomerName = obj.CustomerName;
                    order.Address = obj.Address;
                    order.Mobile = obj.Mobile;
                    order.Status = (int)FDI.CORE.OrderStatus.Complete;
                    order.PrizeMoney = obj.PrizeMoney;
                    order.Discount = obj.Discount;
                    var payment = order.TotalPrice - (order.PrizeMoney ?? 0);
                    order.Payments = obj.Payments > payment ? payment : obj.Payments;
                    order.PriceReceipt = order.Payments;
                    foreach (var items in order.Shop_Order_Details.Where(c => c.Status != 4))
                    {
                        items.Status = (int)FDI.CORE.OrderStatus.Complete;
                    }
                    _da.Save();
                    var url = ":4000/updateorder/" + obj.ID;
                    Node(url);
                    if (payment.HasValue && payment > 0 && order.AgencyId.HasValue)
                        InsertSalary(order.ID, payment.Value, order.UserIdBedDeskID, order.BedDeskID ?? 0, order.AgencyId.Value);
                    if (order.CustomerID.HasValue && order.TotalPrice.HasValue)
                        InsertReward(Agencyid(), order.CustomerID.Value, payment, order.ID, dateCreated, obj.PrizeMoney ?? 0);
                    return Json(order.ID, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// xử lý đơn đặt hàng thành đơn hàng
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ContactToOrder(string key, string code, int id, Guid UserId)
        {
            try
            {
                if (key == Keyapi)
                {
                    var dateCreated = (int)DateTime.Now.TotalSeconds();
                    var dateend = DateTime.Today.AddDays(1);
                    var endDate = (int)dateend.TotalSeconds();
                    var m = ((endDate - dateCreated) / 60);
                    var listb = _da.ContactToOrder(id, UserId, dateCreated, endDate, m);
                    var url = ":4000/updatecontact/" + id;
                    Node(url);
                    foreach (string json in listb.DN_Bed_Desk1.Select(item => new OrderProcessItem
                    {
                        ID = listb.ID,
                        BedDeskID = item.ID,
                        Minute = m,
                        StartDate = dateCreated,
                        EndDate = endDate,
                        AgencyId = Agencyid(),
                        IsEarly = false,
                        Status = 0
                    }).Select(jsonnew => new JavaScriptSerializer().Serialize(jsonnew)))
                    {
                        Node(":4000/addorder/" + json);
                    }
                    return Json(listb, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Thanh toán tách hóa đơn nhà hàng
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public ActionResult CopyRestaurant(string key, string code, string json, Guid UserId)
        {
            try
            {
                if (key == Keyapi)
                {
                    var dateCreated = DateTime.Now.TotalSeconds();
                    var lstDetail = GetListOrderDetailNewItem(code, dateCreated);
                    var obj = JsonConvert.DeserializeObject<OrderGetItem>(json);
                    var total = lstDetail.Sum(c => c.Quantity * c.Price);
                    var order = _da.GetById(obj.ID);
                    var ordernew = new Shop_Orders
                    {
                        UserId = UserId,
                        DN_Bed_Desk1 = order.DN_Bed_Desk1,
                        DateCreated = dateCreated,
                        Shop_Order_Details = lstDetail,
                        CustomerID = obj.CustomerID,
                        StartDate = order.StartDate,
                        EndDate = dateCreated,
                        CustomerName = obj.CustomerName,
                        Address = obj.Address,
                        Mobile = obj.Mobile,
                        Note = obj.Note,
                        IsDelete = false,
                        PrizeMoney = obj.PrizeMoney,
                        Discount = obj.Discount,
                        TotalMinute = order.TotalMinute,
                        AgencyId = Agencyid(),
                        Status = (int)FDI.CORE.OrderStatus.Complete,
                        TotalPrice = total
                    };
                    var payment = ordernew.TotalPrice - (ordernew.PrizeMoney ?? 0);
                    if (obj.IsDeposit == true)
                    {
                        payment = payment - (order.Deposits ?? 0);
                        ordernew.Payments = order.Payments > payment ? payment : order.Payments;
                        ordernew.PriceReceipt = ordernew.Payments;

                        ordernew.Deposits = order.Deposits;
                        ordernew.ContactOrderID = order.ContactOrderID;
                        order.ContactOrderID = null;
                        order.Deposits = 0;
                    }
                    else
                    {
                        ordernew.Payments = order.Payments > payment ? payment : order.Payments;
                        ordernew.PriceReceipt = ordernew.Payments;
                    }
                    foreach (var item in lstDetail.Where(m => order.Shop_Order_Details.Any(o => o.ProductID == m.ProductID && o.Status < 4)))
                    {
                        var objorder = order.Shop_Order_Details.FirstOrDefault(o => o.ProductID == item.ProductID);
                        if (objorder != null && item.Quantity > 0)
                        {
                            objorder.Quantity = objorder.Quantity - item.Quantity;
                        }
                    }
                    order.TotalPrice = order.TotalPrice - total;
                    _da.Add(ordernew);
                    _da.Save();
                    if (payment.HasValue && payment > 0 && ordernew.AgencyId.HasValue)
                        InsertSalary(ordernew.ID, payment.Value, ordernew.UserIdBedDeskID, order.BedDeskID ?? 0, ordernew.AgencyId.Value);
                    if (ordernew.CustomerID.HasValue && payment > 0)
                        InsertReward(Agencyid(), ordernew.CustomerID.Value, payment, ordernew.ID, dateCreated, obj.PrizeMoney ?? 0);
                    return Json(ordernew.ID, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Hủy hóa đơn mua hàng
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StopOrder(string key, string code, int id)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetById(id);
                if (obj != null && obj.AgencyId == Agencyid())
                {
                    obj.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                    obj.EndDate = DateTime.Now.TotalSeconds();
                    foreach (var item in obj.Shop_Order_Details)
                    {
                        item.Status = (int)FDI.CORE.OrderStatus.Cancelled;
                    }
                    foreach (var item in obj.RewardHistories)
                    {
                        item.IsDeleted = true;
                    }
                    foreach (var item in obj.ReceiveHistories)
                    {
                        item.IsDeleted = true;
                    }
                    foreach (var item in obj.WalletOrder_History)
                    {
                        item.IsDelete = true;
                    }
                    _da.Save();
                    var url = ":4000/updateorder/" + obj.ID;
                    Node(url);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProcessingRestaurant(string key, string code, int id, Guid UserId)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetById(id);
                if (obj != null && obj.AgencyId == Agencyid())
                {
                    obj.Status = (int)FDI.CORE.OrderStatus.Processing;
                    obj.UserId = UserId;
                    foreach (var item in obj.Shop_Order_Details.Where(m => m.Status == (int)FDI.CORE.OrderStatus.Pending))
                    {
                        item.Status = (int)FDI.CORE.OrderStatus.Processing;
                    }
                    _da.Save();
                    var url = ":4000/statuseorder/" + obj.ID + "/1";
                    Node(url);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xử lý chi tiết đơn hàng
        public List<Shop_Order_Details> GetListOrderDetailNewItem(string code, decimal date)
        {
            const string url = "Utility/GetListOrderDetailNewItem?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<OrderDetailNewItem>>(urlJson);
            return list.Where(m => m.Quantity > 0).Select(item => new Shop_Order_Details
            {
                GID = item.GID,
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Status = (int)FDI.CORE.OrderStatus.Complete,
                Price = item.Price,
                DateCreated = date,
                QuantityOld = 0,
                Discount = item.Discount,
                ContentPromotion = item.ContentPromotion,
            }).ToList();
        }
        #endregion

    }
}
