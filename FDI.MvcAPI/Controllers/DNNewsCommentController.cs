using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNNewsCommentController : BaseApiController
    {
        //
        // GET: /DNNewsComment/

        private readonly DNNewsCommentDA _dl = new DNNewsCommentDA();

        public ActionResult GetListSimpleByRequest()
        {
            var obj = Request["key"] != Keyapi ? new List<DNNewsCommentItem>() : _dl.GetListSimpleByRequest(Request);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNNewsCommentItem>() : _dl.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_News_Comment() : _dl.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNNewsCommentItem() : _dl.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNNewsCommentItem>() : _dl.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByParentID(string key, int parentId)
        {
            var obj = key != Keyapi ? new List<DNNewsCommentItem>() : _dl.GetListByParentID(parentId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DNNewsCommentItem>(json);
                var obj = new DN_News_Comment();
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
                _dl.Add(obj);
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DNNewsCommentItem>(json);
                var obj = _dl.GetById(id);
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Delete(string key, List<int> listint)
        //{
        //    if (key == Keyapi)
        //    {
        //        var list = _dl.GetListByArrId(listint);
        //        foreach (var item in list)
        //        {
        //            _dl.Delete(item);
        //        }
        //        _dl.Save();
        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}

        public DN_News_Comment UpdateBase(DN_News_Comment newsComment, DNNewsCommentItem newsCommentItem)
        {
            newsComment.AgencyID = newsCommentItem.AgencyID;
            newsComment.Message = newsCommentItem.Message;
            newsComment.UserId = newsCommentItem.UserId;
            newsComment.ParentId = newsCommentItem.ParentId;
            newsComment.IsShow = newsCommentItem.IsShow;
            newsComment.NewsSSCID = newsCommentItem.NewsSSCID;
            newsComment.IsLevel = newsCommentItem.IsLevel;
            newsComment.DateCreated = newsCommentItem.DateCreated;
            return newsComment;
        }
    }
}
