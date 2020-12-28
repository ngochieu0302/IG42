using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Sales
{
    public class WholeSaleOController : BaseController
    {
        //
        // GET: /WholeSaleO/
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        readonly StorageWareHouseDA _storageWareHouseDa = new StorageWareHouseDA("#");
        private readonly OrdersDA _ordersDa = new OrdersDA();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var lstOrder = (List<ModelWholeSaleOItem>)Session["WholeSaleO"] ?? new List<ModelWholeSaleOItem>();
            var model = new ModelWholeSaleOItem();
            if (lstOrder.Any())
            {
                model = lstOrder.FirstOrDefault();
                if (model != null)
                {
                    model.LstKey = lstOrder.Where(c => c.Key != model.Key).Select(c => c.Key).ToList();
                }
            }
            else
            {
                model.Key = Guid.NewGuid();
                model.AgentSaleItem = new AgentSaleItem();
                model.WholeSaleItems = new List<WholeSaleOItem>();
                model.LstKey = new List<Guid>();
                model.TotalPrice = 0;
                lstOrder.Add(model);
                Session["WholeSaleO"] = lstOrder;
            }
            return View(model);
        }
        public ActionResult AddSale(Guid key, int type)
        {
            var lstOrder = (List<ModelWholeSaleItem>)Session["WholeSaleO"] ?? new List<ModelWholeSaleItem>();
            var model = new ModelWholeSaleItem();
            if (lstOrder.Any())
            {
                model = lstOrder.FirstOrDefault(c => c.Key == key);
            }
            else
            {
                model.Key = Guid.NewGuid();
                model.AgentSaleItem = new AgentSaleItem();
                model.WholeSaleItems = new List<WholeSaleItem>();
                model.LstKey = new List<Guid>();
                model.TotalPrice = 0;
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Report()
        {
            var model = _agencyApi.GetItemById(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult Viewdetai(int id)
        {
            var model = _storageWareHouseDa.GetListDNImportItem(id);
            return View(model);
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
                    var lstOrder = (List<ModelWholeSaleOItem>)Session["WholeSaleO"] ?? new List<ModelWholeSaleOItem>();
                    var temp = _storageWareHouseDa.GetByKey(Guid.Parse(keyorder));
                  
                        if (lstOrder.Any())
                        {
                            var model = lstOrder.FirstOrDefault(c => c.Key == Guid.Parse(keyorder));
                            if (model != null)
                            {
                                var lstM = model.WholeSaleItems;
                                var lstDetail = new List<Shop_Order_Details>();
                                foreach (var saleItem in lstM)
                                {
                                    var lstDetail1 = saleItem.ListDnImportItems.Select(item => new Shop_Order_Details
                                    {
                                        ProductID = item.ProductID,
                                        ProductValueID = item.ProductValueID,
                                        Quantity = item.Quantity,
                                        Status = (int)FDI.CORE.OrderStatus.Complete,
                                        QuantityOld = 0,
                                        IsPromotion = false,
                                        CateValueID = saleItem.CateValueID,
                                        Price = item.Price ?? 0,
                                        Barcode = item.Barcode,
                                        DateCreated = dateCreated,
                                        Percent = item.PercentSale,
                                        Value = item.Value,
                                        ImportProductGID = item.Idimport,
                                        PriceSale = item.PriceSale,
                                        Discount = item.Discount,
                                        TotalPrice = item.TotalPrice * item.Quantity,
                                        Total = item.Price * item.Quantity * item.Value,
                                    }).ToList();
                                    lstDetail.AddRange(lstDetail1);
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
                                    order.Discount = model.Discount ?? 0;
                                    var payment = model.Total - (order.PrizeMoney ?? 0) - order.Discount -
                                                  model.DiscountSale;
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
                                   
                                    _storageWareHouseDa.Save();
                                    // xóa key tại session
                                    lstOrder.Remove(model);
                                    Session["WholeSaleO"] = lstOrder;
                                }

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
