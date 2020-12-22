using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using FDI.Web;

namespace FDI.Areas.Admin.Controllers
{
    [LogActionFilter]
    public class ProductSizeController : BaseController
    {
        ProductSizeDA _da = new ProductSizeDA("#");
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelProductSizeItem()
            {
                Container = Request["Container"],
                ListItem = _da.GetListSimpleByRequest(Request, 2010),
                PageHtml = _da.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new Product_Size();
            if (DoAction == ActionType.Edit)
                model = _da.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var product = new Product_Size();
            List<Shop_Product> lstProduct;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(product);
                        //product.AgencyID = 2010;
                        _da.Add(product);
                        _da.Save();

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = product.ID.ToString(),
                            Message = string.Format("Đã thêm mới sản phẩm: <b>{0}</b>", Server.HtmlEncode(product.Name))
                        };
                    }
                    catch (Exception)
                    {
                    }
                    break;
                case ActionType.Edit:
                    try
                    {
                        product = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(product);
                        product.Name = Convert.ToString(product.Name);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = product.ID.ToString(),
                            Message = string.Format("Đã cập nhật sản phẩm: <b>{0}</b>", Server.HtmlEncode(product.Name))
                        };
                    }
                    catch (Exception)
                    {
                        //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
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
