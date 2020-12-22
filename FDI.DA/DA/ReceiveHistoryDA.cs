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
    public class ReceiveHistoryDA : BaseDA
    {
        #region Constructer
        public ReceiveHistoryDA()
        {
        }

        public ReceiveHistoryDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ReceiveHistoryDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<RewardHistoryItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.ReceiveHistories
                        orderby c.ID descending
                        select new RewardHistoryItem
                        {
                            ID = c.ID,
                            Date = c.Date,
                            Price = c.Price
                        };
            return query.ToList();
        }

        public List<RewardHistoryItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.ReceiveHistories
                        where c.AgencyId == agencyId && c.IsDeleted == false
                        orderby c.ID descending
                        select new RewardHistoryItem
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Date = c.Date,
                            FullName = c.DN_Users.LoweredUserName,
                            Price = c.Price,
                            Type = c.Type,
                            OrderID = c.OrderID,
                            AgencyId = c.AgencyId,
                            PriceOrder = c.Shop_Orders.TotalPrice
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal(0);
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.Date >= fromDate && c.Date < toDate);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<RewardHistoryItem> GetListByCustomer(int page, int cusId, int agencyId)
        {
            var query = from c in FDIDB.ReceiveHistories
                        where c.IsDeleted == false && c.CustomerID == cusId && c.AgencyId == agencyId
                        orderby c.ID descending
                        select new RewardHistoryItem
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Date = c.Date,
                            Price = c.Price,
                            Type = c.Type,
                            OrderID = c.OrderID,
                            AgencyId = c.AgencyId,
                            PriceOrder = c.Shop_Orders.TotalPrice
                        };
            return query.Skip((page - 1) * 10).Take(10).ToList();
        }

        public ReceiveHistory GetById(int? customerId, int agencyId)
        {
            var query = from c in FDIDB.ReceiveHistories where c.CustomerID == customerId && c.AgencyId == agencyId select c;
            return query.FirstOrDefault();
        }

        public List<ReceiveHistory> GetById(List<int> ltsArrId)
        {
            var query = from c in FDIDB.ReceiveHistories where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public RewardHistoryItem GetDetailByOrderById(int id)
        {
            var query = from x in FDIDB.ReceiveHistories
                        where x.ID == id
                        select new RewardHistoryItem
                        {
                            ID = x.ID,
                            AgencyId = x.AgencyId,
                            Phone = x.Customer.Phone,
                            CMND = x.Customer.PeoplesIdentity,
                            Date = x.Date,
                            OrderID = x.OrderID,
                            FullName = x.DN_Users.LoweredUserName,
                            Price = x.Price,
                            CustomerName = x.Customer.FullName,
                            CustomerID = x.CustomerID,
                            PriceOrder = x.Price
                        };

            return query.FirstOrDefault();
        }
        public void Add(ReceiveHistory item)
        {
            FDIDB.ReceiveHistories.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
