using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNActiveRoleItem : BaseSimple
    {
        public string NameActive { get; set; }
        public int? Ord { get; set; }
        public virtual ICollection<RolesItem> Roles { get; set; }

    }
    public class ModelDNActiveRoleItem : BaseModelSimple
    {

        public IEnumerable<ActiveRoleItem> ListItem { get; set; }

    }

   
}
