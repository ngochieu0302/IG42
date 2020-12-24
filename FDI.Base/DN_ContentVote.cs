//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class DN_ContentVote
    {
        public DN_ContentVote()
        {
            this.DN_Vote1 = new HashSet<DN_Vote>();
        }
    
        public int ID { get; set; }
        public string Content { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<decimal> DateEvaluation { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> TreeID { get; set; }
        public Nullable<int> LevelVoteID { get; set; }
        public Nullable<int> VoteID { get; set; }
        public Nullable<int> Value { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<int> AgencyId { get; set; }
    
        public virtual DN_Tree DN_Tree { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Vote DN_Vote { get; set; }
        public virtual LevelVote LevelVote { get; set; }
        public virtual ICollection<DN_Vote> DN_Vote1 { get; set; }
    }
}