using System.Collections.Generic;

namespace FDI.Simple
{
    public class TypeCriteriaItem : BaseSimple
    {
        public string Name { get; set; }
        public bool? IsPercent { get; set; }
    }
    public class ModelTypeCriteriaItem : BaseModelSimple
    {
        public IEnumerable<TypeCriteriaItem> ListItem { get; set; }

    }
}
