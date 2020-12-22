using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Simple;

namespace FDI.DA.DA
{
    public class PaymentOrderDA:BaseDA
    {
        #region Constructer
        public PaymentOrderDA()
        {
        }

        public PaymentOrderDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public PaymentOrderDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<PaymentOrderItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Payments_Order
                        orderby o.ID descending
                        select new PaymentOrderItem
                        {
                            ID = o.ID,
                            OrderCode = o.OrderCode,
                            DateCreate = o.DateCreate,
                            Customername = string.IsNullOrEmpty(o.Shop_Orders.CustomerName) ? string.IsNullOrEmpty(o.Shop_Orders.CompanyName) ? o.Shop_Orders.CompanyName : "(Khách lẻ)" : "(Khách lẻ)",
                            Phonenumber = o.Shop_Orders.Mobile,
                            TotalPrice = o.TotalPrice,
                            Method = o.Payment_Method.Name,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public PaymentOrderItem GetItemById(int id)
        {
            var query = from o in FDIDB.Payments_Order
                        where o.ID == id
                        select new PaymentOrderItem
                        {
                            ID = o.ID,
                            OrderCode = o.OrderCode,
                            DateCreate = o.DateCreate,
                            Customername = string.IsNullOrEmpty(o.Shop_Orders.CustomerName) ? string.IsNullOrEmpty(o.Shop_Orders.CompanyName) ? o.Shop_Orders.CompanyName : "(Khách lẻ)" : "(Khách lẻ)",
                            Phonenumber = o.Shop_Orders.Mobile,
                            TotalPrice = o.TotalPrice,
                            Method = o.Payment_Method.Name,
                        };
            return query.FirstOrDefault();
        }
    }
}
