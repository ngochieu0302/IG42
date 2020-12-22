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
    /// <summary>
    /// Class sử dụng cho quản lý màu sắc
    /// tangtc update 1-10-2015
    /// </summary>
    public class SystemColorController : BaseController
    {
        readonly System_ColorDA _colorDA = new System_ColorDA("#");

        /// <summary>
        /// Trang chủ, index. Load ra grid dưới dạng ajax
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            var model = new ModelColorItem
            {
                ListItem = _colorDA.GetListSimpleByRequest(Request,Utility.AgencyId),
                PageHtml = _colorDA.GridHtmlPage
            };
            return View(model);
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var colorModel = _colorDA.GetById(ArrId.FirstOrDefault());
            ViewData.Model = colorModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var colorModel = new System_Color();
            if (DoAction == ActionType.Edit)
                colorModel = _colorDA.GetById(ArrId.FirstOrDefault());

            ViewData.Model = colorModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }


        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var color = new System_Color();
            List<System_Color> ltsColorItems;
            StringBuilder stbMessage;

            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(color);
                    _colorDA.Add(color);
                    _colorDA.Save();
                    msg = new JsonMessage
                              {
                        Erros = false,
                        ID = color.ID.ToString(),
                        Message = string.Format("Đã thêm mới màu sắc: <b>{0}</b>", Server.HtmlEncode(color.Name))
                    };
                    break;

                case ActionType.Edit:
                    color = _colorDA.GetById(ArrId.FirstOrDefault());
                    UpdateModel(color);
                    _colorDA.Save();
                    msg = new JsonMessage
                              {
                        Erros = false,
                        ID = color.ID.ToString(),
                        Message = string.Format("Đã cập nhật màu sắc: <b>{0}</b>", Server.HtmlEncode(color.Name))
                    };
                    break;

                case ActionType.Delete:
                    ltsColorItems = _colorDA.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsColorItems)
                    {
                        _colorDA.Delete(item);
                        stbMessage.AppendFormat("Đã xóa màu sắc <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _colorDA.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsColorItems = _colorDA.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsColorItems)
                    {
                        //item.Shop_Product.Clear();
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã cập nhật màu sắc <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _colorDA.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Hide:
                    ltsColorItems = _colorDA.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsColorItems)
                    {
                        //item.Shop_Product.Clear();
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã cập nhật màu sắc <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _colorDA.Save();
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
