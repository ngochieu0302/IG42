using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerQuestionItem:BaseSimple
    {
        public string Name { get; set; }
        public int? pointID { get; set; }
        public bool? IsDelete { get; set; }
        public int? AgencyID { get; set; }
        public string NamePoint { get; set; }
        public virtual Customer_Point CustomerPoint { get; set; }
        public virtual IEnumerable<Customer_Review_Deltails> CustomerReviewDeltails { get; set; }
    }

    public class ModelCustomerQuestionItem : BaseModelSimple
    {
        public IEnumerable<CustomerQuestionItem> ListItems { get; set; }
    }
}
