using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CityItem : BaseSimple
    {
        public string Name { get; set; }
        public bool Show { get; set; }
        public string Description { get; set; }
        public int TotalDistricts { get; set; }
        public string CodeWerther { get; set; }
        public string IsWerther { get; set; }
        public string LanguageID { get; set; }
        public IEnumerable<DistrictItem> ListDistrictItem { get; set; }
        public IEnumerable<GoogleMapItem> ListGoogleMapItem { get; set; }
    }
    public class ModelCityItem : BaseModelSimple
    {
        public CityItem CityItem { get; set; }
        public IEnumerable<CityItem> ListItem { get; set; }
        public IEnumerable<DistrictItem> ListDistrictItem { get; set; }
        public IEnumerable<GoogleMapItem> ListGoogleMapItem { get; set; }
    }
}
