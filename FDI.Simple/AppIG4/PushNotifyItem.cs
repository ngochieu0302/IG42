using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class PushNotifyItem:BaseSimple
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    public class TokenDeiveAppIG4Item : BaseSimple
    {
        public string Token { get; set; }
        public string Mobile { get; set; }
        public int? AreaId { get; set; }
        public string App { get; set; }
        public string UserId { get; set; }
    }
}
