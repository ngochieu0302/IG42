using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DepartmentController : BaseController
    {
        readonly DepartmentAPI _departmentApi;
        public DepartmentController()
        {
            _departmentApi = new DepartmentAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_departmentApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var customerType = _departmentApi.GetDepartmentItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = customerType;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var department = new DepartmentItem();
            if (DoAction == ActionType.Edit)
            {
                department = _departmentApi.GetDepartmentItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewData.Model = department;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var department = new DepartmentItem();
            var lstDepartment = new List<DepartmentItem>();
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(department);
                    department.IsDelete = false;
                    json = new JavaScriptSerializer().Serialize(department);
                    msg = _departmentApi.Add(UserItem.AgencyID, json);
                    break;

                case ActionType.Edit:
                    UpdateModel(department);
                    department.IsDelete = false;
                    json = new JavaScriptSerializer().Serialize(department);
                    msg = _departmentApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());
                    break;

                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    msg = _departmentApi.Delete(lst1);
                    break;

                default:
                    msg.Message = "Bạn không được phân quyền cho chức năng này.";
                    msg.Erros = true;                    
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
