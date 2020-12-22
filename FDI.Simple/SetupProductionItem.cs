using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class SetupProductionItem : BaseSimple
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? UserCreate { get; set; }
        public decimal? DateCreate { get; set; }
        public bool? IsDelete { get; set; }
        public int? Percent { get; set; }

    }
    public class ModelSetupProductionItem : BaseModelSimple
    {
        public IEnumerable<SetupProductionItem> ListItem { get; set; }
    }
}
