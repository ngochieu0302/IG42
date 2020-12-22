namespace FDI.Simple
{
    public class ExportProductValueItem:BaseSimple
    {
        public int? ExportID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceExport { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public string ValueName { get; set; }
        public string UnitName { get; set; }
        public int? ImportID { get; set; }
        public int? ValueID { get; set; }
        public bool? IsDelete { get; set; }

        public virtual DNExportItem DNExport { get; set; }
        public virtual DNImportItem DNImport { get; set; }
    }
    public class ExportNewItem
    {
        public int? ID { get; set; }
        public int? ExportID { get; set; }
        public int? ImportID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceExport { get; set; }
        public string Key { get; set; }
    }
}
