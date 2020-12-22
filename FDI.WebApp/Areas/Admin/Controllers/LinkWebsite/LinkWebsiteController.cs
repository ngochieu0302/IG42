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
    public class LinkWebsiteController : BaseController
    {
        //
        // GET: /Admin/LinkWebsite/
        //readonly AdminUtilityDA _adminUtilityDA = new AdminUtilityDA();
        private readonly LinkWebsiteDA _linkWebsiteDA;

        public LinkWebsiteController()
        {
            _linkWebsiteDA = new LinkWebsiteDA("#");
        }
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
            var model = new ModelLinkWebsiteItem
            {
                ListItem = _linkWebsiteDA.GetListAll(Request),
                PageHtml = _linkWebsiteDA.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }


        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var linkWebsite = new Website();
            if (DoAction == ActionType.Edit)
                linkWebsite = _linkWebsiteDA.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(linkWebsite);
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
            var linkWebsite = new Website();
            List<Website> ltsLinkWebsite;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(linkWebsite);
                        linkWebsite.LanguageId = Fdisystem.LanguageId;
                        linkWebsite.IsShow = true;
                        _linkWebsiteDA.Add(linkWebsite);
                        _linkWebsiteDA.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = linkWebsite.ID.ToString(),
                            Message = string.Format("Đã thêm Liên Kết: <b>{0}</b>", Server.HtmlEncode(linkWebsite.Name))
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
                        linkWebsite = _linkWebsiteDA.GetById(ArrId.FirstOrDefault());
                        UpdateModel(linkWebsite);
                        _linkWebsiteDA.Save();

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = linkWebsite.ID.ToString(),
                            Message = string.Format("Đã cập nhật Liên Kết: <b>{0}</b>", Server.HtmlEncode(linkWebsite.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsLinkWebsite = _linkWebsiteDA.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsLinkWebsite)
                    {
                        _linkWebsiteDA.Delete(item);
                        stbMessage.AppendFormat("Đã xóa Liên Kết <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));

                    }
                    msg.ID = string.Join(",", ArrId);
                    _linkWebsiteDA.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsLinkWebsite = _linkWebsiteDA.GetListByArrId(ArrId).Where(o => o.IsShow != null && !o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsLinkWebsite)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị Liên Kết <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _linkWebsiteDA.Save();
                    msg.ID = string.Join(",", ltsLinkWebsite.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsLinkWebsite = _linkWebsiteDA.GetListByArrId(ArrId).Where(o => o.IsShow != null && o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsLinkWebsite)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn  Liên Kết<b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _linkWebsiteDA.Save();
                    msg.ID = string.Join(",", ltsLinkWebsite.Select(o => o.ID));
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
