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
    public class DNActiveRoleController : BaseController
    {
        //
        // GET: /Admin/ActiveRole/

        private readonly DNActiveRoleDA _dnActiveRoleDa;
        public DNActiveRoleController()
        {
            _dnActiveRoleDa = new DNActiveRoleDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var listactiveRoleItem = _dnActiveRoleDa.GetListSimpleByRequest(Request).OrderBy(m => m.Ord);
            var model = new ModelActiveRoleItem
                            {
                                ListItem = listactiveRoleItem,
                                PageHtml = _dnActiveRoleDa.GridHtmlPage
                            };
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var role = _dnActiveRoleDa.GetRoleItemById(ArrId.FirstOrDefault());
            ViewData.Model = role;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var activeRole = new DN_Active();
            if (DoAction == ActionType.Edit)
            {
                activeRole = _dnActiveRoleDa.GetById(ArrId.FirstOrDefault());
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
            var activeRole = new DN_Active();

            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(activeRole);
                        _dnActiveRoleDa.Add(activeRole);
                        _dnActiveRoleDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = activeRole.ID.ToString(),
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
                        activeRole = _dnActiveRoleDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(activeRole);
                        _dnActiveRoleDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = activeRole.ID.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(activeRole.NameActive))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    var ltsRolesItems = _dnActiveRoleDa.GetListByArrID(ArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in ltsRolesItems)
                    {
                        _dnActiveRoleDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.NameActive));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _dnActiveRoleDa.Save();
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
