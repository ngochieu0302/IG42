using FDI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class OrderPackageAppIG4Item : BaseSimple
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Packet { get; set; }
        public int? TypeID { get; set; }
        public int? CustomerID { get; set; }
        public string Customername { get; set; }
        public string CustomerPolicy { get; set; }
        public int? CustomerPolicyID { get; set; }
        public string Address { get; set; }
        public decimal? Datecreate { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public decimal? Price { get; set; }
        public string ImagesUrl { get; set; }
        public string Color { get; set; }
        public virtual Customer_Type Customer_Type { get; set; }
    }

    public class ModelOrderPackageAppIG4Item : BaseModelSimple
    {
        public IEnumerable<OrderPackageAppIG4Item> ListItems { get; set; }
        public decimal? Total { get; set; }

    }
    public class OrderPacketAppAppIG4Item
    {
        public int TypeID { get; set; }
        public int? CustomerID { get; set; }
        public string DateStart { get; set; }
    }
}
