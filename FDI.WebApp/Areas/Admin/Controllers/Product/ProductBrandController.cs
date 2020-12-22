using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class ProductBrandController : BaseController
    {
        readonly Shop_BrandDA _brandDa = new Shop_BrandDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var listitem = _brandDa.GetListSimpleByRequest(Request);
            var model = new ModelBrandItem
            {
                Container = Request["Container"],
                ListItem = listitem,
                PageHtml = _brandDa.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var brandModel = _brandDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = brandModel;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var brandModel = new Shop_Brand();

            if (DoAction == ActionType.Edit)
                brandModel = _brandDa.GetById(ArrId.FirstOrDefault());

            ViewData.Model = brandModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var brand = new Shop_Brand();
            List<int> idValues;
            List<string> idValuesTag;
            List<Shop_Brand> brandSelected;

            var pictureId = Request["Value_DefaultImages"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(brand);

                        if (!string.IsNullOrEmpty(pictureId))
                            brand.PictureID = int.Parse(pictureId);
                        brand.IsDeleted = false;
                        brand.ParentID = 1;
                        brand.LanguageId = Fdisystem.LanguageId;
                        brand.NameAscii = FDIUtils.Slug(brand.Name);

                        _brandDa.Add(brand);
                        _brandDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = brand.ID.ToString(),
                            Message = string.Format("Đã thêm mới <b>{0}</b>", Server.HtmlEncode(brand.Name))
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
                        brand = _brandDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(brand);
                        brand.NameAscii = FDIUtils.Slug(brand.Name);
                        if (!string.IsNullOrEmpty(pictureId))
                            brand.PictureID = int.Parse(pictureId);
                        else
                            brand.PictureID = null;

                        _brandDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = brand.ID.ToString(),
                            Message = string.Format("Đã cập nhật <b>{0}</b>", Server.HtmlEncode(brand.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                case ActionType.Show:
                    var ltsBrandItemsshow = _brandDa.GetListByArrId(ArrId);
                    var stbMessageshow = new StringBuilder();
                    foreach (var item in ltsBrandItemsshow)
                    {
                        item.IsShow = true;
                        stbMessageshow.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _brandDa.Save();
                    msg.Message = stbMessageshow.ToString();
                    break;

                case ActionType.Hide:
                    var ltsBrandItemshide = _brandDa.GetListByArrId(ArrId);
                    var stbMessagehide = new StringBuilder();
                    foreach (var item in ltsBrandItemshide)
                    {
                        item.IsShow = false;
                        stbMessagehide.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _brandDa.Save();
                    msg.Message = stbMessagehide.ToString();
                    break;
                case ActionType.Delete:
                    var ltsBrandItems = _brandDa.GetListByArrId(ArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in ltsBrandItems)
                    {
                        if (item.Shop_Product.Any())
                        {
                            stbMessage.AppendFormat("<b>{0}</b> đang được sử dụng, không được phép xóa.<br />", Server.HtmlEncode(item.Name));
                        }
                        else
                        {
                            _brandDa.Delete(item);
                            stbMessage.AppendFormat("Đã xóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _brandDa.Save();
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
