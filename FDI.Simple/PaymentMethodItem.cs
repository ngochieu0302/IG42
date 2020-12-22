using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class PaymentMethodItem:BaseSimple
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class ModelPaymentMethodItem:BaseModelSimple
    {
        public IEnumerable<PaymentMethodItem> ListItems { get; set; }
    }
}
