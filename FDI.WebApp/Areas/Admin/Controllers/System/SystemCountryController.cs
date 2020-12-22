using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class SystemCountryController : BaseController
    {
        readonly System_CountryDA _countryDa = new System_CountryDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelCountryItem
            {
                ListItem = _countryDa.GetListSimpleByRequest(Request),
                PageHtml = _countryDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var countryModel = _countryDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = countryModel;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var countryModel = new System_Country
            {
                Show = true
            };
            if (DoAction == ActionType.Edit)
                countryModel = _countryDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = countryModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var country = new System_Country();
            List<System_Country> ltsCountryItems;
            StringBuilder stbMessage;

            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(country);
                    country.IsDelete = false;
                    _countryDa.Add(country);
                    _countryDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = country.ID.ToString(),
                        Message = string.Format("Đã thêm mới quốc gia: <b>{0}</b>", Server.HtmlEncode(country.Name))
                    };
                    break;

                case ActionType.Edit:
                    country = _countryDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(country);
                    _countryDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = country.ID.ToString(),
                        Message = string.Format("Đã cập nhật quốc gia: <b>{0}</b>", Server.HtmlEncode(country.Name))
                    };
                    break;

                case ActionType.Delete:
                    ltsCountryItems = _countryDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCountryItems)
                    {

                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa quốc gia <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));

                    }
                    msg.ID = string.Join(",", ArrId);
                    _countryDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsCountryItems = _countryDa.GetListByArrId(ArrId).Where(o => o.Show == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCountryItems)
                    {
                        item.Show = true;
                        stbMessage.AppendFormat("Đã hiển thị quốc gia <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _countryDa.Save();
                    msg.ID = string.Join(",", ltsCountryItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsCountryItems = _countryDa.GetListByArrId(ArrId).Where(o => o.Show == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCountryItems)
                    {
                        item.Show = false;
                        stbMessage.AppendFormat("Đã ẩn quốc gia <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _countryDa.Save();
                    msg.ID = string.Join(",", ltsCountryItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _countryDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }
    }
}
