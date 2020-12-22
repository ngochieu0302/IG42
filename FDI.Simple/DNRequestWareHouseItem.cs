using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple.Order;

namespace FDI.Simple
{
    public class DNRequestWareHouseItem : BaseSimple
    {
        public Guid GID { get; set; }
        public int? StorageProductID { get; set; }
        public int? CateID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityActive { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? Date { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public decimal? Today { get; set; }
        public int? Hours { get; set; }
        public string UserUpdate { get; set; }
        public decimal? DateUpdate { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Usercreate { get; set; }
        public string Agencyname { get; set; }
        public string Mobile { get; set; }
        public string Marketname { get; set; }
        public IEnumerable<RequestWareDetail> Details { get; set; }

        public IEnumerable<RequestWareSupplier> RequestWareSuppliers { get; set; }
        public virtual StorageWarehousingItem StorageWarehousing { get; set; }
    }

    public class RequestWareSupplier
    {
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public Guid RequestWareId { get; set; }
        public decimal Quantity { get; set; }
        public int Id { get; set; }
    }
    public class DNRequestWareHouseActiveItem : BaseSimple
    {
        public int? StorageProductID { get; set; }
        public int? CateValueID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public string BarCode { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }

        public IList<RequestWareDetail> Details { get; set; }

        public virtual StorageWarehousingItem StorageWarehousing { get; set; }
    }
    public class DNRequestWareHouseNewItem
    {
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int ProductID { get; set; }
        public string Key { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Hours { get; set; }
        public int? MarketId { get; set; }
        public int? AreaId { get; set; }
    }
    public class DNRequestWareHouseActiveNewItem
    {
        public decimal? Quantity { get; set; }
        public string DateS { get; set; }
        public string DateE { get; set; }
        public decimal? Price { get; set; }
        public int ProductID { get; set; }
        public string Key { get; set; }
        public decimal? TotalPrice { get; set; }
        public string BarCode { get; set; }
        public int CatevalueId { get; set; }
    }
    public class ModelDNRequestWareHouseItem : BaseModelSimple
    {
        public IEnumerable<DNRequestWareHouseItem> ListItems { get; set; }
        public StorageWarehousingItem StorageWare { get; set; }
    }

    public class DNRequestWareHouseKey
    {
        public decimal? ToDay { get; set; }
        public int? ProductId { get; set; }

    }
}
