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
    
    public partial class TM_Answer
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> CategoryAnswerID { get; set; }
        public Nullable<decimal> CreatedDate { get; set; }
        public string Content { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}