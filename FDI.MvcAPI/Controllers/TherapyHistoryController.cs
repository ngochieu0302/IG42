using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class TherapyHistoryController : BaseApiController
    {
        //
        // GET: /TherapyHistory/
        readonly TherapyHistoryDA _da = new TherapyHistoryDA("#");
        public ActionResult GetListByCustomerID(string key, int cusId,string phone)
        {
            var obj = key != Keyapi
                ? new List<TherapyHistoryItem>()
                : _da.GetListbyCustomerID(cusId,phone);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, int agencyId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Therapy_History();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var date = Request["dateOfsale_"];
                var cus = Request["CustomerID"];
                UpdateModel(model);
                if (!string.IsNullOrEmpty(cus))
                {
                    model.CustomerID = int.Parse(cus);
                }
                model.AgencyID = agencyId;
                model.DateCreate = date.StringToDate().TotalSeconds();
                model.IsShow = true;
                model.IsDelete = false;
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
