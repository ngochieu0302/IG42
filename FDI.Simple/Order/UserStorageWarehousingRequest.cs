using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.Order
{
    public class UserStorageWarehousingRequest
    {
        public int orderId { get; set; }
        public Guid[] userIds { get; set; }
    }
}
