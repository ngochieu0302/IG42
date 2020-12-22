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

namespace FDI.DA
{
    public class DebtDA : BaseDA
    {
        #region Constructer
        public DebtDA()
        {
        }

        public DebtDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DebtDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DebtItem> GetListByRequest(HttpRequestBase httpRequest, int agencyid, out decimal total)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.Debts
                        orderby c.ID descending
                        select new DebtItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            UserName = c.DN_Users.UserName,
                            Type = c.Type,
                            IsDeleted = c.IsDeleted,
                            SupplierID = c.SupplierID,
                            //SupplieName = c.DN_Supplie.Name,
                            Note = c.Note,
                            Price = c.Price,
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal(0);
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateCreated >= fromDate && c.DateCreated < toDate);
            }
            var SupplierID = httpRequest["SupplierID"];
            if (!string.IsNullOrEmpty(SupplierID))
            {
                var t = int.Parse(SupplierID);
                query = query.Where(c => c.SupplierID == t);
            }
            total = query.Sum(m => m.Price ?? 0);
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public DebtItem GetItemByID(int id)
        {
            var query = from c in FDIDB.Debts
                        where c.ID == id
                        select new DebtItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            UserName = c.DN_Users.UserName,
                            Type = c.Type,
                            //Status = c.s
                            SupplierID = c.SupplierID,
                            //SupplieName = c.DN_Supplie.Name,
                            Note = c.Note,
                            Price = c.Price,
                        };
            return query.FirstOrDefault();
        }
        public Debt GetById(int id)
        {
            var query = from c in FDIDB.Debts where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<Debt> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Debts
                        where ltsArrId.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public void Add(Debt item)
        {
            FDIDB.Debts.Add(item);
        }

        public void Delete(Debt item)
        {
            FDIDB.Debts.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
