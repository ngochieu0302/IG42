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
    
    public partial class GoogleMap
    {
        public int ID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LanguageId { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<int> Type { get; set; }
        public string Time { get; set; }
    
        public virtual System_City System_City { get; set; }
        public virtual System_District System_District { get; set; }
    }
}