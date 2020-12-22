using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class LevelVoteItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Value { get; set; }
        public int? Soft { get; set; }
        public int? AgencyID { get; set; }

        public virtual ICollection<ContentVoteItem> DN_ContentVote { get; set; }

    }
    public class ModelLevelVoteItem : BaseModelSimple
	{
        public List<LevelVoteItem> ListItems { get; set; }
    }
}
