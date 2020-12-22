using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DepartmentItem : BaseSimple
    {
        public int? AgencyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Sort { get; set; }
        public decimal? DateCreate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsShow { get; set; }
    }
    public class ModelDepartmentItem : BaseModelSimple
    {
        public DepartmentItem Item { get; set; }
        public IEnumerable<DepartmentItem> ListItem { get; set; }
    }
}
