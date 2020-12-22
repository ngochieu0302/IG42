using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.GetAPI.Supplier;
using FDI.Simple;
using FDI.Simple.Supplier;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class SupplierAmountProductController : BaseController
    {
        //
        // GET: /Supplie/
        readonly CategoryAPI _categoryApi = new CategoryAPI();
        readonly SupplieAPI _api = new SupplieAPI();
        readonly SupplierAmountProductApi _apiSupplierAmount = new SupplierAmountProductApi();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_apiSupplierAmount.ListItems(Request.Url.Query));
        }
        public async Task<ActionResult> AjaxForm()
        {
            var lstCate = _categoryApi.GetChildByParentId(false);
            ViewBag.lstCate = lstCate;
            var model = new SupplierAmountProductItem()
            {
                ExpireDate = DateTime.Now.TotalSeconds(),
                PublicationDate = DateTime.Now.TotalSeconds(),
                CallDate = DateTime.Now.TotalSeconds()
            };
            ViewBag.lstSupplier = _api.GetList();
            if (DoAction == ActionType.Edit)
                model = await _apiSupplierAmount.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            var lstID = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    var request = new SupplierAmountProductItem();
                    UpdateModel(request);
                    var result = await _apiSupplierAmount.Add(request);
                    if (result.Erros)
                    {
                        msg.Erros = true;
                        msg.Message = result.Message;
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    var requestUpdate = new SupplierAmountProductItem();
                    UpdateModel(requestUpdate);

                    await _apiSupplierAmount.Update(requestUpdate);
                   
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Delete:
                    await _apiSupplierAmount.Delete(ArrId.FirstOrDefault());
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