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
    
    public partial class System_MessageTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string BodyContent { get; set; }
        public string BccEmail { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> EmailAccountId { get; set; }
        public string EmailSend { get; set; }
        public string PassEmailSend { get; set; }
        public string EmailReceive { get; set; }
        public string CCEmail { get; set; }
        public string SMTP { get; set; }
        public string Port { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
