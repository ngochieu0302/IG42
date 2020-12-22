using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Vote
{
    public class LevelVoteController : BaseController
	{
		private readonly LevelVoteAPI _api = new LevelVoteAPI();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ListItems()
		{
			var model = _api.ListItems(Request.Url.Query);
			return View(model);
		}
		public ActionResult AjaxForm()
		{
			var model = new LevelVoteItem();
			if (DoAction == ActionType.Edit)
				model = _api.GetLevelVoteItem(ArrId.FirstOrDefault());
			ViewBag.Action = DoAction;
			ViewBag.AgencyID = UserItem.AgencyID;
			return View(model);
		}

		public ActionResult AjaxView()
		{
			var customerType = _api.GetLevelVoteItem(ArrId.FirstOrDefault());
			ViewData.Model = customerType;
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Actions()
		{
			var msg = new JsonMessage();
			var url = Request.Form.ToString();
			switch (DoAction)
			{
				case ActionType.Add:
                    msg = _api.Add(url);
					break;

				case ActionType.Edit:
                    msg = _api.Update(url);
					break;

				case ActionType.Delete:
					var lst1 = string.Join(",", ArrId);
                    msg = _api.Delete(lst1);					
					break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không được phân quyền cho chức năng này.";
                    break;
			}			
			return Json(msg, JsonRequestBehavior.AllowGet);
		}

	}
}
