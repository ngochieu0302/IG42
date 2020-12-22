using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;
namespace FDI.GetAPI
{
    public class DNUsersInRoleAPI : BaseAPI
    {
        public List<DNUsersInRolesItem> GetListByRoleId(int agencyId,Guid roleid)
        {
            var urlJson = string.Format("{0}UsersInRole/GetListByRoleId?key={1}&roleid={2}&agencyId={3}", Domain, Keyapi, roleid, agencyId);
            return GetObjJson<List<DNUsersInRolesItem>>(urlJson);
        }
        public List<DNUsersInRolesItem> GetListAddTree(Guid roleid, int dId)
        {
            var urlJson = string.Format("{0}UsersInRole/GetListAddTree?key={1}&roleid={2}&dId={3}", Domain, Keyapi, roleid, dId);
            return GetObjJson<List<DNUsersInRolesItem>>(urlJson);
        }

        public JsonMessage UpdateDepartmentID(int id, int departmentid)
        {
            var urlJson = string.Format("{0}UsersInRole/UpdateDepartmentID?key={1}&id={2}&departmentid={3}", Domain, Keyapi,id, departmentid);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
