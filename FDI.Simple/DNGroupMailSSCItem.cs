using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class DNGroupMailSSCItem : BaseSimple
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public decimal? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public int? AgencyID { get; set; }
        public IEnumerable<DNUserItem> ListDNUserItem { get; set; }
        public string LstUserIds { get; set; }
    }
    public class ModelDNGroupMailSSCItem : BaseModelSimple
    {
        public IEnumerable<DNGroupMailSSCItem> ListItem { get; set; }
        
    }
}
