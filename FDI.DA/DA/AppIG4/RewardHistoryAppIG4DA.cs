using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class RewardHistoryAppIG4DA : BaseDA
    {
        #region Constructer
        public RewardHistoryAppIG4DA()
        {
        }

        public RewardHistoryAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public RewardHistoryAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<RewardHistoryAppIG4Item> GetList(int agencyId, int month, int year, int cusId)
        {
            var date = new DateTime(year, month, 1);
            var fromDate = date.TotalSeconds();
            var toDate = date.AddMonths(1).TotalSeconds();
            var query = from c in FDIDB.RewardHistories
                        where c.AgencyId == agencyId && c.IsDeleted == false && c.CustomerID == cusId
                        orderby c.ID descending
                        select new RewardHistoryAppIG4Item
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Date = c.Date,
                            Price = c.Price,
                            Type = c.Type,
                            OrderID = c.OrderID,
                            CustomerID = c.CustomerID,
                            PriceOrder = c.Order.OrderTotal - (c.Order.CouponPrice + (c.Order.Discount * c.Order.OrderTotal / 100)),
                            PriceDeposit = c.WalletCustomer.TotalPrice,
                            TransitionCode = c.WalletCustomer.Transaction_no,
                            WalletCusID = c.WalletCusId,
                            //TXID = c.Investment.TXID,
                            //CustomerBuy = c.Investment.Customer.FullName,
                            Email = c.WalletCustomer.Customer.Email,
                            Percent = c.Percent,
                        };
            return query.ToList();
        }

        public List<RewardHistoryAppIG4Item> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var cusId = httpRequest["cusId"] ?? "0";
            var Id = int.Parse(cusId);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var datef = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var datet = !string.IsNullOrEmpty(to) ? to.StringToDecimal() : DateTime.Now.AddDays(1).TotalSeconds();
            var query = from c in FDIDB.RewardHistories
                        where  c.IsDeleted == false && c.CustomerID == Id
                               &&  c.Date >= datef && c.Date < datet
                        orderby c.ID descending
                        select new RewardHistoryAppIG4Item
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Date = c.Date,
                            Price = c.Price,
                            Type = c.Type,
                            BonusTypeId = c.BonusTypeId,
                            OrderID = c.OrderID,
                            CustomerID = c.CustomerID,
                            PriceOrder = c.Order.OrderTotal - (c.Order.CouponPrice + (c.Order.Discount * c.Order.OrderTotal / 100)),
                            PriceDeposit = c.WalletCustomer.TotalPrice,
                            TransitionCode = c.WalletCustomer.Transaction_no,
                            WalletCusID = c.WalletCusId,
                            //TXID = c.Investment.TXID,
                            CustomerBuy = c.Order.Customer.FullName,
                            Email = c.WalletCustomer.Customer.Email,
                            Percent = c.Percent,
                        };
            var type = httpRequest["_type"];
            if (!string.IsNullOrEmpty(type))
            {
                var t = int.Parse(type);
                query = query.Where(c => c.Type == t);
            }

            var lstF = httpRequest["_hoahong"];
            if (!string.IsNullOrEmpty(lstF))
            {
                var lstArr = FDIUtils.StringToListInt(lstF);
                query = query.Where(c => lstArr.Contains(c.BonusTypeId ?? 0));
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }




        

       

        public RewardHistory GetById(int? customerId)
        {
            var query = from c in FDIDB.RewardHistories where c.CustomerID == customerId  select c;
            return query.FirstOrDefault();
        }
        public List<RewardHistory> GetById(List<int> ltsArrId)
        {
            var query = from c in FDIDB.RewardHistories where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(RewardHistory item)
        {
            FDIDB.RewardHistories.Add(item);
        }
        public void Add(ReceiveHistory item)
        {
            FDIDB.ReceiveHistories.Add(item);
        }
        public void Delete(RewardHistory item)
        {
            FDIDB.RewardHistories.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
