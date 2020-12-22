using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNCalendarWeeklyScheduleDA : BaseDA
    {
        #region Constructer
        public DNCalendarWeeklyScheduleDA()
        {
        }

        public DNCalendarWeeklyScheduleDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNCalendarWeeklyScheduleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public DN_Calendar_Weekly_Schedule GetById(int id)
        {
            var query = from c in FDIDB.DN_Calendar_Weekly_Schedule where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<DN_Calendar_Weekly_Schedule> GetListByArrId(List<int> ltsArrId)
        {
            var query = from o in FDIDB.DN_Calendar_Weekly_Schedule
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }

        public void Add(DN_Calendar_Weekly_Schedule calendarWeeklySchedule)
        {
            FDIDB.DN_Calendar_Weekly_Schedule.Add(calendarWeeklySchedule);
        }

        public void Delete(DN_Calendar_Weekly_Schedule calendarWeeklySchedule)
        {
            FDIDB.DN_Calendar_Weekly_Schedule.Remove(calendarWeeklySchedule);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
