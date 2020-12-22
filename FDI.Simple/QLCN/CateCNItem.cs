using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.QLCN
{
    public class CateCNItem:BaseSimple
    {
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsShow { get; set; }
    }

    public class ModelCateCNItem : BaseModelSimple
    {
        public IEnumerable<CateCNItem> ListItems { get; set; }
    }
}
