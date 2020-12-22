using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class WallerOrderDA : BaseDA
    {
        #region Constructer
        public WallerOrderDA()
        {

        }

        public WallerOrderDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public WallerOrderDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<WalletOrderHistoryItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.WalletOrder_History
                        where c.IsDelete == false && c.AgencyId == agencyId
                        group c by new
                        {
                            c.CustomerID,
                        } into gr
                        orderby gr.FirstOrDefault().ID descending
                        select new WalletOrderHistoryItem
                        {
                            ID = gr.FirstOrDefault().ID,
                            TotalPrice = gr.Sum(c => c.TotalPrice),
                            Name = gr.FirstOrDefault().Customer.FullName,
                            Address = gr.FirstOrDefault().Customer.Address,
                            AgencyId = gr.FirstOrDefault().AgencyId,
                            CMTND = gr.FirstOrDefault().Customer.PeoplesIdentity,
                            CustomerID = gr.FirstOrDefault().CustomerID,
                            DateCreate = gr.FirstOrDefault().DateCreate,
                            IsDelete = gr.FirstOrDefault().IsDelete,
                            Phone = gr.FirstOrDefault().Customer.Phone,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<WalletOrderHistoryItem> GetListSimpleById(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var cusId = httpRequest["cusId"];
            var Id = int.Parse(cusId);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.WalletOrder_History
                        where c.IsDelete == false && c.AgencyId == agencyId && c.CustomerID == Id
                        orderby c.ID descending
                        select new WalletOrderHistoryItem
                        {
                            ID = c.ID,
                            TotalPrice = c.TotalPrice,
                            Name = c.Customer.FullName,
                            Address = c.Customer.Address,
                            AgencyId = c.AgencyId,
                            CMTND = c.Customer.PeoplesIdentity,
                            CustomerID = c.CustomerID,
                            DateCreate = c.DateCreate,
                            IsDelete = c.IsDelete,
                            Phone = c.Customer.Phone,
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal();
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateCreate >= fromDate && c.DateCreate < toDate);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public WalletOrder_History GetById(int customerId)
        {
            var query = from c in FDIDB.WalletOrder_History where c.ID == customerId select c;
            return query.FirstOrDefault();
        }
        public List<WalletOrder_History> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.WalletOrder_History where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(WalletOrder_History customer)
        {
            FDIDB.WalletOrder_History.Add(customer);
        }
        public void Delete(WalletOrder_History customer)
        {
            customer.IsDelete = true;
            FDIDB.WalletOrder_History.Attach(customer);
            var entry = FDIDB.Entry(customer);
            entry.Property(e => e.IsDelete).IsModified = true;
            // DB.Customers.Remove(customer);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
