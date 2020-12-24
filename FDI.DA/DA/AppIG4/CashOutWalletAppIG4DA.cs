using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA.DA
{
    public class CashOutWalletAppIG4DA : BaseDA
    {
        #region Constructer
        public CashOutWalletAppIG4DA()
        {
        }

        public CashOutWalletAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CashOutWalletAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CashOutWalletAppIG4Item> GetListbyCustomer(int id)
        {
            var query = from c in FDIDB.CashOutWallets
                where c.CustomerID == id
                orderby c.ID descending
                        select new CashOutWalletAppIG4Item()
                {
                    ID = c.ID,
                    Total = c.TotalPrice,
                    DateCreate = c.DateCreate,
                    TypeCash = c.Type,
                    Type = 2,
                    Query = c.Query,
                };
            return query.ToList();
        }
        public void Add(CashOutWallet obj)
        {
            FDIDB.CashOutWallets.Add(obj);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }

}
