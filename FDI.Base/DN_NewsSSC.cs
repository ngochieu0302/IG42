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
    
    public partial class DN_NewsSSC
    {
        public DN_NewsSSC()
        {
            this.DN_News_Comment = new HashSet<DN_News_Comment>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<int> AgencyID { get; set; }
    
        public virtual ICollection<DN_News_Comment> DN_News_Comment { get; set; }
    }
}