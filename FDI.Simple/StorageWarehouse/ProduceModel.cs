using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.StorageWarehouse
{
    public class ProduceModel : BaseModelSimple
    {
        public IList<DNRequestWareItem> CategorysDetail { get; set; }
        public IList<OrderDetailProductItem> RequestWareItems { get; set; }

        public List<ProduceItem> ListItems { get; set; }

        public ProduceItem Produce { get; set; }
        public List<CategoryRecipeItem> CategoryRecipe { get; set; }

    }
}
