using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class OrderDebtItem : BaseSimple
    {
        public int ID { get; set; }
        public Guid? UserID { get; set; }
        public decimal? Pricetotal { get; set; }
        public string Note { get; set; }
        public decimal? Datecreate { get; set; }
        public int? SupplieId { get; set; }
        public string UserName { get; set; }
        public string SupplieName { get; set; }
    }
    public class ModelOrderDebtItem : BaseModelSimple
    {
        public IEnumerable<OrderDebtItem> ListItem { get; set; }
        public OrderDebtItem Item { get; set; }
        public List<SupplieItem> ListNcc { get; set; }
        public int ID { get; set; }
        public Guid? UserID { get; set; }
        public decimal? Pricetotal { get; set; }
        public string Note { get; set; }
        public decimal? Datecreate { get; set; }
        public int? SupplieId { get; set; }
        public int? AgencyId { get; set; }
        public string UserName { get; set; }
        public string SupplieName { get; set; }
        //public IEnumerable<AdvertisingPositionItem> ListPositionItem { get; set; }
    }
}
