using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNUsersInRolesDA : BaseDA
    {
        #region Constructer
        public DNUsersInRolesDA()
        {
        }

        public DNUsersInRolesDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNUsersInRolesDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNUsersInRolesItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_UsersInRoles
                        where o.AgencyID == agencyid && o.IsDelete == false
                        orderby o.ID descending
                        select new DNUsersInRolesItem
                        {
                                       ID = o.ID,
                                       UserId = o.UserId,
                                       RoleId = o.RoleId,
                                       DateCreated = o.DateCreated,
                                       DepartmentID = o.DepartmentID,
                                       DNUserItem = new DNUserItem
                                       {
                                           UserName = o.DN_Users.UserName
                                       },
                                       DNRolesItem = new DNRolesItem
                                       {
                                           RoleName = o.DN_Roles.RoleName
                                       },
                                       DepartmentItem = new DepartmentItem
                                       {
                                           Name = o.DN_Department.Name
                                       }
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_UsersInRoles GetById(int id)
        {
            var query = from o in FDIDB.DN_UsersInRoles where o.ID == id && o.IsDelete == false select o;
            return query.FirstOrDefault();
        }

        public List<DNUsersInRolesItem> GetAll()
        {
            var query = from o in FDIDB.DN_UsersInRoles
                        where o.IsDelete == false
                        select new DNUsersInRolesItem
                        {
                            UserId = o.UserId,
                            RoleId = o.RoleId,
                            DateCreated = o.DateCreated,
                            DepartmentID = o.DepartmentID
                        };
            return query.ToList();
        }

        public List<DNUsersInRolesItem> GetListByRoleId(Guid roleid, int agencyId)
        {
            var query = from o in FDIDB.DN_UsersInRoles
                        where o.RoleId == roleid && o.AgencyID == agencyId && o.IsDelete == false
                        select new DNUsersInRolesItem
                        {
                            ID = o.ID,
                            UserId = o.UserId,
                            DepartmentID = o.DepartmentID,
                            DNUserItem = new DNUserItem
                            {
                                UserName = o.DN_Users.UserName
                            },
                            DepartmentItem = new DepartmentItem
                            {
                                Name = o.DN_Department.Name
                            },
                            RoleId = o.RoleId
                        };
            return query.ToList();
        }
        public List<DNUsersInRolesItem> GetListAddTree(Guid roleid,int dId)
        {
            var query = from o in FDIDB.DN_UsersInRoles
                        where o.RoleId == roleid && o.IsDelete == false
                        select new DNUsersInRolesItem
                        {
                            ID = o.ID,
                            DepartmentID = o.DepartmentID,
                            DNUserItem = new DNUserItem
                            {
                                UserName = o.DN_Users.UserName,
                                LoweredUserName = o.DN_Users.LoweredUserName
                            }
                            
                        };
            return query.ToList();
        }

        public List<DN_UsersInRoles> GetListByArrId(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DN_UsersInRoles where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(DN_UsersInRoles usersInRoles)
        {
            FDIDB.DN_UsersInRoles.Add(usersInRoles);
        }

        public void Delete(DN_UsersInRoles usersInRoles)
        {
            FDIDB.DN_UsersInRoles.Remove(usersInRoles);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
