using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNRoleController : BaseController
    {
        readonly DNRoleAPI _dnRoleApi;
        readonly DNUserAPI _dnUserApi;
        readonly DNUsersInRoleAPI _dnUserIsnRole;
        readonly DNActiveAPI _dnActiveApi;
        readonly DNLevelRoomAPI _dnLevelRoom;
        readonly DepartmentAPI _departmentApi;
        readonly DNModuleAPI _moduleApi = new DNModuleAPI();
        public DNRoleController()
        {
            _dnRoleApi = new DNRoleAPI();
            _dnActiveApi = new DNActiveAPI();
            _dnUserApi = new DNUserAPI();
            _dnUserIsnRole = new DNUsersInRoleAPI();
            _dnLevelRoom = new DNLevelRoomAPI();
            _departmentApi = new DepartmentAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = _dnRoleApi.ListItems(UserItem.AgencyID, Request.Url.Query);
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var model = _dnRoleApi.GetByid(GuiId.FirstOrDefault());           
            return View(model);
        }

        public ActionResult AjaxUserForm()
        {
            var list = _dnUserIsnRole.GetListByRoleId(UserItem.AgencyID, GuiId.FirstOrDefault());
            ViewBag.Department = _departmentApi.GetAll(UserItem.AgencyID);
            return View(list);
        }

        public ActionResult UpdateDepartmentID(int id, int departmentid)
        {
            var msg = _dnUserIsnRole.UpdateDepartmentID(id, departmentid);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxRoleActive()
        {
            var model = new ModelDNRolesItem
            {
                Item = _dnRoleApi.GetByid(Guid.Parse(Request["id"])),
                ActiveRoleItems = _dnActiveApi.GetAll()
            };
            ViewBag.Action = "View";
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        public ActionResult AjaxRoleModule()
        {
            var model = new ModelDNRolesItem
            {
                Item = _dnRoleApi.GetByid(GuiId.FirstOrDefault()),
                ActiveRoleItems = _dnActiveApi.GetAll()
            };
            ViewBag.Action = "RoleModule";
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var obj = new DNRolesItem();
            if (DoAction == ActionType.Edit)
            {
                obj = _dnRoleApi.GetByid(GuiId.FirstOrDefault());
            }
            ViewBag.listroom = _dnLevelRoom.GetListParentID(UserItem.AgencyID);
            ViewBag.listUser = _dnUserApi.GetAll(UserItem.AgencyID);
            ViewData.Model = obj;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult AjaxTree(Guid RoleId, string lstInt)
        {
            var model = new ModelCategoryItem
            {
                UserId = RoleId,
                Listid = lstInt
            };
            return View(model);
        }

        public ActionResult JsonGetListTree(string listid)
        {
            var ltsCategory = _moduleApi.GetListTreeNew(listid, UserItem.AgencyID).Where(c => c.ParentId == 1);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var obj = new DNRolesJsonItem();
            string json;
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(obj);
                    obj.RoleId = Guid.NewGuid();
                    obj.Code = CodeLogin();
                    json = new JavaScriptSerializer().Serialize(obj);
                    msg = _dnRoleApi.Add(UserItem.AgencyID, json, CodeLogin());
                    break;
                case ActionType.Edit:
                    UpdateModel(obj);
                    obj.Code = CodeLogin();
                    obj.RoleId = GuiId.FirstOrDefault();
                    json = new JavaScriptSerializer().Serialize(obj);
                    msg = _dnRoleApi.Update(json, GuiId.FirstOrDefault(), UserItem.AgencyID, CodeLogin());
                    break;

                case ActionType.Delete:
                    var lstId = Request["itemId"];
                    msg = _dnRoleApi.Delete(lstId);                    
                    break;
                case ActionType.View:
                    msg = _dnRoleApi.UpdateActive(GuiId.FirstOrDefault(), Request["chkActiveRoles"]);                    
                    break;
                case ActionType.RoleModule:
                    msg = _dnRoleApi.UpdateModuleActive(GuiId.FirstOrDefault(), Request["chkActiveRoles"]);                    
                    break;
                case ActionType.Active:
                    var listInt = Request["listInt"];
                    var RoleId = Request["RoleId"];
                    msg = _dnRoleApi.AddModuleRole(listInt, RoleId, UserItem.AgencyID);
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
