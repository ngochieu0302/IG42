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
    public class WalletsAppIG4DA : BaseDA
    {
        #region Constructer
        public WalletsAppIG4DA()
        {
        }

        public WalletsAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public WalletsAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public WalletsAppIG4Item GetWalletsItemById(int cusId)
        {
            var query = from c in FDIDB.Wallets
                        where c.CustomerID == cusId
                        select new WalletsAppIG4Item
                        {
                            ID = c.ID,
                            WalletsCus = c.WalletCus,
                            Customername = c.Customer.FullName,
                            CashOutWallet = c.CashOutWallet,
                            Wallets = c.WalletCus - c.CashOutWallet
                        };
            return query.FirstOrDefault();
        }
        public CustomerRewardItem GetWalletsRewardByCusId(int cusId)
        {
            var query = from c in FDIDB.Customer_Reward
                where c.CustomerID == cusId
                select new CustomerRewardItem()
                {
                    ID = c.ID,
                    Total = c.PriceReward
                };
            return query.FirstOrDefault();
        }
        public List<WalletsAppIG4Item> GetListCustomerByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal(0) : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                        where c.WalletCustomers.Any(x => x.DateCreate >= fromDate && x.DateCreate <= toDate)
                        && (!c.IsDelete.HasValue || c.IsDelete== false)
                        orderby c.ID
                        select new WalletsAppIG4Item
                        {
                            ID = c.ID,
                            Customername = c.FullName,
                            Phone = c.Mobile,
                            Email = c.Email,
                            
                            WalletsCus = c.WalletCustomers.Where(a=>(a.IsDelete == false || !a.IsDelete.HasValue) && a.IsActive == true && a.DateCreate >= fromDate && a.DateCreate <= toDate).Sum(v=>v.TotalPrice),
                            CashOutWallet = c.CashOutWallets.Where(a => a.DateCreate >= fromDate && a.DateCreate <= toDate).Sum(v => v.TotalPrice),
                        }; 
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
    }
}
