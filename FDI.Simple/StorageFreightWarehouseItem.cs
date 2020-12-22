using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class StorageFreightWarehouseItem:BaseSimple
    {
        public string Code { get; set; }
        public decimal? DateCreated { get; set; }
        public Guid? UserID { get; set; }
        public string Note { get; set; }
        public int? AgencyId { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? DateImport { get; set; }
        public decimal? Payment { get; set; }
        public Guid? UserActive { get; set; }
        public decimal? DateActive { get; set; }
        public int? AgencyReceiveID { get; set; }
        public string UsernameCreate { get; set; }
        public string UsernameActive { get; set; }
        public int? Status { get; set; }
        public Guid? KeyGuid { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsOrder { get; set; }
        public IEnumerable<FreightWarehouseItem> LstImport { get; set; }
        public IEnumerable<FreightWarehouseActiveItem> LstImportActive { get; set; }
    }

    public class ModelStorageFreightWarehouseItem : BaseModelSimple
    {
        public IEnumerable<StorageFreightWarehouseItem> ListItems { get; set; }
    }

    public class StorageFreightWarehouseItemNew : BaseSimple
    {
        public string Note { get; set; }
        public string Fullname { get; set; }
        public decimal? DateCreate { get; set; }
        public string Url { get; set; }
    }
}
