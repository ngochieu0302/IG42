using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class OrderDebtDA : BaseDA
    {
        #region Constructer
        public OrderDebtDA()
        {
        }

        public OrderDebtDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public OrderDebtDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<OrderDebtItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int Agencyid)
        {

            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Order_Debt
                        orderby o.ID descending
                        select new OrderDebtItem
                        {
                            ID = o.ID,
                            Pricetotal = o.Pricetotal,
                            Note = o.Note,
                            UserName = o.DN_Users.UserName,
                            Datecreate = o.Datecreate,
                            //SupplieName = o.DN_Supplie.Name
                            //DateImport = o.Supplier_Product.Select(m=>m.DateCreated).FirstOrDefault()
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public OrderDebtItem GetItemById(int id)
        {
            var query = from o in FDIDB.Order_Debt
                        where o.ID == id
                        select new OrderDebtItem
                        { 

                            ID = o.ID,
                            Pricetotal = o.Pricetotal,
                            Note = o.Note,
                            UserName = o.DN_Users.UserName,
                            Datecreate = o.Datecreate,
                            //SupplieName = o.DN_Supplie.Name
                        };
            return query.FirstOrDefault();
        }
        public Order_Debt GetById(int id)
        {
            var query = from c in FDIDB.Order_Debt where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<Order_Debt> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Order_Debt
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public void Add(Order_Debt obj)
        {
            FDIDB.Order_Debt.Add(obj);
        }

        public void Delete(Order_Debt obj)
        {
            FDIDB.Order_Debt.Remove(obj);
        }
    }
}
