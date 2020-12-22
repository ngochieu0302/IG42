using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Document
{
    public class TemplateDocumentController : BaseController
    {
        //
        // GET: /TemplateDocument/
        private readonly TemplateDocumentDA _da = new TemplateDocumentDA("#");
        private readonly TemplateDocumentAPI _api = new TemplateDocumentAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.GetListSimpleByRequest(Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new TemplateDocumentItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetTemplateDocItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            ViewBag.ActionText = ActionText;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Actions()
        {
            var msg = new JsonMessage();
            var model = new TemplateDocument();
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(model);
                    model.IsShow = true;
                    model.IsDelete = false;
                    model.DateCreate = DateTime.Now.TotalSeconds();
                    _da.Add(model);
                    _da.Save();
                    break;

                case ActionType.Edit:
                    model = _da.GetById(ArrId.FirstOrDefault());
                    UpdateModel(model);
                   await _api.Update(model);
                    break;
                case ActionType.Delete:
                    var lst = string.Join(",", ArrId);
                    msg = _api.Delete(lst);
                    break;
                default:
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
