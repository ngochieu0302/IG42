using FDI.DA;
using FDI.Simple;
using FDI.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FDI.Utils;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.WebParts;
using FDI.DA.DA;
using FDI.MvcAPI.Common;
using Newtonsoft.Json;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    [CustomerAuthorize]
    public class OrderAppController : BaseAppApiController
    {

        OrderAppIG4DA orderDA = new OrderAppIG4DA();
        readonly CustomerAppIG4DA _customerDa = new CustomerAppIG4DA("#");
        readonly RewardHistoryAppIG4DA _rewardHistoryDa = new RewardHistoryAppIG4DA("#");
        readonly Shop_ProductAppIG4DA _productDa = new Shop_ProductAppIG4DA("#");
        public static string Momo = ConfigurationManager.AppSettings["UrlMomo"];
        private readonly WalletCustomerAppIG4DA _walletCustomerDa = new WalletCustomerAppIG4DA("#");
        private readonly CashOutWalletAppIG4DA _cashOutWalletDa = new CashOutWalletAppIG4DA("");
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Booking(List<OrderAppIG4Item> datas)
        {
            try
            {

                decimal? totalall = 0;
                foreach (var data in datas)
                {
                    if (data.LisOrderDetailItems == null || !data.LisOrderDetailItems.Any())
                    {
                        return Json(new JsonMessage(1000, "Không có sản phẩm trong giỏ hàng"),
                            JsonRequestBehavior.AllowGet);
                    }

                    SaleCode coupon = null;
                    if (!string.IsNullOrEmpty(data.Coupon))
                    {
                        coupon = _productDa.GetSaleCodeUseByCode(data.Coupon);
                        //if (coupon == null)
                        //{
                        //    return Json(new JsonMessage(1000, "Coupon không tồn tại"));
                        //}
                    }

                    var order = new Shop_Orders()
                    {
                        Address = data.Address,
                        CustomerID = CustomerId,
                        Longitude = data.Longitude,
                        Latitude = data.Latitude,
                        Mobile = data.Mobile,
                        CustomerName = data.CustomerName,
                        DateCreated = DateTime.Now.TotalSeconds(),
                        Code = FDIUtils.RandomCode(12),
                        FeeShip = data.FeeShip ?? 0,
                        StatusPayment = (int) PaymentOrder.Process,
                        Coupon = data.Coupon,
                        ShopID = data.ShopID,
                        //Discount = data.Discount,
                        CouponPrice = coupon?.DN_Sale.Price ?? 0,
                        PaymentMethodId = data.PaymentmethodId,
                        Status = (int) StatusOrder.Create,
                        Note = data.Note,
                        CustomerAddressID = data.CustomerAddressID,
                    };
                    order.CouponPrice = !string.IsNullOrEmpty(data.Coupon)
                        ? coupon.DN_Sale.Price ?? 0
                        : order.CouponPrice = 0;

                    foreach (var product in data.LisOrderDetailItems)
                    {
                        var productData = _productDa.GetProductItem(product.ProductId ?? 0);
                        if (productData == null)
                        {
                            return Json(new JsonMessage(1000, "Sản phẩm không tồn tại"), JsonRequestBehavior.AllowGet);
                        }

                        var item = new Shop_Order_Details()
                        {
                            ProductID = product.ProductId,
                            Price = productData.PriceNew ?? 0,
                            Quantity = product.Quantity ?? 1,
                            Status = (int) StatusOrder.Create,
                            TotalPrice = productData.PriceNew * (product.Quantity ?? 1),
                            StatusPayment = (int) PaymentOrder.Process,
                            IsPrestige = product.IsPrestige,
                        };
                        order.Shop_Order_Details.Add(item);
                    }

                    var total = order.Shop_Order_Details.Sum(m => m.Price * m.Quantity);
                    order.Total = total;
                    order.TotalPrice = total;
                    order.Payments = total - order.CouponPrice + data.FeeShip;
                    totalall += total - order.CouponPrice + data.FeeShip;
                    orderDA.Add(order);
                    if (coupon != null)
                    {
                        coupon.IsUse = true;
                    }
                }
                var check = CheckWallets(totalall, CustomerId);
                if (check)
                {
                  await orderDA.SaveAsync();
                  await _productDa.SaveAsync();
                }

            }
            catch (Exception e)
            {
                return Json(new JsonMessage(404, e.ToString()), JsonRequestBehavior.AllowGet);

            }

            return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDiscount()
        {
            // var abc
            var discount = _walletCustomerDa.GetConfigItem();
            return Json(new BaseResponse<ConfigItemAppIG4> { Code = 200, Data = discount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListProductByNew(int shopid, int status, int page, int take)
        {
            var data = orderDA.GetListProductByNew(shopid, status, page, take);
            return Json(new BaseResponse<List<OrderShopAppIG4Item>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListProductByCustomerID()
        {
            var data = orderDA.GetListSimpleByCusId(CustomerId);
            return Json(new BaseResponse<List<OrderAppIG4Item>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult UpdateStatus(int orderId, int type)
        {

            var data = orderDA.GetById(orderId);
            data.Status = type;
            data.Check = 1;
            data.DateUpdateStatus = DateTime.Now.TotalSeconds();
            var totalPrice = data.Payments;
            //var TotalPricegstore = data.OrderTotal - ((data.Discount * data.OrderTotal / 100)) + data.FeeShip;
            //var config = _walletCustomerDa.GetConfig();
            //var totaldis = data.Discount + config.Percent;
            foreach (var item in data.Shop_Order_Details)
            {
                item.Status = type;
                item.DateUpdateStatus = DateTime.Now.TotalSeconds();
                item.Check = 1;
            }
            orderDA.Save();

            if (type == (int)StatusOrder.Complete)
            {
                // tru tien khach hang
                var cashout = new CashOutWallet
                {
                    CustomerID = data.CustomerID,
                    TotalPrice = totalPrice ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    OrderID = data.ID,
                    Type = 1,
                    Code = data.Code,
                };
                _cashOutWalletDa.Add(cashout);
                _cashOutWalletDa.Save();
                decimal? totak = 0;
                foreach (var items in data.Shop_Order_Details)
                {
                    var k = items.Shop_Product.Category.Profit;
                    totak += (items.Shop_Product.Product_Size != null ? (decimal)items.Shop_Product.Product_Size.Value : 1) * items.Quantity * k * 1000;
                    var shop = _customerDa.GetItemByID(data.ShopID ?? 0);
                    var config = _walletCustomerDa.GetConfig();
                    var totalshop = (data.Total - totak) + (totak * shop.PercentDiscount / 100) + data.FeeShip;
                    var walletcus = new WalletCustomer
                    {
                        CustomerID = data.ShopID,
                        TotalPrice = totalshop ?? 0,
                        DateCreate = DateTime.Now.TotalSeconds(),
                        IsActive = true,
                        IsDelete = false,
                        Type = 2,
                        Transaction_no = data.Code,
                        CustomerIDR = data.CustomerID,
                    };
                    _walletCustomerDa.Add(walletcus);
                    _walletCustomerDa.Save();
                    // kiem tra level khach hang.
                    //UpdateLevelCustomer(data.CustomerID ?? 0);
                    var cus = _customerDa.GetItemByID(data.CustomerID ?? 0);
                    //var shopsucess = orderDA.GetNotifyById(5);
                    //var tokenshop = shop.tokenDevice;
                    //Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", totalshop.Money()).Replace("{code}", data.Code).Replace("{customer}", data.Customer.FullName), tokenshop, shopsucess.ID.ToString());
                    
                    var bonusItems = _customerDa.ListBonusTypeItems();
                    #region hoa hồng khách hàng và shop ký gửi
                    var iskg = data.Customer.Type == 2;
                    if (!iskg)
                    {
                        InsertRewardOrderCustomer(cus, config, totak, data.ID, bonusItems);
                        InsertRewardOrderAgency(shop, config, totak, data.ID, bonusItems);
                    }
                    else
                    {
                        // chiết khấu shop ký gửi
                        //decimal totalpres = data.Shop_Order_Details.Where(detail => detail.IsPrestige == true).Sum(detail => detail.TotalPrice ?? 0);
                        //decimal totalnopres = data.Shop_Order_Details.Where(detail => detail.IsPrestige == false || !detail.IsPrestige.HasValue).Sum(detail => detail.TotalPrice ?? 0);
                        //if (totalpres > 0)
                        //{
                        //    //InsertRewardCustomer(data.Customer.ListID, data.Customer.ParentID ?? 0, totalpres, data.ID, bonusItems, 2, data.ShopID ?? 0);
                        //    InsertRewardCustomer(data.Customer.ListID, data.Customer.ParentID ?? 0, totalpres, data.ID, bonusItems, 2, data.ShopID ?? 0);
                        //}
                        //if (totalnopres > 0)
                        //{
                        //    InsertRewardCustomer(data.Customer.ListID,data.Customer.ParentID ?? 0, totalnopres, data.ID, bonusItems);
                        //}
                    }
                    #endregion
                }
                #region // xử lý trừ số lượng trong kho của shop
                foreach (var item in data.Shop_Order_Details)
                {
                    var product = _productDa.GetById(item.ProductID ?? 0);
                    product.QuantityOut += item.Quantity;
                    _productDa.Save();
                }


                #endregion
            }
            #region push notify
            //var notify = orderDA.GetNotifyById(8);
            //var gettoken = _customerDa.GetItemByID(data.CustomerID ?? 0);
            //var token = gettoken.tokenDevice;
            //if (type == (int)StatusOrder.Process)
            //{
            //    Pushnotifycation(notify.Title, notify.Content.Replace("{status}", "đang được xử lý").Replace("{shop}", data.Customer.FullName).Replace("{code}", data.Code), token, notify.ID.ToString());
            //}
            //if (type == (int)StatusOrder.Shipping)
            //{
            //    Pushnotifycation(notify.Title, notify.Content.Replace("{status}", "đang được giao").Replace("{shop}", data.Customer.FullName).Replace("{code}", data.Code), token, notify.ID.ToString());
            //}
            //if (type == (int)StatusOrder.Cancel)
            //{
            //    Pushnotifycation(notify.Title, notify.Content.Replace("{status}", "vừa bị hủy bởi chủ cửa hàng.!").Replace("{shop}", data.Customer.FullName).Replace("{code}", data.Code), token, notify.ID.ToString());
            //}
            //if (type == (int)StatusOrder.Complete)
            //{
            //    notify = orderDA.GetNotifyById(3);
            //    Pushnotifycation(notify.Title, notify.Content.Replace("{shop}", data.Customer.FullName).Replace("{price}", totalPrice.Money()).Replace("{code}", data.Code), token, notify.ID.ToString());
            //}
#endregion
            return Json(new BaseResponse<List<ProductItem>> { Code = 200, Message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetCoupon(string code)
        {
            var coupon = _productDa.GetSaleCodeUseByCode(code);
            if (coupon == null)
            {
                return Json(new JsonMessage(1000, "Coupon không tồn tại"), JsonRequestBehavior.AllowGet);
            }
            var data = new SaleCodeAppIG4Item
            {
                Code = coupon.Code,
                Price = coupon.DN_Sale.Price ?? 0
            };
            return Json(new BaseResponse<SaleCodeAppIG4Item> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }
        public static string publickey = "<RSAKeyValue><Modulus>r+YDcXLRLkdA9uaBddhz8gvkh3CoPNVehawkSXt9VTPoQApdWeVyTkLTXsKSyASdyIqhbQo4rxQ1k38CPvWwQFM21V46rtq5S2j/MeXHUmjlTd22VCEno8vBN+g26m/aQC5R6WAf0PjZ9bu1R3BWaHR51jVBF7eiG8ALNj5M9hMK+tSmtrQc0BiWlUYR232zK8zXElT9Ls5he+GXvRWl0hB+SSXS2KqoN9nwPc+vuTxxisvMS0PW6o3VG1tCP2gQTLdsTnJ2bQZYkx4WQIY9bLucUYAP84t3Ymd2XC9xg6hab6sBdGjbZiReLd4gOxlB7KJPH/7PRCI28TJKAbqqAXREvDv4jUeob9zFTlZpcbsdJy5PURUq0uhZzf/BmwEpokdrjdV5V7btuL4fLNjqyKIDj9spXnk9ohvIFBPmA+eu/9xKxeOwFTSqEvZcJzKDg87K4AIN/T6UBBDI3qGr9eCVQK0BPhw9wHHyELzFQOJKnqtqCBHMXNk8rB6wu+k5RuZjzVmULKNRnNgJ7moxkasyEs/lPYg5uPvCBtCDroEviFdI6+gg7aRaypIcirSUeiqDfoTTgDI2PNH7RNyvXMuqWU+KJ3+nwTi32dwJ02jShMXb22e3SwxkxIMntMWCyTegb5W2qR8vkiPQm4ZOUFHD7a+LajAbcwd2BZZMAjU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public static string PartnerCode = "MOMOYIQU20200912";
        public static string serectkey = "Mod7W9MkCZ5ZhAIvQDhahaUlDMn2Boqd";
        [AllowAnonymous]
        public async Task<ActionResult> PaymentMoMoServer(dataItem item)
        {
            var refId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var model = new HashMoMoItem
            {
                amount = item.amount ?? 0,
                partnerRefId = refId.ToString(),
                partnerCode = PartnerCode,
                //partnerName = "Công ty cổ phần công nghệ G-store",
                partnerTransId = DateTime.Now.TotalSeconds().ToString(),
                description = item.description ?? "",
            };
            var json = new JavaScriptSerializer().Serialize(model);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(4096)) //KeySize
            {
                try
                {
                    // MoMo's public key has format PEM.
                    // You must convert it to XML format. Recommend tool: https://superdry.apphb.com/tools/online-rsa-key-converter
                    rsa.FromXmlString(publickey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            var data1 = new ProcessMoMoItem
            {
                customerNumber = item.customerNumber ?? "",
                partnerCode = PartnerCode,
                partnerRefId = refId.ToString(),
                appData = item.token ?? "",
                hash = result,
                payType = 3,
                description = item.description ?? "",
                version = 2
            };
            var url = Momo + "pay/app";
            ResultMoMoItem kq;
            ResultDataMoMoAppIG4Item resutl;
            new ResultDataMoMoAppIG4Item();
            try
            {
                kq = await PostDataAsync<ResultMoMoItem>(url, data1);
                var requestType = "";
                var obj = new ConfirmMoMoItem();
                if (kq.status == 0)
                {
                    obj.requestType = "capture";
                    requestType = "capture";
                }
                else
                {
                    obj.requestType = "revertAuthorize";
                    requestType = "revertAuthorize";
                }
                var sign = string.Format("partnerCode={0}&partnerRefId={1}&requestType={2}&requestId={3}&momoTransId={4}", PartnerCode, refId.ToString(), requestType, requestId.ToString(), kq.transid);
                string signature = signSHA256(sign, serectkey);
                obj.partnerCode = PartnerCode;
                obj.partnerRefId = refId.ToString();
                obj.customerNumber = item.customerNumber;
                obj.description = item.description;
                obj.momoTransId = kq.transid;
                obj.requestId = requestId.ToString();
                obj.signature = signature;
                var urlconfirm = Momo + "pay/confirm";
                try
                {
                    resutl = await PostDataAsync<ResultDataMoMoAppIG4Item>(urlconfirm, obj);
                    if (resutl.status == 0)
                    {
                        // thêm tiền vào ví chính
                        var wallet = new WalletCustomer
                        {
                            //Price = (decimal)resutl.data.amount,
                            TotalPrice = (decimal)resutl.data.amount,
                            CustomerID = item.CustomerId,
                            DateCreate = DateTime.Now.TotalSeconds(),
                            IsDelete = false,
                            IsActive = true,
                            Note = item.description,
                            Transaction_no = resutl.transid,
                            Type = 1,
                        };
                        _walletCustomerDa.Add(wallet);
                        _walletCustomerDa.Save();
                        var gettoken = _customerDa.GetItemByID(item.CustomerId);
                        // % khuyến mãi vào ví thưởng.
                        var config = _walletCustomerDa.GetConfig();
                        var reward = new RewardHistory
                        {
                            CustomerID = item.CustomerId,
                            Price = config.Percent * ((decimal)resutl.data.amount) / 100,
                            IsActive = true,
                            IsDeleted = false,
                            Date = DateTime.Now.TotalSeconds(),
                            WalletCusId = wallet.ID,
                            Type = (int)Reward.Dep,
                            Percent = config.Percent,
                        };
                        _rewardHistoryDa.Add(reward);
                        _rewardHistoryDa.Save();
                        var sucess = orderDA.GetNotifyById(4);
                        var token = gettoken.tokenDevice;
                        Pushnotifycation(sucess.Title.Replace("{percent}", config.Percent.ToString()), sucess.Content.Replace("{price}", reward.Price.Money()).Replace("{total}", resutl.data.amount.MoneyDouble()), token, sucess.ID.ToString());
                    }
                    return Json(resutl, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    resutl = new ResultDataMoMoAppIG4Item
                    {
                        status = -1,
                        message = ex.ToString()
                    };
                    return Json(resutl, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                resutl = new ResultDataMoMoAppIG4Item
                {
                    status = -1,
                    message = ex.ToString()
                };
                return Json(resutl, JsonRequestBehavior.AllowGet);
            }

        }

        public string signSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;

            }
        }
        //public ActionResult ListOrderProcessNode(string key)
        //{

        //}
    }
}
