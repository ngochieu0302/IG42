using System.Collections.Generic;

namespace FDI.Simple
{
    public class SupplieItem : BaseSimple
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public decimal? Price { get; set; }
        public string Email { get; set; }
        public decimal? TotalPriceOrder { get; set; }
        public decimal? TotalRecive { get; set; }
        public decimal? TotalRewar { get; set; }
        public decimal? PriceSim { get; set; }
        public int PercentSim { get; set; }
        public int Percent { get; set; }
        public int? Status { get; set; }
    }
    public class DNSupplierItem : BaseSimple
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string TaxCode { get; set; }
        public string NameCompaly { get; set; }
        public string AddressCompany { get; set; }
        public string Credits { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? DateCreate { get; set; }
        public decimal? DateImport { get; set; }
        public int? EnterpriseID { get; set; }
        public bool? IsHome { get; set; }
        public decimal? TotalPriceOrder { get; set; }
        public decimal? TotalRecive { get; set; }
        public decimal? TotalRewar { get; set; }
        public decimal? TotalDebt { get; set; }
        public bool? IsLook { get; set; }
        public IEnumerable<StorageProductItem> LstStorageProductItems { get; set; }
        public string Note { get; set; }
    }

    public class ModelDNSupplierItem : BaseModelSimple
    {
        public IEnumerable<DNSupplierItem> ListItem { get; set; }

        //public IEnumerable<AdvertisingPositionItem> ListPositionItem { get; set; }
    }
    public class ModelSupplierItem : BaseModelSimple
    {
        public int ID { get; set; }
        public IEnumerable<SupplieItem> ListItem { get; set; }
        public List<int> ListInts { get; set; }
    }

    public class SupplieProductItem : BaseSimple
    {
        public int CateId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
    }
    public class SupplieProductResponse : BaseModelSimple
    {
        public IEnumerable<SupplieProductItem> ListItem { get; set; }
    }
}
