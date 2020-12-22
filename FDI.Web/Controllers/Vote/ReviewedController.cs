using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Web.Controllers
{
    public class ReviewedController : BaseController
    {
        private readonly ContentVoteAPI _api = new ContentVoteAPI();
        private readonly DNUserAPI _userapi = new DNUserAPI();
        private readonly VoteAPI _voteapi = new VoteAPI();

        public ActionResult Index()
        {
            var model = new ModelContentVoteItem
            {               
                VoteItems = _voteapi.GetListSimple(UserItem.AgencyID)
            };
            return View(model);
        }
        
        public ActionResult ListItems()
        {
            return View(_api.ListItemsUser(UserItem.AgencyID,UserItem.UserId, Request.Url.Query));
        } 
    }
}
