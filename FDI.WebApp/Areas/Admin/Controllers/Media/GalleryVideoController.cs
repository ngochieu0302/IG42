using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Utils;
using FDI.Simple;

namespace FDI.Areas.Admin.Controllers
{
    public class GalleryVideoController : BaseController
    {
        //
        // GET: /Admin/GalleryVideo/

        readonly Gallery_VideoDA _videoDa = new Gallery_VideoDA("#");
        readonly CategoryDA _categoryDa;

        public GalleryVideoController()
        {
            _categoryDa = new CategoryDA("#");
        }

        public ActionResult Index()
        {
            var ltsAllCategory = _categoryDa.GetChildByParentId(true);
            var model = new ModelCategoryItem
            {
                ListItem = ltsAllCategory,
            };
            return View(model);
        }

        public ActionResult ListItems()
        {
            var listVideoItem = _videoDa.GetListSimpleByRequest(Request);
            var model = new ModelVideoItem
            {
                ListItem = listVideoItem,
                PageHtml = _videoDa.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var model = _videoDa.GetById(ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var videoModel = new Gallery_Video
            {
                IsShow = true,
            };

            if (DoAction == ActionType.Edit)
                videoModel = _videoDa.GetById(ArrId.FirstOrDefault());
            ViewBag.VideoCategoryID = _categoryDa.GetChildByParentId(false);
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(videoModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var video = new Gallery_Video();
            List<Gallery_Video> ltsVideoItems;
            StringBuilder stbMessage;
            var pictureId = Request["Value_DefaultImages"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(video);
                        video.LanguageId = Fdisystem.LanguageId;
                        video.DateCreated = DateTime.Now.TotalSeconds();
                        video.IsDeleted = false;
                        if (!string.IsNullOrEmpty(pictureId))
                            video.PictureID = Convert.ToInt32(pictureId);
                        _videoDa.Add(video);
                        _videoDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = video.ID.ToString(),
                            Message = string.Format("Đã thêm mới video: <b>{0}</b>", Server.HtmlEncode(video.Name))
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
                        video = _videoDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(video);
                        if (!string.IsNullOrEmpty(pictureId))
                            video.PictureID = Convert.ToInt32(pictureId);
                        else
                            video.PictureID = null;
                        _videoDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = video.ID.ToString(),
                            Message = string.Format("Đã cập nhật video: <b>{0}</b>", Server.HtmlEncode(video.Name))
                        };
                    }
                    catch (Exception ex)
                    {

                        LogHelper.Instance.LogError(GetType(), ex);
                    }

                    break;

                case ActionType.Delete:
                    ltsVideoItems = _videoDa.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsVideoItems)
                    {
                        try
                        {
                            item.IsDeleted = true;
                            stbMessage.AppendFormat("Đã xóa video <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {

                            LogHelper.Instance.LogError(GetType(), ex);
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _videoDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsVideoItems = _videoDa.GetListByArrID(ArrId).Where(o => o.IsShow != null && !o.IsShow.Value).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsVideoItems)
                    {
                        try
                        {
                            item.IsShow = true;
                            stbMessage.AppendFormat("Đã hiển thị video <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                    }
                    _videoDa.Save();
                    msg.ID = string.Join(",", ltsVideoItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsVideoItems = _videoDa.GetListByArrID(ArrId).Where(o => o.IsShow != null && o.IsShow.Value).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsVideoItems)
                    {
                        try
                        {
                            item.IsShow = false;
                            stbMessage.AppendFormat("Đã ẩn video <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {

                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                    }
                    _videoDa.Save();
                    msg.ID = string.Join(",", ltsVideoItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
            }

            if (!string.IsNullOrEmpty(msg.Message)) return Json(msg, JsonRequestBehavior.AllowGet);
            msg.Message = "Không có hành động nào được thực hiện.";
            msg.Erros = true;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _videoDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }

    }
}
