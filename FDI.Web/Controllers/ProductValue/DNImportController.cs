using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
	public class DNImportController : BaseController
	{
		private readonly DNImportAPI _api = new DNImportAPI();
		private readonly ProductValueAPI _valueApi = new ProductValueAPI();
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
			var model = _api.GetStorageItem(ArrId.FirstOrDefault());
			return View(model);
		}
		public ActionResult AjaxForm()
		{
			var model = new StorageItem();
			if (DoAction == ActionType.Edit)
				model = _api.GetStorageItem(ArrId.FirstOrDefault());			
			ViewBag.UserID = UserItem.UserId;
			ViewBag.User = UserItem.UserName;
			ViewBag.Action = DoAction;
			return View(model);
		}
        
        public ActionResult Auto()
		{
			var query = Request["query"];
			var ltsResults = _valueApi.GetListAuto(query, 10, UserItem.AgencyID);
			var resulValues = new AutoCompleteProduct
			{
				query = query,
				suggestions = ltsResults
			};
			return Json(resulValues, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Actions()
		{
			var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
			var url = Request.Form.ToString();
		    url = HttpUtility.HtmlDecode(url);
			switch (DoAction)
			{
				case ActionType.Add:
					if (_api.Add(url, UserItem.AgencyID, CodeLogin()) == 0)
					{
						msg.Erros = true;
						msg.Message = "Có lỗi xảy ra!";
					}
					break;
				case ActionType.Edit:
                    if (_api.Update(url, UserItem.AgencyID, CodeLogin()) == 0)
					{
						msg.Erros = true;
						msg.Message = "Có lỗi xảy ra!";
					}
					break;
				case ActionType.Delete:
					var lst1 = string.Join(",", ArrId);
					if (_api.Delete(lst1) == 0)
					{
						msg.Erros = true;
						msg.Message = "Có lỗi xảy ra!";
					}
					break;
				default:
					msg.Erros = true;
					msg.Message = "Bạn không có quyền thực hiện chứ năng này.";
					break;
			}
			return Json(msg, JsonRequestBehavior.AllowGet);
		}
	}
}
