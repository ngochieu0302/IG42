using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class WeeklyScheduleDA : BaseDA
    {
        #region Constructer
        public WeeklyScheduleDA()
        {
        }

        public WeeklyScheduleDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public WeeklyScheduleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<WeeklyScheduleItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Weekly_Schedule
                        orderby o.ID descending
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            AgencyID = o.AgencyID,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Weekly_Schedule GetById(int id)
        {
            var query = from c in FDIDB.DN_Weekly_Schedule where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<WeeklyScheduleItem> GetAll(int agencyId, int caledarid)
        {
            var query = from o in FDIDB.DN_Weekly_Schedule
                        where o.AgencyID == agencyId && (!o.DN_Schedule.IsDelete.HasValue || !o.DN_Schedule.IsDelete.Value)
                        orderby o.DN_Weekly.Sort, o.DN_Schedule.Sort
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            WeeklyID = o.WeeklyID,
                            ScheduleID = o.ScheduleID,
                            WeeklyName = o.DN_Weekly.Name,
                            //CalendarName = o.
                            ScheduleItem = new ScheduleItem
                            {
                                Name = o.DN_Schedule.Name,
                                HoursStart = o.DN_Schedule.HoursStart,
                                MinuteStart = o.DN_Schedule.MinuteStart,
                                HoursEnd = o.DN_Schedule.HoursEnd,
                                MinuteEnd = o.DN_Schedule.MinuteEnd,
                            },
                            IsCalender = o.DN_Calendar_Weekly_Schedule.Any(m=>m.CalenderID == caledarid)
                        };
            return query.ToList();
        }
        public List<WeeklyScheduleItem> GetAll(int agencyId)
        {
            var query = from o in FDIDB.DN_Weekly_Schedule
                        where o.AgencyID == agencyId && (!o.DN_Schedule.IsDelete.HasValue || !o.DN_Schedule.IsDelete.Value)
                        orderby o.DN_Weekly.Sort, o.DN_Schedule.Sort
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            WeeklyID = o.WeeklyID,
                            ScheduleID = o.ScheduleID,
                            WeeklyName = o.DN_Weekly.Name,
                            ScheduleItem = new ScheduleItem
                            {
                                Name = o.DN_Schedule.Name,
                                HoursStart = o.DN_Schedule.HoursStart,
                                MinuteStart = o.DN_Schedule.MinuteStart,
                                Soft = o.DN_Schedule.Sort,
                                HoursEnd = o.DN_Schedule.HoursEnd,
                                MinuteEnd = o.DN_Schedule.MinuteEnd,
                            }
                        };
            return query.ToList();
        }
        public List<WeeklyScheduleItem> GetAllUser(int id, int agencyId, Guid userId)
        {
            var query = from o in FDIDB.DN_Weekly_Schedule
                        where o.AgencyID == agencyId &&(!o.DN_Schedule.IsDelete.HasValue || !o.DN_Schedule.IsDelete.Value) && o.DN_Calendar_Weekly_Schedule.Any(m => m.DN_Calendar.DN_Users.Any(n => n.UserId == userId) && m.CalenderID == id)
                        orderby o.DN_Weekly.Sort, o.DN_Schedule.Sort
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            WeeklyID = o.WeeklyID,
                            ScheduleID = o.ScheduleID,
                            WeeklyName = o.DN_Weekly.Name,
                            ScheduleName = o.DN_Schedule.Name,
                            IsCalender = o.DN_Calendar_Weekly_Schedule.Any(),
                            ScheduleItem = new ScheduleItem
                            {
                                Name = o.DN_Schedule.Name,
                                //HoursStart = o.DN_Schedule.HoursStart,
                                //HoursEnd = o.DN_Schedule.HoursEnd
                            }
                        };
            return query.ToList();
        }

        // lấy tất cả nhân viên cun
        public List<DNUserItem> GetAllUserSchedule(int agencyId, Guid userId, decimal date, int scheduleId)
        {
            var query = from o in FDIDB.DN_Users
                        where o.UserId != userId && o.AgencyID == agencyId && o.DN_Calendar.Any(m => m.DN_Calendar_Weekly_Schedule.Any(n => n.DN_Weekly_Schedule.ScheduleID == scheduleId && (!n.DN_Weekly_Schedule.DN_Schedule.IsDelete.HasValue || !n.DN_Weekly_Schedule.DN_Schedule.IsDelete.Value)) && m.DateStart <= date && m.DateEnd > date) && o.DN_UsersInRoles.Any(m => m.UserId == userId && m.IsDelete == false)
                        select new DNUserItem
                        {
                           UserId = o.UserId,
                           UserName = o.UserName,
                        };
            return query.ToList();
        }

        public List<WeeklyScheduleItem> GetAllRole(int id, int agencyId, Guid roleId)
        {
            var query = from o in FDIDB.DN_Weekly_Schedule
                        where o.AgencyID == agencyId && o.DN_Calendar_Weekly_Schedule.Any(m => m.DN_Calendar.DN_Roles.Any(n => n.RoleId == roleId) && m.CalenderID == id) && (!o.DN_Schedule.IsDelete.HasValue || !o.DN_Schedule.IsDelete.Value)
                        orderby o.DN_Weekly.Sort, o.DN_Schedule.Sort
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            WeeklyID = o.WeeklyID,
                            ScheduleID = o.ScheduleID,
                            WeeklyName = o.DN_Weekly.Name,
                            ScheduleName = o.DN_Schedule.Name,
                            IsCalender = o.DN_Calendar_Weekly_Schedule.Any()
                        };
            return query.ToList();
        }

        public List<WeeklyScheduleItem> GetAllByAgencyId(int agencyId, Guid userid)
        {
            var query = from o in FDIDB.DN_Weekly_Schedule
                        where o.AgencyID == agencyId && (!o.DN_Schedule.IsDelete.HasValue || !o.DN_Schedule.IsDelete.Value)
                        orderby o.DN_Weekly.Sort, o.DN_Schedule.Sort
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            WeeklyID = o.WeeklyID,
                            ScheduleID = o.ScheduleID,
                            WeeklyName = o.DN_Weekly.Name,
                            ScheduleName = o.DN_Schedule.Name,
                            ScheduleTimeStart = o.DN_Schedule.HoursStart * 3600 + o.DN_Schedule.MinuteStart * 60,
                            ScheduleTimeEnd = o.DN_Schedule.HoursEnd * 3600 + o.DN_Schedule.MinuteEnd * 60,
                            DNUserBedDeskItem = o.DN_User_BedDesk.Where(m=>m.UserID == userid).Select(m=>new DNUserBedDeskItem
                            {
                                ID = m.ID,
                                BedDeskID = m.BedDeskID,
                                NameBed = m.DN_Bed_Desk.Name,
                            }).FirstOrDefault()
                            
                        };
            return query.ToList();
        }

        public List<WeeklyScheduleItem> GetExitsAllByAgencyId(int agencyId, string lstScheduleId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstScheduleId);
            var query = from o in FDIDB.DN_Weekly_Schedule
                        where o.AgencyID == agencyId && !ltsArrId.Contains(o.ScheduleID.Value) && (!o.DN_Schedule.IsDelete.HasValue || !o.DN_Schedule.IsDelete.Value)
                        orderby o.DN_Weekly.Sort, o.DN_Schedule.Sort
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            WeeklyID = o.WeeklyID,
                            ScheduleID = o.ScheduleID,
                            WeeklyName = o.DN_Weekly.Name,
                            ScheduleName = o.DN_Schedule.Name,
                            ScheduleTimeStart = o.DN_Schedule.HoursStart * 3600 + o.DN_Schedule.MinuteStart * 60,
                            ScheduleTimeEnd = o.DN_Schedule.HoursEnd * 3600 + o.DN_Schedule.MinuteEnd * 60,
                           
                            
                        };
            return query.ToList();
        }

        public List<DN_Weekly_Schedule> GetListByArrId(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DN_Weekly_Schedule where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(DN_Weekly_Schedule weeklySchedule)
        {
            FDIDB.DN_Weekly_Schedule.Add(weeklySchedule);
        }

        public void Delete(DN_Weekly_Schedule weeklySchedule)
        {
            FDIDB.DN_Weekly_Schedule.Remove(weeklySchedule);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
