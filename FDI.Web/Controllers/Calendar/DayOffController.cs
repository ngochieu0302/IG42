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
    public class DayOffController : BaseController
    {
       readonly DNDayOffAPI _dnDayOffApi;
        public DayOffController()
        {
            _dnDayOffApi = new DNDayOffAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_dnDayOffApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var dayOff = _dnDayOffApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = dayOff;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var dayOff = new DayOffItem();
            if (DoAction == ActionType.Edit)
            {
                dayOff = _dnDayOffApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewData.Model = dayOff;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var dayOff = new DayOffItem();
            List<DayOffItem> ltsdayOff;
            var date = Request["DateOff"];
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(dayOff);
                        dayOff.IsDelete = false;
                        if (!string.IsNullOrEmpty(date))
                            dayOff.Date = date.StringToDecimal();
                        json = new JavaScriptSerializer().Serialize(dayOff);
                        _dnDayOffApi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dayOff.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(dayOff.Name))
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
                        UpdateModel(dayOff);
                        dayOff.IsDelete = false;
                        if (!string.IsNullOrEmpty(date))
                            dayOff.Date = date.StringToDecimal();
                        json = new JavaScriptSerializer().Serialize(dayOff);
                        _dnDayOffApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dayOff.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(dayOff.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsdayOff = _dnDayOffApi.GetListByArrId(UserItem.AgencyID, lstId);
                    foreach (var item in ltsdayOff)
                    {
                        item.IsDelete = true;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _dnDayOffApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = dayOff.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsdayOff.Select(c => c.Name))))
                    };
                    break;
                case ActionType.Show:
                    ltsdayOff = _dnDayOffApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    foreach (var item in ltsdayOff)
                    {
                        item.IsDelete = false;
                        item.IsShow = true;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _dnDayOffApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = dayOff.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsdayOff.Select(c => c.Name))))
                    };
                    break;

                case ActionType.Hide:
                    ltsdayOff = _dnDayOffApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    foreach (var item in ltsdayOff)
                    {
                        item.IsDelete = false;
                        item.IsShow = false;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _dnDayOffApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = dayOff.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsdayOff.Select(c => c.Name))))
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
    }
}
