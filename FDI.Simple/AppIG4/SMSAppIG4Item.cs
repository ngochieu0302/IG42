using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.AppIG4
{
    public class SMSAppIG4Item : BaseSimple
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int? CustomerID { get; set; }
        public int? AgencyID { get; set; }
        public decimal? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public bool? isDelete { get; set; }
        public decimal? DateUpdated { get; set; }
    }
}
