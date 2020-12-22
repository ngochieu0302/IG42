using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using FDI.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDI.Web.Controllers
{
    public class BannerController : BaseController
    {
        //
        // GET: /Banner/
        BannerDA da = new BannerDA();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelBannerItem() { ListItem = da.GetAllListSimple(Request), PageHtml = da.GridHtmlPage };

            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var model = new BannerItem();
            if (DoAction == ActionType.Edit)
            {
                model = da.GetItemById(ArrId.FirstOrDefault());
            }

            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Actions()
        {
            var msg = new JsonMessage(false,"");         
            var model = new BannerItem();
        
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(model);
                    var objBanner = new Banner
                    {
                       Name = model.Name,
                       UrlView = model.UrlView,
                       PictureID = model.PictureID,
                       //Datas =model.Datas,
                       Sort = model.Sort,
                       UserId = UserId,
                       DateCreate = DateTime.Now.TotalSeconds(),
                    };
                    da.Add(objBanner);
                    da.Save();
                    break;
                case ActionType.Edit:
                    UpdateModel(model);
                    var modelDb = da.GetById(ArrId.FirstOrDefault());
                    modelDb.Name = model.Name;
                    modelDb.UrlView = model.UrlView;
                    //modelDb.Datas = model.Datas;
                    modelDb.Description = model.Description;
                    modelDb.Details = model.Details;
                    modelDb.Sort = model.Sort;
                    da.Save();
                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }


}
