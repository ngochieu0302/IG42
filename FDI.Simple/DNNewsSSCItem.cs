using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNNewsSSCItem : BaseSimple
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool? IsShow { get; set; }
        public decimal? DateCreated { get; set; }
        public int? AgencyID { get; set; }
        public IEnumerable<DNNewsCommentItem> ListDNNewsCommentItem { get; set; }
    }
    public class ModelDNNewsSSCItem : BaseModelSimple
    {
        public int? Type { get; set; }
        public DNNewsSSCItem DNNewsSSCItem { get; set; }
        public IEnumerable<DNNewsSSCItem> ListItem { get; set; }
    }
}
