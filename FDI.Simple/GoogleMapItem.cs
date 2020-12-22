using System.Collections.Generic;

namespace FDI.Simple
{
    public class GoogleMapItem:BaseSimple
    {
        public int? DistrictID { get; set; }
        public int? CityID { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LanguageId { get; set; }
        public bool? IsShow { get; set; }
        public IEnumerable<DistrictItem> ListDistrictItem { get; set; }
    }

    public class ModelGoogleMapItem:BaseModelSimple
    {
        public IEnumerable<DistrictItem> ListDistrictItem { get; set; }
        public IEnumerable<GoogleMapItem> ListGoogleMapItem { get; set; }
    }
}
