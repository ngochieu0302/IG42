using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class InventoryLaterProductController : BaseController
    {
        readonly StorageProductAPI _api = new StorageProductAPI();
        readonly ProductAPI _productApi = new ProductAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_api.GetListProductLater(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var model = _productApi.GetProductItem(ArrId.FirstOrDefault());
            return View(model);
        }
    }
}