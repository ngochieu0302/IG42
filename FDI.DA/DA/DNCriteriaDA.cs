using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNCriteriaDA : BaseDA
    {
        #region Constructer
        public DNCriteriaDA()
        {
        }

        public DNCriteriaDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNCriteriaDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNCriteriaItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Criteria
                        where o.AgencyID == agencyid
                        orderby o.ID descending
                        select new DNCriteriaItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            NameType = o.DN_TypeCriteria.Name,
                            Price = o.Price,
                            IsAll = o.IsAll,
                            IsSchedule = o.IsSchedule,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Criteria GetById(int id)
        {
            var query = from c in FDIDB.DN_Criteria where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNCriteriaItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Criteria
                        where o.ID == id
                        select new DNCriteriaItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Price = o.Price,
                            IsAll = o.IsAll,
                            IsSchedule = o.IsSchedule,
                            TypeID = o.TypeID,
                            DNUserItem = o.DN_Users.Select(u => new DNUserItem
                            {
                                UserId = u.UserId,
                                UserName = u.UserName
                            }),
                            DNRolesItem = o.DN_Roles.Select(r => new DNRolesItem
                            {
                                RoleId = r.RoleId,
                                RoleName = r.RoleName
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<DNCriteriaItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_Criteria
                        where o.AgencyID == agencyid
                        select new DNCriteriaItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Price = o.Price,
                            Value = o.Value,
                        };
            return query.ToList();
        }
        public List<TypeCriteriaItem> GetTypeAll()
        {
            var query = from o in FDIDB.DN_TypeCriteria
                        select new TypeCriteriaItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            IsPercent = o.IsPercent
                        };
            return query.ToList();
        }

        public List<DNCriteriaItem> GetByIsOrderByMonth(decimal startdate, decimal datenow, int agencyid, int bedid)
        {
            var query = from o in FDIDB.DN_Criteria
                        where o.AgencyID == agencyid
                        select new DNCriteriaItem
                        {
                            ID = o.ID,
                            Price = o.Price,
                            TypeID = o.TypeID,
                            IsAll = o.IsAll,
                            IsSchedule = o.IsSchedule,
                            DNUserItem = o.DN_Users.Where(m => m.AgencyID == agencyid).Select(m => new DNUserItem
                            {
                                UserId = m.UserId,
                                IsOnline = m.DN_Time_Job.Any(c => c.DateCreated >= startdate && c.DateCreated <= datenow && (!c.DateEnd.HasValue || c.DateEnd == c.DateCreated)),
                                IsBed = m.DN_UsersInRoles.Any(l => l.IsDelete == false && l.DN_Roles.DN_Level.DN_Room.Any(r => r.DN_Bed_Desk.Any(b => b.ID == bedid)))
                            }),
                            DNRolesItem = o.DN_Roles.Where(m => m.AgencyID == agencyid).Select(m => new DNRolesItem
                            {
                                LevelRoomId = m.LevelId,
                                DN_UsersInRoles = m.DN_UsersInRoles.Where(v => v.IsDelete == false).Select(n => new DNUsersInRolesItem
                                {
                                    UserId = n.UserId,
                                    IsOnline = n.DN_Users.DN_Time_Job.Any(c => c.DateCreated >= startdate && c.DateCreated <= startdate && (!c.DateEnd.HasValue || c.DateEnd == c.DateCreated)),
                                }),
                                IsBed = m.DN_Level.DN_Room.Any(r => r.DN_Bed_Desk.Any(b => b.ID == bedid)),
                            })
                        };
            return query.ToList();
        }

        public List<DN_Criteria> ListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Criteria
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }

        public List<DNCriteriaItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Criteria
                        where ltsArrId.Contains(o.ID)
                        select new DNCriteriaItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Price = o.Price,
                            Value = o.Value,
                        };
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

        public void Add(DN_Criteria dnCriteria)
        {
            FDIDB.DN_Criteria.Add(dnCriteria);
        }

        public void Delete(DN_Criteria dnCriteria)
        {
            FDIDB.DN_Criteria.Remove(dnCriteria);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
