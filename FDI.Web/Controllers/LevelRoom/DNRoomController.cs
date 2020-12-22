using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNRoomController : BaseController
    {
        private readonly DNRoomAPI _api = new DNRoomAPI();
        private readonly DNLevelRoomAPI _apilevel = new DNLevelRoomAPI();
        private readonly DNBedDeskAPI _bedDeskApi = new DNBedDeskAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            ViewData.Model = _bedDeskApi.GetListByRoomId(ArrId.FirstOrDefault());
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new DNRoomItem
            {
                Sort = 0,
                Column = 1,
                Row = 1
            };
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            if (DoAction == ActionType.Edit)
            {
                model = _api.GetItemById(ArrId.FirstOrDefault());
            }
            ViewBag.list = _apilevel.GetList(UserItem.AgencyID);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(UserItem.AgencyID, url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.Update(UserItem.AgencyID, url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    _api.Delete(lst1);
                    msg.Message = "Cập nhật thành công.";
                    break;
                case ActionType.View:
                   var id = Request["id"];
                   _bedDeskApi.Hide(id, UserItem.AgencyID, CodeLogin());
                    msg.Message = "Cập nhật thành công.";
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
