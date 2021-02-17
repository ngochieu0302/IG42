using FDI.MvcAPI.Common;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using Newtonsoft.Json;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class BaseAppApiController : Controller
    {
        readonly CustomerAppIG4DA _customerDa = new CustomerAppIG4DA("#");
        readonly AgencyDA _agencyDa = new AgencyDA("#");
        readonly RewardHistoryDA _rewardHistoryDa = new RewardHistoryDA("#");
        public static int Type = 1;
        readonly OrderAppIG4DA _orderDa = new OrderAppIG4DA("#");
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult { Data = new JsonMessage(500, filterContext.Exception.Message), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNotNullResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        public async Task<T> PostDataAsync<T>(string url, object data, string token = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("X-ApiKey", token);
            }
            if (data == null)
            {
                data = new object();
            }
            var json = new JavaScriptSerializer().Serialize(data);
            var postdata = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, postdata);
            if (response.IsSuccessStatusCode)
            {
                var datas = await response.Content.ReadAsStringAsync();
                return new JavaScriptSerializer().Deserialize<T>(datas);
            }
            return default(T);
        }
        public int CustomerId
        {
            get
            {
                var token = Request.Headers["token"];

                var tmp = JWTService.Instance.GetTokenClaims(token).FirstOrDefault(m => m.Type == "ID");
                if (tmp == null)
                {
                    return 0;
                }

                return int.TryParse(tmp.Value, out var tmpUserid) ? (int)tmpUserid : 0;
            }
        }
        public double Latitude
        {
            get
            {
                var token = Request.Headers["latitude"];
                if (double.TryParse(token, out var latitude)) return latitude;
                throw new Exception("latitude missing");
            }
        }
        public double Longitude
        {
            get
            {
                var token = Request.Headers["longitude"];
                if (double.TryParse(token, out var longitude)) return longitude;
                throw new Exception("longitude missing");
            }
        }

        public bool CheckWallets(decimal? total, int cusId)
        {
            var cus = _customerDa.GetItemByID(cusId);
            if (cus.TotalWallets >= total)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void InsertRewardOrderCustomer(CustomerAppIG4Item customer, ConfigExchange config, decimal? totalprice, int OrderId, List<BonusTypeItem> bonusItems, int type = 0, int idkho = 0, decimal discountKH = 60)
        {
            var now = DateTime.Now.TotalSeconds();
            //hoa hong cho shop ky gui
            //if (type == 2)
            //{
            //    // chinh sach them cho don hang ban tu kho.
            //    var customer = _customerDa.GetItemByID(idkho);
            //    var bonusItem = bonusItems.FirstOrDefault(m => m.ID == 1);
            //    if (bonusItem != null)
            //    {
            //        var reward = new RewardHistory
            //        {
            //            Price = (totalprice ?? 0) * (bonusItem.Percent / 100),
            //            CustomerID = customer.ID,
            //            Date = now,
            //            OrderID = OrderId,
            //            Type = (int)Reward.Kho,
            //            BonusTypeId = 1,
            //            Percent = bonusItem.Percent,
            //            IsDeleted = false,
            //            IsActive = true,
            //            AgencyId = 1006,
            //            TotalCp = totalprice,
            //        };
            //        _rewardHistoryDa.Add(reward);
            //        var shopsucess = _orderDa.GetNotifyById(9);
            //        var tokenshop = customer.tokenDevice;
            //        Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", reward.Price.Money()), tokenshop);
            //    }
            //    var listcustomer = _customerDa.GetItemByID(parentId);
            //    var bonusItemparent = bonusItems.FirstOrDefault(m => m.ID == 2);
            //    if (bonusItemparent != null)
            //    {
            //        var reward = new RewardHistory
            //        {
            //            Price = (totalprice ?? 0) * (bonusItemparent.Percent / 100),
            //            CustomerID = listcustomer.ID,
            //            Date = now,
            //            OrderID = OrderId,
            //            Type = (int)Reward.Kho,
            //            BonusTypeId = 2,
            //            Percent = bonusItemparent.Percent,
            //            IsDeleted = false,
            //            IsActive = true,
            //            AgencyId = 1006,
            //            TotalCp = totalprice,
            //        };
            //        _rewardHistoryDa.Add(reward);
            //        var shopsucess = _orderDa.GetNotifyById(9);
            //        var tokenshop = listcustomer.tokenDevice;
            //        Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", reward.Price.Money()), tokenshop);
            //    }
            //    var bonusItemroot = bonusItems.FirstOrDefault(m => m.ID == 3);
            //    if (bonusItemroot != null)
            //    {
            //        var reward = new RewardHistory
            //        {
            //            Price = (totalprice ?? 0) * (bonusItemroot.Percent / 100),
            //            CustomerID = 1,
            //            Date = now,
            //            OrderID = OrderId,
            //            Type = (int)Reward.Kho,
            //            BonusTypeId = 3,
            //            Percent = bonusItemroot.Percent,
            //            IsDeleted = false,
            //            IsActive = true,
            //            AgencyId = 1006,
            //            TotalCp = totalprice,
            //        };
            //        _rewardHistoryDa.Add(reward);
            //        //var shopsucess = _orderDa.GetNotifyById(9);
            //        //var tokenshop = root.tokenDevice;
            //        //Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", reward.Price.Money()), tokenshop);
            //    }
            //    _rewardHistoryDa.Save();
            //}
            //else
            //{
            var totalD = totalprice * config.DiscountCTV / 100;
            var ltsArrId = FDIUtils.StringToListInt(customer.ListAgencyId);
            ltsArrId = ltsArrId.Take(bonusItems.Count).ToList();
            var listcustomer = _agencyDa.GetListAgencyListID(ltsArrId);
            decimal per = 0;
            var i = 1;
            foreach (var item in listcustomer)
            {
                if (i <= bonusItems.Count)
                {
                    //if (item.LevelAdd >= i)
                    //{
                        var bonusItem = bonusItems.FirstOrDefault(m => m.ID == i);
                        if (bonusItem != null)
                        {
                            var reward = new RewardHistory
                            {
                                Price = totalD * (bonusItem.Percent / 100),
                                //CustomerID = item.ID,
                                Date = now,
                                OrderID = OrderId,
                                Type = (int)Reward.Cus,
                                BonusTypeId = i,
                                Percent = bonusItem.Percent,
                                IsDeleted = false,
                                IsActive = true,
                                AgencyId = item.ID,
                                TotalCp = totalD,
                            };
                            _rewardHistoryDa.Add(reward);
                            //var cus = _customerDa.GetItemByID(item.ID);
                            //var shopsucess = _orderDa.GetNotifyById(6);
                            //var tokenshop = cus.tokenDevice;
                            //Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", reward.Price.Money().Replace("{hoahong}", bonusItem.Name).Replace("{customer}", item.Fullname)), tokenshop);
                            per += bonusItem.Percent ?? 0;
                    }
                        //}
                }
                i++;
                
            }
            var reward0 = new RewardHistory
            {
                Price = totalD * ((100 - per) / 100),
                //CustomerID = item.ID,
                Date = now,
                OrderID = OrderId,
                Type = (int)Reward.Cus,
                BonusTypeId = i,
                Percent = 100 - per,
                IsDeleted = false,
                IsActive = true,
                AgencyId = customer.AgencyID,
                TotalCp = totalD,
            };
            _rewardHistoryDa.Add(reward0);
            //}
            
            // % chiết khấu cho công ty
            var reward1 = new RewardHistory
            {
                Price = (totalprice ?? 0) * config.DiscountOrder / 100,
                CustomerID = 1,
                Date = now,
                OrderID = OrderId,
                Type = (int)Reward.Cus,
                //BonusTypeId = i,
                Percent = config.DiscountOrder,
                IsDeleted = false,
                IsActive = true,
                AgencyId = 1006,
                TotalCp = totalprice,
            };
            _rewardHistoryDa.Add(reward1);
            // % chiết khấu cho mã QR truy xuất nguồn gốc
            var reward2 = new RewardHistory
            {
                Price = (totalprice ?? 0) * config.DiscountQR / 100,
                CustomerID = 1,
                Date = now,
                OrderID = OrderId,
                Type = (int)Reward.QR,
                //BonusTypeId = i,
                Percent = config.DiscountQR,
                IsDeleted = false,
                IsActive = true,
                AgencyId = 1006,
                TotalCp = totalprice,
            };
            _rewardHistoryDa.Add(reward2);
            // % chiết khấu cho quỹ khuyến mãi KH
            var reward3 = new RewardHistory
            {
                Price = (totalprice ?? 0) * config.DiscountSale / 100,
                CustomerID = 1,
                Date = now,
                OrderID = OrderId,
                Type = (int)Reward.Sale,
                //BonusTypeId = i,
                Percent = config.DiscountSale,
                IsDeleted = false,
                IsActive = true,
                AgencyId = 1006,
                TotalCp = totalprice,
            };
            _rewardHistoryDa.Add(reward3);

            // % phát triển cty
            var reward4 = new RewardHistory
            {
                Price = (totalprice ?? 0) * (100 - discountKH - config.DiscountQR - config.DiscountOrder - config.DiscountSale - config.DiscountCTV - config.DiscountOrderAgency) / 100,
                CustomerID = 1,
                Date = now,
                OrderID = OrderId,
                Type = (int)Reward.Depvelop,
                //BonusTypeId = i,
                Percent = (100 - discountKH - config.DiscountQR - config.DiscountOrder - config.DiscountSale - config.DiscountCTV - config.DiscountOrderAgency),
                IsDeleted = false,
                IsActive = true,
                AgencyId = 1006,
                TotalCp = totalprice,
            };
            _rewardHistoryDa.Add(reward4);
            _rewardHistoryDa.Save();
        }
        public void InsertRewardOrderAgency(CustomerAppIG4Item shop,ConfigExchange config, decimal? totalprice, int OrderId, List<BonusTypeItem> bonusItems, int type = 0, int idkho = 0, decimal discountKH = 60)
        {
            var now = DateTime.Now.TotalSeconds();
            var totalD = totalprice * config.DiscountOrderAgency / 100;
            var ltsArrId = FDIUtils.StringToListInt(shop.ListAgencyId);
            ltsArrId = ltsArrId.Take(bonusItems.Count).ToList();
            var listcustomer = _agencyDa.GetListAgencyListID(ltsArrId);
            decimal per = 0;
            var i = 1;
            foreach (var item in listcustomer)
            {
                if (i <= bonusItems.Count)
                {
                    //if (item.LevelAdd >= i)
                    //{
                    var bonusItem = bonusItems.FirstOrDefault(m => m.ID == i);
                    if (bonusItem != null)
                    {
                        var reward = new RewardHistory
                        {
                            Price = totalD * (bonusItem.Percent / 100),
                            //CustomerID = item.ID,
                            Date = now,
                            OrderID = OrderId,
                            Type = (int)Reward.Agency,
                            BonusTypeId = i,
                            Percent = bonusItem.Percent,
                            IsDeleted = false,
                            IsActive = true,
                            AgencyId = item.ID,
                            TotalCp = totalD,
                        };
                        _rewardHistoryDa.Add(reward);
                        //var cus = _customerDa.GetItemByID(item.ID);
                        //var shopsucess = _orderDa.GetNotifyById(6);
                        //var tokenshop = cus.tokenDevice;
                        //Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", reward.Price.Money().Replace("{hoahong}", bonusItem.Name).Replace("{customer}", item.Fullname)), tokenshop);
                        per += bonusItem.Percent ?? 0;
                    }
                }
                i++;
            }
            var reward0 = new RewardHistory
            {
                Price = totalD * ((100 - per) / 100),
                //CustomerID = item.ID,
                Date = now,
                OrderID = OrderId,
                Type = (int)Reward.Agency,
                BonusTypeId = i,
                Percent = 100 - per,
                IsDeleted = false,
                IsActive = true,
                AgencyId = shop.AgencyID,
                TotalCp = totalD,
            };
            _rewardHistoryDa.Add(reward0);
            _rewardHistoryDa.Save();
        }
        public void InsertRewardOrderPacket(CustomerAppIG4Item shop, ConfigExchange config, decimal? totalprice, int OrderId, List<BonusTypeItem> bonusItems, int type = 0, int idkho = 0, decimal discountKH = 60)
        {
            var now = DateTime.Now.TotalSeconds();
            var today = DateTime.Today.TotalSeconds();
            var totalD = totalprice * config.DiscountOrderPacket / 100;
            var ltsArrId = FDIUtils.StringToListInt(shop.ListAgencyId);
            ltsArrId = ltsArrId.Take(bonusItems.Count).ToList();
            var listcustomer = _agencyDa.GetListAgencyListID(ltsArrId);
            decimal per = 0;
            var i = 1;
            foreach (var item in listcustomer)
            {
                if (i <= bonusItems.Count)
                {
                    //if (item.LevelAdd >= i)
                    //{
                    var bonusItem = bonusItems.FirstOrDefault(m => m.ID == i);
                    if (bonusItem != null)
                    {
                        var reward = new RewardHistory
                        {
                            Price = totalD * (bonusItem.Percent / 100),
                            //CustomerID = item.ID,
                            Date = now,
                            DateCreate = today,
                            OrderPacketID = OrderId,
                            Type = (int)Reward.Packet,
                            BonusTypeId = i,
                            Percent = bonusItem.Percent,
                            IsDeleted = false,
                            IsActive = true,
                            AgencyId = item.ID,
                            TotalCp = totalD,
                        };
                        _rewardHistoryDa.Add(reward);
                        //var cus = _customerDa.GetItemByID(item.ID);
                        //var shopsucess = _orderDa.GetNotifyById(6);
                        //var tokenshop = cus.tokenDevice;
                        //Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", reward.Price.Money().Replace("{hoahong}", bonusItem.Name).Replace("{customer}", item.Fullname)), tokenshop);
                        per += bonusItem.Percent ?? 0;
                    }
                }
                i++;
            }
            var reward0 = new RewardHistory
            {
                Price = totalD * ((100 - per) / 100),
                //CustomerID = item.ID,
                Date = now,
                DateCreate = today,
                OrderPacketID = OrderId,
                Type = (int)Reward.Packet,
                //BonusTypeId = i,
                Percent = 100 - per,
                IsDeleted = false,
                IsActive = true,
                AgencyId = shop.AgencyID,
                TotalCp = totalD,
            };
            _rewardHistoryDa.Add(reward0);
            var reward1 = new RewardHistory
            {
                Price = totalprice * (100-config.DiscountOrderPacket)/100,
                //CustomerID = item.ID,
                Date = now,
                DateCreate = today,
                OrderPacketID = OrderId,
                Type = (int)Reward.Packet,
                //BonusTypeId = i,
                Percent = 100 - config.DiscountOrderPacket,
                IsDeleted = false,
                IsActive = true,
                AgencyId = 1006,
                TotalCp = totalprice * (100 - config.DiscountOrderPacket) / 100,
            };
            _rewardHistoryDa.Add(reward1);
            _rewardHistoryDa.Save();

        }

        public string Pushnotifycation(string title, string content, string token, string type = "1")
        {
            if (!string.IsNullOrEmpty(token))
            {
                var listtoken = new List<string>();
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "POST";
                tRequest.UseDefaultCredentials = true;
                tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
                //định dạng JSON
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization:key={0}", "AAAAGBvodiE:APA91bFe8RB42680q29-faFPseMpONPUJn2p_sp-Dcu-1sOtsPKq5TMKuGTZq65kOMP4vO9OWsCmaXT-BoisvkffCELWXBR1zvBpn_AtXmnzX9fgnw12ZrBwppyTJgq7LOjHfLQrn5_K"));
                tRequest.Headers.Add(string.Format("Sender:id={0}", "103547434529"));
                listtoken.Add(token);
                string[] arrRegid = listtoken.ToArray();
                string RegArr = string.Empty;
                RegArr = string.Join("\",\"", arrRegid);
                string postData = "{ \"registration_ids\": [ \"" + RegArr + "\" ],\"data\": {\"message\": \"" + content + "\",\"body\": \"" + content + "\",\"title\": \"" + title + "\",\"type\": \"" + type + "\",\"collapse_key\":\"" + content + "\"},\"body\": \"" + content + "\",\"title\": \"" + title + "\",\"click_action\":\"FLUTTER_NOTIFICATION_CLICK\",\"notification\": {\"body\": \"" + content + "\",\"title\": \"" + title + "\"}}";

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                var dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                var tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                if (dataStream != null)
                {
                    var tReader = new StreamReader(dataStream);
                    String sResponseFromServer = tReader.ReadToEnd();

                    var txtKetQua = sResponseFromServer; //Lấy thông báo kết quả từ FCM server.
                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                    return txtKetQua;
                }
            }

            return "";
        }
        public string PushnotifycationTopic(string title, string content)
        {

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "POST";
            tRequest.UseDefaultCredentials = true;

            tRequest.PreAuthenticate = true;

            tRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

            //định dạng JSON
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add(string.Format("Authorization:key={0}", "AAAAPOXIQ80:APA91bHLKrH_3n71Tj5gYRmVzaf6kBjTBM9cRQD3YKyblKaabiR3yV6LxnM0THjbjVom-ZdLiZ_3DKcyJNzrTFh4MWUMGABrBAwtrSggtE2HOyNPYwIZWwvb6X8QSxEzGq0uuNyn0AV-"));
            tRequest.Headers.Add(string.Format("Sender:id={0}", "261553144781"));

            string postData = "{\"to\":\"/topics/ALL\",\"notification\": {\"body\": \"" + content + "\",\"title\": \"" + title + "\"},\"data\": {\"message\": \"" + content + "\"}}";
            //string postData = "{\"topic\":\"all\",\"notification\": {\"body\": \"" + content + "\",\"title\": \"" + title + "\"}}";
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            var dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            if (dataStream != null)
            {
                var tReader = new StreamReader(dataStream);
                String sResponseFromServer = tReader.ReadToEnd();

                var txtKetQua = sResponseFromServer; //Lấy thông báo kết quả từ FCM server.
                tReader.Close();
                dataStream.Close();
                tResponse.Close();
                return txtKetQua;
            }
            return "";
        }
        CustomerDA customerDA = new CustomerDA();
        readonly WalletCustomerAppIG4DA _walletCustomerDa = new WalletCustomerAppIG4DA("#");
        readonly CustomerPolicyAppIG4DA _customerPolicyDa = new CustomerPolicyAppIG4DA("#");


        public void UpdateLevelCustomer(int customerId)
        {
            var config = _walletCustomerDa.GetConfig();
            var wallet = customerDA.GetById(customerId);
            if (wallet != null)
            {
                var total = wallet.CashOutWallets.Where(c => c.Type == 1).Sum(c => c.TotalPrice);
                if (total > 0)
                {
                    var point = (decimal)total * (config.Point / config.Price);
                    var policy = _customerPolicyDa.GetItemByPrice(point ?? 0);
                    if (policy != null)
                    {
                        wallet.CustomerPolicyID = policy.ID;
                        customerDA.Save();
                    }
                }
            }
        }
        public static string UrlNode = ConfigurationManager.AppSettings["UrlNote"];
        protected string Keyapi = "Fdi@123";
        public void Node(string url)
        {
            url = UrlNode + url + "/" + Keyapi;
            Utility.GetObjJson<int>(url);
        }
        public void HandlingNode(string port, OrderProcessAppIG4Item order)
        {
            var json = new JavaScriptSerializer().Serialize(order);
            Node(port + ("/addorder/" + json));
        }
        public void SpliceOrderCustomer(string port, int id)
        {
            Node(port + ("/SpliceOrder/" + id));
        }
    }

}
