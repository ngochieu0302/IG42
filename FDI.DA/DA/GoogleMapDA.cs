using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class GoogleMapDA : BaseDA
    {
        #region Constructer
        public GoogleMapDA()
        {

        }

        public GoogleMapDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public GoogleMapDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<GoogleMapItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.GoogleMaps
                        where c.LanguageId == LanguageId
                        orderby c.ID descending
                        select new GoogleMapItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name.Trim(),
                                       Address = c.Address,
                                       Tel = c.Tel,
                                       IsShow = c.IsShow,
                                       Fax = c.Fax,
                                       Longitude = c.Longitude,
                                       Latitude = c.Latitude,
                                       Type = c.Type,
                                       Time = c.Time,
                                       CityID = c.CityID,
                                       DistrictID = c.DistrictID
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public GoogleMapItem GetGoogleMapItemById(int id)
        {
            var query = from c in FDIDB.GoogleMaps
                        where c.ID == id
                        orderby c.ID descending
                        select new GoogleMapItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name.Trim(),
                                       Address = c.Address,
                                       Tel = c.Tel,
                                       Fax = c.Fax,
                                       Longitude = c.Longitude,
                                       Latitude = c.Latitude,
                                       Type = c.Type,
                                       Time = c.Time,
                                       CityID = c.CityID,
                                       DistrictID = c.DistrictID
                                   };
            return query.FirstOrDefault();
        }

        public GoogleMap GetById(int id)
        {
            var query = from c in FDIDB.GoogleMaps where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<GoogleMap> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.GoogleMaps where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(GoogleMap googleMap)
        {
            FDIDB.GoogleMaps.Add(googleMap);
        }

        public void Delete(GoogleMap googleMap)
        {
            FDIDB.GoogleMaps.Remove(googleMap);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
