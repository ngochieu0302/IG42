using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Utils;
using FDI.Simple;

namespace FDI.Web.Controllers
{
    public class InventoryValueController : BaseController
    {
        private readonly DNImportAPI _api = new DNImportAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = _api.GetListValueByRequest(UserItem.AgencyID, Request.Url.Query);
            return View(model);
        }

        public ActionResult AjaxView()
        {

            var model = _api.GetListValueView(UserItem.AgencyID, ArrId.FirstOrDefault());
            return View(model);
        }
    }
}
