using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class GeneralVouchersController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ReceiptPaymentAPI _api = new ReceiptPaymentAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.GetListUserRp(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetReceiptPaymentItem(ArrId.FirstOrDefault());
            return View(model);
        }
    }
}
