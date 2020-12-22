using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNNewsCommentItem : BaseSimple
    {
        public int? NewsSSCID { get; set; }
        public int? ParentId { get; set; }
        public Guid? UserId { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public decimal? DateCreated { get; set; }
        public int? IsLevel { get; set; }
        public bool? IsShow { get; set; }
        public int? AgencyID { get; set; }

        public virtual DNNewsSSCItem DNNewsSSCItem { get; set; }
        public virtual DNUserItem DNUserItem { get; set; }
    }
    public class ModelDNNewsCommentItem : BaseModelSimple
    {
        public int? Type { get; set; }
        public DNNewsCommentItem Item { get; set; }
        public IEnumerable<DNNewsCommentItem> ListItem { get; set; }
    }
}
