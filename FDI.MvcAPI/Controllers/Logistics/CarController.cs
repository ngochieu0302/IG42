using FDI.DA.DA.Supplier;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA.Logistics;
using FDI.Simple.Logistics;

namespace FDI.MvcAPI.Controllers.Supplier
{
    public class CarController : BaseApiAuthController
    {
        //
        // GET: /SupplierAmountProduct/
        private readonly CarDA _da = new CarDA();

        public ActionResult ListItems()
        {
            var obj = new CarResponse() { ListItem = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(CarItem request)
        {
            _da.Add(new Car()
            {
                Name = request.Name,
                Address = request.Address,
                CarType = request.CarType,
                IsDelete = false,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Phone = request.Phone,
                Quantity = request.Quantity,
                UnitID = request.UnitID
            });

            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(CarItem request)
        {
            var model = _da.GetById(request.ID);
            model.Name = request.Name;
            model.Address = request.Address;
            model.Phone = request.Phone;
            model.CarType = request.CarType;
            model.Quantity = request.Quantity;
            model.Latitude = request.Latitude;
            model.Longitude = request.Longitude;
            model.UnitID = request.UnitID;
            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var model = _da.GetById(id);
            model.IsDelete = true;
            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            return Json(_da.GetItemById(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll()
        {
            return Json(_da.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListAssign(int unitId)
        {
            return Json(_da.GetListAssign(unitId));
        }
    }
}
