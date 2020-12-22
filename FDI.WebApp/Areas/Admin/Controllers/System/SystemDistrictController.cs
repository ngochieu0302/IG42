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
    public class SystemDistrictController : BaseController
    {
        readonly DistrictDA _districtDa = new DistrictDA("#");
        readonly CityDA _cityDa = new CityDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelDistrictItem
            {
                ListItem = _districtDa.GetListbyRequest(Request),
                PageHtml = _districtDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var districtModel = new System_District
            {
                IsShow = true
            };

            if (DoAction == ActionType.Edit)
                districtModel = _districtDa.GetById(ArrId.FirstOrDefault());

            ViewBag.DistrictCityID = _cityDa.GetAll();
            ViewData.Model = districtModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var district = new System_District();
            List<System_District> ltsDistrictItems;
            StringBuilder stbMessage;

            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(district);
                    _districtDa.Add(district);
                    _districtDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = district.ID.ToString(),
                        Message = string.Format("Đã thêm mới quận huyện: <b>{0}</b>", Server.HtmlEncode(district.Name))
                    };
                    break;

                case ActionType.Edit:
                    district = _districtDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(district);
                    _districtDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = district.ID.ToString(),
                        Message = string.Format("Đã cập nhật quận huyện: <b>{0}</b>", Server.HtmlEncode(district.Name))
                    };
                    break;

                case ActionType.Delete:
                    ltsDistrictItems = _districtDa.GetByListArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsDistrictItems)
                    {
                        _districtDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa quận huyện <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _districtDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsDistrictItems = _districtDa.GetByListArrId(ArrId).Where(o => !o.IsShow).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsDistrictItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị quận huyện <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _districtDa.Save();
                    msg.ID = string.Join(",", ltsDistrictItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsDistrictItems = _districtDa.GetByListArrId(ArrId).Where(o => o.IsShow).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsDistrictItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn quận huyện <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _districtDa.Save();
                    msg.ID = string.Join(",", ltsDistrictItems.Select(o => o.ID));
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

        [HttpPost]
        public ActionResult GetDistrictByCity(int cityId)
        {
            var list = _districtDa.GetListByCity(cityId, true);
            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _districtDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }
    }
}
