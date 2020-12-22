using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CostTypeController : BaseController
    {
        private readonly CostTypeAPI _api = new CostTypeAPI();
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
            var model = new CostTypeItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetCostTypeItem(ArrId.FirstOrDefault());
            ViewBag.Type = Request["type"] != null ? int.Parse(Request["type"]) : -1;
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
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công !"};
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:

                    if (_api.Add(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    break;
                case ActionType.Edit:
                    if (_api.Update(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    break;

                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    _api.Delete(lst1);
                    if (_api.Delete(lst1) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
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
