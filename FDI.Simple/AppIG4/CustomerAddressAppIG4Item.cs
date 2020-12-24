using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CustomerAddressAppIG4Item : BaseSimple
    {
        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool? IsDefault { get; set; }
        public int? AddressType { get; set; }
        public double? Km { get; set; }
    }

    public class ModelCustomerAddressItem : BaseModelSimple
    {
        public IEnumerable<CustomerAddressAppIG4Item> ListItems { get; set; }
    }


}
