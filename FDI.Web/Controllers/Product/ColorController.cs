using System;
using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ColorController : BaseController
    {
        readonly SystemColorAPI _systemColorApi;
       public ColorController()
        {
            _systemColorApi = new SystemColorAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_systemColorApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
      
        public ActionResult AjaxForm()
        {
            var color = new ColorItem { Value = "#ffffff" };
            if (DoAction == ActionType.Edit)
            {
                color = _systemColorApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.AgencyId = UserItem.AgencyID;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(color);
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var color = new ColorItem();
            var lstId = Request["itemId"];
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(color);
                        _systemColorApi.Add(UserItem.AgencyID, url);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = color.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(color.Name))
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
                        UpdateModel(color);
                        _systemColorApi.Update(UserItem.AgencyID, url, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = color.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(color.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                
                case ActionType.Delete:
                    msg = _systemColorApi.Delete(lstId);
                    break;
                case ActionType.Show:
                    msg = _systemColorApi.Show(lstId);
                    break;
                case ActionType.Hide:
                    msg = _systemColorApi.Hide(lstId);
                    break;
                default:
                    msg.Message = "Bạn không được phần quyền cho chức năng này.";
                    msg.Erros = true;
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
