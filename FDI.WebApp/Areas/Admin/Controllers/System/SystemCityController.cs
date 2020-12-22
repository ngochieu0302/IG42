using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class SystemCityController : BaseController
    {
        readonly CityDA _cityDa = new CityDA("#");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelCityItem
            {
                ListItem = _cityDa.GetListbyRequest(Request),
                PageHtml = _cityDa.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var cityModel = new System_City
            {
                IsShow = true
            };
            if (DoAction == ActionType.Edit)
                cityModel = _cityDa.GetById(ArrId.FirstOrDefault());

            ViewData.Model = cityModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var city = new System_City();
            List<System_City> ltsCityItems;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(city);
                    _cityDa.Add(city);
                    city.LanguageID = Fdisystem.LanguageId;
                    _cityDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = city.ID.ToString(),
                        Message = string.Format("Đã thêm mới thành phố: <b>{0}</b>", Server.HtmlEncode(city.Name))
                    };
                    break;
                case ActionType.Edit:
                    city = _cityDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(city);
                    //city.LanguageID = Fdisystem.LanguageId;
                    _cityDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = city.ID.ToString(),
                        Message = string.Format("Đã cập nhật thành phố: <b>{0}</b>", Server.HtmlEncode(city.Name))
                    };
                    break;
                case ActionType.Delete:
                    ltsCityItems = _cityDa.GetByListArrId(ArrId.ToString());
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCityItems)
                    {
                        if (item.System_District.Any())
                        {
                            stbMessage.AppendFormat("Thành phố <b>{0}</b> đang được sử dụng, không được phép xóa.<br />", Server.HtmlEncode(item.Name));
                        }
                        else
                        {
                            _cityDa.Delete(item);
                            stbMessage.AppendFormat("Đã xóa thành phố <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _cityDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsCityItems = _cityDa.GetByListArrId(ArrId.ToString()).Where(o => !o.IsShow).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCityItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị thành phố <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _cityDa.Save();
                    msg.ID = string.Join(",", ltsCityItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsCityItems = _cityDa.GetByListArrId(ArrId.ToString()).Where(o => o.IsShow).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCityItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn thành phố <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _cityDa.Save();
                    msg.ID = string.Join(",", ltsCityItems.Select(o => o.ID));
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
            var ltsResults = _cityDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }
    }
}
