using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class StorageWarehouseItem:BaseSimple
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
        public bool? IsActive { get; set; }
        public virtual IEnumerable<DNRequestWareHouseItem> DnRequestWareHouseItems { get; set; }
    }
    //public class ModelStorageWarehouseItem
}
