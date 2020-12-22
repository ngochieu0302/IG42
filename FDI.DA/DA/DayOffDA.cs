using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DayOffDA : BaseDA
    {
        #region Constructer
        public DayOffDA()
        {
        }

        public DayOffDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DayOffDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DayOffItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int Agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_DayOff
                        where o.IsDelete == false && o.AgencyID == Agencyid
                        orderby o.ID descending
                        select new DayOffItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Date = o.Date,
                            Quantity = o.Quantity,
                            IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_DayOff GetById(int id)
        {
            var query = from c in FDIDB.DN_DayOff where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DayOffItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_DayOff
                        where o.ID == id
                        select new DayOffItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Date = o.Date,
                            Quantity = o.Quantity,
                            IsShow = o.IsShow
                        };
            return query.FirstOrDefault();
        }

        public List<DayOffItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_DayOff
                        where o.IsShow == true && o.IsDelete == false && o.AgencyID == agencyid
                        select new DayOffItem
                        {
                            ID = o.ID,
                            Name = "<span>" + o.Name.Trim() + "</span>",
                            Date = o.Date,
                            DateEnd = o.Date + (o.Quantity - 1) * 86400,
                        };
            return query.ToList();
        }


        public List<DayOffItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_DayOff
                        where o.IsDelete == false && ltsArrId.Contains(o.ID)
                        select new DayOffItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Date = o.Date,
                            Quantity = o.Quantity,
                            IsShow = o.IsShow,
                            IsDelete = o.IsDelete
                        };
            return query.ToList();
        }

        public void Add(DN_DayOff dayOff)
        {
            FDIDB.DN_DayOff.Add(dayOff);
        }

        public void Delete(DN_DayOff dayOff)
        {
            FDIDB.DN_DayOff.Remove(dayOff);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
