using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ModuleController : BaseController
    {
        private readonly DNModuleAPI _moduleDa;
        private readonly DNUserAPI _userDa;
        private readonly DNRoleAPI _roleDa;

        public ModuleController()
        {
            _userDa = new DNUserAPI();
            _roleDa = new DNRoleAPI();
            _moduleDa = new DNModuleAPI();
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult JsonTreeCategorySelect()
        {
            var ltsCategory = _moduleDa.GetListTree(UserItem.AgencyID);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxTreeSelect()
        {
            return View();
        }

        public ActionResult AjaxSort()
        {
            var ltsSourceCategory = _moduleDa.GetAllListSimpleByParentID(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = ltsSourceCategory;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var model = new ModuleItem();
            ViewBag.RoleId = _roleDa.GetAll(UserItem.AgencyID);
            ViewBag.UserID = _userDa.GetAll(UserItem.AgencyID);
            if (DoAction == ActionType.Edit)
                model = _moduleDa.GetItemById(ArrId.FirstOrDefault());
            ViewData.Model = model;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var module = new ModuleUpdateItem();
            string json;
            switch (DoAction)
            {
                case ActionType.Edit:
                    try
                    {
                        UpdateModel(module);
                        module.ID = ArrId.FirstOrDefault();
                        json = new JavaScriptSerializer().Serialize(module);
                        _moduleDa.Update(UserItem.AgencyID, json, CodeLogin());
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(module.NameModule))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                case ActionType.Show:
                    json = new JavaScriptSerializer().Serialize(ArrId);
                    _moduleDa.Show(json, UserItem.AgencyID);
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = string.Join(",", ArrId),
                        Message = string.Format("Đã hiện thành công : <b>{0}</b>", Server.HtmlEncode(string.Join(",", ArrId)))
                    };
                    break;

                case ActionType.Hide:
                    json = new JavaScriptSerializer().Serialize(ArrId);
                    _moduleDa.Hide(json, UserItem.AgencyID);
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = string.Join(",", ArrId),
                        Message = string.Format("Đã ẩn thành công : <b>{0}</b>", Server.HtmlEncode(string.Join(",", ArrId)))
                    };
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
