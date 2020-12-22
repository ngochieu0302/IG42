using System;
using System.Collections.Generic;


namespace FDI.Simple
{
    public class VoteTypeItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsHome { get; set; }
        public int? Sort { get; set; }
        public DateTime DateCreate { get; set; }
        public string LanguageId { get; set; }

        public IEnumerable<VoteItem> ListVoteItem { get; set; }
    }

    public class ModelVoteTypeItem : BaseModelSimple
    {
        public IEnumerable<VoteTypeItem> ListItem { get; set; }
    }
}
