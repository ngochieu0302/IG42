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
    
    public partial class ST_Group
    {
        public ST_Group()
        {
            this.DN_Agency = new HashSet<DN_Agency>();
            this.DN_Enterprises = new HashSet<DN_Enterprises>();
            this.DN_Module = new HashSet<DN_Module>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Sort { get; set; }
    
        public virtual ICollection<DN_Agency> DN_Agency { get; set; }
        public virtual ICollection<DN_Enterprises> DN_Enterprises { get; set; }
        public virtual ICollection<DN_Module> DN_Module { get; set; }
    }
}