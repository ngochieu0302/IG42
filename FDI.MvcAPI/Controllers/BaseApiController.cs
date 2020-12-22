using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using AuthenticationService.Managers;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA;
using FDI.DA.DA.AppSales;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class BaseApiController : Controller
    {
        private static int _month;
        private readonly CustomerDA _customerDa = new CustomerDA("#");
        private readonly DNSalaryDA _daDNSalaryDA = new DNSalaryDA("#");
        private readonly DNCriteriaDA _dnCriteriaDa = new DNCriteriaDA("#");
        private readonly OrdersDA _ordersDa = new OrdersDA("#");
        private readonly WallerOrderDA _wallerOrderDa = new WallerOrderDA("#");
        private readonly TotalSalaryMonthDA _totalSalaryMonthDa = new TotalSalaryMonthDA("#");
        public static string UrlG = ConfigurationManager.AppSettings["Url"];
        public static string UrlCustomer = ConfigurationManager.AppSettings["UrlCustomer"];
        public static string UrlNode = ConfigurationManager.AppSettings["UrlNote"];
        readonly ReceiveHistoryAPI _receiveHistoryApi = new ReceiveHistoryAPI();
        readonly RewardHistoryAPI _rewardHistoryApi = new RewardHistoryAPI();
        readonly RewardHistoryDA _rewardHistoryDa = new RewardHistoryDA("#");
        protected string Keyapi = "Fdi@123";
        public SqlConnection Getconnection { get; private set; }
        readonly TokenDeviceDA _tokenDeviceDa = new TokenDeviceDA("#");
        public int EnterprisesId()
        {
            //codee
            var id = Request["enterprisesId"];
            return id != null ? int.Parse(id) : 0;
        }
        public int Agencyid()
        {
            var id = Request["agencyId"];
            return id != null ? int.Parse(id) : 0;
        }
        public int MarketId()
        {
            var id = Request["market"];
            return id != null ? int.Parse(id) : 0;
        }
        public int AreaId()
        {
            var id = Request["area"];
            return id != null ? int.Parse(id) : 0;
        }
        public Guid? UserId()
        {
            var guid = Request["UserId"];
            return guid != null ? Guid.Parse(guid) : new Guid();
        }

        public int CustomerId
        {
            get
            {
                var token = Request.Headers["token"];
                if (token == null)
                {
                    return 0;
                }
                var authService = new JWTService();
                if (!authService.IsTokenValid(token))
                {
                    return 0;
                }

                var tmp = authService.GetTokenClaims(token).FirstOrDefault(m => m.Type == "ID");
                if (tmp == null)
                {
                    return 0;
                }

                return int.TryParse(tmp.Value, out var tmpUserid) ? (int)tmpUserid : 0;
            }
        }

        public Guid AgencyUserId
        {
            get
            {
                var token = Request.Headers["tokenAgency"];
                if (string.IsNullOrEmpty(token))
                {
                    return Guid.Empty;
                }
                var authService = new JWTService();
                var tmp = authService.GetTokenClaims(token).FirstOrDefault(m => m.Type == "ID");
                return Guid.Parse(tmp.Value);
            }
        }

        public int AgencyId
        {
            get
            {
                var token = Request.Headers["tokenAgency"];
                var authService = new JWTService();
                var tmp = authService.GetTokenClaims(token).FirstOrDefault(m => m.Type == "AgencyId");

                return int.Parse(tmp.Value);
            }
        }

        public void Node(string url)
        {
            url = UrlNode + url + "/" + Keyapi;
            Utility.GetObjJson<int>(url);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //  MonthSalary();
        }

        public void MonthSalary()
        {
            var date = DateTime.Now;
            if (_month != date.Month)
            {
                _month = date.Month;
                _totalSalaryMonthDa.AddTotal_SalaryMonth(date.Month, date.Year);
            }
        }

        // Tính thu nhập
        public void InsertSalary(int orderid, decimal price, Guid? UserId, int bedid, int Agencyid)
        {
            var criterias = _dnCriteriaDa.GetByIsOrderByMonth(DateTime.Today.TotalSeconds(), DateTime.Now.TotalSeconds(), Agencyid, bedid);
            if (UserId != null) CareSalary(criterias, orderid, price, UserId.Value, Agencyid);
            AllOrder(criterias, orderid, price, Agencyid);
            OrderLevel(criterias, orderid, price, Agencyid);
        }

        /// <summary>
        /// Thưởng trực tiếp đơn hàng.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="orderid"></param>
        /// <param name="price"></param>
        /// <param name="UserId"></param>
        /// <param name="agencySalaryId"></param>

        private void CareSalary(List<DNCriteriaItem> criteria, int orderid, decimal price, Guid UserId, int agencySalaryId)
        {
            foreach (var obj in criteria.Where(m => m.TypeID == (int)Criteria.Live && m.DNUserItem.Any(u => u.UserId == UserId)).Select(dnCriteriaItem => new DN_Salary
            {
                UserID = UserId,
                Salary = Convert.ToInt32(dnCriteriaItem.Price * price / 100),
                DateCreated = DateTime.Now.TotalSeconds(),
                AgencyID = agencySalaryId,
                OrderId = orderid,
                CriteriaId = dnCriteriaItem.ID
            }))
            {
                var item = obj;
                _daDNSalaryDA.Add(item);
            }
            foreach (var obj in criteria.Where(m => m.TypeID == (int)Criteria.Live && m.DNRolesItem.Any(u => u.DN_UsersInRoles.Any(c => c.UserId == UserId))).Select(dnCriteriaItem => new DN_Salary
            {
                UserID = UserId,
                Salary = Convert.ToInt32(dnCriteriaItem.Price * price / 100),
                DateCreated = DateTime.Now.TotalSeconds(),
                AgencyID = agencySalaryId,
                OrderId = orderid,
                CriteriaId = dnCriteriaItem.ID
            }))
            {
                var item = obj;
                _daDNSalaryDA.Add(item);
            }
            _daDNSalaryDA.Save();
        }
        protected static T GetObjJson<T>(string url) where T : new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(url);
            }
            catch (Exception ex)
            {
                return new T();
                Log2File.LogExceptionToFile(ex);
            }
        }
        private void AllOrder(IEnumerable<DNCriteriaItem> criterias, int orderid, decimal price, int agencySalaryId)
        {
            foreach (var criteria in criterias.Where(m => m.TypeID == (int)Criteria.AllOrder).ToList())
            {
                var count = criteria.IsAll == true ? criteria.DNUserItem.Count(m => criteria.IsSchedule == false || m.IsOnline) : 1;
                if (count > 0)
                    foreach (var salary in criteria.DNUserItem.Where(m => criteria.IsSchedule == false || m.IsOnline).Select(orderItem => new DN_Salary
                    {
                        UserID = orderItem.UserId,
                        Salary = (int)(price * criteria.Price / (count * 100)),
                        DateCreated = DateTime.Now.TotalSeconds(),
                        AgencyID = agencySalaryId,
                        OrderId = orderid,
                        CriteriaId = criteria.ID
                    }))
                    {
                        var item = salary;
                        _daDNSalaryDA.Add(item);
                    }
                count = criteria.IsAll == true ? criteria.DNRolesItem.Sum(r => r.DN_UsersInRoles.Count(m => criteria.IsSchedule == false || m.IsOnline)) : 1;
                if (count > 0)
                    foreach (var salary in criteria.DNRolesItem.SelectMany(role => role.DN_UsersInRoles.Where(m => criteria.IsSchedule == false || m.IsOnline).Select(orderItem => new DN_Salary
                    {

                        UserID = orderItem.UserId,
                        Salary = (int)(price * criteria.Price / (count * 100)),
                        DateCreated = DateTime.Now.TotalSeconds(),
                        AgencyID = agencySalaryId,
                        OrderId = orderid,
                        CriteriaId = criteria.ID
                    })))
                    {
                        var item = salary;
                        _daDNSalaryDA.Add(item);
                    }
            }
            _daDNSalaryDA.Save();
        }

        private void OrderLevel(IEnumerable<DNCriteriaItem> criterias, int orderid, decimal price, int agencySalaryId)
        {
            foreach (var criteria in criterias.Where(m => m.TypeID == (int)Criteria.Level && (m.DNRolesItem.Any(r => r.IsBed) || m.DNUserItem.Any(r => r.IsBed))).ToList())
            {
                var listu = criteria.IsSchedule == true ? criteria.DNUserItem.Where(m => m.IsOnline && m.IsBed).ToList() : criteria.DNUserItem.Where(r => r.IsBed).ToList();
                var count = criteria.IsAll == true ? listu.Count() : 1;
                if (count > 0)
                    foreach (var salary in listu
                                .Select(orderItem => new DN_Salary
                                {
                                    UserID = orderItem.UserId,
                                    Salary = (int)(price * criteria.Price / (count * 100)),
                                    DateCreated = DateTime.Now.TotalSeconds(),
                                    AgencyID = agencySalaryId,
                                    OrderId = orderid,
                                    CriteriaId = criteria.ID
                                }))
                    {
                        _daDNSalaryDA.Add(salary);
                    }

                var listr = criteria.IsSchedule == true ? criteria.DNRolesItem.Where(r => r.IsBed).SelectMany(role => role.DN_UsersInRoles.Where(m => m.IsOnline)).ToList() : criteria.DNRolesItem.Where(r => r.IsBed).SelectMany(role => role.DN_UsersInRoles.Where(m => criteria.IsSchedule == false)).ToList();

                count = criteria.IsAll == true ? listr.Count() : 1;
                if (count > 0)
                    foreach (var salary in listr.Select(orderItem => new DN_Salary
                    {
                        UserID = orderItem.UserId,
                        Salary = (int)(price * criteria.Price / (count * 100)),
                        DateCreated = DateTime.Now.TotalSeconds(),
                        AgencyID = agencySalaryId,
                        OrderId = orderid,
                        CriteriaId = criteria.ID
                    }))
                    {
                        _daDNSalaryDA.Add(salary);
                    }
            }
            _daDNSalaryDA.Save();
        }
        protected int ItemId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["ItemID"])) return Convert.ToInt32(Request["ItemID"]);
                return -1;
            }
        }
        public void InsertReward(int agencyId, int customerid, decimal? price, int orderid, decimal date, decimal prizeMoney)
        {
            var customer = _customerDa.GetCustomerAwad(customerid);
            var bonusItem = _ordersDa.GetBonusTypeItem();
            var now = DateTime.Now.TotalSeconds();
            //Tích lũy trực tiếp
            if (bonusItem.Percent > 0)
            {
                var reward = new RewardHistory
                {
                    Price = price * (bonusItem.Percent / 100),
                    CustomerID = customerid,
                    Date = now,
                    OrderID = orderid,
                    Type = (int)Reward.Cus,
                    Percent = bonusItem.Percent,
                    IsDeleted = false
                };
                var json = new JavaScriptSerializer().Serialize(reward);
                _rewardHistoryApi.AddRewardLocal(json);
                //_rewardHistoryDa.Add(reward);
            }
            //Tích lũy giới thiệu
            //if (customer.ParentID != null && customer.ParentID != bonusItem.RootID)
            //{
            //    var reward1 = new RewardHistory
            //    {
            //        Price = price * (bonusItem.PercentParent / 100),
            //        CustomerID = customer.ParentID,
            //        AgencyId = Agencyid(),
            //        Date = now,
            //        OrderID = orderid,
            //        Type = (int)Reward.Parent,
            //        Percent = bonusItem.PercentParent,
            //        IsDeleted = false
            //    };
            //    var json = new JavaScriptSerializer().Serialize(reward1);
            //    _rewardHistoryApi.AddRewardLocal(json);
            //    //_rewardHistoryDa.Add(reward1);
            //}
            //trừ tích lũy
            if (prizeMoney > 0)
            {
                var recive = new ReceiveHistory
                {
                    CustomerID = customerid,
                    AgencyId = agencyId,
                    Price = prizeMoney,
                    Date = now,
                    OrderID = orderid,
                    Type = (int)Reward.Receive1,
                    IsDeleted = false
                };
                //_rewardHistoryDa.Add();
                var json = new JavaScriptSerializer().Serialize(recive);
                _receiveHistoryApi.AddReciveLocal(json);
            }
            //_rewardHistoryDa.Save();
        }
        public void InsertRewardMessage(int agencyId, int customerid, decimal? price, int orderid, decimal date, decimal prizeMoney)
        {
            var customer = _customerDa.GetCustomerAwad(customerid);
            var bonusItem = _ordersDa.GetBonusTypeItem();
            var now = DateTime.Now.TotalSeconds();
            //Tích lũy trực tiếp
            //if (bonusItem.Percent > 0)
            //{
            //    var reward = new RewardHistory
            //    {
            //        Price = price * (bonusItem.Percent / 100),
            //        CustomerID = customerid,
            //        AgencyId = Agencyid(),
            //        Date = now,
            //        OrderID = orderid,
            //        Type = (int)Reward.Cus,
            //        Percent = bonusItem.Percent,
            //        IsDeleted = false
            //    };
            //    _rewardHistoryDa.Add(reward);
            //}
            ////Tích lũy giới thiệu
            //if (customer.ParentID != null && customer.ParentID != bonusItem.RootID)
            //{
            //    var reward1 = new RewardHistory
            //    {
            //        Price = price * (bonusItem.PercentParent / 100),
            //        CustomerID = customer.ParentID,
            //        AgencyId = Agencyid(),
            //        Date = now,
            //        OrderID = orderid,
            //        Type = (int)Reward.Parent,
            //        Percent = bonusItem.PercentParent,
            //        IsDeleted = false
            //    };
            //    _rewardHistoryDa.Add(reward1);
            //}
            //trừ ví
            if (prizeMoney > 0)
            {
                // trừ tích lũy
                //_rewardHistoryDa.Add(new ReceiveHistory
                //{
                //    CustomerID = customerid,
                //    AgencyId = agencyId,
                //    Price = prizeMoney,
                //    Date = now,
                //    OrderID = orderid,
                //    Type = (int)Reward.Receive1,
                //    IsDeleted = false
                //});
                _wallerOrderDa.Add(new WalletOrder_History
                {
                    CustomerID = customerid,
                    AgencyId = agencyId,
                    TotalPrice = prizeMoney,
                    DateCreate = now,
                    OrderID = orderid,
                    IsDelete = false
                });
            }
            _wallerOrderDa.Save();
        }

        public void InsertRewardCheckBarcode(int cusId, decimal price, Guid id)
        {
            var reward = new RewardHistory
            {
                Price = price,
                CustomerID = cusId,
                ImportID = id,
                Date = DateTime.Now.TotalSeconds(),
                Type = (int)Reward.Cus,
                Percent = 2,
                IsDeleted = false
            };
            _rewardHistoryDa.Add(reward);
            _rewardHistoryDa.Save();
        }
        public void HandlingNode(string port, List<int> listbed, List<int> listbednew, Shop_Orders order, bool isorder, bool check)
        {
            var listnew = listbednew.Where(m => listbed.All(n => n != m));
            var list = listbed.Where(m => listbednew.All(n => n != m));
            foreach (string json in listnew.Select(item => new OrderProcessItem
            {
                ID = order.ID,
                BedDeskID = item,
                Minute = order.TotalMinute,
                StartDate = (int)order.StartDate,
                EndDate = (int)order.EndDate,
                AgencyId = Agencyid(),
                IsEarly = false,
                Status = 0
            }).Select(jsonnew => new JavaScriptSerializer().Serialize(jsonnew)))
            {
                Node(port + (isorder ? "/addorder/" : "/addcontactorder/") + json);
            }
            if (check) Node(port + "/statuseorder/" + order.ID + "/0");
            foreach (var url in list.Select(id => port + (isorder ? "/updateorderbed/" : "/updatecontactbed/") + id))
            {
                Node(url);
            }
        }
        public void DbConnect()
        {
            try
            {
                var strCon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Getconnection = new SqlConnection(strCon);
                Getconnection.Open();
            }
            catch (Exception)
            {
                throw new Exception("Lỗi kết nối DataBase !");
            }
        }
        public DataTable Gets(string query)
        {
            var sql = query;
            var da = new SqlDataAdapter(sql, Getconnection);
            var ds = new DataTable();
            da.Fill(ds);
            Getconnection.Close();
            return ds;
        }

        public string Pushnotifycation(string title, string content)
        {
            var listtoken = _tokenDeviceDa.GetListToken();
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "POST";
            tRequest.UseDefaultCredentials = true;

            tRequest.PreAuthenticate = true;

            tRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

            //định dạng JSON
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add(string.Format("Authorization:key={0}", "AAAAPOXIQ80:APA91bHLKrH_3n71Tj5gYRmVzaf6kBjTBM9cRQD3YKyblKaabiR3yV6LxnM0THjbjVom-ZdLiZ_3DKcyJNzrTFh4MWUMGABrBAwtrSggtE2HOyNPYwIZWwvb6X8QSxEzGq0uuNyn0AV-"));
            tRequest.Headers.Add(string.Format("Sender:id={0}", "261553144781"));

            string[] arrRegid = listtoken.Select(x => x.Token).ToArray();
            string RegArr = string.Empty;
            RegArr = string.Join("\",\"", arrRegid);
            string postData = "{ \"registration_ids\": [ \"" + RegArr + "\" ],\"data\": {\"message\": \"" + content + "\",\"body\": \"" + content + "\",\"title\": \"" + title + "\",\"collapse_key\":\"" + content + "\"},\"body\": \"" + content + "\",\"title\": \"" + title + "\",\"click_action\":\"FLUTTER_NOTIFICATION_CLICK\",\"notification\": {\"body\": \"" + content + "\",\"title\": \"" + title + "\"}}";

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
    }
}