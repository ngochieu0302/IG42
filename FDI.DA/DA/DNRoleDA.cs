using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNRoleDA : BaseDA
    {
        #region Constructer
        public DNRoleDA()
        {
        }
        public DNRoleDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNRoleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<DNRolesItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Roles
                        where c.AgencyID == agencyid && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        orderby c.RoleName descending
                        select new DNRolesItem
                                   {
                                       RoleId = c.RoleId,
                                       RoleName = c.RoleName,
                                       Description = c.Description,
                                       LoweredRoleName = c.DN_Level.Name,
                                       ListModuleID = c.DN_Module.Where(v => v.IsDelete == false && v.IsShow == true).Select(v => v.ID)
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DN_Active> GetListActive()
        {
            var query = from c in FDIDB.DN_Active
                        select c;
            return query.ToList();
        }
        public List<DNRolesItem> GetByAll(int agencyId)
        {
            var query = from c in FDIDB.DN_Roles
                        where c.AgencyID == agencyId && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        select new DNRolesItem
                        {
                            RoleId = c.RoleId,
                            RoleName = c.RoleName,
                            Description = c.Description
                        };
            return query.ToList();
        }
        public DNRolesItem GetRoleItemById(Guid roleId)
        {
            var query = from c in FDIDB.DN_Roles
                        where c.RoleId == roleId && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        select new DNRolesItem
                                   {
                                       RoleId = c.RoleId,
                                       RoleName = c.RoleName,
                                       Description = c.Description,
                                       LoweredRoleName = c.DN_Level.Name,
                                       LevelRoomId = c.LevelId,
                                       DN_UsersInRoles = c.DN_UsersInRoles.Where(v => v.IsDelete == false).Select(m => new DNUsersInRolesItem
                                       {
                                           UserId = m.UserId
                                       }),
                                       ModuleItems = c.DN_Module.Select(m => new ModuleItem
                                       {
                                           ID = m.ID,
                                           NameModule = m.NameModule
                                       }),
                                       RoleModuleActiveItems = c.DN_Role_ModuleActive.Select(m => new Role_ModuleActiveItem
                                       {
                                           ID = m.ID,
                                           ModuleId = m.ModuleId,
                                           ActiveRoleId = m.ActiveId,
                                           ActiveName = m.DN_Active.NameActive,
                                           Active = m.Active
                                       }),
                                       UserItems = c.DN_UsersInRoles.Where(v => v.IsDelete == false).Select(m => new DNUserSimpleItem
                                       {
                                           UserId = m.UserId,
                                           UserName = m.DN_Users.UserName,
                                           LoweredUserName = m.DN_Department.Name
                                       }),

                                   };
            var result = query.FirstOrDefault();
            return result;
        }
        public DN_Roles GetById(Guid roleId)
        {
            var query = from c in FDIDB.DN_Roles
                        where c.RoleId == roleId && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        select c;
            return query.FirstOrDefault();
        }
        public DN_Roles GetByName(string name)
        {
            var query = from c in FDIDB.DN_Roles
                        where c.RoleName == name && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        select c;
            return query.FirstOrDefault();
        }
        public void UpdateRoleActive(Guid roleid, string listint)
        {
            FDIDB.UpdateRoleActive(roleid, listint);
        }
        public void UpdateRoleModuleActive(Guid roleid, string listint)
        {
            FDIDB.UpdateRoleModuleActive(roleid, listint);
        }
        public List<DN_Roles> GetListByArrId(List<Guid> LtsArrID)
        {
            var query = from c in FDIDB.DN_Roles where LtsArrID.Contains(c.RoleId) select c;
            return query.ToList();
        }
        public List<DN_Module> GetListModulebyListInt(List<int> lst)
        {
            var query = from c in FDIDB.DN_Module
                        where lst.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public void Add(DN_Roles roles)
        {
            FDIDB.DN_Roles.Add(roles);
        }
        public void Delete(DN_Roles roles)
        {
            FDIDB.DN_Roles.Remove(roles);
        }
        public void Delete(DN_UsersInRoles usersInRoles)
        {
            FDIDB.DN_UsersInRoles.Remove(usersInRoles);
        }
        public void Delete(DN_Role_ModuleActive module)
        {
            FDIDB.DN_Role_ModuleActive.Remove(module);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
