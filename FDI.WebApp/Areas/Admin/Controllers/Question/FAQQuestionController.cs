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
    public class FAQQuestionController : BaseController
    {
        readonly FAQQuestionDA _questionDa = new FAQQuestionDA("#");
        readonly CategoryDA _categoryDa;
        readonly FAQ_AnswerDA _answerDa = new FAQ_AnswerDA("#");

        public FAQQuestionController()
        {
            _categoryDa = new CategoryDA("#");
        }

        #region dành cho câu hỏi

        public ActionResult AjaxFormAnswer()
        {
            var answerModel = new FAQ_Answer
            {
                IsShow = true,
                QuestionID = Convert.ToInt32(Request["QuestionID"])
            };

            if (DoAction == ActionType.Edit)
                answerModel = _answerDa.GetById(ArrId.FirstOrDefault());

            ViewData.Model = answerModel;
            ViewBag.FAQ_Question = _questionDa.GetById(answerModel.QuestionID);
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AnswerActions()
        {
            var msg = new JsonMessage();
            var answer = new FAQ_Answer();
            List<FAQ_Answer> ltsAnswerItems;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(answer);
                    answer.TitleAscii = FDIUtils.Slug(answer.Title);
                    answer.DateCreated = DateTime.Now;
                    _answerDa.Add(answer);
                    _answerDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = answer.ID.ToString(),
                        Message = string.Format("Đã thêm mới câu trả lời: <b>{0}</b>", Server.HtmlEncode(answer.Title))
                    };
                    break;

                case ActionType.Edit:
                    answer = _answerDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(answer);
                    answer.TitleAscii = FDIUtils.Slug(answer.Title);

                    _answerDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = answer.ID.ToString(),
                        Message = string.Format("Đã cập nhật câu trả lời: <b>{0}</b>", Server.HtmlEncode(answer.Title))
                    };
                    break;

                case ActionType.Delete:
                    ltsAnswerItems = _answerDa.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsAnswerItems)
                    {
                        _answerDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa câu trả lời <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _answerDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsAnswerItems = _answerDa.GetListByArrID(ArrId).Where(o => !o.IsShow).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsAnswerItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị câu trả lời <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                    }
                    _answerDa.Save();
                    msg.ID = string.Join(",", ltsAnswerItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsAnswerItems = _answerDa.GetListByArrID(ArrId).Where(o => o.IsShow).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsAnswerItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn câu trả lời <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                    }
                    _answerDa.Save();
                    msg.ID = string.Join(",", ltsAnswerItems.Select(o => o.ID));
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

        #endregion
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var lst = _questionDa.GetListSimpleByRequest(Request);
            var model = new ModelFAQQuestionItem
            {
                ListItem = lst,
                PageHtml = _questionDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var model = _questionDa.GetById(ArrId.FirstOrDefault());
            return View(model);
        }
        
        public ActionResult AjaxForm()
        {
            var questionModel = new FAQ_Question
            {
                IsShow = true
            };
            if (DoAction == ActionType.Edit)
                questionModel = _questionDa.GetById(ArrId.FirstOrDefault());
            ViewBag.CategoryID = _categoryDa.GetChildByParentId(false, (int)ModuleType.FAQ);
            ViewData.Model = questionModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }


        public ActionResult AjaxReply()
        {
            var questionModel = new FAQ_Question
            {
                IsShow = true
            };

            if (Request["do"] != null && Request["do"] == "reply")
                questionModel = _questionDa.GetById(ArrId.FirstOrDefault());
            ViewBag.CategoryID = _categoryDa.GetChildByParentId(true, (int)ModuleType.FAQ);
            ViewData.Model = questionModel;
            ViewBag.Action = "reply";
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var question = new FAQ_Question();
            List<FAQ_Question> ltsQuestionItems;
            StringBuilder stbMessage;

            if (Request["do"] != null && Request["do"] == "reply")
            {
                try
                {
                    question = _questionDa.GetById(ArrId.FirstOrDefault());
                    var content = question.Content;
                    UpdateModel(question);
                    if (question.FAQ_Answer != null && question.FAQ_Answer.FirstOrDefault() != null)
                    {
                        var answer = question.FAQ_Answer.FirstOrDefault();
                        if (answer != null)
                        {
                            answer.Content = question.Content;
                            question.Content = content;
                            answer.DateCreated = DateTime.Now;
                            answer.IsShow = true;
                        }
                    }
                    else
                    {
                        var faqAnswer = new FAQ_Answer {Content = question.Content};
                        question.Content = content;
                        faqAnswer.DateCreated = DateTime.Now;
                        faqAnswer.IsShow = true;

                        if (question.FAQ_Answer != null) question.FAQ_Answer.Add(faqAnswer);
                    }

                    _questionDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        Message = "Đã trả lời câu hỏi"
                    };
                }
                catch (Exception)
                {
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(question);
                    question.TitleAscii = FDIUtils.Slug(question.Title);
                    question.DateCreated = DateTime.Now;
                    question.LanguageId = Fdisystem.LanguageId;
                    _questionDa.Add(question);
                    _questionDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = question.ID.ToString(),
                        Message = string.Format("Đã thêm mới câu hỏi: <b>{0}</b>", Server.HtmlEncode(question.Title))
                    };
                    break;

                case ActionType.Edit:
                    question = _questionDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(question);
                    _questionDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = question.ID.ToString(),
                        Message = string.Format("Đã cập nhật câu hỏi: <b>{0}</b>", Server.HtmlEncode(question.Title))
                    };
                    break;

                case ActionType.Delete:
                    ltsQuestionItems = _questionDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsQuestionItems)
                    {
                        _questionDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa câu hỏi <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        
                    }
                    msg.ID = string.Join(",", ArrId);
                    _questionDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsQuestionItems = _questionDa.GetListByArrId(ArrId).Where(o => o.IsShow == false).ToList(); 
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsQuestionItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị câu hỏi <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                    }
                    _questionDa.Save();
                    msg.ID = string.Join(",", ltsQuestionItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsQuestionItems = _questionDa.GetListByArrId(ArrId).Where(o => o.IsShow == true).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsQuestionItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn câu hỏi <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                    }
                    _questionDa.Save();
                    msg.ID = string.Join(",", ltsQuestionItems.Select(o => o.ID));
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
        
        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _questionDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }
    }
}
