using System;
using System.Collections.Generic;
using System.Text;

namespace FDI.Simple
{
    public class UserViewItem
    {
        public Guid UserId { get; set; }
        public string CodeUser { get; set; }
        public string FullName { get; set; }
        public Decimal? DateCreated { get; set; }
        public string Content { get; set; }
        public string CodeCheckIn { get; set; }
    }
}
