using System.Collections.Generic;

namespace FDI.Simple
{
    public class AttributeDynamicItem:BaseSimple
    {
        public int? AttributeGroupID { get; set; }
        public int? ControlType { get; set; }
        public int? CategoryControlID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public int? ProductId { get; set; }
        public int? Validate { get; set; }
        public int? Sort { get; set; }
        public string LanguageId { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? AgencyID { get; set; }
        public IEnumerable<int> LstInt { get; set; }
    }

    public class ModelAttributeDynamic : BaseModelSimple
    {
        public List<AttributeDynamicItem> ListItems { get; set; }
    }
}
