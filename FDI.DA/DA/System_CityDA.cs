using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class System_CityDA : BaseDA
    {
        public System_CityDA()
        {
        }

        public System_CityDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public System_CityDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }


        public List<CityItem> GetAll(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.System_City
                        where c.LanguageID == LanguageId && c.Show
                        orderby c.Name
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public CityItem GetItemById(int id)
        {
            var query = from c in FDIDB.System_City
                        where c.LanguageID == LanguageId && c.Show
                        orderby c.Name
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.FirstOrDefault();
        }

        public List<CityItem> GetListSimple()
        {
            var query = from c in FDIDB.System_City
                        where c.LanguageID == LanguageId && c.Show
                        orderby c.Name
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }
        public List<DistrictItem> GetListDistrict(int cityId)
        {
            var query = from c in FDIDB.System_District
                        where c.CityID == cityId
                        orderby c.Name
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }
        public List<CityItem> GetListSimpleAll(bool isShow)
        {
            var query = from c in FDIDB.System_City
                        where (c.Show == isShow) && c.LanguageID == LanguageId
                        orderby c.Name
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }
        public List<DistrictItem> GetListSimpleAll(int cityId)
        {
            var query = from c in FDIDB.System_District
                        where c.CityID == cityId
                        orderby c.Name
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }

        public System_City GetById(int id)
        {
            var query = from c in FDIDB.System_City
                where c.ID == id
                orderby c.ID descending 
                select c;
            return query.FirstOrDefault();
        }

        public List<System_City> GetListByArrId(List<int> lstId)
        {
            var query = from c in FDIDB.System_City
                        where lstId.Contains(c.ID)
                        orderby c.ID descending
                        select c;
            return query.ToList();
        }

        public void Add(System_City systemCity)
        {
            FDIDB.System_City.Add(systemCity);
        }
        public void Delete(System_City systemCity)
        {
            FDIDB.System_City.Remove(systemCity);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
