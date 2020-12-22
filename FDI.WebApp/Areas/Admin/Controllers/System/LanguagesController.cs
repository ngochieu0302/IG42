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
    public class LanguagesController : BaseController
    {
        //
        // GET: /Admin/Languages/
        private readonly LanguagesDA _da = new LanguagesDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelLanguagesItem
            {
                ListItem = _da.GetListSimpleByRequest(Request),
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var language = _da.GetById(ArrId.FirstOrDefault());
            return View(language);
        }

        public ActionResult AjaxForm()
        {
            var language = new Language();
            if (DoAction == ActionType.Edit)
                language = _da.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(language);
        }

        public ActionResult LoadLanguage()
        {
            var httpCookie = Request.Cookies["LanguageId"];
            if (httpCookie == null)
            {
                Utility.Setcookie("vi", "LanguageId", 1);
            }
            var model = _da.GetListSimpleAll(true);
            return View(model);
        }

        public JsonResult ChangelanguageAdmin(string lang)
        {
            Utility.Setcookie(lang, "LanguageId", 1);
            return Json("0", JsonRequestBehavior.AllowGet);
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
            var language = new Language();
            List<Language> ltsLanguages;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(language);
                        language.CreatedDate = DateTime.Now;
                        language.Icon = !string.IsNullOrEmpty(Request["Value_DefaultImages"]) ? Convert.ToInt32(Request["Value_DefaultImages"]) : 0;
                        _da.Add(language);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = language.Id.ToString(),
                            Message = string.Format("Đã thêm mới: <b>{0}</b>", Server.HtmlEncode(language.Name))
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
                        language = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(language);
                        language.Icon = !string.IsNullOrEmpty(Request["Value_DefaultImages"]) ? Convert.ToInt32(Request["Value_DefaultImages"]) : 0;
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = language.Id.ToString(),
                            Message = string.Format("Đã cập nhật: <b>{0}</b>", Server.HtmlEncode(language.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsLanguages = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsLanguages)
                    {
                        _da.Delete(item);
                        stbMessage.AppendFormat("Đã xóa ngôn ngữ<b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _da.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsLanguages = _da.GetListByArrId(ArrId).Where(o => o.IsShow != null && !o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsLanguages)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị ngôn ngữ <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsLanguages.Select(o => o.Id));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsLanguages = _da.GetListByArrId(ArrId).Where(o => o.IsShow != null && o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsLanguages)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn ngôn ngữ <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsLanguages.Select(o => o.Id));
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
