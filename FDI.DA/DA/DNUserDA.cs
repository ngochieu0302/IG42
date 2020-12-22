using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class DNUserDA : BaseDA
    {
        #region User

        public DNUserDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNUserDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public DNUserItem GetPassByUserName(string userName, string domain)
        {
            //var s = Utility.S;
            var date = DateTime.Now;
            var time = date.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.UserName == userName && (!c.IsLockedOut.HasValue || c.IsLockedOut == false) && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        //&& c.AgencyID.HasValue && c.DN_Agency.DN_Enterprises.DateStart < time && c.DN_Agency.DN_Enterprises.DateEnd > time &&
                        //c.DN_Agency.DN_Enterprises.IsLocked == false && c.DN_Agency.DN_Enterprises.IsDeleted == false
                        //&& c.DN_Agency.IsLock == false
                        //&& c.DN_Agency.DN_Enterprises.Url.ToLower() == domain.ToLower()
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            listRole = c.DN_UsersInRoles.Where(v => (!v.IsDelete.HasValue || v.IsDelete == false) && (!v.DN_Roles.IsDeleted.HasValue || v.DN_Roles.IsDeleted == false)).Select(o => o.DN_Roles.RoleName),
                            listRoleID = c.DN_UsersInRoles.Where(v => (!v.IsDelete.HasValue || v.IsDelete == false) && (!v.DN_Roles.IsDeleted.HasValue || v.DN_Roles.IsDeleted == false)).Select(o => o.DN_Roles.ID),
                            EnterprisesID = c.DN_Agency.EnterpriseID,
                            AgencyID = c.AgencyID ?? 0,
                            AreaID = c.DN_Agency.Areas.FirstOrDefault() != null ? c.DN_Agency.Areas.FirstOrDefault().ID : (c.DN_Agency.Market.AreaID ?? 0),
                            MarketID = c.DN_Agency.MarketID ?? 0,
                            LoweredUserName = c.LoweredUserName,
                            AgencyName = c.DN_Agency.Name,
                            Password = c.Password,
                            PasswordSalt = c.PasswordSalt,
                            AgencyDeposit = c.DN_Agency.Documents.Sum(a => a.Deposit),
                            AgencyAddress = c.DN_Agency.Address,
                            AgencyWallet = c.DN_Agency.WalletValue,
                            RoleId = c.DN_UsersInRoles.Where(v => !v.IsDelete.HasValue || v.IsDelete == false).Select(m => m.RoleId).FirstOrDefault(),
                            Url = c.DN_Module.Any() ? c.DN_Module.Select(m => m.Link).FirstOrDefault() : c.DN_UsersInRoles.Any(v => !v.IsDelete.HasValue || v.IsDelete == false) ? FDIDB.DN_Module.Where(m => !string.IsNullOrEmpty(m.Link) && m.DN_Roles.Any(r => c.DN_UsersInRoles.Any(u => u.RoleId == r.RoleId && (!u.IsDelete.HasValue || u.IsDelete == false)))).OrderBy(m => m.Level).ThenBy(m => m.Ord).Select(m => m.Link).FirstOrDefault() : "",

                        };
            try
            {
                return query.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public bool CheckUserName(string txt, Guid id, int agencyid)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserName.ToLower().Equals(txt.ToLower()) && c.UserId != id && (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                        select c;
            return query.Any();
        }
        public bool CheckUserName(string txt, bool isAgency)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserName.ToLower().Equals(txt.ToLower()) && (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                              && (c.IsAgency == isAgency || (c.IsAgency == null && !isAgency))
                        select c;
            return query.Any();
        }



        public DN_Users GetUserByUserName(string userName)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserName == userName
                        select c;
            return query.FirstOrDefault();
        }
        public List<DNUserSimpleItem> GetListSimple(int agencyId)
        {
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && c.IsDeleted == false
                        select new DNUserSimpleItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                        };
            return query.ToList();
        }
        public List<DNUserItem> GetListAll(int agencyId)
        {
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && c.IsDeleted == false
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            IsApproved = c.IsApproved,
                            LoweredUserName = c.LoweredUserName,
                        };
            return query.ToList();
        }

        public List<DNUserItem> GetListAllKt(int agencyId)
        {
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && c.IsDeleted == false && c.IsApproved == true
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            IsApproved = c.IsApproved
                        };
            return query.ToList();
        }
        public List<DNUserItem> GetListAllAgency(int agencyId)
        {
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && c.IsDeleted == false && c.IsAgency == true
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            IsApproved = c.IsApproved,
                            CodeCheckIn = c.CodeCheckIn
                        };
            return query.ToList();
        }
        public List<DNUserItem> GetListAllChoose(int agencyId)
        {
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && c.IsDeleted == false && c.IsService == true
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            IsApproved = c.IsApproved,
                            LoweredUserName = c.LoweredUserName,
                        };
            return query.ToList();
        }
        public List<DNUserItem> GetListAllSevice(int agencyId, string json)
        {
            var date = DateTime.Now.TotalSeconds();
            var list = FDIUtils.StringToListString(json);
            var query = from c in FDIDB.DN_Users
                        where (!c.IsLockedOut.HasValue || !c.IsLockedOut.Value) && c.AgencyID == agencyId
                        && (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && (c.IsService.HasValue && c.IsService.Value) && list.Contains(c.CodeCheckIn)
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            CodeCheckIn = c.CodeCheckIn,
                            Times = c.Shop_Orders2.Where(m => m.EndDate > date && m.Status == 3).OrderByDescending(m => m.EndDate).Select(o => new DNUserOrderItem
                            {
                                BId = o.BedDeskID,
                                S = o.StartDate,
                                E = o.EndDate
                            }),
                            LoweredUserName = c.LoweredUserName,
                        };
            return query.ToList();
        }

        public List<DNUserCalendarItem> GetListCalendar(int agencyId, decimal dates, decimal datee)
        {
            var date = DateTime.Today;
            var dat = date.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && (c.IsDeleted.HasValue && c.IsDeleted == false)
                        orderby c.DN_UsersInRoles.Select(m => m.DN_Tree.OrderBy(t => t.Level).Select(t => t.Level).FirstOrDefault()).FirstOrDefault()
                        select new DNUserCalendarItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            LoweredUserName = c.LoweredUserName,
                            NameRole = c.DN_UsersInRoles.OrderBy(m => m.DN_Roles.ID).Select(m => m.DN_Roles.RoleName).FirstOrDefault(),
                            IsOnline = c.DN_Time_Job.Any(m => m.DateCreated > dat && !m.ScheduleEndID.HasValue),
                            ListNameTree = FDIDB.DN_Tree.Where(m => m.DN_UsersInRoles.UserId == c.UserId).Select(m => m.Name).Distinct(),
                            EditScheduleItems = FDIDB.DN_EDIT_Schedule.Where(e => (e.UserId == c.UserId || e.UserChangeId == c.UserId) && e.Date >= dates && e.Date <= datee).Select(e => new CEditScheduleItem
                            {
                                ID = e.ID,
                                Name = e.DN_Schedule.Name,
                                UserId = e.UserId,
                                UserChangeId = e.UserChangeId,
                                Date = e.Date,
                                ScheduleID = e.ScheduleID
                            }),
                            CalendarItems = c.DN_Calendar.Where(m => ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new CalendarItem
                            {
                                ID = m.ID,
                                Name = m.Name,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new CalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                })
                            }),
                            RCalendarItems = FDIDB.DN_Calendar.Where(m => m.DN_Roles.Any(r => r.DN_UsersInRoles.Any(u => u.UserId == c.UserId && u.IsDelete == false)) && ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new RCalendarItem
                            {
                                ID = m.ID,
                                Name = m.Name,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new RCalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                })
                            }),
                        };
            return query.ToList();
        }

        public List<DNUserCalendarItem> GetListCalendar(int agencyId, decimal dates, decimal datee, Guid userid)
        {
            var date = DateTime.Today;
            var dat = date.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && (c.IsDeleted.HasValue && c.IsDeleted == false) && c.UserId == userid
                        orderby c.DN_UsersInRoles.Select(m => m.DN_Tree.OrderBy(t => t.Level).Select(t => t.Level).FirstOrDefault()).FirstOrDefault()
                        select new DNUserCalendarItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            LoweredUserName = c.LoweredUserName,
                            NameRole = c.DN_UsersInRoles.OrderBy(m => m.DN_Roles.ID).Select(m => m.DN_Roles.RoleName).FirstOrDefault(),
                            IsOnline = c.DN_Time_Job.Any(m => m.DateCreated > dat && !m.ScheduleEndID.HasValue),
                            ListNameTree = FDIDB.DN_Tree.Where(m => m.DN_UsersInRoles.UserId == c.UserId).Select(m => m.Name).Distinct(),
                            EditScheduleItems = FDIDB.DN_EDIT_Schedule.Where(e => (e.UserId == c.UserId || e.UserChangeId == c.UserId) && e.Date >= dates && e.Date < datee).Select(e => new CEditScheduleItem
                            {
                                ID = e.ID,
                                Name = e.DN_Schedule.Name,
                                UserId = e.UserId,
                                UserChangeId = e.UserChangeId,
                                Date = e.Date,
                                ScheduleID = e.ScheduleID
                            }),
                            CalendarItems = c.DN_Calendar.Where(m => ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new CalendarItem
                            {
                                ID = m.ID,
                                Name = m.Name,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new CalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                })
                            }),
                            RCalendarItems = FDIDB.DN_Calendar.Where(m => m.DN_Roles.Any(r => r.DN_UsersInRoles.Any(u => u.UserId == c.UserId && u.IsDelete == false)) && ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new RCalendarItem
                            {
                                ID = m.ID,
                                Name = m.Name,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new RCalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                })
                            }),
                        };
            return query.ToList();
        }

        public List<DNUserCalendarItem> GetListTotalMonth(int agencyId, decimal? dates, decimal datee)
        {
            var date = dates.DecimalToDate();
            var mont = date.Month;
            var year = date.Year;
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && (c.IsDeleted.HasValue && c.IsDeleted == false)
                        select new DNUserCalendarItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            LoweredUserName = c.LoweredUserName,
                            FixedSalary = c.FixedSalary,
                            NameRole = c.DN_UsersInRoles.Select(m => m.DN_Roles.RoleName).FirstOrDefault(),
                            EditScheduleItems = FDIDB.DN_EDIT_Schedule.Where(e => (e.UserId == c.UserId || e.UserChangeId == c.UserId) && e.Date >= dates && e.Date < datee).Select(e => new CEditScheduleItem
                            {
                                ID = e.ID,
                                UserId = e.UserId,
                                UserChangeId = e.UserChangeId,
                                Date = e.Date,
                                ScheduleID = e.ScheduleID
                            }),
                            CalendarItems = c.DN_Calendar.Where(m => ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new CalendarItem
                            {
                                ID = m.ID,
                                DateStart = m.DateStart,
                                DateEnd = m.DateEnd,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new CalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                    Weekid = d.DN_Weekly_Schedule.WeeklyID
                                })
                            }),
                            RCalendarItems = FDIDB.DN_Calendar.Where(m => m.DN_Roles.Any(r => r.DN_UsersInRoles.Any(u => u.UserId == c.UserId && u.IsDelete == false)) && ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new RCalendarItem
                            {
                                ID = m.ID,
                                DateStart = m.DateStart,
                                DateEnd = m.DateEnd,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new RCalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                    Weekid = d.DN_Weekly_Schedule.WeeklyID
                                })
                            }),
                            DateOffItems = FDIDB.DN_DayOff.Where(m => (m.Date >= dates && m.Date <= datee) || (m.Date + (m.Quantity * 86400) >= dates && m.Date + (m.Quantity * 86400) <= datee) || (m.Date <= dates && m.Date + (m.Quantity * 86400) >= datee)).Select(m => new DateOffItem
                            {
                                Date = m.Date,
                                Quantity = m.Quantity
                            }),
                            DN_Time_Job = c.DN_Time_Job.Where(m => m.DateCreated >= dates && m.DateCreated < datee).Select(m => new CDNTimeJobItem { DateCreated = m.DateCreated, DateEnd = m.DateEnd, MinutesLater = m.MinutesLater, MinutesEarly = m.MinutesEarly }),
                            TotalSalaryMonthItem = c.DN_Total_SalaryMonth.Where(m => m.Month == mont && m.Year == year).Select(m => new TotalSalaryMonthItem
                            {
                                ID = m.ID,
                                FixedSalary = m.FixedSalary,
                                TotalDateCC = m.TotalDateCC,
                                TotalSchedule = m.TotalSchedule,
                                TotalMuon = m.TotalSom,
                                TotalSom = m.TotalSom,
                                SalaryAward = m.SalaryAward,
                                Month = m.Month,
                                Year = m.Year,
                                Note = m.Note,
                                CriteriaList = m.Criteria_Total.Where(n => n.Value > 0).Select(n => new DNCriteriaTotalitem
                                {
                                    Id = n.CriteriaID,
                                    Name = n.DN_Criteria.Name,
                                    Value = n.Value
                                })
                            }).FirstOrDefault()
                        };
            return query.ToList();
        }

        public List<DNUserCalendarItem> ListUserNotSId(int agencyId, decimal? dates, int sid, Guid Userid)
        {
            var date = dates.DecimalToDate();
            var t = date.FdiDayOfWeek();
            var dat = date.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.IsLockedOut == false && c.AgencyID == agencyId && ((!c.DN_Calendar.Any(m => m.DateStart < dat && m.DateEnd > dat && m.DN_Calendar_Weekly_Schedule.Any(d => d.DN_Weekly_Schedule.WeeklyID == t && d.DN_Weekly_Schedule.ScheduleID == sid))
                        && !c.DN_UsersInRoles.Any(r => (!r.IsDelete.HasValue || r.IsDelete == false) && r.DN_Roles.DN_Calendar.Any(m => m.DateStart < dat && m.DateEnd > dat && m.DN_Calendar_Weekly_Schedule.Any(d => d.DN_Weekly_Schedule.WeeklyID == t && d.DN_Weekly_Schedule.ScheduleID == sid)))) || c.UserId == Userid)
                        select new DNUserCalendarItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                        };
            return query.ToList();
        }

        public List<DNUserItem> FindUser(string name)
        {
            var query = from c in FDIDB.DN_Users
                        where c.IsDeleted == false
                        orderby c.UserName descending
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            LoweredUserName = c.LoweredUserName
                        };

            return query.Take(10).ToList();
        }
        public List<DNUserItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyid && c.IsDeleted == false
                        orderby c.UserName descending
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            Mobile = c.Mobile,
                            Address = c.Address,
                            Email = c.Email,
                            StartDate = c.StartDate,
                            BirthDay = c.BirthDay,
                            LoweredUserName = c.LoweredUserName,
                            IsLockedOut = c.IsLockedOut,
                            FixedSalary = c.FixedSalary,
                            AgencyName = c.DN_Agency.Name,
                            CodeCheckIn = c.CodeCheckIn,
                            IsService = c.IsService,
                            ListModuleID = c.DN_Module.Where(v => v.IsDelete == false).Select(v => v.ID)
                        };
            total = query.Sum(m => m.FixedSalary);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public void UpdateUserActive(Guid userid, string listint)
        {
            FDIDB.UpdateUserActive(userid, listint);
        }

        public List<DN_Users> GetListByArrId(string lstId)
        {
            var ltsArrId = ConvertUtil.LsGuiId(lstId);
            var query = from o in FDIDB.DN_Users
                        where ltsArrId.Contains(o.UserId)
                        select o;
            return query.ToList();
        }

        public List<DNUserItem> GetListByAgency(int agencyId)
        {
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyId && c.IsDeleted == false
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            Mobile = c.Mobile,
                            Address = c.Address,
                            FixedSalary = c.FixedSalary,
                            Email = c.Email,
                            LoweredUserName = c.LoweredUserName,
                            IsLockedOut = c.IsLockedOut,
                            AgencyName = c.DN_Agency.Name,
                            ListDNGroupMailSSCItem = c.DN_GroupEmail.Where(m => m.IsShow == true).Select(n => new DNGroupMailSSCItem
                            {
                                ID = n.ID,
                                Name = n.Name
                            })
                        };
            return query.ToList();
        }

        public List<DNUserItem> GetListBirthDay(int agencyId)
        {
            var date = DateTime.Today.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyId && c.BirthDay >= date && c.BirthDay <= date
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            Mobile = c.Mobile,
                            Address = c.Address,
                            FixedSalary = c.FixedSalary,
                            Email = c.Email,
                            BirthDay = c.BirthDay,
                            LoweredUserName = c.LoweredUserName,
                            IsLockedOut = c.IsLockedOut,
                            AgencyName = c.DN_Agency.Name
                        };
            return query.ToList();
        }

        public List<SalaryMonthDetailItem> GetListUserRolesMonth(int agencyId, int month, int year)
        {
            var dates = (new DateTime(year, month, 1)).TotalSeconds();
            var datee = (new DateTime(year, month + 1, 1)).TotalSeconds();
            var query = from c in FDIDB.DN_Total_SalaryMonth
                        where c.AgencyID == agencyId && c.Month == month && c.Year == year
                        select new SalaryMonthDetailItem
                        {
                            ID = c.ID,
                            UserId = c.UserId,
                            FixedSalary = c.FixedSalary,
                            TotalDateCC = c.TotalDateCC,
                            TotalMuon = c.TotalMuon,
                            TotalSom = c.TotalSom,
                            SalaryAward = c.SalaryAward,
                            Month = c.Month,
                            Year = c.Year,
                            UserName = c.DN_Users.UserName,
                            LoweredUserName = c.DN_Users.LoweredUserName,
                            RolesUserItems = c.DN_Users.DN_UsersInRoles.Select(x => new DNUserInRolesUserItem
                            {
                                ID = x.ID,
                                RoleId = x.DN_Roles.RoleId,
                                RoleName = x.DN_Roles.RoleName,
                                DepartmentName = x.DN_Department.Name
                            }),
                            CalendarItems = c.DN_Users.DN_Calendar.Where(m => ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new CalendarItem
                            {
                                ID = m.ID,
                                DateStart = m.DateStart,
                                DateEnd = m.DateEnd,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new CalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                    Weekid = d.DN_Weekly_Schedule.WeeklyID
                                })
                            }),
                            DN_Time_Job = c.DN_Users.DN_Time_Job.Select(m => new CDNTimeJobItem { DateCreated = m.DateCreated, MinutesLater = m.MinutesLater, MinutesEarly = m.MinutesEarly }),
                            RCalendarItems = FDIDB.DN_Calendar.Where(m => m.DN_Roles.Any(r => r.DN_UsersInRoles.Any(u => u.UserId == c.UserId && (!u.IsDelete.HasValue || u.IsDelete == false))) && ((m.DateStart >= dates && m.DateStart <= datee) || (m.DateEnd >= dates && m.DateEnd <= datee) || (m.DateStart <= dates && m.DateEnd >= datee)) && m.DN_Calendar_Weekly_Schedule.Any()).Select(m => new RCalendarItem
                            {
                                ID = m.ID,
                                DateStart = m.DateStart,
                                DateEnd = m.DateEnd,
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.Select(d => new RCalendarWeeklyScheduleItem
                                {
                                    MWSID = d.MWSID,
                                    Weekid = d.DN_Weekly_Schedule.WeeklyID
                                })
                            }),
                            CriteriaList = c.Criteria_Total.Select(x => new DNCriteriaTotalitem
                            {
                                Id = x.DN_Criteria.ID,
                                Name = x.DN_Criteria.Name,
                                Value = x.Value
                            })
                        };
            return query.ToList();
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var query = from c in FDIDB.DN_Users
                        where c.IsDeleted == false && c.UserName.Contains(keword) && c.AgencyID == agencyId
                        select new SuggestionsProduct
                        {
                            GuiID = c.UserId,
                            value = c.UserName,
                            title = c.UserName,
                            data = c.UserName
                        };
            return query.Take(showLimit).ToList();
        }
        public DNUserItem GetUserIdByCodeCheckIn(string codecheckin, int agencyid)
        {
            var query = from c in FDIDB.DN_Users
                        where c.CodeCheckIn == codecheckin && c.AgencyID == agencyid
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            IsService = c.IsService
                        };
            return query.FirstOrDefault();
        }

        public DNUserItem GetItemById(Guid id)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserId == id
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            Email = c.Email,
                            Gender = c.Gender,
                            Address = c.Address,
                            Mobile = c.Mobile,
                            Comment = c.Comment,
                            StartDate = c.StartDate,
                            BirthDay = c.BirthDay,
                            CustomerID = c.CustomerID,
                            CustomerName = c.Customer.FullName,
                            LoweredUserName = c.LoweredUserName,
                            FixedSalary = c.FixedSalary,
                            IsLockedOut = c.IsLockedOut,
                            IsAgency = c.IsAgency,
                            IsApproved = c.IsApproved,
                            Password = c.Password,
                            CardSerial = c.Customer.DN_Card.Serial,
                            CodeCheckIn = c.CodeCheckIn,
                            IsService = c.IsService,
                            listRole = c.DN_UsersInRoles.Where(v => (!v.IsDelete.HasValue || v.IsDelete == false) && (!v.DN_Roles.IsDeleted.HasValue || v.DN_Roles.IsDeleted == false)).Select(v => v.DN_Roles.RoleName),
                            ListModule = c.DN_Module.Where(v => v.IsDelete == false).Select(v => v.NameModule)
                        };
            return query.FirstOrDefault();
        }

        public DNUserItem GetItemModuleById(Guid id)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserId == id
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            DN_User_ModuleActive = c.DN_User_ModuleActive.OrderBy(m => m.ActiveId).Select(m => new DNUserModuleActiveItem
                            {
                                ID = m.ID,
                                Active = m.Active,
                                ActiveRoleId = m.ActiveId,
                                ActiveName = m.DN_Active.NameActive,
                                ModuleId = m.ModuleId
                            }),
                            DN_Module = c.DN_Module.Select(m => new ModuleItem
                            {
                                ID = m.ID,
                                NameModule = m.NameModule,
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<DN_Active> GetListActive()
        {
            var query = from c in FDIDB.DN_Active
                        select c;
            return query.ToList();
        }

        public DN_Users GetById(Guid id)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserId == id
                        select c;
            return query.FirstOrDefault();
        }

        public DNUserItem GetScheduleById(Guid id)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserId == id
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            ListCalendarItem = c.DN_Calendar.Select(m => new DNCalendarItem
                            {
                                DNCalendarWeeklySchedule = m.DN_Calendar_Weekly_Schedule.GroupBy(t => t.DN_Weekly_Schedule.ScheduleID).Select(n => new DNCalendarWeeklyScheduleItem
                                {
                                    DN_Weekly_Schedule = new WeeklyScheduleItem
                                    {
                                        ScheduleName = n.FirstOrDefault().DN_Weekly_Schedule.DN_Schedule.Name
                                    }
                                })
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<DN_Module> GetListModulebyListInt(List<int> lst)
        {
            var query = from c in FDIDB.DN_Module
                        where lst.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public void Delete(DN_Users dnUsers)
        {
            FDIDB.DN_Users.Remove(dnUsers);
        }

        public void Delete(DN_User_ModuleActive module)
        {
            FDIDB.DN_User_ModuleActive.Remove(module);
        }

        public void Add(DN_Users dnUsers)
        {
            FDIDB.DN_Users.Add(dnUsers);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
