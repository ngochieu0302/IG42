using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.GetAPI.QLCN;
using FDI.Simple;
using FDI.Simple.QLCN;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class MenCNController : BaseController
    {
        //
        // GET: /MenCN/

        private readonly MenCNAPI _api = new MenCNAPI();
        readonly DNUnitAPI _unitApi = new DNUnitAPI();
        readonly NguyenlieuCNAPI _nguyenlieuCnapi = new NguyenlieuCNAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new MenCNItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetMenCNItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.lstUnit = _unitApi.GetListUnit(UserItem.AgencyID);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(url,CodeLogin()) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;

                case ActionType.Edit:
                    if (_api.Update(url,CodeLogin()) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;
                case ActionType.Delete:
                    if (_api.Delete(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoNguyenlieu()
        {
            var query = Request["query"];
            var ltsResults = _nguyenlieuCnapi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
    }
}
