using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class SaleFashionController : BaseController
    {
        private readonly OrderAPI _ordersApi = new OrderAPI();
        private readonly ProductAPI _productApi = new ProductAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        private readonly CustomerAPI _customerApi = new CustomerAPI();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View();
        }
        public ActionResult Report()
        {
            var model = _agencyApi.GetItemById(UserItem.AgencyID);
            return View(model);
        }

        public ActionResult Auto()
        {
            var query = Request["query"];
            var type = Request["type"] ?? "0";
            var ltsResults = _productApi.GetListAuto(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults.Select(c => new SuggestionsProduct
                {
                    ID = c.ID,
                    IsCombo = 0,
                    value = c.value,
                    title = c.title,
                    data = "Giá: " + c.pricenew.Money(),
                    name = "Mã SP: " + c.value + " | Màu: " + c.Color + " | Size: " + c.Size,
                    pricenew = c.pricenew,
                    Unit = c.Unit,
                    Type = c.Type
                })
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoCustomer()
        {
            var query = Request["query"];
            query = query.Replace("%", "");
            query = query.Replace("?", "");
            var ltsResults = _customerApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
            var order = new OrderGetItem();
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(order);
                    var dateOfSale = Request["DateOfSale_"];
                    order.EndDate = dateOfSale.StringToDecimal();
                    var json = new JavaScriptSerializer().Serialize(order);
                    var b = _ordersApi.AddFashion(UserItem.AgencyID, json, UserItem.UserId, CodeLogin(), "");
                    if (b == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    break;
                default:
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg);
        }
    }
}
