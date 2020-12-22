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
    public class RewardHistoryDA : BaseDA
    {
        #region Constructer
        public RewardHistoryDA()
        {
        }

        public RewardHistoryDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public RewardHistoryDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<RewardHistoryItem> GetList(int agencyId,int month, int year,int cusId)
        {            
            var date = new DateTime(year, month, 1);
            var fromDate = date.TotalSeconds();
            var toDate = date.AddMonths(1).TotalSeconds();
            var query = from c in FDIDB.RewardHistories
                        where  c.IsDeleted == false && c.CustomerID == cusId
                        orderby c.ID descending
                        select new RewardHistoryItem
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Date = c.Date,
                            Price = c.Price,
                            Type = c.Type,
                            OrderID = c.OrderID,
                            CustomerID = c.CustomerID,
                            PriceOrder = c.Shop_Orders.TotalPrice
                        };           
            
            return query.ToList();
        }

        public List<RewardHistoryItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var cusId = httpRequest["cusId"];
            var Id = int.Parse(cusId);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];            
            var query = from c in FDIDB.RewardHistories
                        where c.IsDeleted == false && c.CustomerID == Id
                        orderby c.ID descending
                        select new RewardHistoryItem
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Date = c.Date,
                            Price = c.Price,
                            Type = c.Type,
                            OrderID = c.OrderID,
                            CustomerID = c.CustomerID,
                            PriceOrder = c.Shop_Orders.TotalPrice
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal();
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.Date >= fromDate && c.Date < toDate);
            }            
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }




        public List<RewardHistoryItem> GetListByCustomer(int page, int cusId, int agencyId)
        {
            var query = from c in FDIDB.RewardHistories
                        where c.IsDeleted == false && c.CustomerID == cusId 
                        orderby c.ID descending
                        select new RewardHistoryItem
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Date = c.Date,
                            Price = c.Price,
                            Type = c.Type,
                            OrderID = c.OrderID,
                            PriceOrder = c.Shop_Orders.TotalPrice
                        };
            return query.Skip((page - 1) * 10).Take(10).ToList();
        }

        public RewardHistory GetById(int? customerId, int agencyId)
        {
            var query = from c in FDIDB.RewardHistories where c.CustomerID == customerId select c;
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
