using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class PacketController : BaseController
    {
        readonly PacketAPI _api = new PacketAPI();
        readonly DNBedDeskAPI _bedDeskApi = new DNBedDeskAPI();
        readonly ProductAPI _productApi = new ProductAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new PacketItem
            {
                Sort = 0,
                LstBedDesk = new List<BedDeskItem>(),
                LstProduct = new List<ProductItem>(),
            };
            if (DoAction == ActionType.Edit){
                model = _api.GetPacketItems(ArrId.FirstOrDefault());
            }
            
            ViewBag.Action = DoAction;
            ViewBag.AgencyID = UserItem.AgencyID;
            ViewBag.BedDeskID = _bedDeskApi.GetList(UserItem.AgencyID);
            ViewBag.Product = _productApi.GetListByAgency(UserItem.AgencyID);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage {Erros = false};
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.Update(url) == 0)
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
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductAdd()
        {
            var model = _productApi.GetListByAgency(UserItem.AgencyID);
            return View(model);
        }

        public ActionResult GetPricebiProductID(int id)
        {
            var model = _productApi.GetProductItem(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
