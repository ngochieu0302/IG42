using System;
using System.Collections.Generic;

namespace FDI.Simple.StorageWarehouse
{
    public class TotalProductToDayItem : BaseSimple
    {
        public decimal ToDayCode { get; set; }
        public decimal Quantity { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public decimal? Price { get; set; } 
        public Guid UserId { get; set; }
        public decimal UnitPrice { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public decimal QuantityActive { get; set; }
    }

    public class TotalProductToDayConfirmItem : BaseSimple
    {
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public int SupplierId { get; set; }
        public string SupplierAddress { get; set; }
        public IList<ProductConfirm> Details;
    }

    public class ProductConfirm
    {
        public int ProductId { get; set; }
        public  string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityActive { get; set; }
        public decimal QuantityReady { get; set; }
        public int SupplierId { get; set; }
    }

    public class TotalProductToDayModel
    {
        public decimal? Quantity { get; set; }
        public decimal? QuantityActive { get; set; }
        public CategoryItem CategoryModel { get; set; }
    }

    public class ModelTotalProductToDayItem : BaseModelSimple
    {
        public IList<TotalProductToDayItem> ListItems { get; set; }
    }
}
