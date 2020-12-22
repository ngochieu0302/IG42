namespace FDI.Simple
{
    public class ExportProductItem : BaseSimple
    {
        public int? ExportID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityOut { get; set; }
        public decimal? Price { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public int? ProductID { get; set; }
        public int? ImportID { get; set; }
        public bool? IsDelete { get; set; }


        public virtual DNExportProductItem DnExportProduct { get; set; }
        public virtual ImportProductItem ImportProduct { get; set; }
    }
    public class ExportProductNewItem
    {
        public int? ID { get; set; }
        public int? ExportID { get; set; }
        public int? ImportID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Key { get; set; }
    }
}
