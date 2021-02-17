using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.DN_Sales;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.Web.Controllers
{
    public class SaleTPController : BaseController
    {
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        readonly ProductAPI _api = new ProductAPI();
        readonly DNPromotionDA _da = new DNPromotionDA("#");
        private readonly OrdersDA _ordersDa = new OrdersDA();
        private readonly CustomerAPI _customerApi = new CustomerAPI();
        readonly DNSalesDA _dnSalesDa = new DNSalesDA("#");
        readonly PaymentMethodAPI _methodApi = new PaymentMethodAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var lstOrder = (List<ModelSaleItem>)Session["AddSale"] ?? new List<ModelSaleItem>();
            var model = new ModelSaleItem();
            //var paymentmethod = _methodApi.GetAll();
            if (lstOrder.Any())
            {
                model = lstOrder.FirstOrDefault();
                if (model != null)
                {
                    model.LstKey = lstOrder.Where(c => c.Key != model.Key).Select(c => c.Key).ToList();
                    //model.PaymentMethodItems = paymentmethod;
                }
            }
            else
            {
                model.Key = Guid.NewGuid();
                model.CusSaleItem = new CusSaleItem();
                model.SaleItems = new List<SaleItem>();
                model.LstKey = new List<Guid>();
                model.TotalPrice = 0;
                //model.PaymentMethodItems = paymentmethod;
                lstOrder.Add(model);
                Session["AddSale"] = lstOrder;
            }
            return View(model);
        }

        public ActionResult AddSale(string json, Guid key, int type, Guid pid, int sl = 0, decimal price = 0, string code = "")
        {
            var lstOrder = (List<ModelSaleItem>)Session["AddSale"] ?? new List<ModelSaleItem>();
            var model = lstOrder.FirstOrDefault(c => c.Key == key);
            decimal? voucher = 0;
            if (model != null)
            {
                model.Check = false;
                model.SaleCode = "";
                model.VoucherPer =  0;
                model.VoucherPrice = 0;
                switch (type)
                {
                    case 1:
                        var item = JsonConvert.DeserializeObject<SaleItem>(json);
                        var saleProducts = _dnSalesDa.GetSaleProduct(item.ProductdetailID, UserItem.AgencyID);
                        var pricesale = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.Price) : 0;
                        var percent = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.PercentSale) : 0;
                        item.Key = CodeLogin();
                        item.PriceSale = pricesale;
                        item.PercentSale = percent;
                        var discount = pricesale + (item.Price * item.Value * percent / 100);
                        item.Discount = discount;
                        item.TotalPrice = (item.Price * item.Value) - discount;
                        var itemnew = model.SaleItems.FirstOrDefault(c => c.Idimport == item.Idimport);
                        if (itemnew == null)
                        {
                            //var checkpromotion = _da.GetPromotionProduct(item.ProductdetailID, UserItem.AgencyID, item.Quantity ?? 1);
                            //item.PromotionPs = checkpromotion;
                            //model.SaleItems.Add(item);
                        }
                        break;
                    case 2:
                        var itemRemove = model.SaleItems.SingleOrDefault(c => c.Idimport == pid);
                        if (itemRemove != null) model.SaleItems.Remove(itemRemove);
                        break;
                    case 3:
                        var itemQuan = model.SaleItems.FirstOrDefault(c => c.Idimport == pid);
                        if (itemQuan != null)
                        {
                            //itemQuan.Quantity = sl;
                            //var checkpromotion = _da.GetPromotionProduct(itemQuan.ProductdetailID, UserItem.AgencyID, sl);
                            //itemQuan.PromotionPs = checkpromotion;
                        }
                        break;
                    case 4:
                        var itemPrice = model.SaleItems.FirstOrDefault(c => c.Idimport == pid);
                        if (itemPrice != null)
                        {
                            discount = itemPrice.PriceSale + (price * itemPrice.PercentSale / 100);
                            itemPrice.Price = price;
                            itemPrice.TotalPrice = price - discount;
                        }
                        break;
                    case 5:
                        var itemCus = JsonConvert.DeserializeObject<CusSaleItem>(json);
                        if (itemCus != null) model.CusSaleItem = itemCus;
                        break;
                    case 6:
                        if (lstOrder.Count == 1)
                        {
                            model.SaleItems.Clear();
                            model.CusSaleItem = new CusSaleItem();
                            model.Check = true;
                        }
                        else lstOrder.Remove(model);
                        break;
                    case 7:
                        if (!string.IsNullOrEmpty(code))
                        {
                            var check = _dnSalesDa.CheckSaleCode(code, UserItem.AgencyID);
                            var totalP = model.SaleItems.Sum(c => c.TotalPrice * c.Quantity);
                            voucher = check.PriceSale > 0
                                ? check.PriceSale
                                : (check.Percent > 0 ? (check.Percent * totalP / 100) : 0);
                            model.SaleCode = code ?? "";
                            model.VoucherPer = check.Percent ?? 0;
                            model.VoucherPrice= check.PriceSale ?? 0;
                        }
                        break;
                }
                var total = model.SaleItems.Sum(c => c.TotalPrice * c.Quantity);
                var sale = _dnSalesDa.GetSaleByTotalOrderBirthday((decimal)total, (model.CusSaleItem != null ? (model.CusSaleItem.Birthday ?? 0) : 0), UserItem.AgencyID);
                model.SaleOrder = sale;
                var disc = (total * sale.Sum(p => p.PercentSale) / 100) + sale.Sum(p => p.Price) + voucher;
                model.Total = model.SaleItems.Sum(c => c.Price * c.Quantity * c.Value);
                model.Discount =   disc ?? 0;
                model.SalePercent = sale.Sum(p => p.PercentSale);
                model.SalePrice = sale.Sum(p => p.Price);
                model.TotalPrice = total;
                //var promotion = _da.GetPromotionOrder(UserItem.AgencyID, (decimal)total);
                //model.PromotionOrder = promotion;
                //model.TotalSaleSP = total;
                Session["AddSale"] = lstOrder;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrder()
        {
            var lstOrder = (List<ModelSaleItem>)Session["AddSale"] ?? new List<ModelSaleItem>();
            var model = new ModelSaleItem
            {
                Key = Guid.NewGuid(),
                CusSaleItem = new CusSaleItem(),
                SaleItems = new List<SaleItem>(),
                LstKey = new List<Guid>(),
                TotalPrice = 0
            };
            lstOrder.Add(model);
            Session["AddSale"] = lstOrder;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            var model = _agencyApi.GetItemById(UserItem.AgencyID);
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            return View();
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
            var ltsResults = _api.GetListAutoOne(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
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

        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var order = new Shop_Orders();
            switch (DoAction)
            {
                case ActionType.Add:
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
                                IsPromotion = false,
                                Value = saleItem.Value,
                                Price = saleItem.Price,
                                Barcode = saleItem.Barcode,
                                ImportProductGID = saleItem.Idimport,
                                //DateCreated = dateCreated,
                                Percent = saleItem.PercentSale,
                                PriceSale = saleItem.PriceSale,
                                Discount = saleItem.Discount,
                                TotalPrice = saleItem.TotalPrice * saleItem.Quantity,
                                Total = saleItem.TotalPrice - saleItem.Discount,
                            };
                            lstDetail.Add(shopOrderDetails);
                            if (saleItem.PromotionPs.Any())
                            {
                                lstDetail.AddRange(from shopOrderDetailse in saleItem.PromotionPs
                                                   from orderDetailse in shopOrderDetailse.PromotionSPItems
                                                   select new Shop_Order_Details
                                                   {
                                                       ProductID = orderDetailse.ProductID,
                                                       Quantity = orderDetailse.Quantity,
                                                       Status = (int)FDI.CORE.OrderStatus.Complete,
                                                       QuantityOld = 0,
                                                       Price = orderDetailse.PriceSp ??0,
                                                       Percent = orderDetailse.Percent,
                                                       PriceSale = orderDetailse.Price,
                                                       //DateCreated = dateCreated,
                                                       IsPromotion = true,
                                                       PromotionProductID = orderDetailse.ID,
                                                       Discount = orderDetailse.Percent * orderDetailse.Quantity * orderDetailse.PriceSp / 100 + orderDetailse.Price,
                                                       TotalPrice = orderDetailse.TotalPrice * orderDetailse.Quantity,
                                                       Total = orderDetailse.TotalPrice * orderDetailse.Quantity - (orderDetailse.Percent * orderDetailse.Quantity * orderDetailse.PriceSp + orderDetailse.Price),
                                                       ContentPromotion = orderDetailse.Name
                                                   });
                            }

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
                                               Price = items.PriceSp ??0,
                                               //DateCreated = dateCreated,
                                               IsPromotion = true,
                                               PromotionProductID = items.ID,
                                               Percent = items.Percent,
                                               PriceSale = items.Price,
                                               Discount = items.PriceSp + (items.PriceSp * items.Percent / 100),
                                               TotalPrice = items.TotalPrice * items.Quantity,
                                               ContentPromotion = items.Title,
                                               Total = (items.TotalPrice - items.PriceSp - (items.PriceSp * items.Percent / 100))*items.Quantity,
                                           });
                        if (lstDetail.Any())
                        {
                            UpdateModel(order);
                            var dateOfSale = Request["DateOfSale_"];
                            order.StartDate = dateOfSale.StringToDate().TotalSeconds();
                            order.DateCreated = DateTime.Now.TotalSeconds();
                            order.TotalPrice = model.TotalPrice;
                            order.Total = model.Total;
                            order.Status = (int)FDI.CORE.OrderStatus.Complete;
                            order.IsDelete = false;
                            order.UserId = UserItem.UserId;
                            order.UserCreate = UserItem.UserId;
                            order.AgencyId = UserItem.AgencyID;
                            order.SalePercent = model.SalePercent;
                            order.SalePrice = model.SalePrice;
                            order.Shop_Order_Details = lstDetail;
                            decimal? price = 0;
                            if (!string.IsNullOrEmpty(order.SaleCode))
                            {
                                var temp = _dnSalesDa.GetSaleCodebyCode(order.SaleCode);
                                temp.IsUse = true;
                                temp.DateUse = DateTime.Now.TotalSeconds();
                                price = (temp.DN_Sale.Price ?? 0) + ((temp.DN_Sale.Percent ?? 0) * order.TotalPrice / 100);
                                _dnSalesDa.Save();
                            }
                            order.Discount = model.Discount;
                            var payment = model.Total - (order.PrizeMoney ?? 0) - order.Discount - price ?? 0;
                            order.Payments = payment;
                            order.PriceReceipt = order.Payments;
                            if (!string.IsNullOrEmpty(order.Company) && !string.IsNullOrEmpty(order.CodeCompany))
                            {
                                order.Isinvoice = true;
                            }
                            _ordersDa.Add(order);
                            _ordersDa.Save();
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
                    break;
                default:
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg);
        }

        public ActionResult ApplyCode(string salecode)
        {
            JsonMessage msg;
            var check = _dnSalesDa.CheckSaleCode(salecode, UserItem.AgencyID);
            if (check == null)
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Mã giảm giá không chính xác Hoặc đã hết hạn"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            msg = new JsonMessage
            {
                Erros = false,
                Message = "Áp dụng mã giảm giá thành công.!",
                SaleCodeItem = check
            };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
