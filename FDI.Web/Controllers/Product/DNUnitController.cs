using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNUnitController : BaseController
    {

        private readonly DNUnitAPI _api = new DNUnitAPI();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new DNUnitItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetUnitItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            return View(model);
        }

        public string CheckByName(string Name, int id)
        {
            var result = _api.CheckByName(Name, id, UserItem.AgencyID);
            return result == 1 ? "false" : "true";
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.AddUnit(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;

                case ActionType.Edit:
                    if (_api.UpdateUnit(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;
	            case ActionType.Delete:
		            if (_api.DeleteUnit(url) == 0)
		            {
			            msg.Erros = true;
			            msg.Message = "Cập nhật thất bại.";
		            }
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
