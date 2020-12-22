using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNImportItem : BaseSimple
    {
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityOut { get; set; }
        public int? QuantityDay { get; set; }
        public decimal? Price { get; set; }       
        public string ValueName { get; set; }
        public string UnitName { get; set; }
        public int? ValueId { get; set; }
        public Guid? UserID { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsDelete { get; set; }
        public string Key { get; set; }

        public static implicit operator DNImportItem(List<DNImportItem> v)
        {
            throw new NotImplementedException();
        }
    }
    public class DNImportNewItem
    {
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? ValueId { get; set; }
        public int? QuantityDay { get; set; }
        public string Key { get; set; }
    }
}
