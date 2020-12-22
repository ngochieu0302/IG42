using System;
using System.Collections.Generic;
using System.Linq;
using FDI.Base;

namespace FDI.DA
{
    public class DNRoleModuleActiveDA :BaseDA
    {
        #region Constructer
        public DNRoleModuleActiveDA()
        {
        }

        public DNRoleModuleActiveDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNRoleModuleActiveDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<DN_Role_ModuleActive> GetListRoleModuleActivekt(Guid id, int moduleid)
        {
            var query = from c in FDIDB.DN_Role_ModuleActive
                        where c.RoleId == id && c.ModuleId == moduleid
                        select c;
            return query.ToList();
        }

        public List<DN_Role_ModuleActive> GetListRoleModuleActivekt(Guid id)
        {
            var query = from c in FDIDB.DN_Role_ModuleActive
                        where c.RoleId == id
                        select c;
            return query.ToList();
        }
        public DN_Role_ModuleActive GetByRoleModuleActiveId(int id)
        {
            var query = from c in FDIDB.DN_Role_ModuleActive
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }


        public void Delete(DN_Role_ModuleActive roleModuleActive)
        {
            FDIDB.DN_Role_ModuleActive.Remove(roleModuleActive);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
