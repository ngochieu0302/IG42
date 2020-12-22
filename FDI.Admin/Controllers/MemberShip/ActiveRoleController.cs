using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class ActiveRoleController : BaseController
    {
        //
        // GET: /Admin/ActiveRole/
        private readonly ActiveRoleDA _activeRoleDa;
        public ActiveRoleController()
        {
            _activeRoleDa = new ActiveRoleDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var listactiveRoleItem = _activeRoleDa.GetListSimpleByRequest(Request).OrderBy(m => m.Ord);
            var model = new ModelActiveRoleItem
                            {
                                ListItem = listactiveRoleItem,
                                PageHtml = _activeRoleDa.GridHtmlPage
                            };
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var role = _activeRoleDa.GetRoleItemById(ArrId.FirstOrDefault());
            ViewData.Model = role;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var activeRole = new ActiveRole();
            if (DoAction == ActionType.Edit)
            {
                activeRole = _activeRoleDa.GetById(ArrId.FirstOrDefault());
            }

            ViewData.Model = activeRole;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var activeRole = new ActiveRole();

            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(activeRole);
                        _activeRoleDa.Add(activeRole);
                        _activeRoleDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = activeRole.Id.ToString(),
                            Message =
                                string.Format("Đã thêm mới hành động: <b>{0}</b>",
                                              Server.HtmlEncode(activeRole.NameActive))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        activeRole = _activeRoleDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(activeRole);
                        _activeRoleDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = activeRole.Id.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(activeRole.NameActive))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    var ltsRolesItems = _activeRoleDa.GetListByArrID(ArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in ltsRolesItems)
                    {
                        if (item.aspnet_Roles.Count > 0)
                        {
                            stbMessage.AppendFormat("Chuyên mục <b>{0}</b> đang được sử dụng, không được phép xóa.<br />", Server.HtmlEncode(item.NameActive));
                            //msg.Erros = true;
                        }
                        else
                        {
                            _activeRoleDa.Delete(item);
                            stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.NameActive));
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _activeRoleDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
