using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNDiscountItem:BaseSimple
    {
        public string Name { get; set; }
        public bool? IsAll { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Price { get; set; }
        public bool? IsShow { get; set; }
        public int? Level { get; set; }
        public decimal? TotalOrder { get; set; }
        public decimal? Deposit { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsAgency { get; set; }
        public Guid? UserCreate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? UserUpdate { get; set; }
        public bool? IsMonth { get; set; }
        public bool? IsDay { get; set; }
        public int? Type { get; set; }
    }
    public class ModelDNDiscountItem : BaseModelSimple
    {
        public IEnumerable<DNDiscountItem> ListItems { get; set; }
    }
}
