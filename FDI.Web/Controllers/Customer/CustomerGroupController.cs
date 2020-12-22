using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CustomerGroupController : BaseController
    {

        private readonly CustomerGroupAPI _api = new CustomerGroupAPI();
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
            var model = new CustomerGroupItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetCustomerGroupItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url, UserItem.AgencyID);
                    break;

                case ActionType.Edit:
                    msg = _api.Update(url, UserItem.AgencyID);
                    break;
                case ActionType.Delete:
                    msg = _api.Delete(string.Join(",", ArrId));
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
