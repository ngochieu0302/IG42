using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerPolicyAppIG4Item : BaseSimple
    {
        public string Name { get; set; }
        public decimal? StartMoney { get; set; }
        public decimal? EndMoney { get; set; }
        public int? Number { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? StageID { get; set; }
        public string StageName { get; set; }
        public  IEnumerable<CustomerItem> Customers { get; set; }
        
    }
    public class ModelCustomerPolicyItem : BaseModelSimple
    {
        public IEnumerable<CustomerPolicyAppIG4Item> ListItems { get; set; }
    }
}
