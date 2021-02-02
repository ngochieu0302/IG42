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
            var temp = new SupplierAmountProductFormItem();
            try
            {
                switch (DoAction)
                {
                    case ActionType.Add:
                        
                        
                        UpdateModel(temp);
                        var request = new SupplierAmountProductItem()
                        {
                            SupplierId = temp.SupplierId ?? 1,
                            ProductID = temp.ProductID,
                            PublicationDate = temp._PublicationDate.StringToDecimal(),
                            AmountEstimate = temp.AmountEstimate ?? 0,
                            AmountPayed = temp.AmountPayed ?? 0,
                            CallDate = temp._CallDate.StringToDecimal(),
                            ExpireDate = temp._ExpireDate.StringToDecimal(),
                            IsAlwayExist = temp.IsAlwayExist == true,
                            Note = temp.Note,
                            UserActiveId = UserItem.UserId,
                        };
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
                        UpdateModel(temp);
                        var requestUpdate = new SupplierAmountProductItem()
                        {
                            SupplierId = temp.SupplierId ?? 1,
                            ProductID = temp.ProductID,
                            PublicationDate = temp._PublicationDate.StringToDecimal(),
                            AmountEstimate = temp.AmountEstimate ?? 0,
                            AmountPayed = temp.AmountPayed ?? 0,
                            CallDate = temp._CallDate.StringToDecimal(),
                            ExpireDate = temp._ExpireDate.StringToDecimal(),
                            IsAlwayExist = temp.IsAlwayExist == true,
                            Note = temp.Note,
                            UserActiveId = UserItem.UserId,
                        };
                        UpdateModel(requestUpdate);

                        await _apiSupplierAmount.Update(requestUpdate);

                        msg.Message = "Cập nhật dữ liệu thành công !";
                        break;
                    case ActionType.Delete:
                        await _apiSupplierAmount.Delete(ArrId.FirstOrDefault());
                        msg.Message = "Cập nhật thành công.";
                        break;
                }
            }
            catch (Exception e)
            {
               
                    msg.Erros = true;
                    msg.Message = e.ToString();
            }
           
           
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}