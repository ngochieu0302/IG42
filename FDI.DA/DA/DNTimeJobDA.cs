using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNTimeJobDA : BaseDA
    {
        #region Constructer
        public DNTimeJobDA()
        {
        }

        public DNTimeJobDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNTimeJobDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNTimeJobItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal(0) : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Time_Job
                        where o.AgencyID == agencyid && o.DateCreated > fromDate && o.DateCreated < toDate
                        orderby o.ID descending
                        select new DNTimeJobItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated ?? 0,
                            UserId = o.UserId,
                            ScheduleID = o.ScheduleID,
                            ScheduleEndID = o.ScheduleEndID,
                            DateEnd = o.DateEnd ?? 0,
                            MinutesLater = o.MinutesLater,
                            MinutesEarly = o.MinutesEarly,
                            DNUserItem = new DNUserItem
                            {
                                UserName = o.DN_Users.UserName
                            }
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Time_Job GetById(int id)
        {
            var query = from c in FDIDB.DN_Time_Job where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<UserViewItem> GetAllOnlineByToday(int agencyid)
        {
            var date = DateTime.Today.TotalSeconds();
            var query = from c in FDIDB.DN_Time_Job
                        where c.AgencyID == agencyid && c.DateCreated > date && !c.ScheduleEndID.HasValue
                        orderby c.ID descending
                        select new UserViewItem
                        {
                            UserId = c.DN_Users.UserId,
                            FullName = c.DN_Users.LoweredUserName,
                            CodeUser = c.DN_Users.UserName,
                            CodeCheckIn = c.DN_Users.CodeCheckIn,
                            DateCreated = c.DateCreated,
                        };
            return query.ToList();
        }
        public DNTimeJobItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Time_Job
                        where o.ID == id
                        select new DNTimeJobItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserId = o.UserId,
                            DateEnd = o.DateEnd,
                            DNUserItem = new DNUserItem
                            {
                                UserName = o.DN_Users.UserName
                            }
                        };
            return query.FirstOrDefault();
        }

        public DNTimeJobItem GetItemByScheduleId(int agencyId, int scheduleId)
        {
            var query = from o in FDIDB.DN_Time_Job
                        where o.ScheduleID == scheduleId && o.AgencyID == agencyId
                        select new DNTimeJobItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserId = o.UserId,
                            DateEnd = o.DateEnd,
                            ScheduleID = o.ScheduleID,
                            ScheduleEndID = o.ScheduleEndID
                        };
            return query.FirstOrDefault();
        }

        public DN_Time_Job GetItemByScheduleEndId(Guid userId)
        {
            var date = DateTime.Today;
            var totals = date.TotalSeconds();
            var query = from o in FDIDB.DN_Time_Job
                        where !o.ScheduleEndID.HasValue && o.UserId == userId && o.DateCreated > totals
                        select o;
            return query.FirstOrDefault();
        }

        public bool CheckScheduleEndId(Guid userId)
        {
            var date = DateTime.Today;
            var totals = date.TotalSeconds();
            var query = from o in FDIDB.DN_Time_Job
                        where !o.DateEnd.HasValue && o.UserId == userId && o.DateCreated > totals
                        select o.ID;
            return query.Any();
        }

        public List<DNTimeJobItem> GetAll()
        {
            var query = from o in FDIDB.DN_Time_Job
                        select new DNTimeJobItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserId = o.UserId,
                            DateEnd = o.DateEnd,
                            DateJob = o.DateCreated.DecimalToDate(),
                            DayInDateJob = o.DateCreated.DecimalToDate().Day,
                            MonthInDateJob = o.DateCreated.DecimalToDate().Month,
                            YearInDateJob = o.DateCreated.DecimalToDate().Year,
                        };
            return query.ToList();
        }
        public List<DNUserTimeJobItem> GetAllByMonth(int month, int year, int agencyid)
        {
            decimal datee;
            var dates = Utility.MonthTotalSeconds(month, year, out datee);
            var query = from o in FDIDB.DN_Users
                        where o.IsDeleted == false && o.AgencyID == agencyid
                        select new DNUserTimeJobItem
                        {
                            UserId = o.UserId,
                            UserName = o.UserName,
                            LoweredUserName = o.LoweredUserName,
                            DNTimeJobItems = o.DN_Time_Job.Where(m => m.DateCreated >= dates && m.DateCreated < datee).Select(m => new DNTimeJobItem
                            {
                                MinutesEarly = m.MinutesEarly,
                                MinutesLater = m.MinutesLater,
                                DateCreated = m.DateCreated,
                                DateEnd = m.DateEnd
                            }),
                            CalendarItems = FDIDB.DN_Calendar.Where(m => (m.DN_Users.Any(u => u.UserId == o.UserId) || m.DN_Roles.Any(r => r.DN_UsersInRoles.Any(u => u.UserId == o.UserId && u.IsDelete == false))) && ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new CalendarItem
                            {
                                ID = m.ID,
                                Name = m.Name,
                                DateStart = m.DateStart,
                                DateEnd = m.DateEnd,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new CalendarWeeklyScheduleItem
                                {
                                    Weekid = d.DN_Weekly_Schedule.WeeklyID,
                                })
                            }),
                            EditScheduleItems = FDIDB.DN_EDIT_Schedule.Where(m => m.DN_Users1.UserId == o.UserId && m.Date >= dates && m.Date < datee).Select(m => new CkeckEditScheduleItem
                            {
                                Date = m.Date,
                                Check = m.DN_Users.UserId == o.UserId
                            }),
                            DateOffItem = FDIDB.DN_DayOff.Where(m => (m.Date >= dates && m.Date <= datee) || (m.Date + (m.Quantity * 86400) >= dates && m.Date + (m.Quantity * 86400) <= datee) || (m.Date <= dates && m.Date + (m.Quantity * 86400) >= datee)).Select(m => new DateOffItem
                            {
                                Date = m.Date,
                                Quantity = m.Quantity
                            })
                        };
            return query.ToList();
        }

        public List<DNTimeJobItem> GetAllByAgencyId(int agencyId, Guid userId, int month)
        {
            var query = from o in FDIDB.DN_Time_Job
                        where o.AgencyID == agencyId && o.ScheduleID > 0 && o.ScheduleEndID > 0 && o.UserId == userId && o.DateCreated.DecimalToDate().Month == month && o.DateCreated.DecimalToDate().Year == DateTime.Now.Year
                        select new DNTimeJobItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserId = o.UserId,
                            DateEnd = o.DateEnd,
                            DateJob = o.DateCreated.DecimalToDate()
                        };
            return query.ToList();
        }

        public List<DNTimeJobItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Time_Job
                        where ltsArrId.Contains(o.ID)
                        select new DNTimeJobItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserId = o.UserId,
                            DateEnd = o.DateEnd
                        };
            return query.ToList();
        }

        public void Add(DN_Time_Job dnTimeJob)
        {
            FDIDB.DN_Time_Job.Add(dnTimeJob);
        }

        public void Delete(DN_Time_Job dnTimeJob)
        {
            FDIDB.DN_Time_Job.Remove(dnTimeJob);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
