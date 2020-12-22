using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class WalletsDA : BaseDA
    {
        #region Constructer
        public WalletsDA()
        {
        }

        public WalletsDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public WalletsDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<WalletItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Wallets
                        where c.AgencyId == agencyId
                        orderby c.WalletCus descending
                        select new WalletItem
                        {
                            ID = c.ID,
                            CustomerID = c.CustomerID,
                            CustomerName = c.Customer.FullName,
                            Customerphone = c.Customer.Phone,
                            WalletCus = c.WalletCus,
                            WalletOrder = c.WalletOrder,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<WalletCustomerItem> GetListWalletCusByCustomer(int page, int cusId, int agencyId)
        {
            var query = from c in FDIDB.WalletCustomers
                        where c.IsDelete == false && c.CustomerID == cusId && c.AgencyId == agencyId
                        orderby c.ID descending
                        select new WalletCustomerItem
                        {
                            ID = c.ID,
                            Name = c.Customer.FullName,
                            DateCreate = c.DateCreate,
                            TotalPrice = c.TotalPrice,
                            AgencyId = c.AgencyId,
                        };
            return query.Skip((page - 1) * 10).Take(10).ToList();
        }
        public List<CashOutWalletItem> GetListWalletCashByCustomer(int page, int cusId, int agencyId)
        {
            var query = from c in FDIDB.CashOutWallets
                        where c.IsDelete == false && c.CustomerID == cusId && c.AgencyId == agencyId
                        orderby c.ID descending
                        select new CashOutWalletItem
                        {
                            ID = c.ID,
                            Name = c.Customer.FullName,
                            DateCreate = c.DateCreate,
                            TotalPrice = c.TotalPrice,
                            AgencyId = c.AgencyId,
                        };
            return query.Skip((page - 1) * 10).Take(10).ToList();
        }
        public List<WalletOrderHistoryItem> GetListWalletOrderByCustomer(int page, int cusId, int agencyId)
        {
            var query = from c in FDIDB.WalletOrder_History
                        where c.IsDelete == false && c.CustomerID == cusId && c.AgencyId == agencyId
                        orderby c.ID descending
                        select new WalletOrderHistoryItem
                        {
                            ID = c.ID,
                            Name = c.Customer.FullName,
                            DateCreate = c.DateCreate,
                            TotalPrice = c.TotalPrice,
                            AgencyId = c.AgencyId,
                        };
            return query.Skip((page - 1) * 10).Take(10).ToList();
        }
        public List<WalletItem> GetListCustomerByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal(0) : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                        where c.WalletCustomers.Any(x => x.DateCreate >= fromDate && x.DateCreate <= toDate)
                        orderby c.ID
                        select new WalletItem
                        {
                            ID = c.ID,
                            CustomerName = c.FullName,
                            Customerphone = c.Phone,
                            CashOut = c.Wallets.Sum(a => a.CashOutWallet),
                            WalletCus = c.Wallets.Sum(a => a.WalletCus),
                            WalletOrder = c.Wallets.Sum(a => a.WalletOrder),
                            Username = c.UserName
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

    }
}
