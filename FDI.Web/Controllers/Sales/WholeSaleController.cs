using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.DN_Sales;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Sales
{
    public class WholeSaleController : BaseController
    {
        //
        // GET: /WholeSale/
        readonly PaymentMethodAPI _methodApi = new PaymentMethodAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        private readonly OrdersDA _ordersDa = new OrdersDA();
        readonly StorageFreightWarehouseDA _freightWarehouseDa = new StorageFreightWarehouseDA("#");
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var lstOrder = (List<ModelWholeSaleItem>)Session["WholeSale"] ?? new List<ModelWholeSaleItem>();
            var model = new ModelWholeSaleItem();
            var paymentmethod = _methodApi.GetAll();
            if (lstOrder.Any())
            {
                model = lstOrder.FirstOrDefault();
                if (model != null)
                {
                    model.LstKey = lstOrder.Where(c => c.Key != model.Key).Select(c => c.Key).ToList();
                    model.PaymentMethodItems = paymentmethod;
                }
            }
            else
            {
                model.Key = Guid.NewGuid();
                model.AgentSaleItem = new AgentSaleItem();
                model.WholeSaleItems = new List<WholeSaleItem>();
                model.LstKey = new List<Guid>();
                model.TotalPrice = 0;
                model.PaymentMethodItems = paymentmethod;
                lstOrder.Add(model);
                Session["WholeSale"] = lstOrder;
            }
            return View(model);
        }
        public ActionResult Report()
        {
            var model = _agencyApi.GetItemById(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult AddSale(Guid key, int type)
        {
            var lstOrder = (List<ModelWholeSaleItem>)Session["WholeSale"] ?? new List<ModelWholeSaleItem>();
            var model = new ModelWholeSaleItem();
            if (lstOrder.Any())
            {
                model = lstOrder.FirstOrDefault(c => c.Key == key);
            }
            else
            {
                var paymentmethod = _methodApi.GetAll();
                model.Key = Guid.NewGuid();
                model.AgentSaleItem = new AgentSaleItem();
                model.WholeSaleItems = new List<WholeSaleItem>();
                model.LstKey = new List<Guid>();
                model.TotalPrice = 0;
                model.PaymentMethodItems = paymentmethod;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
            
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
                    var lstOrder = (List<ModelWholeSaleItem>)Session["WholeSale"] ?? new List<ModelWholeSaleItem>();
                    var temp = _freightWarehouseDa.GetByKey(Guid.Parse(keyorder));
                    if (temp.IsOrder == true)
                    {
                        msg = new JsonMessage
                        {
                            Erros = true,
                            Message = "Đơn Yêu cầu này đã được tạo đơn hàng"
                        };
                         return Json(msg);
                    }
                    else
                    {
                        if (lstOrder.Any())
                        {
                            var model = lstOrder.FirstOrDefault(c => c.Key == Guid.Parse(keyorder));
                            if (model != null)
                            {
                                var lstM = model.WholeSaleItems;
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
                                        Price = saleItem.Price,
                                        Barcode = saleItem.Barcode,
                                        DateCreated = dateCreated,
                                        Percent = saleItem.PercentSale,
                                        Value = saleItem.Value,
                                        ImportProductGID = saleItem.Idimport,
                                        PriceSale = saleItem.PriceSale,
                                        Discount = saleItem.Discount,
                                        TotalPrice = saleItem.TotalPrice * saleItem.Quantity,
                                        Total = saleItem.TotalPrice - saleItem.Discount,
                                    };
                                    lstDetail.Add(shopOrderDetails);
                                    if (saleItem.PromotionPs != null)
                                    {
                                        lstDetail.AddRange(from shopOrderDetailse in saleItem.PromotionPs
                                                           from orderDetailse in shopOrderDetailse.PromotionSPItems
                                                           select new Shop_Order_Details
                                                           {
                                                               ProductID = orderDetailse.ProductID,
                                                               Quantity = orderDetailse.Quantity,
                                                               Status = (int)FDI.CORE.OrderStatus.Complete,
                                                               QuantityOld = 0,
                                                               Price = orderDetailse.PriceSp,
                                                               Percent = orderDetailse.Percent,
                                                               PriceSale = orderDetailse.Price,
                                                               DateCreated = dateCreated,
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
                                if (lstO != null)
                                {
                                    lstDetail.AddRange(from itemP in lstO
                                                       from items in itemP.PromotionSPItems
                                                       select new Shop_Order_Details
                                                       {
                                                           ProductID = items.ProductID,
                                                           Quantity = items.Quantity,
                                                           Status = (int)FDI.CORE.OrderStatus.Complete,
                                                           QuantityOld = 0,
                                                           Price = items.PriceSp,
                                                           DateCreated = dateCreated,
                                                           IsPromotion = true,
                                                           PromotionProductID = items.ID,
                                                           Percent = items.Percent,
                                                           PriceSale = items.Price,
                                                           Discount = items.PriceSp + (items.PriceSp * items.Percent / 100),
                                                           TotalPrice = items.TotalPrice * items.Quantity,
                                                           ContentPromotion = items.Title,
                                                           Total = (items.TotalPrice - items.PriceSp - (items.PriceSp * items.Percent / 100)) * items.Quantity,
                                                       });
                                }

                                if (lstDetail.Any())
                                {
                                    UpdateModel(order);
                                    var dateOfSale = Request["DateOfSale_"];
                                    order.StartDate = dateOfSale.StringToDate().TotalSeconds();
                                    order.DateCreated = DateTime.Now.TotalSeconds();
                                    order.TotalPrice = model.Total;
                                    order.Total = model.Total;
                                    order.Status = (int)FDI.CORE.OrderStatus.Complete;
                                    order.IsDelete = false;
                                    order.UserId = UserItem.UserId;
                                    order.UserCreate = UserItem.UserId;
                                    order.AgencyId = UserItem.AgencyID;
                                    order.SalePercent = model.SalePercent;
                                    order.SalePrice = model.SalePrice;
                                    order.Shop_Order_Details = lstDetail;
                                    order.Discount = model.Discount;
                                    var payment = model.Total - (order.PrizeMoney ?? 0) - order.Discount - model.DiscountSale;
                                    order.Payments = payment;
                                    order.PriceReceipt = order.Payments;
                                    if (!string.IsNullOrEmpty(order.Company) && !string.IsNullOrEmpty(order.CodeCompany))
                                    {
                                        order.Isinvoice = true;
                                    }
                                    order.Type = (int)TypeOrder.Banbuon;
                                    _ordersDa.Add(order);
                                    _ordersDa.Save();
                                    // set trạng thái đơn yêu cầu đã đc đặt hàng
                                    temp.IsOrder = true;
                                    _freightWarehouseDa.Save();
                                    // xóa key tại session
                                    lstOrder.Remove(model);
                                    Session["WholeSale"] = lstOrder;
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
                                msg.Message = "Không thể tạo đơn hàng.";
                            }
                        }
                        else
                        {
                            msg.Erros = true;
                            msg.Message = "Bạn chưa chọn sản phẩm nào.";
                        }
                        
                    }
                    
                    break;
                default:
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg);
        }

    }
}
