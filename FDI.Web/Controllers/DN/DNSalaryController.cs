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
    public class DNSalaryController : BaseController
    {
       readonly DNSalaryAPI _dnSalaryApi;
       public DNSalaryController()
        {
            _dnSalaryApi = new DNSalaryAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_dnSalaryApi.GetListSimpleByRequest(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var dnSalary = _dnSalaryApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = dnSalary;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var dnSalary = new DNSalaryItem();
            if (DoAction == ActionType.Edit)
            {
                dnSalary = _dnSalaryApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewData.Model = dnSalary;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var dnSalary = new DNSalaryItem();
            List<DNSalaryItem> ltsDnSalary;
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        // insert lương cơ bản
                        UpdateModel(dnSalary);
                        json = new JavaScriptSerializer().Serialize(dnSalary);
                        _dnSalaryApi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnSalary.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(dnSalary.UserName))
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
                        UpdateModel(dnSalary);
                        json = new JavaScriptSerializer().Serialize(dnSalary);
                        _dnSalaryApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnSalary.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(dnSalary.UserName))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsDnSalary = _dnSalaryApi.GetListByArrId(UserItem.AgencyID, lstId);
                    foreach (var item in ltsDnSalary)
                    {
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _dnSalaryApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = dnSalary.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsDnSalary.Select(c => c.UserName))))
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
