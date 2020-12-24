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
    
    public partial class BonusType
    {
        public BonusType()
        {
            this.RewardHistories = new HashSet<RewardHistory>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> PercentParent { get; set; }
        public Nullable<int> RootID { get; set; }
        public Nullable<decimal> PercentRoot { get; set; }
        public Nullable<decimal> Percent { get; set; }
        public Nullable<bool> IsExit { get; set; }
        public Nullable<decimal> DateCreate { get; set; }
        public Nullable<int> EnterprisesId { get; set; }
        public Nullable<int> Type { get; set; }
    
        public virtual DN_Enterprises DN_Enterprises { get; set; }
        public virtual ICollection<RewardHistory> RewardHistories { get; set; }
    }
}
