using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class UsersInRoleController : BaseApiController
    {
        //
        // GET: /UsersInRole/

        private readonly DNUsersInRolesDA _dl = new DNUsersInRolesDA("#");

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNUsersInRolesItem()
                : new ModelDNUsersInRolesItem { ListItem = _dl.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _dl.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByRoleId(string key, Guid roleid, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNUsersInRolesItem>() : _dl.GetListByRoleId(roleid, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListAddTree(string key, Guid roleid, int dId)
        {
            var obj = key != Keyapi ? new List<DNUsersInRolesItem>() : _dl.GetListAddTree(roleid, dId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDepartmentID(string key, int id, int departmentid)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var obj = _dl.GetById(id);
                    if (departmentid != 0) obj.DepartmentID = departmentid;
                    else obj.DepartmentID = null;
                    _dl.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
