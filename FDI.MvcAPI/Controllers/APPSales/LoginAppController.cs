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
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class LoginAppController : BaseApiAuthAppSaleController
    {
        //
        // GET: /DNUser/

        private readonly DNLoginAppDA _dl = new DNLoginAppDA("#");
        private readonly AgencyDA _da = new AgencyDA();
        private readonly DNUserDA _daUserDa = new DNUserDA("#");
        private readonly DNRoleDA _darRoleDa = new DNRoleDA("#");
        private readonly CustomerDA _customerDa = new CustomerDA("#");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code">Codelogin</param>
        /// <returns></returns>
        public ActionResult GetUserItemByCode(string key, string code)
        {
            var obj = key != Keyapi ? new DNUserAppItem() : _dl.GetUserItemByCode(code);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllGroup(string key)
        {
            if (key == Keyapi)
            {
                return Json(_dl.GetAllGroup(), JsonRequestBehavior.AllowGet);
            }
            return Json(new List<GroupAgencyItem>(), JsonRequestBehavior.AllowGet);
        }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code">Codelogin khởi tạo</param>
        /// <param name="username">Tài khoản login</param>
        /// <param name="pass">Pass login</param>
        /// <param name="ischeck">Trạng thái duy trì 5 ngày</param>
        /// <param name="domain">App</param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Login(string username, string pass)
        {

            var obj = _dl.GetPassByUserName(username, true);

            if (obj == null)
            {
                return Json(new BaseResponse<AgencyItem>() { Erros = true, Message = "Tên tài khoản không đúng" });
            }

            var pas = FDIUtils.CreatePasswordHash(pass, obj.PasswordSalt);
            if (obj.Password != pas)
            {
                return Json(new BaseResponse<AgencyItem>() { Erros = true, Message = "Mật khẩu không đúng" });
            }
            if (obj.isLock != true)
            {
                return Json(new BaseResponse<AgencyItem>() { Erros = true, Message = "Tài khoản chưa được kích hoạt" });
            }

            IAuthContainerModel model = new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, obj.UserName),
                    new Claim("ID",obj.UserId.ToString()),
                    new Claim("AgencyId",obj.AgencyID.ToString()),
                }
            };
                IAuthService authService = new JWTService();
                var token = authService.GenerateToken(model);
                var result = new BaseResponse<AgencyItem>()
                {
                    Erros = false,
                    Data = new AgencyItem()
                    {
                        FullName = obj.FullName,
                        Phone = obj.Phone,
                        Token = token,
                        AgencyLevelId = obj.AgencyLevelId
                    }
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <param name="mobile">UserName</param>
            /// <param name="mobile">SĐT3G</param>
            /// <param name="pass">Mật khẩu</param>
            /// <param name="address">Địa chỉ</param>
            /// <returns></returns>
            public ActionResult Addapp(string mobile, string pass, string address, string name, string email)
            {
                int gid = 3;
                var objr = new DNUserAppItem
                {
                    UserName = mobile,
                    EnterprisesID = 3,
                    Status = 0
                };
                try
                {
                    if (string.IsNullOrEmpty(mobile))
                    {
                        return Json(new BaseResponse<AgencyItem>() { Erros = true, Message = "Số điện thoại không được để trống" });
                    }
                    if (string.IsNullOrEmpty(pass))
                    {
                        return Json(new BaseResponse<AgencyItem>() { Erros = true, Message = "Mật khẩu không được để trống" });
                    }
                    if (_daUserDa.CheckUserName(mobile, true))
                    {
                        return Json(new BaseResponse<AgencyItem>() { Erros = true, Message = "Số điện thoại đã tồn tại" });
                    }

                    var daten = DateTime.Now;
                    var date = daten.TotalSeconds();
                    var obj = new DN_Agency
                    {
                        Address = address,
                        FullName = name,
                        Name = mobile,
                        Phone = mobile,
                        GroupID = gid,
                        EnterpriseID = 3,
                        IsShow = true,
                        IsDelete = false,
                        IsLock = false,
                        IsOut = false,
                        IsFdi = false,
                        CreateDate = date,
                        WalletValue = 0,
                        CashOut = 0,
                        Email = email

                    };
                    _da.Add(obj);
                    _da.Save();
                    if (obj.GroupID > 0) _da.InsertDNModule(obj.GroupID, obj.ID);
                    var saltKey = FDIUtils.CreateSaltKey(5);
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(pass, saltKey);
                    var user = new DN_Users
                    {
                        UserId = Guid.NewGuid(),
                        PasswordSalt = saltKey,
                        Password = sha1PasswordHash,
                        UserName = mobile,
                        LoweredUserName = name,
                        Email = pass,
                        Address = address,
                        AgencyID = obj.ID,
                        Mobile = mobile,
                        IsApproved = true,
                        IsLockedOut = false,
                        CreateDate = date,
                        IsDeleted = false,
                        IsAgency = true
                    };
                    _daUserDa.Add(user);
                    _daUserDa.Save();
                    var role = new DN_Roles { RoleId = Guid.NewGuid(), RoleName = "Admin", LoweredRoleName = "admin", AgencyID = obj.ID, Description = "Quản trị" };
                    _darRoleDa.Add(role);
                    var dnUsersInRoles = new DN_UsersInRoles
                    {
                        UserId = user.UserId,
                        AgencyID = obj.ID,
                        DateCreated = date,
                        IsDelete = false
                    };
                    role.DN_UsersInRoles.Add(dnUsersInRoles);
                    _darRoleDa.Save();

                    var result = new BaseResponse<AgencyItem>()
                    {
                        Erros = false,

                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Log2File.LogExceptionToFile(ex);
                }
                return Json(new BaseResponse<AgencyItem>() { Erros = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
