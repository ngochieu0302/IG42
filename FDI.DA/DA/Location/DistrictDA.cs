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
    public class DistrictDA : BaseDA
    {
        public DistrictDA()
        {
        }

        public DistrictDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DistrictDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<DistrictItem> GetListbyRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.System_District
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            CityID = c.CityID,
                        };
            var city = httpRequest["CityID"];
            if (!string.IsNullOrEmpty(city))
            {
                var cityId = int.Parse(city);
                query = query.Where(c => c.CityID == cityId);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DistrictItem> GetAllListSimple()
        {
            var query = from c in FDIDB.System_District
                        orderby c.Name
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }
        public DistrictItem GetItemById(int id)
        {
            var query = from c in FDIDB.System_District
                        where c.IsDeleted != true && c.IsShow == true
                        && c.ID == id
                        orderby c.Name
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            CityID = c.CityID,
                        };
            return query.FirstOrDefault();
        }
        public List<DistrictItem> GetListSimple()
        {
            var query = from c in FDIDB.System_District
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }

        public System_District GetById(int id)
        {
            var query = from c in FDIDB.System_District
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public List<System_District> GetByListArrId(string lstInt)
        {
            var lst = FDIUtils.StringToListInt(lstInt);
            var query = from c in FDIDB.System_District
                        where lst.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public List<System_District> GetByListArrId(List<int> lstInt)
        {
            //var lst = FDIUtils.StringToListInt(lstInt);
            var query = from c in FDIDB.System_District
                        where lstInt.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public List<DistrictItem> GetListByCity(int cityId, bool isShow)
        {
            try
            {
                var query = from c in FDIDB.System_District
                            where c.CityID == cityId && c.IsShow == isShow
                            orderby c.Name
                            select new DistrictItem
                            {
                                ID = c.ID,
                                Name = c.Name.Trim()
                            };
                return query.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<DistrictItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.System_District
                        orderby c.Name
                        where c.IsShow == isShow
                        && c.Name.StartsWith(keyword)
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.Take(showLimit).ToList();
        }
        public void Add(System_District District)
        {
            FDIDB.System_District.Add(District);
        }

        public void Delete(System_District District)
        {
            FDIDB.System_District.Remove(District);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
