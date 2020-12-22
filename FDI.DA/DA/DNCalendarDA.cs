using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNCalendarDA : BaseDA
    {
        #region Constructer
        public DNCalendarDA()
        {
        }

        public DNCalendarDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNCalendarDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNCalendarItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Calendar
                        where o.IsDelete == false && o.AgencyID == agencyid
                        orderby o.Sort descending
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            DateStart = o.DateStart,
                            DateEnd = o.DateEnd,
                            IsShow = o.IsShow,
                            WeeklyScheduleItems = (from c in FDIDB.DN_Weekly_Schedule
                                                   where c.AgencyID == agencyid && (!c.DN_Schedule.IsDelete.HasValue || !c.DN_Schedule.IsDelete.Value) && c.DN_Calendar_Weekly_Schedule.Any(m => m.CalenderID == o.ID)
                                                   orderby c.WeeklyID, c.DN_Schedule.Sort
                                                   select new WeeklyScheduleItem
                                                   {
                                                       WeeklyID = c.WeeklyID,
                                                       ScheduleName = c.DN_Schedule.Name,
                                                   })
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<DNCalendarItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid, Guid? userid)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now);
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Calendar
                        where o.IsDelete == false && o.DateEnd > date && o.AgencyID == agencyid && o.DN_Users.Any(m => m.UserId == userid)
                        orderby o.Sort descending
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            DateStart = o.DateStart,
                            DateEnd = o.DateEnd,
                            IsShow = o.IsShow,
                            WeeklyScheduleItems = (from c in FDIDB.DN_Weekly_Schedule
                                                   where c.AgencyID == agencyid && c.DN_Calendar_Weekly_Schedule.Any(m => m.CalenderID == o.ID)
                                                   orderby c.WeeklyID, c.DN_Schedule.Sort
                                                   select new WeeklyScheduleItem
                                                   {
                                                       WeeklyID = c.WeeklyID,
                                                       ScheduleName = c.DN_Schedule.Name,
                                                   })
                        };
            if (!query.Any())
            {
                query = from o in FDIDB.DN_Calendar
                        where o.IsDelete == false && o.AgencyID == agencyid && o.DN_Roles.Any(m => m.DN_UsersInRoles.Any(n => n.UserId == userid && n.IsDelete == false))
                        orderby o.Sort descending
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            DateStart = o.DateStart,
                            DateEnd = o.DateEnd,
                            IsShow = o.IsShow,
                            WeeklyScheduleItems = (from c in FDIDB.DN_Weekly_Schedule
                                                   where c.AgencyID == agencyid && c.DN_Calendar_Weekly_Schedule.Any(m => m.CalenderID == o.ID)
                                                   orderby c.WeeklyID, c.DN_Schedule.Sort
                                                   select new WeeklyScheduleItem
                                                   {
                                                       WeeklyID = c.WeeklyID,
                                                       ScheduleName = c.DN_Schedule.Name,
                                                   })
                        };
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<WeeklyScheduleItem> GetAll(int agencyId, int caledarid)
        {
            var query = from o in FDIDB.DN_Weekly_Schedule
                        where o.AgencyID == agencyId
                        orderby o.WeeklyID, o.DN_Schedule.Sort
                        select new WeeklyScheduleItem
                        {
                            ID = o.ID,
                            WeeklyID = o.WeeklyID,
                            ScheduleID = o.ScheduleID,
                            WeeklyName = o.DN_Weekly.Name,
                            ScheduleName = o.DN_Schedule.Name,
                            IsCalender = o.DN_Calendar_Weekly_Schedule.Any(m => m.CalenderID == caledarid)
                        };
            return query.ToList();
        }

        public DN_Calendar GetById(int id)
        {
            var query = from c in FDIDB.DN_Calendar where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNCalendarItem GetByCalendarWeeklyScheduleItemId(int id)
        {
            var query = from o in FDIDB.DN_Calendar
                        where o.ID == id
                        select new DNCalendarItem
                        {
                            DNCalendarWeeklySchedule = o.DN_Calendar_Weekly_Schedule.Select(m => new DNCalendarWeeklyScheduleItem
                            {
                                ID = m.ID,
                                MWSID = m.MWSID
                            })
                        };
            return query.FirstOrDefault();
        }

        public DNCalendarItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Calendar
                        where o.ID == id
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            DateCreated = o.DateCreated,
                            AgencyId = o.AgencyID,
                            Sort = o.Sort,
                            IsShow = o.IsShow,
                            ListDnUserItem = o.DN_Users.Where(m => m.IsDeleted == false).Select(m => new DNUserItem
                            {
                                UserId = m.UserId,
                                UserName = m.UserName
                            }),
                            ListDnRolesItem = o.DN_Roles.Select(r => new DNRolesItem
                            {
                                RoleId = r.RoleId,
                                RoleName = r.RoleName
                            }),
                            DateStart = o.DateStart,
                            DateEnd = o.DateEnd
                        };
            return query.FirstOrDefault();
        }

        public List<DNCalendarItem> GetItemByUserId(int agencyid, Guid userid)
        {
            var query = from o in FDIDB.DN_Calendar
                        where o.AgencyID == agencyid && o.DN_Users.Any(m => m.UserId == userid)
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = "<span>" + o.Name.Trim() + "</span>",
                            Sort = o.Sort,
                            IsShow = o.IsShow,
                            DateEnd = o.DateEnd,
                            DateStart = o.DateStart,
                            DNCalendarWeeklySchedule = o.DN_Calendar_Weekly_Schedule.Select(m => new DNCalendarWeeklyScheduleItem
                            {
                                MWSID = m.MWSID,
                                DNUserBedDeskItem = FDIDB.DN_User_BedDesk.Where(n => n.UserID == userid && n.MWSID == m.MWSID).Select(n => new DNUserBedDeskItem
                                {
                                    BedDeskID = n.BedDeskID
                                }).FirstOrDefault()
                            })
                        };
            if (query.Any()) return query.ToList();
            query = from o in FDIDB.DN_Calendar
                    where o.AgencyID == agencyid && o.DN_Roles.Any(m => m.DN_UsersInRoles.Any(n => n.UserId == userid && n.IsDelete == false))
                    select new DNCalendarItem
                    {
                        ID = o.ID,
                        Name = "<span>" + o.Name.Trim() + "</span>",
                        Sort = o.Sort,
                        IsShow = o.IsShow,
                        DateEnd = o.DateEnd,
                        DateStart = o.DateStart,
                        DNCalendarWeeklySchedule = o.DN_Calendar_Weekly_Schedule.Select(m => new DNCalendarWeeklyScheduleItem
                        {
                            MWSID = m.MWSID,
                            DNUserBedDeskItem = FDIDB.DN_User_BedDesk.Where(n => n.UserID == userid && n.MWSID == m.MWSID).Select(n => new DNUserBedDeskItem
                            {
                                BedDeskID = n.BedDeskID
                            }).FirstOrDefault()
                        }),

                    };
            return query.ToList();
        }

        public List<CheckInItem> GetItemByUserIdDate(int agencyid, Guid userid, decimal date)
        {
            var td = DateTime.Today;
            var today = td.TotalSeconds();
            var thu = td.Thu();
            var query = from o in FDIDB.DN_Schedule
                        where (!o.IsDelete.HasValue || o.IsDelete == false) && o.AgencyID == agencyid
                        && !o.DN_EDIT_Schedule.Any(m => m.DN_Users.UserId == userid && m.Date == today)//Lấy ra người ko nghỉ
                        && o.DN_Weekly_Schedule.Any(m => m.WeeklyID == thu &&
                            m.DN_Calendar_Weekly_Schedule.Any(c => c.DN_Calendar.DateStart <= today && c.DN_Calendar.DateEnd > today && (c.DN_Calendar.DN_Users.Any(u => u.UserId == userid)
                                || c.DN_Calendar.DN_Roles.Any(u => u.DN_UsersInRoles.Any(n => n.UserId == userid && (!n.IsDelete.HasValue || n.IsDelete == false))))))
                            || o.DN_EDIT_Schedule.Any(m => m.DN_Users1.UserId == userid && m.Date == today)
                        orderby o.Sort
                        select new CheckInItem
                        {
                            ID = o.ID,
                            Hms = o.HoursStart * 60 + o.MinuteStart,
                            Hme = o.HoursEnd * 60 + o.MinuteEnd,
                        };
            return query.ToList();
        }

        public List<DNCalendarItem> GetItemByRoleId(int agencyid, Guid roleId)
        {
            var query = from o in FDIDB.DN_Calendar
                        where o.AgencyID == agencyid && o.DN_Roles.Any(m => m.RoleId == roleId)
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = "<span>" + o.Name.Trim() + "</span>",
                            Sort = o.Sort,
                            IsShow = o.IsShow,
                            DateEnd = o.DateEnd,
                            DateStart = o.DateStart,
                            DNCalendarWeeklySchedule = o.DN_Calendar_Weekly_Schedule.Select(m => new DNCalendarWeeklyScheduleItem
                            {
                                MWSID = m.MWSID
                            }),

                        };
            if (query.Any())
                return query.ToList();
            query = from o in FDIDB.DN_Calendar
                    where o.AgencyID == agencyid && o.DN_Roles.Any(m => m.DN_UsersInRoles.Any(n => n.RoleId == roleId && n.IsDelete == false))
                    select new DNCalendarItem
                    {
                        ID = o.ID,
                        Name = o.Name.Trim(),
                        Sort = o.Sort,
                        IsShow = o.IsShow,
                        DateEnd = o.DateEnd,
                        DateStart = o.DateStart,
                        DNCalendarWeeklySchedule = o.DN_Calendar_Weekly_Schedule.Select(m => new DNCalendarWeeklyScheduleItem
                        {
                            MWSID = m.MWSID
                        }),

                    };
            return query.ToList();
        }

        public List<BedDeskItem> GetLisBedDesk(int agencyId)
        {
            var query = from c in FDIDB.DN_Bed_Desk
                        where c.IsShow == true && c.AgencyId == agencyId
                        orderby c.ID descending
                        select new BedDeskItem
                        {
                            ID = c.ID,
                            Name = c.Name
                        };
            return query.ToList();
        }

        public List<DN_Bed_Desk> GetBedDeskArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Bed_Desk
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }

        public List<DN_Users> GetUserArrId(string lstId)
        {
            var ltsArrId = FDIUtils.ConvertStringToGuids(lstId);
            var query = from o in FDIDB.DN_Users
                        where ltsArrId.Contains(o.UserId)
                        select o;
            return query.ToList();
        }

        public List<DN_Roles> GetRolesArrId(string lstId)
        {
            var ltsArrId = FDIUtils.ConvertStringToGuids(lstId);
            var query = from o in FDIDB.DN_Roles
                        where ltsArrId.Contains(o.RoleId)
                        select o;
            return query.ToList();
        }


        public List<DNCalendarItem> GetAll()
        {
            var query = from o in FDIDB.DN_Calendar
                        where o.IsShow == true && o.IsDelete == false
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            DateCreated = o.DateCreated,
                            AgencyId = o.AgencyID,
                            IsShow = o.IsShow,
                            //ListDnBedDeskItem = o.DN_Bed_Desk.Where(m => m.IsShow == true).Select(m => new DNBedDeskItem
                            //{
                            //    ID = m.ID,
                            //    Name = m.Name
                            //})
                        };
            return query.ToList();
        }


        public List<DNCalendarItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Calendar
                        where o.IsDelete == false && ltsArrId.Contains(o.ID)
                        select new DNCalendarItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            DateCreated = o.DateCreated,
                            AgencyId = o.AgencyID,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public void Add(DN_Calendar calendar)
        {
            FDIDB.DN_Calendar.Add(calendar);
        }

        public void Delete(DN_Calendar calendar)
        {
            FDIDB.DN_Calendar.Remove(calendar);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
