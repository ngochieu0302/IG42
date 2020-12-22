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
    public class DnEditScheduleController : BaseController
    {
        readonly DNEDITScheduleAPI _dneditScheduleApi;
        readonly DNUserAPI _userApi;
        //readonly DNScheduleAPI _scheduleApi;
        private readonly DNWeeklyScheduleAPI _weeklyScheduleApi;
        public DnEditScheduleController()
        {
            _dneditScheduleApi = new DNEDITScheduleAPI();
            _userApi = new DNUserAPI();
            //_scheduleApi = new DNScheduleAPI();
            _weeklyScheduleApi = new DNWeeklyScheduleAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_dneditScheduleApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var dayOff = _dneditScheduleApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = dayOff;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var editSchedule = new EditScheduleItem();
            if (DoAction == ActionType.Edit)
            {
                editSchedule = _dneditScheduleApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            var lstUser = _userApi.GetAll(UserItem.AgencyID).Where(m => m.UserId != UserId);
            ViewBag.UserID = lstUser;
            ViewBag.UserChangeId = lstUser;
            ViewData.Model = editSchedule;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult GetScheduleById(string userId, DateTime date)
        {
            var lstSchedule = GetWeeklyScheduleday(Guid.Parse(userId), date);
            return Json(lstSchedule, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListUser(string userId, int scheduleId, DateTime date)
        {
            var datestart = ConvertDate.TotalSeconds(date);
            var lstSchedule = _weeklyScheduleApi.GetAllUserSchedule(UserItem.AgencyID, Guid.Parse(userId), datestart, scheduleId);
            return Json(lstSchedule, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var editSchedule = new EditScheduleItem();
            List<EditScheduleItem> ltsEditSchedule;
            var date = Request["_Date"];
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {

                        UpdateModel(editSchedule);
                        if (!string.IsNullOrEmpty(Request["_Date"]))
                        {
                            var dateOff = Request["_Date"].StringToDate();
                            var objDayOff = GetListDayOffItem(dateOff, dateOff, CodeLogin()).FirstOrDefault();
                            if (objDayOff != null)
                                editSchedule.DayOffId = objDayOff.ID;
                        }

                        editSchedule.Datecreated = DateTime.Now.TotalSeconds();
                        if (!string.IsNullOrEmpty(date))
                            editSchedule.Date = date.StringToDecimal();
                        json = new JavaScriptSerializer().Serialize(editSchedule);
                        _dneditScheduleApi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = editSchedule.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(editSchedule.UserId.ToString()))
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
                        UpdateModel(editSchedule);
                        if (!string.IsNullOrEmpty(date))
                            editSchedule.Date = date.StringToDecimal();
                        json = new JavaScriptSerializer().Serialize(editSchedule);
                        _dneditScheduleApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = editSchedule.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(editSchedule.ID.ToString()))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                //case ActionType.Delete:
                //    ltsEditSchedule = _dneditScheduleApi.GetListByArrId(UserItem.AgencyID, lstId);
                //    foreach (var item in ltsEditSchedule)
                //    {
                //        UpdateModel(item);
                //        json = new JavaScriptSerializer().Serialize(item);
                //        _dneditScheduleApi.Update(UserItem.AgencyID, json, item.ID);
                //    }
                //    msg = new JsonMessage
                //    {
                //        Erros = false,
                //        ID = editSchedule.ID.ToString(),
                //        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsEditSchedule.Select(c => c.Name))))
                //    };
                //    break;
                //case ActionType.Show:
                //    ltsEditSchedule = _dneditScheduleApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                //    foreach (var item in ltsEditSchedule)
                //    {
                //        item.IsDelete = false;
                //        item.IsShow = true;
                //        UpdateModel(item);
                //        json = new JavaScriptSerializer().Serialize(item);
                //        _dneditScheduleApi.Update(UserItem.AgencyID, json, item.ID);
                //    }
                //    msg = new JsonMessage
                //    {
                //        Erros = false,
                //        ID = dayOff.ID.ToString(),
                //        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsEditSchedule.Select(c => c.Name))))
                //    };
                //    break;

                //case ActionType.Hide:
                //    ltsEditSchedule = _dneditScheduleApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                //    foreach (var item in ltsEditSchedule)
                //    {
                //        item.IsDelete = false;
                //        item.IsShow = false;
                //        UpdateModel(item);
                //        json = new JavaScriptSerializer().Serialize(item);
                //        _dneditScheduleApi.Update(UserItem.AgencyID, json, item.ID);
                //    }
                //    msg = new JsonMessage
                //    {
                //        Erros = false,
                //        ID = dayOff.ID.ToString(),
                //        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsEditSchedule.Select(c => c.Name))))
                //    };
                //    break;
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
