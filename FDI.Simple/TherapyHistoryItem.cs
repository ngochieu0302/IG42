using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class TherapyHistoryItem:BaseSimple
    {
        public string Name { get; set; }
        public int? CustomerID { get; set; }
        public decimal? DateCreate { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }
        public string Note { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? Gender { get; set; }

        public virtual CustomerItem Customer { get; set; }
    }

    public class ModelTherapyHistoryItem : BaseModelSimple
    {
        public IEnumerable<TherapyHistoryItem> ListItems { get; set; }
    }
}
