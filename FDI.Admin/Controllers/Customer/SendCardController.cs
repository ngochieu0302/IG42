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
    public class SendCardController : BaseController
    {
        //
        // GET: /Admin/Customer/
        private readonly SendCardDA _da;
        public SendCardController()
        {
            _da = new SendCardDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelSendCardItem
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
            var model = new Send_Card();
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
            var model = new Send_Card();
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        var customerid = Request["CustomerID"];
                        var lstSerial = Request["LstSerial"];
                        var lstInt = FDIUtils.StringToListInt(lstSerial);
                        foreach (var i in lstInt)
                        {
                            _da.Add(new Send_Card
                            {
                                CustomerID = int.Parse(customerid),
                                //CardID = i,
                                DateCreate = ConvertDate.TotalSeconds(DateTime.Now)
                            });
                        }
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.ID.ToString(),
                            Message = "Đã cập nhật thành công"
                        };
                    }
                    catch (Exception ex)
                    {
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
