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
    
    public partial class DN_Bed_Desk
    {
        public DN_Bed_Desk()
        {
            this.DN_Exchange = new HashSet<DN_Exchange>();
            this.DN_Exchange1 = new HashSet<DN_Exchange>();
            this.DN_User_BedDesk = new HashSet<DN_User_BedDesk>();
            this.Shop_ContactOrder = new HashSet<Shop_ContactOrder>();
            this.Shop_Orders = new HashSet<Shop_Orders>();
            this.DN_Packet = new HashSet<DN_Packet>();
            this.Shop_ContactOrder1 = new HashSet<Shop_ContactOrder>();
            this.Shop_Orders1 = new HashSet<Shop_Orders>();
        }
    
        public int ID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Name { get; set; }
        public Nullable<int> RoomId { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> PacketID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Row { get; set; }
        public Nullable<int> Column { get; set; }
    
        public virtual DN_Room DN_Room { get; set; }
        public virtual DN_Status DN_Status { get; set; }
        public virtual ICollection<DN_Exchange> DN_Exchange { get; set; }
        public virtual ICollection<DN_Exchange> DN_Exchange1 { get; set; }
        public virtual ICollection<DN_User_BedDesk> DN_User_BedDesk { get; set; }
        public virtual ICollection<Shop_ContactOrder> Shop_ContactOrder { get; set; }
        public virtual ICollection<Shop_Orders> Shop_Orders { get; set; }
        public virtual ICollection<DN_Packet> DN_Packet { get; set; }
        public virtual ICollection<Shop_ContactOrder> Shop_ContactOrder1 { get; set; }
        public virtual ICollection<Shop_Orders> Shop_Orders1 { get; set; }
    }
}
