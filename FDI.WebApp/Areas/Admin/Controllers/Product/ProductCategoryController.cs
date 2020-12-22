using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FDI.Areas.Admin.Controllers;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        ShopProductDetailDA _da = new ShopProductDetailDA("#");
        System_ColorDA _colorDa = new System_ColorDA("#");
        ProductSizeDA _productSizeDa = new ProductSizeDA("#");
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelShopProductDetailItem()
            {
                Container = Request["Container"],
                ListItem = _da.GetListSimpleByRequest(Request,Utility.AgencyId),
                PageHtml = _da.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new Shop_Product_Detail();
            if (DoAction == ActionType.Edit)
                model = _da.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
           
            return View(model);
        }
        public ActionResult AjaxFormAdd()
        {
            var productId = Convert.ToInt32(Request.QueryString["productId"]);
            var model = _da.GetById(productId);
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            ViewBag.listcolor = _colorDa.GetAll(2010);
            ViewBag.listSize = _productSizeDa.GetAll(2010);
            return View(model);
        }
        public ActionResult GetListColor()
        {
            var model = _colorDa.GetAll(2010);
            return View(model);
        }
        public ActionResult GetListSize()
        {
            var model = _productSizeDa.GetAll(2010);
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var productModel = _da.GetById(ArrId.FirstOrDefault());
            ViewData.Model = productModel;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var product = new Shop_Product_Detail();
            List<Shop_Product_Detail> lstProduct;
            StringBuilder stbMessage;

            var lsttag = Request["values-arr-tag"];
            var images = Request["Value_Images"];
            var lstCate = Request["Value_CategoryValues"];
            var lstimages = Request["Value_ImagesProducts"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(product);
                        product.DateCreate = DateTime.Now.TotalSeconds();
                        product.IsDelete = false;
                        //product.AgencyID = Utility.AgencyId;
                        product.IsShow = true;
                        //product.LanguageId = Fdisystem.LanguageId;
                        if (!string.IsNullOrEmpty(images))
                            product.PictureID = Convert.ToInt32(images);
                        if (!string.IsNullOrEmpty(lstCate))
                        {
                            product.Categories = _da.GetListCateByArrId(lstCate);
                        }
                        if (!string.IsNullOrEmpty(lstimages))
                        {
                            product.Gallery_Picture2 = _da.GetListPictureByArrId(lstimages);
                        }
                        if (string.IsNullOrEmpty(lsttag))
                        {
                            product.System_Tag = _da.GetListIntTagByArrId(lsttag);
                        }
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
                        if (!string.IsNullOrEmpty(images))
                            product.PictureID = Convert.ToInt32(images);
                        product.Categories.Clear();
                        if (!string.IsNullOrEmpty(lstCate))
                        {
                            product.Categories = _da.GetListCateByArrId(lstCate);
                        }
                        product.Gallery_Picture2.Clear();
                        if (!string.IsNullOrEmpty(lstimages))
                        {
                            product.Gallery_Picture2 = _da.GetListPictureByArrId(lstimages);
                        }
                        product.System_Tag.Clear();
                        if (!string.IsNullOrEmpty(lsttag))
                        {
                            product.System_Tag = _da.GetListIntTagByArrId(lsttag);
                        }
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
                    }
                    break;
                case ActionType.Delete:
                    lstProduct = _da.GetListProductDetailByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lstProduct)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));

                    }
                    msg.ID = string.Join(",", ArrId);
                    _da.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    lstProduct = _da.GetListProductDetailByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lstProduct)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", lstProduct.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    lstProduct = _da.GetListProductDetailByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lstProduct)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", lstProduct.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.UserModule:
                    var model = _da.GetProductById(ArrId.FirstOrDefault());
                    product = _da.GetById(ArrId.FirstOrDefault());
                    var list = new List<Shop_Product>();
                    foreach (var item in model)
                    {
                        var name = Request["Size_old" + item.ID];
                        if (string.IsNullOrEmpty(name))
                        {
                            list.Add(item);
                        }
                        else
                        {
                            item.SizeID = int.Parse(name);
                            //item.SizeID = ConvertUtil.ToInt32(Request["Size_old" + item.ID]);
                            //item.PriceNew = ConvertUtil.ToDecimal(Request["PriceNew_old" + item.ID]);
                            //item.PriceOld = ConvertUtil.ToDecimal(Request["PriceOld_old" + item.ID]);
                        }
                    }
                    foreach (var item in list)
                    {
                        _da.DeleteProduct(item);
                    }
                    var stt = ConvertUtil.ToInt32(Request["do_stt"]);
                    for (int i = 1; i <= stt; i++)
                    {
                        var name = Request["Size_add_" + i];
                        if (!string.IsNullOrEmpty(name))
                        {
                            var obj = new Shop_Product()
                            {
                               
                                ProductDetailID = product.ID,
                                //PriceNew = ConvertUtil.ToDecimal(Request["PriceNew_add_" + i]),
                                //PriceOld = ConvertUtil.ToDecimal(Request["PriceOld_add_" + i]),
                                SizeID = int.Parse(name),
                                //SizeID = ConvertUtil.ToInt32(Request["Size_add_" + i]),
                                IsShow = true,
                                IsDelete = false,
                            };
                            _da.AddProduct(obj);
                        }
                    }
                    _da.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = product.ID.ToString(),
                        Message = string.Format("Đã thêm mới sản phẩm: <b>{0}</b>", Server.HtmlEncode(product.Name))
                    };
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string CheckByName(string Name, int productId)
        {
            var nameAscii = !string.IsNullOrEmpty(Name) ? Name : string.Empty;
            var result = _da.CheckName(nameAscii, productId);
            return result ? "false" : "true";
        }
        [HttpGet]
        public string CheckByNameAscii(string NameAscii, int productId)
        {
            var nameAscii = !string.IsNullOrEmpty(NameAscii) ? NameAscii : string.Empty;
            var result = _da.CheckNameAscii(nameAscii, productId);
            return result ? "false" : "true";
        }
    }
}