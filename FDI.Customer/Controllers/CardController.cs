using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Customer.Controllers
{
    public class CardController : BaseController
    {
        //
        // GET: /Card/
        readonly CustomerAPI _api = new CustomerAPI();
        public ActionResult Index()
        {
            var model = _api.GetListByCustomer(UserItem.ID);
            return View(model);
        }
        public ActionResult AddCard(string card, string code)
        {
            var json = _api.AddCard(UserItem.ID, card, code);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
