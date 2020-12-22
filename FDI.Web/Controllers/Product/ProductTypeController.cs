using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ProductTypeController : BaseController
    {
        readonly ProductTypeAPI _productTypeApi;
        public ProductTypeController()
        {
            _productTypeApi = new ProductTypeAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_productTypeApi.ListItems(Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var productType = _productTypeApi.GetItemById(ArrId.FirstOrDefault());
            ViewData.Model = productType;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var productType = new ProductTypeItem();
            if (DoAction == ActionType.Edit)
            {
                productType = _productTypeApi.GetItemById(ArrId.FirstOrDefault());
            }
            ViewData.Model = productType;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var productType = new ProductTypeItem();
            List<ProductTypeItem> ltsproductType;
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(productType);
                       
                        json = new JavaScriptSerializer().Serialize(productType);
                        _productTypeApi.Add(json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = productType.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(productType.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        UpdateModel(productType);
                       
                        json = new JavaScriptSerializer().Serialize(productType);
                        _productTypeApi.Update(json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = productType.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(productType.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    var check = _productTypeApi.Delete(lst1);
                    if (check == 1)
                    {
                        msg.Erros = false;
                        msg.Message = "Đã xóa Type";
                    }
                    else
                    {
                        msg.Erros = true;
                        msg.Message = "Lỗi khi xóa Type";
                    }
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
