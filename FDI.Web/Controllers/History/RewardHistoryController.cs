using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class RewardHistoryController : BaseController
    {
        // GET: /Admin/Order/       
        readonly RewardHistoryAPI _api = new RewardHistoryAPI();
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
    }
}
