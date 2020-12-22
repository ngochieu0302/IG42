using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class VoteItem : BaseSimple
    {
        public string Name { get; set; }
        public DNUserVoteItem DNUserVoteItem { get; set; }
        public int? Soft { get; set; }
        public int? AgencyID { get; set; }
        public bool? IsVote { get; set; }
        public int? Value { get; set; }

	    public int? TotalValue { get; set; }
		public IEnumerable<ContentVoteItem> ContentVoteItems { get; set; }
    }

    public class ModelVoteItem : BaseModelSimple
    {
        public List<VoteItem> ListItems { get; set; }
        public string Dates { get; set; }
        public string Datee { get; set; }
        public string UserID { get; set; }
        public List<LevelVoteItem> LevelVoteItems { get; set; }
    }
}
