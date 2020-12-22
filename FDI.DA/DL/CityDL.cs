using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class CityDl : BaseDA
    {
        public CityItem GetCityItemById(int id)
        {
            var query = from c in FDIDB.System_City
                        where c.ID == id
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            ListGoogleMapItem = c.GoogleMaps.Where(m => m.IsShow == true && m.LanguageId == LanguageId && m.CityID == id).OrderByDescending(m=>m.ID).Select(g => new GoogleMapItem
                            {
                                ID = g.ID,
                                Name = g.Name,
                                Address = g.Address,
                                Tel = g.Tel,
                                Fax = g.Fax,
                                Longitude = g.Longitude,
                                Latitude = g.Latitude
                            })
                        };
            return query.FirstOrDefault();
        }

        public CityItem GetDistrictItemByCityId(int id)
        {
            var query = from c in FDIDB.System_City
                        where c.ID == id
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            ListDistrictItem = c.System_District.Where(m=>m.IsShow == true && m.GoogleMaps.Any()).Select(d=> new DistrictItem
                            {
                                ID = d.ID,
                                Name = d.Name
                            }),
                           
                        };
            return query.FirstOrDefault();
        }

        public DistrictItem GetListGoogleMapByDistrictID(int id)
        {
            var query = from c in FDIDB.System_District
                        where c.ID == id
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            ListGoogleMapItem = c.GoogleMaps.Where(m => m.IsShow == true).OrderByDescending(m => m.ID).Select(g => new GoogleMapItem
                            {
                                ID = g.ID,
                                Name = g.Name,
                                Tel = g.Tel,
                                Fax = g.Fax,
                                Longitude = g.Longitude,
                                Latitude = g.Latitude,
                                Address = g.Address
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<CityItem> GetAllListByGoogleMap()
        {
            var query = from c in FDIDB.System_City
                        where c.IsWerther == true && c.IsShow && c.LanguageID == LanguageId && c.GoogleMaps.Any()
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            CodeWerther = c.CodeWerther
                        };
            return query.ToList();
        }
        // lay list by weather
        public List<CityItem> GetAllListByWeather()
        {
            var query = from c in FDIDB.System_City
                        where c.IsWerther == true && c.IsShow == true && c.LanguageID == LanguageId && c.CodeWerther != null
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            CodeWerther = c.CodeWerther
                        };
            return query.ToList();
        }
    }
}