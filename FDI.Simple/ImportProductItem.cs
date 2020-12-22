using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class ImportProductItem : BaseSimple
    {
        public int? Stt { get; set; }
        public Guid GID { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public string Code { get; set; }
        public string Imei { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityOut { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDelete { get; set; }
        public bool IsIn { get; set; }
        public bool IsDate { get; set; }
        public decimal? Value { get; set; }
        public int? CateValueID { get; set; }
        public int? ProductValueID { get; set; }
        public decimal? PriceNew { get; set; }
        public string UrlPicture { get; set; }

    }
    public class ImportProductAddItem : BaseSimple
    {
        public int? Stt { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool IsIn { get; set; }
        public decimal? Value { get; set; }
        public int? CateValueID { get; set; }
        public int? ProductValueID { get; set; }
        public decimal? PriceNew { get; set; }
    }
    public class ImportProductNewItem
    {
        public int? Quantity { get; set; }
        public string DateS { get; set; }
        public string DateE { get; set; }
        public decimal? Price { get; set; }
        public int ProductID { get; set; }
        public string BarCode { get; set; }
        public string Key { get; set; }
        public decimal? Value { get; set; }
    }
    public class ModelImportProductItem : BaseModelSimple
    {
        public ImportProductItem Item { get; set; }
        public IEnumerable<ImportProductItem> ListItem { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalOld { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductValueItem:BaseSimple
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? PriceCost { get; set; }
        public decimal? PriceNew { get; set; }
        public decimal? DateCreate { get; set; }
        public decimal? DateImport { get; set; }
        public string Barcode { get; set; }
        public decimal? Value { get; set; }
        public string Unitname { get; set; }
        public int? AgencyID { get; set; }
        public  int ProductId { get; set; }
        public IEnumerable<ImportProductItem> ListImportProductItems { get; set; }

    }
    public class ProductValueAddItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceNew { get; set; }
        public string Barcode { get; set; }
        public decimal? Value { get; set; }
        public string Unitname { get; set; }
        public int? AgencyID { get; set; }
        public int? ID { get; set; }
        public int? Stt { get; set; }
        public List<ImportProductAddItem> ListImportProductItems { get; set; }

    }
    public class CateValueItem : BaseSimple
    {
        public int? CateID { get; set; }
        public int? AgencyID { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal? PriceNew { get; set; }
        public decimal? PriceCost { get; set; }
        public decimal? DateCreate { get; set; }
        public decimal? DateImport { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityOut { get; set; }
        public string Unitname { get; set; }
        public string Code { get; set; }
        public IEnumerable<ProductValueItem> ListProductValueItems { get; set; }
    }

    public class CateValueAddItem : BaseSimple
    {
        public int? CateID { get; set; }
        public int? AgencyID { get; set; }
        public int? Pi { get; set; }
        public int? Stt { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal? PriceNew { get; set; }
        public decimal? PriceCost { get; set; }
        public decimal? DateCreate { get; set; }
        public decimal? DateImport { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityOut { get; set; }
        public string Unitname { get; set; }
        public string Code { get; set; }
        public List<ProductValueAddItem> ListProductValueItems { get; set; }
    }

    public class ModelCateValueItem : BaseModelSimple
    {
        public IEnumerable<CateValueItem> LisItems { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalOld { get; set; }
        public decimal? Quantity { get; set; }
    }
}
