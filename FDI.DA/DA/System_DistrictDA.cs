using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class System_DistrictDA : BaseDA
    {
        #region Constructer
        public System_DistrictDA()
        {
        }

        public System_DistrictDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public System_DistrictDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

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

        public List<DistrictItem> GetListByCity(int cityId)
        {
            try
            {
                var query = from c in FDIDB.System_District
                            where c.CityID == cityId && c.Show == true
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
                        where c.Show == isShow
                        && c.Name.StartsWith(keyword)
                        select new DistrictItem
                                   {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.Take(showLimit).ToList();
        }

        public DistrictItem GetItemById(int id)
        {
            var query = from c in FDIDB.System_District
                        orderby c.Name
                        where c.ID == id
                        select new DistrictItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.FirstOrDefault();
        }

    
        public List<DistrictItem> GetListByCityId(int cityId)
        {
            var query = from o in FDIDB.System_District
                        where o.CityID ==  cityId
                        orderby o.Name
                        select new DistrictItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Show = o.Show,
                            Description = o.Description,
                            CityName = o.System_City.Name.Trim(),
                            CityID = o.System_City.ID
                        };
            return query.ToList();
        }

        public List<DistrictItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.System_District
                        orderby o.Name
                        select new DistrictItem
                                   {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Show = o.Show,
                            Description = o.Description,
                            CityName = o.System_City.Name.Trim(),
                            CityID = o.System_City.ID
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public System_District GetById(int id)
        {
            var query = from c in FDIDB.System_District where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<System_District> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.System_District where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        
        public void Add(System_District systemDistrict)
        {
            FDIDB.System_District.Add(systemDistrict);
        }

        public void Delete(System_District systemDistrict)
        {
            FDIDB.System_District.Remove(systemDistrict);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
