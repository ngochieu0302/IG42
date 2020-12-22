using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Utils;
using FDI.Simple;

namespace FDI.Web.Controllers
{
	public class ShopProductValueController : BaseController
	{
		private readonly ProductValueAPI _api = new ProductValueAPI();
		private readonly DNUnitAPI _dnUnit = new DNUnitAPI();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ListItems()
		{
			var model = _api.ListItems(UserItem.AgencyID, Request.Url.Query);
			return View(model);
		}
		public ActionResult AjaxView()
		{
			return View();
		}
		public ActionResult AjaxForm()
		{
			var model = new ShopProductValueItem();
			if (DoAction == ActionType.Edit)
				model = _api.GetProductValueItem(ArrId.FirstOrDefault());
			ViewBag.Unit = _dnUnit.GetListUnit(UserItem.AgencyID);
			ViewBag.Action = DoAction;
			ViewBag.AgencyId = UserItem.AgencyID;
			return View(model);
		}
		public ActionResult Auto()
		{
			var query = Request["query"];
			var ltsResults = _api.GetListAuto(query, 10, UserItem.AgencyID);
			var resulValues = new AutoCompleteProduct
			{
				query = query,
				suggestions = ltsResults,
			};
			return Json(resulValues, JsonRequestBehavior.AllowGet);
		}

        public string CheckByName(string Name, int id)
        {
            var slug = FomatString.Slug(Name);
            var result = _api.CheckByName(slug, id, UserItem.AgencyID);
            return result == 1 ? "false" : "true";
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
					msg.Message = "Cập nhật dữ liệu thành công !";
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
					var lst1 = string.Join(",", ArrId);
					_api.Delete(lst1);
					msg.Message = "Cập nhật thành công.";
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
