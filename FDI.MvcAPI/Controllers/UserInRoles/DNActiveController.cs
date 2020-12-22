using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Base;

namespace FDI.MvcAPI.Controllers
{
    public class DNActiveController : BaseApiController
    {
        //
        // GET: /DNActive/
        
        private readonly DNActiveRoleDA _dl = new DNActiveRoleDA("#");

        public ActionResult GetListSimpleByRequest()
        {
            var obj = Request["key"] != Keyapi ? new List<ActiveRoleItem>() : _dl.GetListSimpleByRequest(Request);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<ActiveRoleItem>() : _dl.GetByAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRoleItemById(string key, int id)
        {
            var obj = key != Keyapi ? new ActiveRoleItem() : _dl.GetRoleItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, ActiveRoleItem activeRole)
        {
            if (key == Keyapi)
            {
                var obj = new DN_Active();
                UpdateRole(obj, activeRole);
                _dl.Save();
                _dl.Add(obj);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, ActiveRoleItem activeRole)
        {
            if (key == Keyapi)
            {
                var obj = _dl.GetById(activeRole.ID);
                UpdateRole(obj, activeRole);
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, List<int> listint)
        {
            if (key == Keyapi)
            {
                var list = _dl.GetListByArrID(listint);
                foreach (var item in list)
                {
                    _dl.Delete(item);
                }
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public DN_Active UpdateRole(DN_Active activeRole, ActiveRoleItem activeRoleItem)
        {
            activeRole.NameActive = activeRoleItem.NameActive;
            activeRole.Ord = activeRoleItem.Ord;
            return activeRole;
        }
    }
}
