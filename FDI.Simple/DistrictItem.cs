using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class DistrictItem : BaseSimple
    {
        public string Name { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public bool Show { get; set; }
        public string Description { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<GoogleMapItem> ListGoogleMapItem { get; set; }
    }

    public class ModelDistrictItem : BaseModelSimple
    {
        public DistrictItem DistrictItem { get; set; }
        public IEnumerable<DistrictItem> ListItem { get; set; }

    }


}
