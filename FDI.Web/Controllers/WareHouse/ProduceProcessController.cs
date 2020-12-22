using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA.StorageWarehouse;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.Web.Controllers.WareHouse
{
    public class ProduceProcessController : BaseController
    {
        private readonly ProduceDa _produceDa = new ProduceDa();
        //
        // GET: /ProduceProcess/

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListItems()
        {
            var model = new ProduceModel()
            {
                ListItems = _produceDa.GetListSimpleByRequest(Request),
                PageHtml = _produceDa.GridHtmlPage
            };

            return View(model);
        }
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            switch (DoAction)
            {
                case ActionType.Delete:
                    var item = _produceDa.GetById(ArrId.FirstOrDefault());

                    if (item != null && item.Status == (int)ProduceStatus.New)
                    {
                        item.UserUpdate = UserItem.UserId;
                        item.DateUpdate = ConvertDate.TotalSeconds(DateTime.Now);
                        item.Isdelete = true;
                    }
                    //update status dn_request
                    foreach (var itemMapProduceRequestWare in item.MapProduceRequestWares)
                    {
                        itemMapProduceRequestWare.DN_RequestWare.Status = (int)CORE.DNRequestStatus.New;
                    }

                    _produceDa.DeleteMapDnRequest(item.MapProduceRequestWares.ToList());


                    _produceDa.Save();

                    msg.Message = "Đã xóa lệnh sản xuất";
                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeStatus(int status, int id)
        {
            var item = _produceDa.GetById(id);
            item.Status = status;
            item.DateUpdate = CORE.ConvertDate.TotalSeconds(DateTime.Now);
            item.UserUpdate = UserItem.UserId;

            _produceDa.Save();

            return Json(new JsonMessage());
        }

        public ActionResult AjaxView()
        {
            var model = new ProduceModel();
            model.Produce = _produceDa.GetItemById(ArrId.FirstOrDefault());
            model.CategoryRecipe = _produceDa.GetCategoryRecipe(model.Produce.ProductId);
            model.RequestWareItems = _produceDa.GetDetail(model.Produce.ID);

            return View(model);
        }

    }
}
