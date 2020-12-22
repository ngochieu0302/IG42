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
    public class HomeNetworkController : BaseController
    {
        private readonly HomeNetworkDA _da;

        public HomeNetworkController()
        {
            _da = new HomeNetworkDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelDNHomeNetworkItem
            {
                ListItem = _da.GetListSimpleByRequest(Request,Utility.AgencyId),
                PageHtml = _da.GridHtmlPage
            };            
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var customerType = _da.GetById(ArrId.FirstOrDefault());
            ViewData.Model = customerType;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var model = new HomeNetwork();
            if (DoAction == ActionType.Edit)
            {
                model = _da.GetById(ArrId.FirstOrDefault());
            }            
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var model = new HomeNetwork();            
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);                       
                        _da.Add(model);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.ID.ToString(),
                            Message = string.Format("Đã thêm mới vị trí : <b>{0}</b>", Server.HtmlEncode(model.Name))
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
                        model = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(model);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(model.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
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
