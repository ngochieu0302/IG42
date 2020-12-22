using System;

namespace FDI.Simple
{
    public class AttributeOptionsItem : BaseSimple
    {
        public int? AttributeID { get; set; }
        public int? ProductID { get; set; }
        public string Values { get; set; }
        public int? ControlValuesID { get; set; }
        public string Description { get; set; }
        public int? Sort { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserName { get; set; }
        public int? UpdatedDate { get; set; }
        public int? UpdatedUserName { get; set; }
        public int? LanguageId { get; set; }
        public int? IsShow { get; set; }
        public int? IsDeleted { get; set; }
        public int? AgencyID { get; set; }
    }
}
