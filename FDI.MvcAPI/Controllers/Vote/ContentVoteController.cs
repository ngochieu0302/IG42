using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.DA;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class ContentVoteController : BaseApiController
    {
        private readonly ContentVoteDa _da = new ContentVoteDa("#");
        private readonly VoteDA _voteDa = new VoteDA("#");        

        public ActionResult ListItems(int agencyId)
        {
            int total;
            var obj = Request["key"] != Keyapi
                ? new ModelContentVoteItem()
                : new ModelContentVoteItem
                {
                    ListItems = _da.GetListSimpleByRequest(Request, agencyId,out total),
                    TotalValue = total,
                    PageHtml = _da.GridHtmlPage
                };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListExport(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelContentVoteItem()
                : new ModelContentVoteItem
                {
                    ListItems = _da.GetListSimpleByRequestExcel(Request, agencyId),
                };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItemsUser(int agencyId,Guid userId)
        {
            int total;
            var obj = Request["key"] != Keyapi
                ? new ModelContentVoteItem()
                : new ModelContentVoteItem
                {
                    ListItems = _da.ListItemsUser(Request, agencyId, userId, out total),
                    TotalValue = total,
                    PageHtml = _da.GridHtmlPage
                };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItemsUserNew(int agencyId, string userId)
        {
            int total;
            var obj = Request["key"] != Keyapi
                ? new ModelContentVoteItem()
                : new ModelContentVoteItem
                {
                    ListItems = _da.ListItemsUser(Request, agencyId, userId, out total),
                    TotalValue = total,
                    PageHtml = _da.GridHtmlPage
                };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContentVoteByUserId(string key, Guid UserId)
        {
            var obj = key != Keyapi ? new List<ContentVoteItem>() : _da.GetListByUserId(UserId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItemByUserId(string key, int agencyId, Guid? userid, string date)
        {
            var obj = key != Keyapi ? new List<ContentVoteItem>() : _da.ListItemByUserId(userid, agencyId, date);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }        
        
        public ActionResult GeneralListTotal(string key, int year, int agencyId)
        {
            var obj = key != Keyapi
                ? new List<GeneralVoteItem>()
                : _da.GeneralListTotal(year, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SumContentVoteItem(string key, Guid userId)
        {
            var obj = key != Keyapi ? new SumContentVoteItem() : _da.SumWMY(userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddUpdate(string json, string key, int agencyId, Guid userId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            var obj = new ContentVoteItem();
            try
            {
                if (key == Keyapi)
                {
                    var fromDate = Request["fromDate"];
                    var now = DateTime.Now;
                    var datecode = string.Format("{0} {1}:{2}", fromDate, now.Hour, now.Minute);
                    var date = datecode.StringToDate();
                    var datenow = new DateTime(now.Year, now.Month, now.Day);
                    if (date < datenow.AddDays(-1) || date >= datenow.AddDays(1))
                    {
                        msg.Erros = true;
                        msg.Message = "Quá thời gian hoặc chưa đến thời gian đánh giá đánh giá.";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }

                    var startDate = fromDate.StringToDecimal();
                    var endDate = fromDate.StringToDecimal(1);
                    UpdateModel(obj);
                    var model = _da.GetByDate(obj.VoteID ?? 0, obj.TreeID ?? 0, startDate, endDate);

                    if (model != null)
                    {
                        model.Content = HttpUtility.UrlDecode(obj.Content);
                        model.LevelVoteID = obj.LevelVoteID;
                        model.Value = obj.Value;
                        model.DateEvaluation = date.TotalSeconds();
                    }
                    else
                    {
                        model = new DN_ContentVote
                        {
                            LevelVoteID = obj.LevelVoteID,
                            Content = HttpUtility.UrlDecode(obj.Content),
                            VoteID = obj.VoteID,
                            TreeID = obj.TreeID,
                            UserID = userId,
                            Value = obj.Value,
                            DateCreated = now.TotalSeconds(),
                            DateEvaluation = date.TotalSeconds(),
                            AgencyId = agencyId
                        };
                        _da.Add(model);
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu Chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }       
    }
}
