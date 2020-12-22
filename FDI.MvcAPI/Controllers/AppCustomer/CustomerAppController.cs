using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using FDI.DA;
using FDI.DA.DA.AppCustomer;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    [CustomAuthorize]
    public class CustomerAppController : BaseApiAuthController
    {
        //
        // GET: /CustomerApp/
        private readonly CusLoginAppDA _dl = new CusLoginAppDA("#");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code">Codelogin</param>
        /// <returns></returns>
        public ActionResult GetUserItemByCode(string key, string code)
        {
            var obj = key != Keyapi ? new CustomerAppItem() : _dl.GetUserItemByCode(code);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult UpdateUser(string key, string username, int gender, string address, string birthday, string fullname, string pass, int aid)
        //{
        //    if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
        //    try
        //    {
        //        var model = _dl.GetByUserName(username);
        //        if (model != null)
        //        {
        //            model.Address = address;
        //            model.Gender = (gender == 1);
        //            if (aid > 0) model.AgencyID = aid;
        //            model.Birthday = ConvertUtil.ToDateTime(birthday).TotalSeconds();
        //            model.FullName = fullname;
        //            if (!string.IsNullOrEmpty(pass))
        //            {
        //                var sha1PasswordHash = FDIUtils.CreatePasswordHash(pass, model.PasswordSalt);
        //                model.PassWord = sha1PasswordHash;
        //            }
        //            _dl.Save();
        //            return Json(1, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(e.Message, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(3, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult UpdateUser(CustomerItem customer)
        {
            try
            {
                var model = _dl.GetByUserID(CustomerId);
                if (model != null)
                {
                    model.Address = customer.Address;
                    model.Gender = (customer.Gender!=null && customer.Gender.Value);
                    model.FullName = customer.FullName;
                    model.Longitude = customer.Longitude;
                    model.Latitude = customer.Latitude;
                    model.LatitudeBuyRecently = customer.LatitudeBuyRecently;
                    model.LongitudeBuyRecently = customer.LongitudeBuyRecently;
                    model.AddressBuyRecently = customer.AddressBuyRecently;
                  
                    _dl.Save();
                    return Json(new JsonMessage(false,"Cập nhật thành công"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(true, e.Message), JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonMessage(true, "Đã có lỗi xảy ra"), JsonRequestBehavior.AllowGet);
        }

        public string GenQrCodeCustomer(string key, int cusId)
        {
            if (key != Keyapi) return "";
            var model = _dl.GetByUserID(cusId);
            try
            {
                model.QRCode = "IG4Qr" + cusId + DateTime.Now.Second + DateTime.Now.Millisecond;
                _dl.Save();
                return model.QRCode;
            }
            catch (Exception)
            {
                return model.QRCode;
            }
        }

        [HttpPost]
        public ActionResult GetInfo()
        {
            var customer = _dl.GetCustomerItem(CustomerId);
            return Json(customer);
        }

        [HttpPost]
        public ActionResult ChangePass(string oldPass, string newPass)
        {
            var customer = _dl.GetByUserID(CustomerId);
            var pas = FDIUtils.CreatePasswordHash(oldPass, customer.PasswordSalt);

            if (customer.PassWord != pas)
            {
                return Json(new JsonMessage(true, "Mật khẩu cũ không đúng"));
            }

            var saltKey = FDIUtils.CreateSaltKey(5);
            var sha1PasswordHash = FDIUtils.CreatePasswordHash(newPass, saltKey);
            customer.PasswordSalt = saltKey;
            customer.PassWord = sha1PasswordHash;
            _dl.Save();

            return Json(new JsonMessage(false, "Đổi mật khẩu thành công"));
        }

        [HttpPost]
        public ActionResult UpdateAgency(int id)
        {
            var model = _dl.GetByUserID(CustomerId);
            if (model.AgencyID> 0)
            {
                return Json(new JsonMessage(true, "Không được thay đổi đại lý"));
            }
            model.AgencyID = id;
            _dl.Save();
            return Json(new JsonMessage(false, "Cập nhật thành công"));
        }
    }
}
