using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ScheduleDA : BaseDA
    {
        #region Constructer
        public ScheduleDA()
        {
        }

        public ScheduleDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ScheduleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<ScheduleItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Schedule
                        where o.AgencyID == agencyid && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new ScheduleItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            HoursStart = o.HoursStart,
                            MinuteStart = o.MinuteStart,
                            HoursEnd = o.HoursEnd,
                            MinuteEnd = o.MinuteEnd,
                            IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Schedule GetById(int id)
        {
            var query = from c in FDIDB.DN_Schedule where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public ScheduleItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Schedule
                        where o.ID == id
                        select new ScheduleItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            HoursStart = o.HoursStart,
                            MinuteStart = o.MinuteStart,
                            HoursEnd = o.HoursEnd,
                            MinuteEnd = o.MinuteEnd,
                            IsShow = o.IsShow,
                        };
            return query.FirstOrDefault();
        }

        public List<ScheduleItem> GetAll()
        {
            var query = from o in FDIDB.DN_Schedule
                        where o.IsShow == true && (!o.IsDelete.HasValue || !o.IsDelete.Value)
                        select new ScheduleItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            HoursStart = o.HoursEnd,
                            HoursEnd = o.HoursEnd,
                            MinuteStart = o.MinuteStart,
                            MinuteEnd = o.MinuteEnd,
                            AgencyID = o.AgencyID
                        };
            return query.ToList();
        }

        public List<ScheduleItem> GetAllByUserId(Guid userId)
        {
            var query = from o in FDIDB.DN_Schedule
                        where o.IsShow == true && (!o.IsDelete.HasValue || !o.IsDelete.Value) && o.DN_Weekly_Schedule.Any(m => m.DN_Calendar_Weekly_Schedule.Any(n => n.DN_Calendar.DN_Users.Any(t => t.UserId == userId)))
                        select new ScheduleItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            HoursStart = o.HoursEnd,
                            HoursEnd = o.HoursEnd,
                            IsShow = o.IsShow,
                        };
            return query.ToList();
        }

        public List<DN_Weekly> GetWeeklyArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Weekly
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }

        public List<ScheduleItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Schedule
                        where ltsArrId.Contains(o.ID) && (!o.IsDelete.HasValue || !o.IsDelete.Value)
                        select new ScheduleItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            HoursStart = o.HoursEnd,
                            HoursEnd = o.HoursEnd,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public List<DN_Schedule> GetListByArrId(List<int> ltsArrId)
        {
            var query = from o in FDIDB.DN_Schedule
                        where ltsArrId.Contains(o.ID) && (!o.IsDelete.HasValue || !o.IsDelete.Value)
                        select o;
            return query.ToList();
        }

        public void Add(DN_Schedule schedule)
        {
            FDIDB.DN_Schedule.Add(schedule);
        }

        public void Delete(DN_Schedule schedule)
        {
            FDIDB.DN_Schedule.Remove(schedule);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
