using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using FDI.Web;

namespace FDI.Areas.Admin.Controllers
{
    [LogActionFilter]
    public class ProductController : BaseController
    {
        readonly ProductDA _productDa = new ProductDA("#");
        readonly CategoryDA _categoryDa = new CategoryDA("#");
        public ActionResult Index()
        {
            var model = _categoryDa.GetChildByParentId(false);
            return View(model);
        }
        public ActionResult ListItems()
        {
            var model = new ModelProductItem
            {
                Container = Request["Container"],
                ListItem = _productDa.GetListSimpleByRequest(Request,2010),
                PageHtml = _productDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxView()
        {
            var productModel = _productDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = productModel;
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new Shop_Product();
            if (DoAction == ActionType.Edit)
                model = _productDa.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var product = new Shop_Product();
            List<Shop_Product> lstProduct;
            StringBuilder stbMessage;
            var lsttag = Request["values-arr-tag"];
            var lstCate = Request["Value_CategoryValues"];
            var images = Request["Value_Images"];
            var lstimages = Request["Value_ImagesProducts"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(product);
                        product.CreateDate = DateTime.Now.TotalSeconds();
                        product.IsDelete = false;
                        product.CreateBy = User.Identity.Name;
                        product.LanguageId = Fdisystem.LanguageId;
                      
                        _productDa.Add(product);
                        _productDa.Save();

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = product.ID.ToString(),
                            Message = string.Format("Đã thêm mới sản phẩm: <b>{0}</b>", Server.HtmlEncode(product.Shop_Product_Detail.Name))
                        };
                    }
                    catch (Exception)
                    {
                    }
                    break;
                case ActionType.Edit:
                    try
                    {
                        product = _productDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(product);
                        product.IsDelete = false;
                        _productDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = product.ID.ToString(),
                            Message = string.Format("Đã cập nhật sản phẩm: <b>{0}</b>", Server.HtmlEncode(product.Shop_Product_Detail.Name))
                        };
                    }
                    catch (Exception)
                    {
                        //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    break;

                case ActionType.Delete:
                    lstProduct = _productDa.GetListArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lstProduct)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Shop_Product_Detail.Name));

                    }
                    msg.ID = string.Join(",", ArrId);
                    _productDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    lstProduct = _productDa.GetListArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lstProduct)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Shop_Product_Detail.Name));
                    }
                    _productDa.Save();
                    msg.ID = string.Join(",", lstProduct.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    lstProduct = _productDa.GetListArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lstProduct)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Shop_Product_Detail.Name));
                    }
                    _productDa.Save();
                    msg.ID = string.Join(",", lstProduct.Select(o => o.ID));
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
        
    }
}
