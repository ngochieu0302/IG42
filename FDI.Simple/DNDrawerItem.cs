using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNDrawerItem : BaseSimple
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CabinetDocumentID { get; set; }
        public string CabinetDocumentName { get; set; }
        public int? AgencyId { get; set; }
        public int Sort { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }

        //public virtual DN_CabinetDocument DN_CabinetDocument { get; set; }
    }

    public class ModelDNDrawerItem : BaseModelSimple
    {
        public List<DNDrawerItem> ListItem { get; set; }
    }
}
