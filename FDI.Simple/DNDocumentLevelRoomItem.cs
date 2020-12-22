using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNDocumentLevelItem : BaseSimple
    {
        public string Name { get; set; }
        public string NameLevel { get; set; }

        public int? ParentId { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Sort { get; set; }
        public int? Level { get; set; }
    }



    public class ModelDNDocumentLevelItem : BaseModelSimple
    {
        public List<DNDocumentLevelItem> ListItem { get; set; }
        public int? AgencyId { get; set; }
    }
}
