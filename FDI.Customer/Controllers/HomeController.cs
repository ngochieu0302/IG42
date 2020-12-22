using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Customer.Controllers
{
    public class HomeController : BaseController
    {
        readonly CustomerAPI _api = new CustomerAPI();
        private readonly DNMailSSCAPI _dnMailSscapi = new DNMailSSCAPI();
        public ActionResult Index()
        {
            var model = _api.GetCustomerItem(UserItem.ID);
            ViewBag.City = _api.GetListCity();
            ViewBag.District = _api.GetListDistrict(model.CityID ?? 0);

            return View(model);
        }
        public ActionResult GetDistrict(int cityId)
        {
            var model = _api.GetListDistrict(cityId);
            return Json(model);
        }
        public ActionResult Infomation()
        {
            return PartialView(UserItem);
        }
        public ActionResult Menu()
        {
            return PartialView();
        }

        public ActionResult AlertEmail()
        {
            var model = new ModelDNMailSSCItem
            {
                TotalMailInbox = _dnMailSscapi.CustomerCountInboxNew(UserItem.ID, 1).Count
            };
            return View(model);
        }
    }
}
