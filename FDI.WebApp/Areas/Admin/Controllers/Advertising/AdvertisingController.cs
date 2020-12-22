using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class AdvertisingController : BaseController
    {
        //
        // GET: /Admin/Advertising/
        

        readonly AdvertisingDA _advertisingDa = new AdvertisingDA("#");
        readonly Advertising_TypeDA _typeDa = new Advertising_TypeDA();
        private readonly Advertising_PositionDA _positionDa;

        public AdvertisingController()
        {
            _positionDa = new Advertising_PositionDA();
        }
        
        public ActionResult Index()
        {
            var model = new ModelAdvertisingPositionItem
            {
                ListItem = _positionDa.GetAllListSimple(Utility.AgencyId),
            };
            return View(model);
        }
       
        public ActionResult ListItems()
        {
            var listactiveRoleItem = _advertisingDa.GetListSimpleByRequest(Request, Utility.AgencyId);
            var model = new ModelAdvertisingItem
            {
                ListItem = listactiveRoleItem,
                PageHtml = _advertisingDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var advertisingModel = _advertisingDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = advertisingModel;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var advertisingModel = new Advertising
            {
                Show = true,
            };

            if (DoAction == ActionType.Edit)
                advertisingModel = _advertisingDa.GetById(ArrId.FirstOrDefault());
            ViewBag.AdvertisingTypeID = _typeDa.GetAllListSimple();
            ViewBag.AdvertisingPositionID = _positionDa.GetAllListSimple(Utility.AgencyId);
            ViewData.Model = advertisingModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var advertising = new Advertising();
            List<Advertising> ltsAdvertisingsItems;
            StringBuilder stbMessage;

            var pictureId = Request["Value_DefaultImages"];
            //var positionId = Request["PositionID"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(advertising);
                        if (!string.IsNullOrEmpty(pictureId))
                            advertising.PictureID = int.Parse(pictureId);
                        //if (!string.IsNullOrEmpty(positionId))
                        //    advertising.PositionID = Convert.ToInt32(positionId);
                        advertising.IsDeleted = false;
                        advertising.CreateOnUtc = DateTime.Now.TotalSeconds();
                        advertising.LanguageId = Fdisystem.LanguageId;
                        _advertisingDa.Add(advertising);
                        _advertisingDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = advertising.ID.ToString(),
                            Message =
                                string.Format("Đã thêm mới banner: <b>{0}</b>", Server.HtmlEncode(advertising.Name))
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
                        advertising = _advertisingDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(advertising);

                        if (!string.IsNullOrEmpty(pictureId))
                            advertising.PictureID = int.Parse(pictureId);

                        //if (!string.IsNullOrEmpty(Request["PositionID"]))
                        //    advertising.PositionID = Convert.ToInt32(Request["PositionID"]);
                        _advertisingDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = advertising.ID.ToString(),
                            Message =
                                string.Format("Đã cập nhật banner: <b>{0}</b>", Server.HtmlEncode(advertising.Name))
                        };
                    }

                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsAdvertisingsItems = _advertisingDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsAdvertisingsItems)
                    {
                        try
                        {
                            item.IsDeleted = true;
                            stbMessage.AppendFormat("Đã xóa banner <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {

                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                    }
                    msg.ID = string.Join(",", ArrId);
                    _advertisingDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsAdvertisingsItems = _advertisingDa.GetListByArrId(ArrId).Where(o => o.Show != true).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsAdvertisingsItems)
                    {
                        try
                        {
                            item.Show = true;
                            stbMessage.AppendFormat("Đã hiển thị banner <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {

                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                    }
                    _advertisingDa.Save();
                    msg.ID = string.Join(",", ltsAdvertisingsItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsAdvertisingsItems = _advertisingDa.GetListByArrId(ArrId).Where(o => o.Show == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsAdvertisingsItems)
                    {
                        try
                        {
                            item.Show = false;
                            stbMessage.AppendFormat("Đã ẩn banner <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {

                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                    }
                    _advertisingDa.Save();
                    msg.ID = string.Join(",", ltsAdvertisingsItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _advertisingDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }

    }
}
