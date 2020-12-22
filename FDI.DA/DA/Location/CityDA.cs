using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class CityDA : BaseDA
    {
        public CityDA()
        {
        }

        public CityDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CityDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<CityItem> GetListbyRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.System_City
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public CityItem GetItemById(int id)
        {
            var query = from c in FDIDB.System_City
                        where c.IsDeleted != true && c.IsShow == true
                        && c.ID == id
                        orderby c.Name
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.FirstOrDefault();
        }
        public List<CityItem> GetAll()
        {
            var query = from c in FDIDB.System_City
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }
        public List<DistrictItem> GetListDistrict(int id)
        {
            var query = from c in FDIDB.System_District
                        where c.IsDeleted != true && c.IsShow == true
                        && c.CityID == id
                        orderby c.Name
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }
        public List<CityItem> GetListSimple()
        {
            var query = from c in FDIDB.System_City
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new CityItem
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
                        select c;
            return query.FirstOrDefault();
        }
        public List<System_City> GetByListArrId(string lstInt)
        {
            var lst = FDIUtils.StringToListInt(lstInt);
            var query = from c in FDIDB.System_City
                        where lst.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public List<CityItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.System_City
                        orderby c.Name
                        where c.IsShow == isShow
                        && c.Name.StartsWith(keyword)
                        select new CityItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.Take(showLimit).ToList();
        }
        public void Add(System_City City)
        {
            FDIDB.System_City.Add(City);
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
