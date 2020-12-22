using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerReviewDetailsItem:BaseSimple
    {
        public int? ReviewID { get; set; }
        public int? PointID { get; set; }
        public int? QuestionID { get; set; }
        public string Note { get; set; }
        public bool? IsDelete { get; set; }
        public string Rep { get; set; }
        public int? Point { get; set; }
        public string Question { get; set; }

        public virtual Customer_Point CustomerPoint { get; set; }
        public virtual Customer_question Customerquestion { get; set; }
        public virtual Customer_Review CustomerReview { get; set; }
    }
    public class ModelCustomerReviewDetailsItem : BaseModelSimple
    {
        public IEnumerable<CustomerReviewDetailsItem> ListItems { get; set; }
    }
}
