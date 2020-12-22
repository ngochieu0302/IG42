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

    public class PartnersController : BaseController
    {
        private readonly PartnerDA _da = new PartnerDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var lstNews = _da.GetList(Request);
            var model = new ModelPartnerItem
            {
                Container = Request["Container"],
                ListItem = lstNews,
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var model = _da.GetById(ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var model = new Partner();
            if (DoAction == ActionType.Edit)
            {
                model = _da.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var partner = new Partner();
            List<Partner> ltsPartner;
            StringBuilder stbMessage;
            List<int> idValues;
            var images = Request["Value_Images"];
            var lstimages = Request["Value_ImagesProducts"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(partner);
                        partner.LanguageId = Fdisystem.LanguageId;
                        partner.Slug = FDIUtils.Slug(partner.Name);
                        partner.DateCreated = DateTime.Now;
                        if (!string.IsNullOrEmpty(images))
                            partner.PictureID = Convert.ToInt32(images);
                        if (!string.IsNullOrEmpty(lstimages))
                        {
                            var lstInt = FDIUtils.StringToListInt(lstimages);
                            partner.Gallery_Picture1 = _da.GetListPictureByArrId(lstInt);
                        }

                        partner.IsDeleted = false;
                        _da.Add(partner);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = partner.ID.ToString(),
                            Message = string.Format("Đã thêm mới bài viết: <b>{0}</b>", Server.HtmlEncode(partner.Name))
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
                        partner = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(partner);
                        if (!string.IsNullOrEmpty(images))
                            partner.PictureID = Convert.ToInt32(images);
                        partner.Gallery_Picture1.Clear();
                        if (!string.IsNullOrEmpty(lstimages))
                        {
                            var lstInt = FDIUtils.StringToListInt(lstimages);
                            partner.Gallery_Picture1 = _da.GetListPictureByArrId(lstInt);
                        }
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = partner.ID.ToString(),
                            Message = string.Format("Đã cập nhật bài viết: <b>{0}</b>", Server.HtmlEncode(partner.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }

                    break;

                case ActionType.Delete:
                    ltsPartner = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsPartner)
                    {
                        try
                        {
                            item.IsDeleted = true;
                            stbMessage.AppendFormat("Đã xóa bài viết <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                            _da.Save();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _da.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsPartner = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsPartner)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị bài viết <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsPartner.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsPartner = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsPartner)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn bài viết <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsPartner.Select(o => o.ID));
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

        [HttpGet]
        public string CheckTitleAsciiExits(string name, int id)
        {
            var result = _da.CheckExits(name, id);
            return result ? "false" : "true";
        }
    }
}
