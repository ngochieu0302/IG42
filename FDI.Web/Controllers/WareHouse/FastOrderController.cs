using FDI.CORE;
using FDI.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.Web.Controllers.WareHouse
{
    public class FastOrderController : BaseController
    {
        private readonly ContactOrderDA _da = new ContactOrderDA();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new FDI.Simple.ModelOrderDetailItem();

            model.ListItems = _da.GetFastOrder(Request, DateTime.Today.TotalSeconds());
            model.PageHtml = _da.GridHtmlPage;

            return View(model);
        }

        public ActionResult Actions()
        {
            var msg = new JsonMessage {Erros = false};
            var model = new ProduceItem();
            switch (DoAction)
            {
                case ActionType.Delete:
                    var id = GuiId.FirstOrDefault();
                    var obj = _da.GetDetailById(id);
                    obj.IsDeleted = true;
                    _da.Save();
                    return Json(new JsonMessage(false,""));
                    break;
            }

            return Json(1);

        }
    }
}
