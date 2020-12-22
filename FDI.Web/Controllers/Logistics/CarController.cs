using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.GetAPI.Supplier;
using FDI.Simple;
using FDI.Simple.Logistics;
using FDI.Simple.Supplier;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CarController : BaseController
    {
        //
        // GET: /Supplie/
        readonly CategoryAPI _categoryApi = new CategoryAPI();
        readonly SupplieAPI _api = new SupplieAPI();
        readonly SupplierAmountProductApi _apiSupplierAmount = new SupplierAmountProductApi();
        private  readonly CarApi _carApi =new CarApi();
        private  readonly  DNUnitAPI _dnUnitApi = new DNUnitAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_carApi.ListItems(Request.Url.Query));
        }
        public async Task<ActionResult> AjaxForm()
        {
            var lstCate = _dnUnitApi.GetAllList();
            ViewBag.lstCate = lstCate;
            var model = new CarItem()
            {
              
            };
            if (DoAction == ActionType.Edit)
                model = await _carApi.GetById(ArrId.FirstOrDefault());
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
                    var request = new CarItem();
                    UpdateModel(request);
                    var result = await _carApi.Add(request);
                    if (result.Erros)
                    {
                        msg.Erros = true;
                        msg.Message = result.Message;
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    var requestUpdate = new CarItem();
                    UpdateModel(requestUpdate);

                    await _carApi.Update(requestUpdate);
                   
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Delete:
                    await _carApi.Delete(ArrId.FirstOrDefault());
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