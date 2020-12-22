using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNFileMailItem : BaseSimple
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public string Folder { get; set; }
        public int? AgencyId { get; set; }
    }
    public class ModelDNFileMailItem : BaseModelSimple
    {
        public DNFileMailItem Item { get; set; }
        public IEnumerable<DNFileMailItem> ListItem { get; set; }
    }
}
