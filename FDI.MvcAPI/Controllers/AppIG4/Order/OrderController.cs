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

namespace FDI.MvcAPI.Controllers.Order
{
    [CustomerAuthorize]
    public class OrderController : BaseAppApiController
    {

        OrderDA orderDA = new OrderDA();
        readonly CustomerDA _customerDa = new CustomerDA("#");
        readonly RewardHistoryDA _rewardHistoryDa = new RewardHistoryDA("#");
        readonly Shop_ProductDA _productDa = new Shop_ProductDA("#");
        public static string Momo = ConfigurationManager.AppSettings["UrlMomo"];
        private readonly WalletCustomerDA _walletCustomerDa = new WalletCustomerDA("#");
        private readonly CashOutWalletDA _cashOutWalletDa = new CashOutWalletDA("");
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Booking(List<OrderItem> datas)
        {
            foreach (var data in datas)
            {
                if (data.LisOrderDetailItems == null || !data.LisOrderDetailItems.Any())
                {
                    return Json(new JsonMessage(1000, "Không có sản phẩm trong giỏ hàng"), JsonRequestBehavior.AllowGet);
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
                var order = new FDI.Base.Order
                {
                    Address = data.Address,
                    CustomerID = CustomerId,
                    Longitude = data.Longitude,
                    Latitude = data.Latitude,
                    Phone = data.Mobile,
                    CustomerName = data.CustomerName,
                    CreatedOnUtc = DateTime.Now,
                    Code = FDIUtils.RandomCode(12),
                    FeeShip = data.FeeShip ?? 0,
                    StatusPayment = (int)PaymentOrder.Process,
                    Coupon = data.Coupon,
                    ShopID = data.ShopID,
                    //Discount = data.Discount,
                    CouponPrice = coupon?.DN_Sale.Price ?? 0,
                    PaymentMethodId = data.PaymentmethodId,
                    Status = (int)StatusOrder.Create,
                    Note = data.Note,
                    CustomerAddressID = data.CustomerAddressID,
                };
                if (!string.IsNullOrEmpty(data.Coupon))
                {
                    order.CouponPrice = coupon.DN_Sale.Price ?? 0;
                }
                else
                {
                    order.CouponPrice = 0;
                }
                foreach (var product in data.LisOrderDetailItems)
                {
                    var productData = _productDa.GetProductItem(product.ID);
                    if (productData == null)
                    {
                        return Json(new JsonMessage(1000, "Sản phẩm không tồn tại"), JsonRequestBehavior.AllowGet);
                    }
                    var item = new OrderDetail
                    {
                        ProductID = product.ID,
                        Price = productData.PriceNew,
                        Quantity = product.Quantity ?? 1,
                        CustomerId = data.ShopID,
                        Status = (int)StatusOrder.Create,
                        DateCreate = DateTime.Now.TotalSeconds(),
                        TotalPrice = productData.PriceNew * (product.Quantity ?? 1),
                        StatusPayment = (int)PaymentOrder.Process,
                        IsPrestige = product.IsPrestige,
                    };
                    order.OrderDetails.Add(item);
                }
                var total = order.OrderDetails.Sum(m => m.Price * m.Quantity);
                order.OrderTotal = total;
                order.Payment = total - order.CouponPrice + decimal.Round(data.FeeShip ?? 0);
                orderDA.Add(order);
                if (coupon != null)
                {
                    coupon.IsUse = true;
                }
                await orderDA.SaveAsync();
                await _productDa.SaveAsync();
                var notify = orderDA.GetNotifyById(7);
                var gettoken = _customerDa.GetItemByID(data.ShopID);
                var token = gettoken.tokenDevice;
                Pushnotifycation(notify.Title, notify.Content.Replace("{customer}", data.CustomerName), token, notify.ID.ToString());
            }
            return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDiscount()
        {
            var discount = _walletCustomerDa.GetConfigItem();
            return Json(new BaseResponse<ConfigItem> { Code = 200, Data = discount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListProductByNew(int shopid, int status, int page, int take)
        {
            var data = orderDA.GetListProductByNew(shopid, status, page, take);
            return Json(new BaseResponse<List<OrderShopItem>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult UpdateStatus(int orderId, int type)
        {
            var data = orderDA.GetById(orderId);
            data.Status = type;
            data.Check = 1;
            data.DateUpdateStatus = DateTime.Now.TotalSeconds();
            var totalprice = data.OrderTotal - data.CouponPrice + data.FeeShip;
            //var totalpricegstore = data.OrderTotal - ((data.Discount * data.OrderTotal / 100)) + data.FeeShip;
            //var config = _walletCustomerDa.GetConfig();
            //var totaldis = data.Discount + config.Percent;
            foreach (var item in data.OrderDetails)
            {
                item.Status = type;
                item.DateUpdateStatus = DateTime.Now.TotalSeconds();
                item.Check = 1;
            }
            orderDA.Save();

            if (type == (int)StatusOrder.Complete)
            {
                var cashout = new CashOutWallet
                {
                    CustomerID = data.CustomerID,
                    Totalprice = totalprice ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    OrderID = data.ID,
                    Type = 1,
                    Code = data.Code,
                };
                _cashOutWalletDa.Add(cashout);
                _cashOutWalletDa.Save();
                var walletcus = new WalletCustomer
                {
                    CustomerID = 1,
                    TotalPrice = totalprice ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    IsActive = true,
                    IsDelete = false,
                    Type = 2,
                    Transaction_no = data.Code,
                    CustomerIDR = data.CustomerID,
                };
                _walletCustomerDa.Add(walletcus);
                _walletCustomerDa.Save();
                // xử lý trừ số lượng trong kho của shop
                foreach (var item in data.OrderDetails)
                {
                    var product = _productDa.GetById(item.ProductID ?? 0);
                    product.QuantityOut = item.Quantity;
                    _productDa.Save();
                }
                var processOrder = new OrderProcessItem
                {
                    OrderId = orderId,
                    EndDate = DateTime.Now.AddHours(24).TotalSeconds(),
                };
                HandlingNode(":4000", processOrder);
            }
            var notify = orderDA.GetNotifyById(8);
            var gettoken = _customerDa.GetItemByID(data.CustomerID ?? 0);
            var token = gettoken.tokenDevice;
            if (type == (int)StatusOrder.Process)
            {
                Pushnotifycation(notify.Title, notify.Content.Replace("{status}", type == 2 ? "đang được giao" : "").Replace("{shop}", data.Customer1.FullName).Replace("{code}", data.Code), token,notify.ID.ToString());
            }
            if (type == (int)StatusOrder.Cancel)
            {
                Pushnotifycation(notify.Title, notify.Content.Replace("{status}", type == -1 ? "vừa bị hủy bởi chủ cửa hàng.!" : "").Replace("{shop}", data.Customer1.FullName).Replace("{code}", data.Code), token,notify.ID.ToString());
            }
            if (type == (int)StatusOrder.Complete)
            {
                notify = orderDA.GetNotifyById(3);
                Pushnotifycation(notify.Title, notify.Content.Replace("{shop}", data.Customer1.FullName).Replace("{price}", totalprice.Money()).Replace("{code}", data.Code), token, notify.ID.ToString());
            }
            return Json(new BaseResponse<List<ProductItem>> { Code = 200, Message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult ProcessOrderShip(string key, string json)
        {
            if (key == "Fdi@123")
            {
                var model = new JavaScriptSerializer().Deserialize<OrderProcessItem>(json);
                #region xử lý đơn hàng thành công tự động trừ tiền của KH.
                var data = orderDA.GetById(model.OrderId);
                var totalpricegstore = data.OrderTotal + data.FeeShip;
                var config = _walletCustomerDa.GetConfig();
                // chuyen tien tu vi trung gian cua gstore customerId = 1 den shop
                var cashout = new CashOutWallet
                {
                    CustomerID = 1,
                    Totalprice = totalpricegstore ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    OrderID = data.ID,
                    Type = 1,
                    Code = data.Code,
                };
                _cashOutWalletDa.Add(cashout);
                _cashOutWalletDa.Save();

                var totalshop = data.OrderTotal - (data.OrderTotal * config.DiscountOrder / 100) + data.FeeShip;
                var walletcus = new WalletCustomer
                {
                    CustomerID = data.ShopID,
                    TotalPrice = totalshop ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    IsActive = true,
                    IsDelete = false,
                    Type = 2,
                    Transaction_no = data.Code,
                    CustomerIDR = 1,
                };
                _walletCustomerDa.Add(walletcus);
                _walletCustomerDa.Save();

                #region đơn hàng đã giao thành công cộng tiền cho shop
                var shop = _customerDa.GetItemByID(data.ShopID ?? 0);
                var shopsucess = orderDA.GetNotifyById(5);
                var tokenshop = shop.tokenDevice;
                Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", totalshop.Money()).Replace("{code}", data.Code).Replace("{customer}", data.Customer.FullName), tokenshop, shopsucess.ID.ToString());
                #endregion
                #endregion
                var bonusItems = _customerDa.ListBonusTypeItems();
                #region hoa hồng khách hàng và shop ký gửi
                var iskg = data.Customer1.Type == 2;
                if (!iskg)
                {
                    InsertRewardCustomer(data.Customer.ParentID ?? 0, data.OrderTotal, data.ID, bonusItems);
                }
                else
                {
                    decimal totalpres = data.OrderDetails.Where(detail => detail.IsPrestige == true).Sum(detail => detail.TotalPrice ?? 0);
                    decimal totalnopres = data.OrderDetails.Where(detail => detail.IsPrestige == false || !detail.IsPrestige.HasValue).Sum(detail => detail.TotalPrice ?? 0);
                    if (totalpres > 0)
                    {
                        InsertRewardCustomer(data.Customer1.ParentID ?? 0, totalpres, data.ID, bonusItems, 2, data.ShopID ?? 0);
                    }
                    if (totalnopres > 0)
                    {
                        InsertRewardCustomer(data.Customer.ParentID ?? 0, totalnopres, data.ID, bonusItems);
                    }
                }
                #endregion
                // kiem tra level khach hang.
                UpdateLevelCustomer(data.CustomerID ?? 0);
                return Json(new BaseResponse<JsonMessage> { Code = 200, Message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new BaseResponse<JsonMessage> { Code = -1, Message = "Bạn không có quyền truy cập.!" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetCoupon(string code)
        {
            var coupon = _productDa.GetSaleCodeUseByCode(code);
            if (coupon == null)
            {
                return Json(new JsonMessage(1000, "Coupon không tồn tại"), JsonRequestBehavior.AllowGet);
            }
            var data = new SaleCodeItem
            {
                Code = coupon.Code,
                Price = coupon.DN_Sale.Price ?? 0
            };
            return Json(new BaseResponse<SaleCodeItem> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
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
                partnerTransId = DateTime.Now.TotalMilliSeconds().ToString(),
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
            ResultDataMoMoItem resutl;
            new ResultDataMoMoItem();
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
                    resutl = await PostDataAsync<ResultDataMoMoItem>(urlconfirm, obj);
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
                        Pushnotifycation(sucess.Title.Replace("{percent}", config.Percent.ToString()), sucess.Content.Replace("{price}", reward.Price.Money()).Replace("{total}", resutl.data.amount.MoneyDouble()), token,sucess.ID.ToString());
                    }
                    return Json(resutl, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    resutl = new ResultDataMoMoItem
                    {
                        status = -1,
                        message = ex.ToString()
                    };
                    return Json(resutl, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                resutl = new ResultDataMoMoItem
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
