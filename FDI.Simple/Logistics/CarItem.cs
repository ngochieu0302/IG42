using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.Logistics
{
    public class CarItem : BaseSimple
    {
        public string Name { get; set; }
        public string CarType { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }
        public long? Latitude { get; set; }
        public long? Longitude { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
    }

    public class CarResponse : BaseModelSimple
    {
        public List<CarItem> ListItem { get; set; }
    }
}
