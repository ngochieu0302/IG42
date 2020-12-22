using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNNewsSSCController : BaseController
    {
        readonly DNNewsSSCAPI _newsSscapi;
        readonly DNNewsCommentAPI _newsCommentApi;
       public DNNewsSSCController()
        {
            _newsSscapi = new DNNewsSSCAPI();
           _newsCommentApi = new DNNewsCommentAPI();
        }
        public ActionResult Index()
        {
            var model = new ModelDNNewsSSCItem
            {
                Type = 6,
                ListItem = _newsSscapi.GetAll(UserItem.AgencyID)
            };
            return View(model);
        }

        public ActionResult DetailsNews(int id)
        {
            var model = new ModelDNNewsSSCItem
            {
                Type = 6,
                DNNewsSSCItem = _newsSscapi.GetItemById(UserItem.AgencyID, id)
            };
            return View(model);
        }

        public ActionResult ReloadNewsComment(int parentId)
        {
            var model = new ModelDNNewsCommentItem
            {
                Type = 6,
                ListItem = _newsCommentApi.GetListByParentID(UserItem.AgencyID, parentId).GroupBy(m=> m.ID).Select(m=> m.FirstOrDefault())
            };
            return View(model);
        }

        public ActionResult ProcessReplyMember(int parentId, int newsSscid, string message)
        {
            var msg = new JsonMessage();
            var newsCommentItem = new DNNewsCommentItem();
            try
            {
                newsCommentItem.ParentId = parentId;
                newsCommentItem.IsLevel = 2;
                newsCommentItem.NewsSSCID = newsSscid;
                newsCommentItem.Message = message;
                newsCommentItem.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                newsCommentItem.IsShow = true;
                newsCommentItem.UserId = UserId;
                var json = new JavaScriptSerializer().Serialize(newsCommentItem);
                _newsCommentApi.Add(UserItem.AgencyID, json);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = newsCommentItem.ID.ToString(),
                    Message = "Bạn bình luận thành công !"
                };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProcessComment()
        {
            var msg = new JsonMessage();
            var newsCommentItem = new DNNewsCommentItem();
            var id = Convert.ToInt32(Request["ParentId"]);
            try
            {
                if (id > 1)
                {
                    newsCommentItem.ParentId = id;
                    newsCommentItem.IsLevel = 2;
                }
                else
                {
                    newsCommentItem.IsLevel = 1;
                    newsCommentItem.ParentId = 1;
                }
                   
                newsCommentItem.NewsSSCID = Convert.ToInt32(Request["NewsSSCID"]);
                newsCommentItem.Message = Request.Unvalidated["Message"];
                newsCommentItem.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                newsCommentItem.IsShow = true;
                newsCommentItem.UserId = UserId;
                var json = new JavaScriptSerializer().Serialize(newsCommentItem);
                _newsCommentApi.Add(UserItem.AgencyID, json);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = newsCommentItem.ID.ToString(),
                    Message = "Bạn bình luận thành công !"
                };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
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
