using FDI.DA.DA.Supplier;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA.Logistics;
using FDI.Simple.Logistics;

namespace FDI.MvcAPI.Controllers.Supplier
{
    public class OrderCarController : BaseApiAuthController
    {
        private readonly OrderCarDa _da = new OrderCarDa();
        private  readonly  OrderCarProductDetailDA _carProductDetailDa = new OrderCarProductDetailDA();
        //
        // GET: /SupplierAmountProduct/


        public ActionResult ListItems(decimal todayCode)
        {
            var obj = new OrderCarResponse() { ListItem = _da.GetListSimpleByRequest(Request, todayCode), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItemsByStatus(int[] status)
        {
            var lst = new OrderCarResponse() { ListItem = _da.GetListSimpleByRequest(Request, status), PageHtml = _da.GridHtmlPage };
            foreach (var orderCarItem in lst.ListItem)
            {
                
                orderCarItem.QuantityReceived = _carProductDetailDa.GetAmountRecevied(orderCarItem.ID)??0;
            }
            
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Add(OrderCarItem request)
        {
            _da.Add(new OrderCar()
            {
                SupplierId = request.SupplierId,
                Quantity = request.Quantity,
                CarId = request.CarId,
                Code = request.Code,
                DateCreate = ConvertDate.TotalSeconds(DateTime.Now),
                DepartureDate = request.DepartureDate,
                ReceiveDate = request.ReceiveDate,
                ReturnDate = request.ReturnDate,
                ProductId = request.ProductId,
                Price = request.Price,
                PriceNow = request.PriceNow,
                IsDelete = false,
                Status = (int)OrderCarStatus.News,
                UserCreateId = Userid.Value,
                TodayCode = request.TodayCode.Value,
                WorkshopID = request.WorkshopID
            });

            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(OrderCarItem request)
        {
            var model = _da.GetById(request.ID);
            model.Quantity = request.Quantity;
            model.DepartureDate = request.DepartureDate;
            model.ReturnDate = request.ReturnDate;
            model.ReceiveDate = request.ReceiveDate;
            model.Status = (int) request.Status;
            model.CarId = request.CarId;
            model.WorkshopID = request.WorkshopID;

            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var model = _da.GetById(id);
            var status = (OrderCarStatus)model.Status;
            if (status != OrderCarStatus.News)
            {
                return Json(new JsonMessage() { Erros = true, Message = $"Trạng thái {status.GetDisplayName()} không thể xóa" }, JsonRequestBehavior.AllowGet);
            }
            model.IsDelete = true;
            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            return Json(_da.GetItemById(id), JsonRequestBehavior.AllowGet);
        }

    }
}
