using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class STGroupItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Sort { get; set; }

        public virtual ICollection<AgencyItem> DN_Agency { get; set; }
        public virtual ICollection<EnterprisesItem> DN_Enterprises { get; set; }
        public virtual ICollection<ModuleItem> ST_Module { get; set; }
    }

    public class ModelSTGroupItem : BaseModelSimple
    {
        public IEnumerable<STGroupItem> ListItem { get; set; }
    }
}
