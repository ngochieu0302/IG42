using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class HomeNetworkItem :BaseSimple
    {
        public string Name { get; set; }
    }

    public class ModelHomeSupplierItem : BaseSimple
    {
        public IEnumerable<HomeNetworkItem> ListNetworkItems { get; set; }
        public IEnumerable<SupplierItem> ListSupplierItems { get; set; }
    }
}
