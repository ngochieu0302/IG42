using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNUserBedDeskItem : BaseSimple
    {
        public Guid? UserID { get; set; }
        public int? BedDeskID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string NameBed { get; set; }
        public int? MWSID { get; set; }
        public int countorder { get; set; }

        public decimal? DateCreated { get; set; }
        public virtual WeeklyScheduleItem DN_Weekly_Schedule { get; set; }
    }
}
