using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class PaymentOrderItem:BaseSimple
    {
        public string OrderCode { get; set; }
        public int? MethodId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? DateCreate { get; set; }
        public int? OrderID { get; set; }
        public string Customername { get; set; }
        public string Method { get; set; }
        public string Phonenumber { get; set; }
        public  PaymentMethodItem PaymentMethodItem { get; set; }
        public  OrderItem OrderItem { get; set; }
    }
    public class ModelPaymentOrderItem : BaseModelSimple
    {
        public IEnumerable<PaymentOrderItem> ListItems { get; set; }
    }
}
