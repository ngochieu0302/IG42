using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNTimeJobController : BaseController
    {
        readonly DNTimeJobAPI _dnTimeJobApi;
        readonly DNUserAPI _userApi;
       public DNTimeJobController()
        {
            _dnTimeJobApi = new DNTimeJobAPI();
           _userApi = new DNUserAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_dnTimeJobApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }


        public ActionResult AjaxView()
        {
            var dnTimeJob = _dnTimeJobApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = dnTimeJob;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var dnTimeJob = new DNTimeJobItem();
            if (DoAction == ActionType.Edit)
            {
                dnTimeJob = _dnTimeJobApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.UserId = _userApi.GetAll(UserItem.AgencyID);
            ViewData.Model = dnTimeJob;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var dnTimeJob = new DNTimeJobItem();
            List<DNTimeJobItem> ltsDNTimeJob;
            //var dateCreated = Request["_DateCreated"];
            //var dateEnd = Request["_DateEnd"];
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {

                        UpdateModel(dnTimeJob);
                        // check in chấm công
                        //var inJob = JobTimeCheckIn(UserId, dnTimeJob);
                        //if (inJob != null)
                        //{
                        //    dnTimeJob.ScheduleID = inJob.ScheduleID;
                        //    dnTimeJob.ScheduleEndID = null;
                        //    dnTimeJob.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        //}
                        ////// check out chấm công
                        //var outJob = JobTimeCheckOut(UserId, dnTimeJob);
                        //if (outJob != null)
                        //{
                        //    dnTimeJob.DateEnd = outJob.DateEnd;
                        //    dnTimeJob.ScheduleEndID = outJob.ScheduleEndID;
                        //}
                     
                        json = new JavaScriptSerializer().Serialize(dnTimeJob);
                        if (Convert.ToInt32(ArrId.FirstOrDefault()) > 0)
                            _dnTimeJobApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());
                        else
                            _dnTimeJobApi.Add(CodeLogin(), json, UserItem.AgencyID);

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnTimeJob.ID.ToString(),
                            Message = string.Format("Cập nhật: <b>{0}</b>", Server.HtmlEncode(dnTimeJob.UserId.ToString()))
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
                        UpdateModel(dnTimeJob);
                        dnTimeJob.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        dnTimeJob.DateEnd = ConvertDate.TotalSeconds(DateTime.Now);
                        json = new JavaScriptSerializer().Serialize(dnTimeJob);
                        _dnTimeJobApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnTimeJob.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(dnTimeJob.UserId.ToString()))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsDNTimeJob = _dnTimeJobApi.GetListByArrId(UserItem.AgencyID, lstId);
                    foreach (var item in ltsDNTimeJob)
                    {
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _dnTimeJobApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = dnTimeJob.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsDNTimeJob.Select(c => c.UserId))))
                    };
                    break;
                
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public DNTimeJobItem JobTimeCheckIn(Guid userId, DNTimeJobItem objItem)
        {
            var date = DateTime.Now;
            var totalS = date.Hour * 3600 + date.Minute * 60;
            var list = GetWeeklyScheduleday(userId, DateTime.Today);
            var obj = list.FirstOrDefault(m => m.ScheduleTimeStart >= totalS || (m.ScheduleTimeStart < totalS && m.ScheduleTimeEnd > totalS));
            objItem.DateCreated = ConvertDate.TotalSeconds(date);
            if (obj != null)
            {
                objItem.ScheduleID = obj.ScheduleID;
                objItem.MinutesLater = (int)(obj.ScheduleTimeStart - totalS) / 60;
            }
            objItem.UserId = userId;
            return objItem;
        }

        public DNTimeJobItem JobTimeCheckOut(Guid userId, DNTimeJobItem objItem)
        {
            var date = DateTime.Now;
            var totalS = date.Hour * 3600 + date.Minute * 60;
            var list = GetWeeklyScheduleday(userId, DateTime.Today).OrderByDescending(m => m.ScheduleTimeStart);
            var obj = list.FirstOrDefault(m => (m.ScheduleTimeStart >= totalS && m.ScheduleTimeEnd > totalS) || m.ScheduleTimeEnd <= totalS);
            objItem.DateEnd = ConvertDate.TotalSeconds(date);
            if (obj != null)
            {
                objItem.ScheduleEndID = obj.ScheduleID;
                objItem.MinutesEarly = (int)(obj.ScheduleTimeEnd - totalS) / 60;
            }
            objItem.UserId = userId;
            return objItem;
        }
        
    }
}
