using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
	public class ScheduleController : BaseController
	{
		readonly DNScheduleAPI _scheduleApi;
		readonly DNWeeklyAPI _dnWeeklyApi;
		public ScheduleController()
		{
			_scheduleApi = new DNScheduleAPI();
			_dnWeeklyApi = new DNWeeklyAPI();
		}
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ListItems()
		{
			return View(_scheduleApi.ListItems(UserItem.AgencyID, Request.Url.Query));
		}
		public ActionResult AjaxView()
		{
			var customerType = _scheduleApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
			ViewData.Model = customerType;
			return View();
		}

		public ActionResult AjaxForm()
		{
			var schedule = new ScheduleItem();
			if (DoAction == ActionType.Edit)
			{
				schedule = _scheduleApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
			}
			ViewData.Model = schedule;
			ViewBag.Action = DoAction;
			ViewBag.ActionText = ActionText;
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Actions()
		{
			var msg = new JsonMessage();
			var schedule = new ScheduleItem();
			List<ScheduleItem> ltsschedule;
			var json = "";
			var lstId = Request["itemId"];
			switch (DoAction)
			{
				case ActionType.Add:
					try
					{
						UpdateModel(schedule);
						json = new JavaScriptSerializer().Serialize(schedule);
						_scheduleApi.Add(UserItem.AgencyID, json);

						msg = new JsonMessage
						{
							Erros = false,
							ID = schedule.ID.ToString(),
							Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(schedule.Name))
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
						UpdateModel(schedule);
						json = new JavaScriptSerializer().Serialize(schedule);
						_scheduleApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

						msg = new JsonMessage
						{
							Erros = false,
							ID = schedule.ID.ToString(),
							Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(schedule.Name))
						};
					}
					catch (Exception ex)
					{
						LogHelper.Instance.LogError(GetType(), ex);
					}
					break;

				case ActionType.Delete:
			        msg = _scheduleApi.Delete(lstId);
					break;
                case ActionType.Show:
                    msg = _scheduleApi.Show(lstId);
                    break;

                case ActionType.Hide:
                    msg = _scheduleApi.Hide(lstId);
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
