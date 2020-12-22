using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.SYS;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class NewsContentController : BaseController
    {
        private readonly NewsDA _newsDa;
        readonly CategoryDA _categoryDa;
        public NewsContentController()
        {
            _newsDa = new NewsDA("#");
            _categoryDa = new CategoryDA("#");
        }
        public ActionResult Index()
        {            
            var model = _categoryDa.GetChildByParentId(false);
            return View(model);
        }
        public ActionResult ListItems()
        {
            var lstNews = _newsDa.GetAllListSimple(Request);
            var model = new ModelNewsItem
            {
                Container = Request["Container"],
                ListItem = lstNews,
                PageHtml = _newsDa.GridHtmlPage
            };
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var newsModel = _newsDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = newsModel;
            return View();
        }
        public ActionResult AjaxForm()
        {
            var newsModel = new News_News
            {
                IsShow = true,
                IsHot = false,
                Gallery_Picture = new Gallery_Picture()
            };

            if (DoAction == ActionType.Edit)
            {
                newsModel = _newsDa.GetById(ArrId.FirstOrDefault());
            }
            
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(newsModel);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var model = new News_News();
            List<News_News> ltsNewsItems;
            StringBuilder stbMessage;
            List<Category> lstCategory;
            var lsttag = Request["values-arr-tag"];
            var images = Request["Value_DefaultImages"];
            var lstCate = Request["Value_CategoryValues"];
            var datecreate = Request["DateCreated_"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        model.LanguageId = Fdisystem.LanguageId;
                        //model.Viewer = 0;
                        var membershipUser = Membership.GetUser();
                        //model.TitleAscii = FomatString.Slug(model.Title);
                        if (membershipUser != null)
                        {
                            model.Author = (Guid)membershipUser.ProviderUserKey;
                        }
                        model.DateCreated = !string.IsNullOrEmpty(datecreate) ? ConvertUtil.ToDateTime(datecreate) : DateTime.Now;
                        if (!string.IsNullOrEmpty(lstCate))
                        {
                            var lstInt = FDIUtils.StringToListInt(lstCate);
                            model.Categories = _newsDa.GetListCategory(lstInt);
                        }
                        if (!string.IsNullOrEmpty(images))
                            model.PictureID = int.Parse(images);

                        if (!string.IsNullOrEmpty(lsttag))
                        {
                            var lstInt = FDIUtils.StringToListInt(lsttag);
                            model.System_Tag = _newsDa.GetListIntTagByArrID(lstInt);
                        }
                        model.IsDeleted = false;
                        _newsDa.Add(model);
                        _newsDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.ID.ToString(),
                            Message = string.Format("Đã thêm mới bài viết: <b>{0}</b>", Server.HtmlEncode(model.Title))
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
                        model = _newsDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(model);
                        //model.TitleAscii = FomatString.Slug(model.Title);
                        model.DateCreated = !string.IsNullOrEmpty(datecreate) ? ConvertUtil.ToDateTime(datecreate) : DateTime.Now;
                        var membershipUser = Membership.GetUser();
                        if (membershipUser != null)
                        {
                            var providerUserKey = membershipUser.ProviderUserKey;
                            if (providerUserKey != null)
                                model.Modifier = (Guid)providerUserKey;
                        }
                        model.Categories.Clear();
                        if (!string.IsNullOrEmpty(lstCate))
                        {
                            var lstInt = FDIUtils.StringToListInt(lstCate);
                            model.Categories = _newsDa.GetListCategory(lstInt);
                        }
                        if (!string.IsNullOrEmpty(images))
                        {
                            model.PictureID = int.Parse(images);
                        }

                        model.System_Tag.Clear();
                        if (!string.IsNullOrEmpty(lsttag))
                        {
                            var lstInt = FDIUtils.StringToListInt(lsttag);
                            model.System_Tag = _newsDa.GetListIntTagByArrID(lstInt);
                        }
                        //model.DateUpdated = DateTime.Now;
                        _newsDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.ID.ToString(),
                            Message = string.Format("Đã cập nhật bài viết: <b>{0}</b>", Server.HtmlEncode(model.Title))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }

                    break;

                case ActionType.Delete:
                    ltsNewsItems = _newsDa.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsNewsItems)
                    {
                        try
                        {
                            item.IsDeleted = true;
                            stbMessage.AppendFormat("Đã xóa bài viết <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            _newsDa.Save();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _newsDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsNewsItems = _newsDa.GetListByArrID(ArrId).Where(o => o.IsShow != true).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsNewsItems)
                    {
                        try
                        {
                            var providerUserKey = Membership.GetUser().ProviderUserKey;
                            if (providerUserKey != null)
                                item.Modifier = (Guid)providerUserKey;
                            item.IsShow = true;
                            stbMessage.AppendFormat("Đã hiển thị bài viết <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                    }

                    _newsDa.Save();
                    msg.ID = string.Join(",", ltsNewsItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsNewsItems = _newsDa.GetListByArrID(ArrId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsNewsItems)
                    {
                        try
                        {
                            var providerUserKey = Membership.GetUser().ProviderUserKey;
                            if (providerUserKey != null)
                                item.Modifier = (Guid)providerUserKey;
                            item.IsShow = false;
                            stbMessage.AppendFormat("Đã ẩn bài viết <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                    }

                    _newsDa.Save();
                    msg.ID = string.Join(",", ltsNewsItems.Select(o => o.ID));
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
        public string CheckTitleAsciiExits(string Title, int id)
        {
            var ascii = FomatString.Slug(Title);
            var result = _newsDa.CheckTitleAsciiExits(ascii, id);

            return result ? "false" : "true";
        }       
    }
}
