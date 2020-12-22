using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Simple;

namespace FDI.MvcAPI.Models
{
    public class OrderGabModel: CategoryItem
    {
        public List<OrderDetailProductItem> Products { get; set; }
        
    }

}