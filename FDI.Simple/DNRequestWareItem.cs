using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.CORE;
using FDI.Simple.StorageWarehouse;

namespace FDI.Simple
{
    public class DNRequestWareItem : BaseSimple
    {
        public Guid GID { get; set; }
        public int? StorageProductID { get; set; }
        public int? CateID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Sale { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? Date { get; set; }

        public decimal? TotalPrice
        {
            get
            {
                if (Quantity == null || Price == null)
                {
                    return 0;
                }
                return Price * Quantity - Sale ?? 0;
            }
        }

        public decimal? Today { get; set; }
        public int? Hour { get; set; }
        public string UserUpdate { get; set; }
        public decimal? DateUpdate { get; set; }
        public decimal? QuantityActive { get; set; }
        public int? AgencyID { get; set; }
        public int? MarketID { get; set; }
        public int? AreaID { get; set; }
        public int? Day { get; set; }
        public decimal? DateEnd { get; set; }
        public decimal? QuantityUsed { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public StatusWarehouse OrderStatus { get; set; }
        public string AgencyMobile { get; set; }
        public string AgencyAddress { get; set; }
    }

    public class DNRequestWareTotalItem
    {
        public decimal? Today { get; set; }
        public int? CateID { get; set; }
        public int SupplierId { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityActive { get; set; }
        public decimal? QuantityNotConfirm { get; set; }
        public List<TotalProductToDayItem> Details { get; set; }
    }
    public class ModelDNRequestWareItem : BaseModelSimple
    {
        public IEnumerable<DNRequestWareItem> ListItems { get; set; }
    }

    public class OrderDetailProductItem
    {
        public int CateID { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SizeId { get; set; }
        public string UnitName { get; set; }
        public decimal Quantity { get; set; }
        public double UnitValue { get; set; }
        public decimal Weight { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal WeightRecipe { get; set; }
    }
}
