using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class ContentVoteItem :BaseSimple
    {
        public string Content { get; set; }
        public decimal? DateCreated { get; set; }
        public decimal? DateEvaluation { get; set; }
        public int? TreeID { get; set; }
        public int? VoteID { get; set; }
        public int? LevelVoteID { get; set; }
        public string VoteName { get; set; }
        public int? Value { get; set; }
        public Guid? UserID { get; set; }
        public Guid? UserTreeID { get; set; }
        public bool IsEdit { get; set; }
        public string NVDG { get; set; }
        public string US_NVDG { get; set; }
        public string NVBDG { get; set; }
        public string US_NVBDG { get; set; }
        public string LevelName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int? AgencyId { get; set; }
        public DNTreeItem DNTreeItem { get; set; }
	    public DNUserItem UserItem { get; set; }
	}

    public class SumContentVoteItem 
    {
        public decimal? Week { get; set; }
        public decimal? Month { get; set; }
        public decimal? Year { get; set; }
    }
    public class ModelContentVoteItem : BaseModelSimple
    {
        public List<ContentVoteItem> ListItems { get; set; }
        public List<DNUserSimpleItem>  UserItems { get; set; }
        public List<VoteItem> VoteItems { get; set; }
        public int? TotalValue { get; set; }
    }
}
