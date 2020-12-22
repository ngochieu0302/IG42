using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNDocumentRoomItem : BaseSimple
    {
        public string Name { get; set; }
        public int? DocumentLevelID { get; set; }
        public string DocumentLevelName { get; set; }
        public int? Sort { get; set; }
        public int? AgencyID { get; set; }
        public int? IsDeleted { get; set; }
    }

    public class ModelDNDocumentRoomItem : BaseModelSimple
    {
        public List<DNDocumentRoomItem> ListItem { get; set; }
    }
}
