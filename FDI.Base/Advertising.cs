//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class Advertising
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Show { get; set; }
        public Nullable<decimal> StartDate { get; set; }
        public Nullable<decimal> EndDate { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> PictureID { get; set; }
        public Nullable<int> PositionID { get; set; }
        public Nullable<decimal> CreateOnUtc { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<int> Clicked { get; set; }
        public Nullable<bool> IsMobile { get; set; }
        public string MobileLink { get; set; }
        public string LanguageId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> AgencyID { get; set; }
    
        public virtual Advertising_Position Advertising_Position { get; set; }
        public virtual Advertising_Type Advertising_Type { get; set; }
        public virtual Gallery_Picture Gallery_Picture { get; set; }
    }
}
