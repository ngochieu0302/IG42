using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class UserRewardController : BaseController
    {
        // GET: /Admin/Order/       
        readonly CustomerRewardAPI _api = new CustomerRewardAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_api.GetListRewardRequest(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            return View();
        } 
    }
}
