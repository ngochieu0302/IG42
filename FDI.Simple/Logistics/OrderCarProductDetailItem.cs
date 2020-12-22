using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.CORE;

namespace FDI.Simple.Logistics
{
    public class OrderCarProductDetailItem : BaseSimple
    {
        public int OrderCarID { get; set; }
        public decimal Quantity { get; set; } 
        public string Code { get; set; }
        public string ProductName { get; set; }
        public  decimal PriceUnit { get; set; }
        public CateValueStatus Status { get; set; }
        public string Note { get; set; }

        public decimal Price
        {
            get { return Quantity * PriceUnit; }
        }
    }

    public class OrderCarProductDetailModel
    {
        public OrderCarProductDetailModel()
        {
            Order = new OrderCarItem();
            Item = new OrderCarProductDetailItem();
            Units = new List<DNUnitItem>();
        }
        public  OrderCarItem Order { get; set; }
        public OrderCarProductDetailItem Item { get; set; }
        public List<DNUnitItem> Units { get; set; }
    }

    public class OrderCarProductDetailResponse : BaseModelSimple
    {
        public List<OrderCarProductDetailItem> ListItem { get; set; }
    }
}
