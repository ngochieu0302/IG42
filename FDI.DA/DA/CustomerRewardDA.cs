using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Simple;
using FDI.Utils;
using FDI.Base;
using FDI.CORE;

namespace FDI.DA
{
    public class CustomerRewardDA : BaseDA
    {
        #region Constructer
        public CustomerRewardDA()
        {
        }

        public CustomerRewardDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerRewardDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CustomerRewardItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customer_Reward
                        where c.AgencyID == agencyId
                        orderby c.PriceReward descending
                        select new CustomerRewardItem
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            CustomerID = c.CustomerID,
                            PriceReward = c.PriceReward,
                            PriceReceive = c.PriceReceive
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNUserItem> GetListRewardRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now;
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyId && c.CustomerID != null
                        orderby c.UserName
                        select new DNUserItem
                        {

                            CustomerName = c.Customer.FullName,
                            LoweredUserName = c.LoweredUserName,
                            UserName = c.UserName,
                            CustomerID = c.CustomerID,
                            TotalReward = c.Customer.RewardHistories.Where(v => v.Date >= fromDate && v.Date <= toDate).Sum(v => v.Price),
                            TotalReceip = c.Customer.ReceiveHistories.Where(v => v.Date >= fromDate && v.Date <= toDate).Sum(v => v.Price)
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CustomerRewardItem> GetListCustomerByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal(0) : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            const int personal = (int)Reward.Cus;
            const int parent = (int)Reward.Parent;
            var query = from c in FDIDB.Customers
                        where c.RewardHistories.Any(x =>  x.Date >= fromDate && x.Date <= toDate)
                        orderby c.ID
                        select new CustomerRewardItem
                        {
                            ID = c.ID,
                            CustomerName = c.FullName,
                            Phone = c.Phone,
                            PricePersonal = c.RewardHistories.Where(u => u.IsDeleted == false && u.Type == personal && u.Date >= fromDate && u.Date <= toDate).Sum(v => v.Price),
                            PriceParent = c.RewardHistories.Where(u => u.IsDeleted == false && u.Type == parent && u.Date >= fromDate && u.Date <= toDate).Sum(v => v.Price),
                            TotalReward = c.RewardHistories.Where(u => u.IsDeleted == false && u.Date >= fromDate && u.Date <= toDate).Sum(v => v.Price),
                            PriceReward = c.Customer_Reward.Where(u => u.AgencyID == agencyId).Sum(v => v.PriceReward - v.PriceReceive),
                            TotalReceipt = c.ReceiveHistories.Where(u => u.AgencyId == agencyId && u.IsDeleted == false && u.DateCreate >= fromDate && u.DateCreate <= toDate).Sum(v => v.Price),
                            
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<CustomerRewardItem> GetListUserRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var name = httpRequest["Keyword"];
            var query = from c in FDIDB.DN_Users
                        where c.CustomerID > 0 && c.Customer.RewardHistories.Any() && c.AgencyID == agencyId
                        orderby c.CustomerID
                        select new CustomerRewardItem
                        {
                            CustomerID = c.CustomerID,
                            CustomerUser = c.Customer.UserName,
                            CMND = c.Customer.PeoplesIdentity,
                            Phone = c.Customer.Phone,
                            UserId = c.UserId,
                            CustomerName = c.LoweredUserName,
                            UserName = c.Customer.UserName,
                            TotalReward = c.Customer.RewardHistories.Where(u => u.CustomerID == c.CustomerID && u.IsDeleted == false).Sum(v => v.Price),
                            TotalReceipt = c.Customer.ReceiveHistories.Where(u => u.AgencyId == agencyId && u.CustomerID == c.CustomerID && u.IsDeleted == false).Sum(v => v.Price),
                        };
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.CustomerName.Contains(name));
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public CustomerRewardItem GetCustomerRewardItem(int agencyId, int cusId, int month, int year, Guid? Userid)
        {
            var query = from c in FDIDB.DN_Users
                        where c.CustomerID == cusId && c.Customer.RewardHistories.Any()
                        orderby c.CustomerID
                        select new CustomerRewardItem
                        {
                            Month = DateTime.Now.Month,
                            Year = DateTime.Now.Year,
                            CustomerID = c.CustomerID,
                            FullName = c.LoweredUserName,
                            UserId = Userid.Value,
                            CustomerUser = c.Customer.UserName,
                            CMND = c.Customer.PeoplesIdentity,
                            Phone = c.Customer.Phone,
                            CustomerName = c.Customer.FullName,
                            UserName = c.Customer.UserName,
                            TotalReward = c.Customer.RewardHistories.Where(u => u.CustomerID == c.CustomerID && u.IsDeleted == false).Sum(v => v.Price),
                            TotalReceipt = c.Customer.ReceiveHistories.Where(u => u.AgencyId == agencyId && u.CustomerID == c.CustomerID && u.IsDeleted == false).Sum(v => v.Price),
                        };

            return query.FirstOrDefault();
        }
        public List<CustomerRewardItem> GetListByCustomer(int page, int cusId, int agencyId)
        {
            var query = from c in FDIDB.Customer_Reward
                        where c.CustomerID == cusId && c.AgencyID == agencyId
                        orderby c.ID descending
                        select new CustomerRewardItem
                        {
                            ID = c.ID,
                            TotalReceipt = c.PriceReceive,
                            TotalReward = c.PriceReward,
                        };
            return query.ToList();
        }


        public bool CheckReciveHistory(int month, int year, int Idcus, int Agencyid, decimal price)
        {
            var date = new DateTime(year, month, 1);
            var fromDate = date.TotalSeconds();
            var toDate = date.AddMonths(1).TotalSeconds();
            var query = from c in FDIDB.Customer_Reward
                        where c.CustomerID == Idcus && c.AgencyID == Agencyid
                        orderby c.CustomerID
                        select new CustomerRewardItem
                        {
                            TotalReward = c.PriceReward,
                            TotalReceipt = c.PriceReceive
                        };
            var TotalReward = query.FirstOrDefault().TotalReward;
            var totalPrice = price + query.FirstOrDefault().TotalReceipt;
            if (totalPrice > TotalReward)
                return false;
            else
                return true;
        }

        public void Add(ReceiveHistory item)
        {
            FDIDB.ReceiveHistories.Add(item);
        }
    }
}
