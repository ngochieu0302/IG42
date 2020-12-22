using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;

using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class AdvertisingPositionController : BaseController
    {
        //
        // GET: /Admin/AdvertisingPosition/


        private readonly Advertising_PositionDA _positionDa;

        public AdvertisingPositionController()
        {
            _positionDa = new Advertising_PositionDA("#");
        }
        
        public ActionResult Index()
        {        
            return View();
        }

        public ActionResult ListItems()
        {
            var listAdvertisingPositionItem = _positionDa.GetListSimpleByRequest(Request, Utility.AgencyId);
            var model = new ModelAdvertisingPositionItem
            {
                ListItem = listAdvertisingPositionItem,
                PageHtml = _positionDa.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var positionModel = new Advertising_Position();

            if (DoAction == ActionType.Edit)
                positionModel = _positionDa.GetById(ArrId.FirstOrDefault());

            ViewData.Model = positionModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var position = new Advertising_Position();

            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(position);
                        position.LanguageId = Fdisystem.LanguageId;
                        position.AgencyID = Utility.AgencyId;
                        position.IsDeleted = false;
                        _positionDa.Add(position);
                        _positionDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = position.ID.ToString(),
                            Message =
                                string.Format("Đã thêm mới vùng hiển thị: <b>{0}</b>",
                                              Server.HtmlEncode(position.Name))
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
                        position = _positionDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(position);
                        _positionDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = position.ID.ToString(),
                            Message =
                                string.Format("Đã cập nhật vùng hiển thị: <b>{0}</b>",
                                              Server.HtmlEncode(position.Name))
                        };
                    }

                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    var ltsPositionItems = _positionDa.GetListByArrId(ArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in ltsPositionItems)
                    {
                        item.IsDeleted = true;
                        stbMessage.AppendFormat("Đã xóa banner <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _positionDa.Save();
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
            var ltsResults = _positionDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }

    }
}
