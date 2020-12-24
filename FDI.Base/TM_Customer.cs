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
    
    public partial class TM_Customer
    {
        public TM_Customer()
        {
            this.TM_Module_Rating = new HashSet<TM_Module_Rating>();
            this.TM_News_Comment = new HashSet<TM_News_Comment>();
            this.TM_Products_Comment = new HashSet<TM_Products_Comment>();
            this.TM_Rate_Comment = new HashSet<TM_Rate_Comment>();
        }
    
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public Nullable<decimal> Birthday { get; set; }
        public Nullable<int> PictureID { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Nullable<decimal> DateActive { get; set; }
        public string UserActive { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        public virtual ICollection<TM_Module_Rating> TM_Module_Rating { get; set; }
        public virtual ICollection<TM_News_Comment> TM_News_Comment { get; set; }
        public virtual ICollection<TM_Products_Comment> TM_Products_Comment { get; set; }
        public virtual ICollection<TM_Rate_Comment> TM_Rate_Comment { get; set; }
    }
}