using System;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using FDI.Web.Common;
using System.Collections.Generic;
using FDI.CORE;

namespace FDI.Web.Areas.Contact.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactBL _bl = new ContactBL();
        private readonly CustomerBL _cusBl = new CustomerBL();
        private readonly ContactDL _dl = new ContactDL();
        private readonly ModuleSettingBL _sysbl = new ModuleSettingBL();
        readonly GoogleMapBL _googleMapBl = new GoogleMapBL();
        public PartialViewResult Config(int ctrId, string url)
        {
            var model = new ModelSystemConfigItem
            {
                Item = _bl.SysConfigItems(),
                LstMapItems = _googleMapBl.GetAllListSimple(),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }
        
        public ActionResult Address()
        {
            var model = _bl.SysConfigItems();
            return PartialView(model);
        }
        
        public PartialViewResult FanPage(int ctrId, string url)
        {
            var model = new ModelSystemConfigItem { CtrId = ctrId, CtrUrl = url };
            var item = _sysbl.GetKey(ctrId);
            model.Value = item != null ? item.Value : "";
            return PartialView(model);
        }
        public PartialViewResult Contact(int ctrId, string url)
        {
            var model = new ModelSystemConfigItem
            {
                Item = _bl.SysConfigItems(),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }
        public PartialViewResult Contact2(int ctrId, string url)
        {
            var model = new ModelSystemConfigItem
            {
                Item = _bl.SysConfigItems(),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }
        public PartialViewResult ContactPartner(int ctrId, string url)
        {
            //ViewBag.Config = _bl.SysConfig();
            //var model = _bl.GetGoogleMap();
            //return PartialView(model);
            return PartialView();
        }

        
        public JsonResult SendContact()
        {
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Cảm ơn bạn đã gửi liên hệ cho chúng tôi. Chúng tôi sẽ phản hồi lại bạn trong thời gian sớm nhất",
            };
            var randomCode = Session["RandomCode"].ToString();
            var capcha = Request["Capcha"];
            if (!randomCode.Equals(capcha))
            {
                msg.Erros = true;
                msg.Message = "Mã xác nhận không đúng!";
                return Json(msg);
            }
            var cus = new CustomerContact();
            try
            {
                UpdateModel(cus);
                cus.CreatedOnUtc = DateTime.Now;
                cus.IsDelete = false;
                cus.Status = false;
                cus.IsShow = true;
                _dl.Add(cus);
                _dl.Save();
                var model = _bl.SysConfigItems();
                var content = string.Format("Họ tên: {0}, ĐT: {1}, Email: {2}, Nội dung: {3}", cus.Name,
                    cus.Phone, cus.Email, cus.Message);
                Utility.SendEmail(model.EmailSend, model.EmailSendPwd, model.EmailReceive, cus.Subject, content);
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Vui lòng thử lại!";
            }
            return Json(msg);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendContact2()
        {
            var msg = new JsonMessage();
            var cus = new CustomerContact();
            try
            {
                var randomCode = Session["RandomCode"].ToString();
                var capcha = Request["Capcha"];
                if (!randomCode.Equals(capcha))
                {
                    msg = new JsonMessage
                    {
                        Erros = true,
                        Message = "Mã xác nhận không đúng!"
                    };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                UpdateModel(cus);
                cus.CreatedOnUtc = DateTime.Now;
                cus.IsDelete = false;
                cus.Status = false;
                cus.IsShow = true;
                _dl.Add(cus);
                _dl.Save();
                var model = _bl.SysConfigItems();
                var content = string.Format("Họ tên: {0}, ĐT: {1},Email:{2}  Nội dung: {3}", cus.Name, cus.Phone,cus.Email,cus.Message);
                Utility.SendEmail(model.EmailSend, model.EmailSendPwd, model.EmailReceive, cus.Subject, content);
                msg = new JsonMessage
                {
                    Erros = false,
                    Message = "Cảm ơn bạn đã gửi liên hệ cho chúng tôi. Chúng tôi sẽ phản hồi lại bạn trong thời gian sớm nhất",
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Vui lòng thử lại!";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendEmail()
        {
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Cảm ơn bạn đã gửi liên hệ cho chúng tôi. Chúng tôi sẽ phản hồi lại bạn trong thời gian sớm nhất",
            };
            var cus = new CustomerContact();

            try
            {
                UpdateModel(cus);
                cus.CreatedOnUtc = DateTime.Now;
                cus.IsDelete = false;
                cus.Status = false;
                cus.IsShow = true;
                _dl.Add(cus);
                _dl.Save();
                var model = _bl.SysConfigItems();
                var content = string.Format("Địa chỉ Email: {0}", cus.Email);
                Utility.SendEmail(model.EmailSend, model.EmailSendPwd, model.EmailReceive, cus.Subject, content);
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Vui lòng thử lại!";
            }
            return Json(msg);
        }
        public CapcharResult ShowCaptchaImage()
        {
            return new CapcharResult();
        }
        public JsonResult Register()
        {
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Đăng ký thành công",
            };
            try
            {
                var cus = new Customer();
                var da = new CustomerDA();
                UpdateModel(cus);
                cus.DateCreated = DateTime.Now.TotalSeconds();
                cus.IsDelete = false;
                cus.PasswordSalt = FDIUtils.RandomKey(8);
                cus.PassWord = FDIUtils.Encrypt(cus.PassWord, cus.PasswordSalt);
                da.Add(cus);
                da.Save();
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Vui lòng thử lại!";
            }
            return Json(msg);
        }
        public ActionResult Infomation()
        {
            var user = Utility.Getcookie("c_user");
            if (string.IsNullOrEmpty(user)) return PartialView();
            var txtId = FDIUtils.Decrypt(user, "@123");
            var model = _cusBl.GetByid(int.Parse(txtId));
            return PartialView(model);
        }
    }
}
