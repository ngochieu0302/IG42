using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class VoteController : BaseController
    {
        private readonly VoteAPI _api = new VoteAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListSimple(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new VoteItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetVoteItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.AgencyID = UserItem.AgencyID;
            return View(model);
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
                    msg = _api.Update(url);
                    break;

                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    _api.Delete(lst1);
                    msg.Message = "Cập nhật thành công.";
                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }            
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
