using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class StorageItem:BaseSimple
    {
        public string Code { get; set; }
        public decimal? DateCreated { get; set; }
        public decimal? DateImport { get; set; }
        public Guid? UserID { get; set; }
        public Guid? UserGet { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public int? AgencyId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Payment { get; set; }
        public bool? IsDelete { get; set; }
        public IEnumerable<DNImportItem> LstImport { get; set; }
    }

    public class ModelStorageItem : BaseModelSimple
    {
        public IEnumerable<StorageItem> ListItem { get; set; }
    }
}
