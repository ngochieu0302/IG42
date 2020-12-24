using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA.DA
{
    public class CashOutWalletDA:BaseDA
    {
        #region Constructer
        public CashOutWalletDA()
        {
        }

        public CashOutWalletDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CashOutWalletDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion


        public List<CashOutWalletItem> GetListbyCustomer(int id)
        {
            var query = from c in FDIDB.CashOutWallets
                where c.CustomerID == id
                orderby c.ID descending
                        select new CashOutWalletItem()
                {
                    ID = c.ID,
                    Total = c.Totalprice,
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
