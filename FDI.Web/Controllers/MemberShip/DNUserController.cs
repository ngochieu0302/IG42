using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNUserController : BaseController
    {
        readonly DNUserAPI _dnUserApi = new DNUserAPI();
        readonly CustomerAPI _customerApi = new CustomerAPI();
        readonly DNActiveAPI _dnActiveApi = new DNActiveAPI();
        readonly DNModuleAPI _moduleApi = new DNModuleAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = _dnUserApi.ListItems(UserItem.AgencyID, Request.Url.Query);
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxView()
        {
            var model = _dnUserApi.GetItemById(UserItem.AgencyID, GuiId.FirstOrDefault());           
            return View(model);
        }
        public ActionResult FindUser(string name)
        {
            var lst = _dnUserApi.FindByName(name);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxForm()
        {
            var dnUser = new DNUserItem
            {
                PasswordSalt = "ssc123456",
                StartDate = DateTime.Now.TotalSeconds()
            };
            if (DoAction == ActionType.Edit)
            {
                dnUser = _dnUserApi.GetItemById(UserItem.AgencyID, GuiId.FirstOrDefault());
            }
            ViewData.Model = dnUser;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public string CheckByUserName(string UserName, Guid id)
        {
            var result = _dnUserApi.CheckUserName(UserName.Trim(), id, UserItem.AgencyID);
            return result ? "false" : "true";
        }

        public ActionResult AjaxRoleModule()
        {
            var model = new ModelDNUserItem
            {
                Item = _dnUserApi.GetItemModuleById(GuiId.FirstOrDefault()),
                ActiveRoleItems = _dnActiveApi.GetAll()
            };
            ViewBag.Action = "RoleModule";
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        public ActionResult GetCustomer(string serial)
        {
            var item = _customerApi.GetCustomerItemBySerial(serial);
            if (item == null)
                return Json(0, JsonRequestBehavior.AllowGet);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxTree(Guid UserId, string lstInt)
        {
            var model = new ModelCategoryItem
            {
                UserId = UserId,
                Listid = lstInt
            };
            return View(model);
        }

        public ActionResult JsonGetListTree(string listid)
        {
            var ltsCategory = _moduleApi.GetListTreeNew(listid, UserItem.AgencyID);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Auto()
        {
            var query = Request["query"];
            var ltsResults = _dnUserApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var dnUser = new DNUserAddItem();
            var json = "";
            var date = Request["StartDay"];
            var birthDay = Request["BirthDay_"];
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(dnUser);
                    if (!string.IsNullOrEmpty(date))
                        dnUser.StartDate = date.StringToDecimal(0);
                    if (!string.IsNullOrEmpty(birthDay))
                        dnUser.BirthDay = birthDay.StringToDecimal(0);
                    dnUser.UserId = Guid.NewGuid();
                    json = new JavaScriptSerializer().Serialize(dnUser);
                    msg = _dnUserApi.Add(UserItem.AgencyID, json);
                    break;

                case ActionType.Edit:
                    UpdateModel(dnUser);
                    if (!string.IsNullOrEmpty(date))
                        dnUser.StartDate = date.StringToDecimal(0);
                    if (!string.IsNullOrEmpty(birthDay))
                        dnUser.BirthDay = birthDay.StringToDecimal(0);
                    dnUser.UserId = GuiId.FirstOrDefault();
                    json = new JavaScriptSerializer().Serialize(dnUser);
                    msg = _dnUserApi.Update(UserItem.AgencyID, json);
                    break;
                case ActionType.Active:
                    var listInt = Request["listInt"];
                    var userId = Request["userId"];
                    msg = _dnUserApi.AddModuleUser(listInt, userId, UserItem.AgencyID);
                    break;

                case ActionType.Delete:
                    msg = _dnUserApi.Delete(json, Request["itemID"]);
                    break;
                case ActionType.RoleModule:
                    msg = _dnUserApi.UpdateModuleActive(GuiId.FirstOrDefault(), Request["chkActiveRoles"]);
                    break;
                case ActionType.Show:
                    var c = _dnUserApi.CheckinUser(ArrId.FirstOrDefault(), UserItem.AgencyID);
                    if (c == 1)
                    {
                        msg.Erros = false;
                        msg.Message = "Bạn đã chấm công thành công.";
                    }
                    break;
                case ActionType.Hide:
                    msg = _dnUserApi.ShowHide(GuiId.FirstOrDefault(), true);
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
