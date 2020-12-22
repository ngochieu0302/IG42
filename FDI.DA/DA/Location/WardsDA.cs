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
    public class WardsDA : BaseDA
    {
        public WardsDA()
        {
        }

        public WardsDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public WardsDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<WardsItem> GetListbyRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Wards
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new WardsItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            DistrictID = c.DistrictID
                        };
            var dis = httpRequest["districtId"];
            if (!string.IsNullOrEmpty(dis))
            {
                var disId = int.Parse(dis);
                query = query.Where(c => c.DistrictID == disId);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public WardsItem GetItemById(int id)
        {
            var query = from c in FDIDB.Wards
                        where c.IsDeleted != true && c.IsShow == true
                        && c.ID == id
                        orderby c.Name
                        select new WardsItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            Coordinates = c.Coordinates,
                            DistrictID = c.DistrictID
                        };
            return query.FirstOrDefault();
        }
        public List<WardsItem> GetListSimple()
        {
            var query = from c in FDIDB.Wards
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new WardsItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }

        public Ward GetById(int id)
        {
            var query = from c in FDIDB.Wards
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public List<Ward> GetByListArrId(string lstInt)
        {
            var lst = FDIUtils.StringToListInt(lstInt);
            var query = from c in FDIDB.Wards
                        where lst.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public void Add(Ward Wards)
        {
            FDIDB.Wards.Add(Wards);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
