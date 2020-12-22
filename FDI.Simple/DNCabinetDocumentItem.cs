using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNCabinetDocumentItem : BaseSimple
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? RoomID { get; set; }
        public string RoomName { get; set; }
        public int? Sort { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? AgencyID { get; set; }

        //public virtual DN_DocumentRoom DN_DocumentRoom { get; set; }
        //public virtual ICollection<DN_Drawer> DN_Drawer { get; set; }
    }

    public class ModelDNCabinetDocumentItem : BaseModelSimple
    {
        public List<DNCabinetDocumentItem> ListItem { get; set; }
    }
}
