using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.MvcAPI.Controllers;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.API.Controllers
{
    public class DNUsersInRoleController : BaseApiController
    {
        //
        // GET: /DNUsersInRole/

        private readonly DNUsersInRolesDA _dl = new DNUsersInRolesDA("#");
        //private readonly DNLoginDA _dllogin = new DNLoginDA("#");
        //private readonly DNUsersInRoles _bl = new DNUserBL();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNUsersInRolesItem()
                : new ModelDNUsersInRolesItem { ListItem = _dl.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _dl.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByRoleId(string key, string code, Guid roleid)
        {
            var obj = key != Keyapi ? new List<DNUsersInRolesItem>() : _dl.GetListByRoleId(roleid, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDepartmentID(string key, string code, int id, int departmentid)
        {
            if (key == Keyapi)
            {
                var obj = _dl.GetById(id);
                obj.DepartmentID = departmentid;
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
