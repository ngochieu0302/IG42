using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNUnitItem:BaseSimple
    {
        public string Name { get; set; }
        public int? AgencyId { get; set; }
    }

    public class ModelDNUnitItem : BaseModelSimple
    {
        public List<DNUnitItem> ListItems { get; set; }
    }
}
