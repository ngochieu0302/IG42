using System;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class BonusTypeController : BaseController
    {
        //
        // GET: /Admin/Customer/
        private readonly BonusTypeDA _da;
        public BonusTypeController()
        {
            _da = new BonusTypeDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelBonusTypeItem
            {
                ListItems = _da.GetListSimpleByRequest(Request),
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _da.GetById(ArrId.FirstOrDefault());
            return View(model);
        }
       
        public ActionResult AjaxForm()
        {
            var model = new BonusType();
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
            var model = new BonusType();
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        model.DateCreate = DateTime.Now.TotalSeconds();
                        model.IsExit = false;
                        _da.Add(model);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.ID.ToString(),
                            Message = model.Name
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
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(model.Name))
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
