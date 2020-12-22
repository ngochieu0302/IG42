using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerPointItem:BaseSimple
    {
        public int? Point { get; set; }
        public string Name { get; set; }
        public bool? IsDelete { get; set; }
        public int? AgencyID { get; set; }

        public virtual IEnumerable<Customer_question> Customerquestion { get; set; }
        public virtual IEnumerable<Customer_Review_Deltails> CustomerReviewDeltails { get; set; }
    }

    public class ModelCustomerPointItem : BaseModelSimple
    {
        public IEnumerable<CustomerPointItem> ListItems { get; set; }
    }
}
