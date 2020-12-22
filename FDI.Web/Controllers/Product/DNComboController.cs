using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
	public class DNComboController : BaseController
	{
		readonly DNComboAPI _api = new DNComboAPI();
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
			var model = _api.GetComboItem(ArrId.FirstOrDefault());
			return View(model);
		}
		public ActionResult AjaxForm()
		{
			var model = new DNComboItem();
			if (DoAction == ActionType.Edit)
				model = _api.GetComboItem(ArrId.FirstOrDefault());
			ViewBag.Action = DoAction;
			ViewBag.AgencyId = UserItem.AgencyID;
			return View(model);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Actions()
		{
			var msg = new JsonMessage { Erros = false };
			var url = Request.Form.ToString();
			switch (DoAction)
			{
				case ActionType.Add:
					if (_api.Add(url) == 0)
					{
						msg.Erros = true;
						msg.Message = "Có lỗi xảy ra!";
					}
					msg.Message = "Thêm dữ liệu thành công !";
					break;
				case ActionType.Edit:
					if (_api.Update(url) == 0)
					{
						msg.Erros = true;
						msg.Message = "Có lỗi xảy ra!";
					}
					msg.Message = "Cập nhật dữ liệu thành công !";
					break;
				case ActionType.Delete:
					var lst = string.Join(",", ArrId);
					_api.Delete(lst);
					msg.Message = "Đã xóa combo!";
					break;
			}
			if (string.IsNullOrEmpty(msg.Message))
			{
				msg.Message = "Không có hành động nào được thực hiện.";
				msg.Erros = true;
			}
			return Json(msg, JsonRequestBehavior.AllowGet);
		}
	}
}