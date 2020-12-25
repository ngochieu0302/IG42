using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AuthenticationService.Managers;
using AuthenticationService.Models;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.AppCustomer;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class CusLoginAppController : BaseApiAuthController
    {
        //
        // GET: /CusLoginApp/
        private readonly CusLoginAppDA _dl = new CusLoginAppDA("#");
        private readonly CustomerDA _da = new CustomerDA("#");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code">Codelogin</param>
        /// <returns></returns>
        public ActionResult Logout(string key, string code)
        {
            if (key == Keyapi)
            {
                _dl.Logout(code);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllGroup(string key)
        {
            if (key == Keyapi)
            {
                return Json(_dl.GetAllGroup(), JsonRequestBehavior.AllowGet);
            }
            return Json(new List<CustomerGroupItem>(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code">Codelogin khởi tạo</param>
        /// <param name="username">Tài khoản login</param>
        /// <param name="pass">Pass login</param>
        /// <param name="ischeck">Trạng thái duy trì 5 ngày</param>
        /// <returns></returns>
        public ActionResult Login(string key, string username, string pass, bool ischeck)
        {
            var objr = new CustomerAppItem
            {
                Status = 0,
            };
            //var lg = new Ultils();
            var code = Guid.NewGuid().ToString();

            //  if (key != Keyapi) return Json(objr, JsonRequestBehavior.AllowGet);
            var obj = _dl.GetPassByUserName(username);
            if (obj != null)
            {
                var date = DateTime.Now;
                var dateend = date.AddMinutes(20);
                if (ischeck) dateend = date.AddDays(5);
                var timeend = dateend.TotalSeconds();
                var pas = FDIUtils.CreatePasswordHash(pass, obj.PasswordSalt);
                if (obj.Password == pas)
                {
                    var dNlogin = new DN_Login
                    {
                        CustomerID = obj.ID,
                        DateCreated = date.TotalSeconds(),
                        DateEnd = timeend,
                        Code = code,
                        IsOut = false
                    };
                    _dl.Add(dNlogin);
                    _dl.Save();
                    obj.UserName = obj.UserName;
                    obj.CodeLogin = code;
                    obj.Status = 1;
                    obj.ID = obj.ID;

                    IAuthContainerModel model = new JWTContainerModel()
                    {
                        Claims = new Claim[]
                        {
                            new Claim(ClaimTypes.Name, obj.UserName),
                            new Claim("ID",obj.ID.ToString()),
                        }
                    };
                    IAuthService authService = new JWTService();
                    var token = authService.GenerateToken(model);
                    var result = new BaseResponse<CustomerItem>()
                    {
                        Erros = false,
                        Data = new CustomerItem()
                        {
                            FullName = obj.FullName,
                            Phone = obj.Phone,
                            Token = token
                        }
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonMessage(true, "Mật khẩu không đúng"), JsonRequestBehavior.AllowGet);
            }


            return Json(new JsonMessage(true, "Tài khoản không tồn tại"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="phone">UserName</param>
        /// <param name="mobile">SĐT3G</param>
        /// <param name="pass">Mật khẩu</param>
        /// <param name="address">Địa chỉ</param>
        /// <returns></returns>
        public ActionResult Addapp(string key, string phone, string mobile, string pass, string address, string name, int gid)
        {
            var objr = new CustomerAppItem
            {
                UserName = phone,
                Status = 0
            };
            try
            {
                if (key == Keyapi && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(pass))
                {
                    if (_da.CheckUserName(phone)) return Json(objr, JsonRequestBehavior.AllowGet);
                    var daten = DateTime.Now;
                    var date = daten.TotalSeconds();
                    var saltKey = FDIUtils.CreateSaltKey(5);
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(pass, saltKey);
                    var obj = new Base.Customer
                    {
                        Address = address,
                        FullName = name,
                        GroupID = gid,
                        Phone = phone,
                        PasswordSalt = saltKey,
                        PassWord = sha1PasswordHash,
                        UserName = phone,
                        DateCreated = date,
                        IsDelete = false,
                        IsActive = true,
                        Reward = 0,
                    };
                    _da.Add(obj);
                    _da.Save();
                    var datee = daten.AddDays(5).TotalSeconds();
                    //var lg = new Ultils();
                    var code = Ultils.CodeLogin(daten);
                    var dNlogin = new DN_Login
                    {
                        CustomerID = obj.ID,
                        DateCreated = date,
                        DateEnd = datee,
                        Code = code,
                        IsOut = false
                    };
                    _dl.Add(dNlogin);
                    _dl.Save();
                    objr = new CustomerAppItem
                    {
                        ID = obj.ID,
                        UserName = phone,
                        Address = address,
                        FullName = name,
                        Phone = mobile,
                        Reward = 0,
                        GroupID = gid,
                        Status = 1,
                    };
                }
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
            }
            return Json(objr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(CustomerItem customer)
        {
            var objr = new CustomerAppItem
            {
                Status = 0
            };

            try
            {
                if (!string.IsNullOrEmpty(customer.Phone) && !string.IsNullOrEmpty(customer.Password))
                {
                    if (_da.CheckUserName(customer.Phone))
                    {

                        return Json(new BaseResponse<CustomerItem>()
                        {
                            Erros = true,
                            Message = "Số điện thoại đã tồn tại",
                        }, JsonRequestBehavior.AllowGet);
                    }
                    var daten = DateTime.Now;
                    var date = daten.TotalSeconds();
                    var saltKey = FDIUtils.CreateSaltKey(5);
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(customer.Password, saltKey);

                    //get agencyinfo 
                    var agencyDA = new AgencyDA();
                    var agency = agencyDA.GetItem(customer.PhoneAgency);
                    var obj = new Base.Customer
                    {
                        FullName = customer.FullName,
                        Phone = customer.Phone,
                        PasswordSalt = saltKey,
                        PassWord = sha1PasswordHash,
                        UserName = customer.UserName,
                        DateCreated = date,
                        IsDelete = false,
                        IsActive = true,
                        Reward = 0,
                        AgencyID = agency?.ID
                    };
                    _da.Add(obj);
                    _da.Save();

                    IAuthContainerModel model = new JWTContainerModel()
                    {
                        Claims = new Claim[]
                        {
                            new Claim(ClaimTypes.Name, obj.UserName),
                            new Claim("ID",obj.ID.ToString()),
                        }
                    };

                    IAuthService authService = new JWTService();
                    var token = authService.GenerateToken(model);
                    var result = new BaseResponse<CustomerItem>()
                    {
                        Erros = false,
                        Data = new CustomerItem()
                        {
                            FullName = obj.FullName,
                            Phone = obj.Phone,
                            Token = token
                        }
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);

                    var datee = daten.AddDays(5).TotalSeconds();
                    //var lg = new Ultils();
                    var code = Ultils.CodeLogin(daten);
                    var dNlogin = new DN_Login
                    {
                        CustomerID = obj.ID,
                        DateCreated = date,
                        DateEnd = datee,
                        Code = code,
                        IsOut = false
                    };
                    _dl.Add(dNlogin);
                    _dl.Save();
                    objr = new CustomerAppItem
                    {
                        ID = obj.ID,
                        Status = 1,
                    };
                }
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
            }
            return Json(objr, JsonRequestBehavior.AllowGet);
        }
    }
}
