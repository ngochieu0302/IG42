using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class SetupProductionController : BaseController
    {
        private readonly SetupProductionAPI _api = new SetupProductionAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListSimple(Request.Url.Query,UserItem.AgencyID);
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new SetupProductionItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetSetupProductionItem(ArrId.FirstOrDefault());          
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            ViewBag.UserCreate = UserItem.UserId;
            return View(model);
        }
        public ActionResult Auto(int type)
        {
            var query = Request["query"];
            var ltsResults = _api.GetListAuto(query, 10, UserItem.AgencyID, type);
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
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url);                    
                    break;
                case ActionType.Edit:
                    msg = (_api.Update(url));
                    break;
                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    _api.Delete(lst1);
                    msg = _api.Delete(lst1);
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
