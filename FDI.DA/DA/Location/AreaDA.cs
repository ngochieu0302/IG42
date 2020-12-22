using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class AreaDA:BaseDA
    {
        public AreaDA()
        {
        }

        public AreaDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public AreaDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        
        public List<AreaItem> GetListbyRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Areas
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new AreaItem
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

        public List<AreaStaticItem> GetListStaticbyRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var date = DateTime.Today.TotalSeconds();
            var df = !string.IsNullOrEmpty(httpRequest["datefrom"])
                ? ConvertUtil.ToDateTime(httpRequest["datefrom"]).TotalSeconds()
                : 0;
            var dt = !string.IsNullOrEmpty(httpRequest["dateto"])
               ? ConvertUtil.ToDateTime(httpRequest["dateto"]).TotalSeconds()
               : date;
            var cate = int.Parse(httpRequest["CategoryID"] ?? "0");
            var query = from c in FDIDB.Areas
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new AreaStaticItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            CityID = c.CityID,
                            AgencyID = c.AgencyID,
                            Total = c.DN_Agency.Cate_Value.Where(b => (!b.IsDelete.HasValue || b.IsDelete == false) && b.DateImport >= df && b.DateImport <= dt && (cate == 0 || b.CateID == cate)).Sum(a => a.Quantity),
                            TotalB = c.DN_Agency.Cate_Value.Where(b => (!b.IsDelete.HasValue || b.IsDelete == false) && b.Quantity == b.QuantityOut && b.DateImport >= df && b.DateImport <= dt && (cate == 0 || b.CateID == cate)).Sum(a => a.Quantity),
                            TotalC = c.DN_Agency.Cate_Value.Where(b => (!b.IsDelete.HasValue || b.IsDelete == false) && b.QuantityOut < b.Quantity && b.DateImport >= df && b.DateImport <= dt && (cate == 0 || b.CateID == cate)).Sum(a => (a.Quantity -a.QuantityOut ))
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

        public AreaItem GetItemById(int id)
        {
            var query = from c in FDIDB.Areas
                        where c.IsDeleted != true && c.IsShow == true
                        && c.ID == id
                        orderby c.Name
                        select new AreaItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            Coordinates = c.Coordinates,
                            CityID = c.CityID,
                            AgencyID = c.AgencyID
                        };
            return query.FirstOrDefault();
        }
        public List<AreaItem> GetListSimple()
        {
            var query = from c in FDIDB.Areas
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new AreaItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }

        public Area GetById(int id)
        {
            var query = from c in FDIDB.Areas
                where c.ID == id
                select c;
            return query.FirstOrDefault();
        }
        public List<Area> GetByListArrId(string lstInt)
        {
            var lst = FDIUtils.StringToListInt(lstInt);
            var query = from c in FDIDB.Areas
                        where lst.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public void Add(Area area)
        {
            FDIDB.Areas.Add(area);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
