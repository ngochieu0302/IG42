using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNRoomItem : BaseSimple
    {
        public string Name { get; set; }
        public string NameLevel { get; set; }
        public int? LevelID { get; set; }
        public int? Row { get; set; }
        public int? Column { get; set; }
        public int? Sort { get; set; }
        public int? AgencyID { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }

        public virtual IEnumerable<BedDeskItem> DN_Bed_Desk { get; set; }
        public virtual DNLevelRoomItem DN_Level { get; set; }
        public virtual IEnumerable<DNRolesItem> DN_Roles { get; set; }
    }

    public class ModelDNRoomItem : BaseModelSimple
    {
        public List<DNRoomItem> ListItems { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
    }
}
