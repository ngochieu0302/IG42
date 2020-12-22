using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class StorageProductItem : BaseSimple
    {
        public string Code { get; set; }
        public decimal? DateCreated { get; set; }
        public decimal? DateImport { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public int? AgencyId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Payment { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsOrder { get; set; }
        public int? CateID { get; set; }
        public int? SupID { get; set; }
        public int? TotalID { get; set; }
        public int? Hour { get; set; }
        public int? HourImport { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Suppliername { get; set; }
        public string Catename { get; set; }
        public decimal? Today { get; set; }
        public int? Hours { get; set; }
        public IEnumerable<ImportProductItem> LstImport { get; set; }
        public IEnumerable<ExportProductDetailItem> LstExport { get; set; }
    }

    public class ModelStorageProductItem : BaseModelSimple
    {
        public IEnumerable<StorageProductItem> ListItem { get; set; }
    }

    


   
}
