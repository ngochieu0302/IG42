using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class SupplierProductController : BaseController
    {
        //
        // GET: /Supplie/
        readonly CategoryAPI _categoryApi = new CategoryAPI();
        readonly SupplieAPI _api = new SupplieAPI();
        public ActionResult Index(int supplierId)
        {
            ViewBag.id = supplierId;
            return View();
        }
        public ActionResult ListItems(int supplierId)
        {
            return View(_api.GetListSupplierProductById(supplierId, Request.Url.Query));
        }
        public ActionResult AjaxForm(int supplierId)
        {
            var lstCate = _categoryApi.GetChildByParentId(false);
            ViewBag.lstCate = lstCate;
            ViewBag.supplierId = supplierId;
            var model = new SupplieProductItem()
            {
                SupplierId = supplierId
            };


            if (DoAction == ActionType.Edit)
                model = _api.GetSupplierProductById(ArrId.FirstOrDefault());
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
                    var request = new SupplieProductItem();
                    UpdateModel(request);
                    var result = await _api.AddProduct(request);
                    if (result.Erros)
                    {
                        msg.Erros = true;
                        msg.Message = result.Message;
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.Update(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Show:
                    if (_api.Show(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    _api.Show(lstID);
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Hide:
                    if (_api.Hide(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    _api.Hide(lstID);
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Delete:
                    _api.Delete(lstID);
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