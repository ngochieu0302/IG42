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
    public class ProductSizeController : BaseController
    {
        readonly ProductSizeAPI _productSizeApi;
        public ProductSizeController()
        {
            _productSizeApi = new ProductSizeAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_productSizeApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var productSize = _productSizeApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = productSize;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var productSize = new ProductSizeItem();
            if (DoAction == ActionType.Edit)
            {
                productSize = _productSizeApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewData.Model = productSize;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var productSize = new ProductSizeItem();
            List<ProductSizeItem> ltsproductSize;
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(productSize);
                        productSize.AgencyID = UserItem.AgencyID;
                        json = new JavaScriptSerializer().Serialize(productSize);
                        _productSizeApi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = productSize.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(productSize.Name))
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
                        UpdateModel(productSize);
                        productSize.AgencyID = UserItem.AgencyID;
                        json = new JavaScriptSerializer().Serialize(productSize);
                        _productSizeApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = productSize.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(productSize.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    var check = _productSizeApi.Delete(lst1);
                    if (check == 1)
                    {
                        msg.Erros = false;
                        msg.Message = "Đã xóa size";
                    }
                    else
                    {
                        msg.Erros = true;
                        msg.Message = "Lỗi khi xóa size";
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
