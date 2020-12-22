using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.AppCustomer;
using FDI.DA.DA.AppSales;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.APPSales
{
    public class UserAppController : BaseApiAuthAppSaleController
    {
        //
        // GET: /UserApp/
        readonly UserAppDA _da = new UserAppDA("#");
        readonly AgencyDA _agencyDa = new AgencyDA("#");
        private readonly CusLoginAppDA _dl = new CusLoginAppDA("#");
        public ActionResult GetItemByUsername(string key, string username)
        {
            var obj = Request["key"] != Keyapi ? new DNUserAppItem() : _da.GetItemByUsername(username);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUser(string key, string username, int gender, string address, string birthday,
            string fullname, string pass, string company, string depart, string mst, string stk, string bankname, string latitute, string longitude)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            try
            {
                var model = _da.GetByUsername(username);
                if (model != null)
                {
                    model.Address = address;
                    model.Gender = gender == 1;
                    model.BirthDay = ConvertUtil.ToDateTime(birthday).TotalSeconds();
                    model.LoweredUserName = fullname;
                    if (!string.IsNullOrEmpty(pass))
                    {
                        var saltKey = FDIUtils.CreateSaltKey(5);
                        var sha1PasswordHash = FDIUtils.CreatePasswordHash(pass, saltKey);
                        model.Password = sha1PasswordHash;
                    }
                    var agency = _agencyDa.GetById(model.AgencyID ?? 0);
                    agency.Company = company;
                    agency.MST = mst;
                    agency.Department = depart;
                    agency.STK = stk;
                    agency.BankName = bankname;
                    agency.Latitute = latitute;
                    agency.Longitude = longitude;
                    _agencyDa.Save();
                    _da.Save();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(3, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetInfo()
        {
            var userid = _da.GetItemById(AgencyUserId);
            var agengy = _agencyDa.GetItemById(userid.AgencyID);

            return Json(agengy);
        }

        [HttpPost]
        public ActionResult GetCustomerInfo(string userName)
        {
            var _customerDA = new CustomerDA();
            var customer = _customerDA.GetCustomerItemByUserName(userName);
            
            return Json(customer);
        }
    }
}
