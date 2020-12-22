using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class OrderCustomerDA : BaseDA
    {
        #region Contruction

        public OrderCustomerDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public OrderCustomerDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public List<CusContactItem> GetListAll(int id)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.CustomerID == id
                        orderby c.ID descending
                        select new CusContactItem
                        {
                            ID = c.ID,
                            N = c.DN_Agency.Name,
                            T = c.TotalPrice,
                            S = c.Status,
                        };
            return query.ToList();
        }
        public Shop_ContactOrder GetByID(int id)
        {
            var query = from c in FDIDB.Shop_ContactOrder
                where c.ID == id
                select c;
            return query.FirstOrDefault();
        }
        public CusContactItem GetListByCusId(int id)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.CustomerID == id && c.EndDate > date
                        orderby c.ID descending
                        select new CusContactItem
                        {
                            ID = c.ID,
                            N = c.DN_Agency.Name,
                            T = c.TotalPrice,
                            S = c.Status,
                            A = c.Address,
                            La = c.Latitute,
                            Lo = c.Longitude,
                            M = c.Mobile,
                            LItem = c.Shop_ContactOrder_Details.Select(m => new CusContactDetaiItem
                            {
                                PId = m.ProductID,
                                N = m.Shop_Product_Detail.Name,
                                Q = m.Quantity,
                                P = m.Price,
                                S = m.Status
                            })
                        };
            return query.FirstOrDefault();
        }

        public void Add(Shop_ContactOrder item)
        {
            FDIDB.Shop_ContactOrder.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
