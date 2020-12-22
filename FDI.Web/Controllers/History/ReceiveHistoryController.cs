using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class ReceiveHistoryController : BaseController
    {
        // GET: /Admin/Order/       
        readonly ReceiveHistoryAPI _api = new ReceiveHistoryAPI();
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
	        var model = _api.GetDetailByOrderById(ArrId.FirstOrDefault());
			return View(model);
		} 
    }
}
