using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class InventoryLaterValueController : BaseController
    {
        private readonly DNImportAPI _api = new DNImportAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = _api.GetListValueLater(UserItem.AgencyID, Request.Url.Query);
            return View(model);
        }
    }
}
