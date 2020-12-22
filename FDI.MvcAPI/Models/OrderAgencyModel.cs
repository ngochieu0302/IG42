using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FDI.MvcAPI.Models
{
    public class OrderAgencyModel
    {
        public string[] BarCodes { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
    }
}