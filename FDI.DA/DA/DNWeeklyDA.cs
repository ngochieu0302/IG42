using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNWeeklyDA : BaseDA
    {
        #region Constructer
        public DNWeeklyDA()
        {
        }

        public DNWeeklyDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNWeeklyDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<WeeklyItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Weekly 
                        orderby o.ID descending
                        select new WeeklyItem
                        {
                                       ID = o.ID,
                                       Name = o.Name.Trim(),
                                       IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Weekly GetById(int id)
        {
            var query = from c in FDIDB.DN_Weekly where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public WeeklyItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Weekly
                        where o.ID == id
                        select new WeeklyItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                           
                            IsShow = o.IsShow,
                        };
            return query.FirstOrDefault();
        }

        public List<WeeklyItem> GetAll()
        {
            var query = from o in FDIDB.DN_Weekly
                        select new WeeklyItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            IsShow = o.IsShow,
                        };
            return query.ToList();
        }


        public List<WeeklyItem> GetListByArrId(string ltsArrID)
        {
            var ltsArrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from o in FDIDB.DN_Weekly
                        where  ltsArrId.Contains(o.ID)
                        select new WeeklyItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public List<DN_Schedule> GetScheduleArrId(string lst)
        {
            var ltsArrId = FDIUtils.StringToListInt(lst);
            var query = from c in FDIDB.DN_Schedule where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }


        public void Add(DN_Weekly weekly)
        {
            FDIDB.DN_Weekly.Add(weekly);
        }

        public void Delete(DN_Weekly weekly)
        {
            FDIDB.DN_Weekly.Remove(weekly);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
