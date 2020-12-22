using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class GoogleMapDl : BaseDA
    {
        public List<GoogleMapItem> GetGoogleMapsItemByDistrictID(int districtID)
        {
            var query = from g in FDIDB.GoogleMaps
                        where g.DistrictID == districtID where g.IsShow == true
                        orderby g.ID descending 
                        select new GoogleMapItem
                        {
                            ID = g.ID,
                            Name = g.Name,
                            Address = g.Address,
                            Tel = g.Tel,
                            Fax = g.Fax,
                            Longitude = g.Longitude,
                            Latitude = g.Latitude,
                            Type = g.Type,
                            CityID = g.CityID,
                            DistrictID = g.DistrictID
                        };
            return query.ToList();
        }

        public List<GoogleMapItem> GetGoogleMapsItemByCityId(int cityId)
        {
            var query = from g in FDIDB.GoogleMaps
                        where g.CityID == cityId && g.IsShow == true
                        orderby g.ID descending 
                        select new GoogleMapItem
                        {
                            ID = g.ID,
                            Name = g.Name,
                            Address = g.Address,
                            Tel = g.Tel,
                            Fax = g.Fax,
                            Longitude = g.Longitude,
                            Latitude = g.Latitude,
                            Type = g.Type,
                            CityID = g.CityID,
                            DistrictID = g.DistrictID
                        };
            return query.ToList();
        }

        public List<GoogleMapItem> GetAllListSimple()
        {
            var query = from g in FDIDB.GoogleMaps
                        where  g.IsShow == true && g.LanguageId == LanguageId
                        orderby g.ID descending 
                        select new GoogleMapItem
                        {
                            ID = g.ID,
                            Name = g.Name,
                            Address = g.Address,
                            Tel = g.Tel,
                            Fax = g.Fax,
                            Longitude = g.Longitude,
                            Latitude = g.Latitude,
                            Type = g.Type,
                            CityID = g.CityID,
                            DistrictID = g.DistrictID,
                            Time = g.Time
                        };
            return query.ToList();
        }
    }
}