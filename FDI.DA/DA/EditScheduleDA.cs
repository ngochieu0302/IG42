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
    public class EditScheduleDA : BaseDA
    {
        #region Constructer
        public EditScheduleDA()
        {
        }

        public EditScheduleDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public EditScheduleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<EditScheduleItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_EDIT_Schedule
                        where o.AgencyID == agencyid
                        orderby o.ID descending
                        select new EditScheduleItem
                        {
                            ID = o.ID,
                            Date = o.Date,
                            Name = o.Name,
                            Datecreated = o.Datecreated,
                            UserId = o.UserId,
                            UserChangeId = o.UserChangeId,
                            AgencyID = o.AgencyID,
                            ScheduleName = o.DN_Schedule.Name,
                            UserName = o.DN_Users.UserName,
                            UserNameChange = o.DN_Users1.UserName,
                            Type = o.Type
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_EDIT_Schedule GetById(int id)
        {
            var query = from c in FDIDB.DN_EDIT_Schedule where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public EditScheduleItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_EDIT_Schedule
                        where o.ID == id
                        orderby o.ID descending
                        select new EditScheduleItem
                        {
                            ID = o.ID,
                            Date = o.Date,
                            Name = o.Name,
                            Datecreated = o.Datecreated,
                            UserId = o.UserId,
                            Type = o.Type,
                            UserChangeId = o.UserChangeId,
                            AgencyID = o.AgencyID,
                            ScheduleName = o.DN_Schedule.Name,
                            UserName = o.DN_Users.UserName,
                            UserNameChange = o.DN_Users1.UserName
                        };
            return query.FirstOrDefault();
        }

        public List<EditScheduleItem> GetAll(decimal dates, decimal datee, int agencyid)
        {
            var query = from o in FDIDB.DN_EDIT_Schedule
                        where o.Date >= dates && o.Date <= datee && o.AgencyID == agencyid
                        orderby o.ID descending
                        select new EditScheduleItem
                        {
                            ID = o.ID,
                            Date = o.Date,
                            UserId = o.UserId,
                            ScheduleID = o.ScheduleID,
                            UserChangeId = o.UserChangeId,
                        };
            return query.ToList();
        }

        public List<EditScheduleItem> GetAll()
        {
            var query = from o in FDIDB.DN_EDIT_Schedule
                        orderby o.ID descending
                        select new EditScheduleItem
                        {
                            ID = o.ID,
                            Date = o.Date,
                            Name = o.Name,
                            Datecreated = o.Datecreated,
                            UserId = o.UserId,
                            UserChangeId = o.UserChangeId,
                            AgencyID = o.AgencyID
                        };
            return query.ToList();
        }

        public List<DNUserItem> GetAllByAgencyId(int agencyId, int month)
        {
            var monthstart = ConvertDate.TotalSecondsMonth(month);
            var monthend = ConvertDate.TotalSecondsMonth(month + 1);
            var query = from o in FDIDB.DN_Users
                        where o.AgencyID == agencyId && (o.DN_EDIT_Schedule.Any(m => m.Datecreated >= monthstart && m.Datecreated <= monthend) || o.DN_EDIT_Schedule1.Any(m => m.Datecreated >= monthstart && m.Datecreated <= monthend))
                        select new DNUserItem
                        {
                            UserId = o.UserId,
                            DN_EDIT_Schedule = o.DN_EDIT_Schedule.Select(m => new EditScheduleItem
                            {
                                ID = m.ID,
                            }),
                            DN_EDIT_Schedule1 = o.DN_EDIT_Schedule1.Select(m => new EditScheduleItem
                            {
                                ID = m.ID,
                            })
                        };
            return query.ToList();
        }

        public List<DN_EDIT_Schedule> GetListByArrId(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DN_EDIT_Schedule where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(DN_EDIT_Schedule editSchedule)
        {
            FDIDB.DN_EDIT_Schedule.Add(editSchedule);
        }

        public void Delete(DN_EDIT_Schedule editSchedule)
        {
            FDIDB.DN_EDIT_Schedule.Remove(editSchedule);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
