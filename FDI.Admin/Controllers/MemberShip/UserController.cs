using System;
using System.Linq;
using System.Web.Mvc;
using FDI.DA;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Admin/User/

        private readonly UserDA _userDa;
        public UserController()
        {
            _userDa = new UserDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var listcustomer = _userDa.GetListSimpleByRequest(Request);
            ViewBag.PageHtml = _userDa.GridHtmlPage;
            return View(listcustomer);
        }

        public ActionResult AjaxForm()
        {
            //ViewBag.Edit = systemActionItem.Edit;
            //ViewBag.Delete = systemActionItem.Delete;
            var userId = GuiId.FirstOrDefault();
            var user = _userDa.GetById(userId);
            var actionRole = _userDa.GetActiveRoleAll();
            ViewBag.ItemId = userId;
            ViewBag.ActiveRoles = actionRole;
            return View(user);
        }

        public ActionResult DeleteRoleAction()
        {
            JsonMessage msg;
            try
            {
                int moduleid = Convert.ToInt16(Request["moduleid"]);
                var userId = Guid.Parse(Request["ItemID"]);
                var role = _userDa.GetById(userId);
                var module = role.Modules.FirstOrDefault(m => m.ID == moduleid);
                if (module != null)
                {
                    var namemodule = module.NameModule;
                    role.Modules.Remove(module);
                    _userDa.Save();
                    var userModuleActive = _userDa.GetListUserModuleActivekt(userId, moduleid);
                    foreach (var moduleActive in userModuleActive)
                    {
                        _userDa.Delete(moduleActive);
                        _userDa.Save();
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = moduleid.ToString(),
                        Message = string.Format("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(namemodule))
                    };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Không có hành động nào được thực hiện."
                };
            }
            catch (Exception)
            {

                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Không có hành động nào được thực hiện."
                };
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult EditRoleAction()
        {
            JsonMessage msg;
            int moduleid = Convert.ToInt16(Request["moduleid"]);
            var userId = Guid.Parse(Request["ItemID"]);

            var module = moduleid != 0
                            ? _userDa.GetListUserModuleActivekt(userId, moduleid)
                            : _userDa.GetListUserModuleActivekt(userId);
            if (module.Count > 0)
            {
                foreach (var user in module.Select(t => _userDa.GetByUserModuleActiveId(t.ID)))
                {
                    var check = Request[user.ID.ToString()];
                    user.Active = check != null;
                    _userDa.Save();
                }
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = moduleid.ToString(),
                    Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>.<br />", Server.HtmlEncode("Thành công!"))
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            msg = new JsonMessage
            {
                Erros = true,
                Message = "Không có hành động nào được thực hiện."
            };
            return Json(msg, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetMessenger(string str)
        {
            var msg = new JsonMessage { Message = str, Erros = true };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
