using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerReviewItem:BaseSimple
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public int? OrderID { get; set; }
        public decimal? DateCreate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool? IsDelete { get; set; }
        public int? AgencyID { get; set; }
        public string Username { get; set; }
        public int? QuestionID { get; set; }
        public int? PointID { get; set; }
        public string Rep { get; set; }
        public string RepNote { get; set; }
        public string PointName { get; set; }
        public virtual IEnumerable<CustomerReviewDetailsItem> CustomerReviewDeltails { get; set; }
        public virtual Shop_Orders ShopOrders { get; set; }
    }

    public class ModelCustomerReviewItem : BaseModelSimple
    {
        public IEnumerable<CustomerReviewItem> ListItems { get; set; }
    }
}
