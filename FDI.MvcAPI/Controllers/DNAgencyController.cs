using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class DNAgencyController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly AgencyDA _da = new AgencyDA();
        private readonly DNUserDA _daUserDa = new DNUserDA("#");
        private readonly DNRoleDA _darRoleDa = new DNRoleDA("#");
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new AgencyItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemByStatic(string key, int id)
        {
            var obj = key != Keyapi ? new AgencyItem() : _da.GetItemByStatic(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItems(int areaId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelAgencyItem()
                : new ModelAgencyItem { ListItem = _da.GetListSimpleByRequest(Request, EnterprisesId(),areaId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsStatic(int areaId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelAgencyItem()
                : new ModelAgencyItem { ListItem = _da.GetListSimpleByRequestStatic(Request, EnterprisesId(), areaId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<AgencyItem>() : _da.GetAll(EnterprisesId());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetByCustomer(string key, int customerId)
        {
            var obj = key != Keyapi ? new List<AgencyItem>() : _da.GetByCustomer(customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var date = DateTime.Now.TotalSeconds();
                    var objitem = JsonConvert.DeserializeObject<AgencyItem>(json);
                    var obj = new DN_Agency
                    {
                        IsDelete = false,
                        IsLock = false,
                        IsOut = false,
                        CreateDate = date,
                        WalletValue = 10000000,
                        CashOut = 0,
                    };
                    obj = UpdateBase(obj, objitem);
                    _da.Add(obj);
                    _da.Save();
                    if (objitem.GroupID > 0)
                    {
                        _da.InsertDNModule(objitem.GroupID, obj.ID);
                    }
                    var saltKey = FDIUtils.CreateSaltKey(5);
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(objitem.Pass, saltKey);
                    var user = new DN_Users
                    {
                        UserId = Guid.NewGuid(),
                        PasswordSalt = saltKey,
                        Password = sha1PasswordHash,
                        UserName = objitem.UserName,
                        LoweredUserName = obj.Name,
                        Email = obj.Email,
                        Address = obj.Address,
                        AgencyID = obj.ID,
                        Mobile = obj.Phone,
                        IsApproved = true,
                        IsLockedOut = false,
                        CreateDate = date,
                        IsDeleted = false
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
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Addapp(string key)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var date = DateTime.Now.TotalSeconds();
                    var obj = new DN_Agency();
                    var pass = Request["Pass"];
                    var username = Request["UserName"];
                    UpdateModel(obj);
                    obj.IsShow = true;
                    obj.IsDelete = false;
                    obj.IsLock = false;
                    obj.IsOut = false;
                    obj.CreateDate = date;
                    obj.WalletValue = 0;
                    obj.CashOut = 0;
                    _da.Add(obj);
                    obj.EnterpriseID = 3;
                    _da.Save();
                    if (obj.GroupID > 0)
                    {
                        _da.InsertDNModule(obj.GroupID, obj.ID);
                    }
                    var saltKey = FDIUtils.CreateSaltKey(5);
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(pass, saltKey);
                    var user = new DN_Users
                    {
                        UserId = Guid.NewGuid(),
                        PasswordSalt = saltKey,
                        Password = sha1PasswordHash,
                        UserName = username,
                        LoweredUserName = obj.Name,
                        Email = obj.Email,
                        Address = obj.Address,
                        AgencyID = obj.ID,
                        Mobile = obj.Phone,
                        IsApproved = true,
                        IsLockedOut = false,
                        CreateDate = date,
                        IsDeleted = false
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
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var objitem = JsonConvert.DeserializeObject<AgencyItem>(json);
                    var obj = _da.GetById(objitem.ID);
                    var check = obj.GroupID != objitem.GroupID;
                    if (obj.GroupID > 0 && check)
                    {
                        _da.InsertDNModule(objitem.GroupID, obj.ID, true);
                    }

                    obj = UpdateBase(obj, objitem);
                    _da.Save();
                    if (!string.IsNullOrEmpty(objitem.Pass))
                    {
                        var user = _daUserDa.GetUserByUserName(objitem.UserName);
                        if (user != null)
                        {
                            var sha1PasswordHash = FDIUtils.CreatePasswordHash(objitem.Pass, user.PasswordSalt);
                            user.Password = sha1PasswordHash;
                        }
                        else
                        {
                            var date = ConvertDate.TotalSeconds(DateTime.Now);
                            var role = _darRoleDa.GetByName("Admin");
                            if (role == null)
                            {
                                role = new DN_Roles { RoleId = Guid.NewGuid(), RoleName = "Admin", LoweredRoleName = "admin", AgencyID = obj.ID, Description = "Quản trị" };
                                _darRoleDa.Add(role);
                                _darRoleDa.Save();
                            }
                            var saltKey = FDIUtils.CreateSaltKey(5);
                            var sha1PasswordHash = FDIUtils.CreatePasswordHash(objitem.Pass, saltKey);
                            user = new DN_Users
                            {
                                UserId = Guid.NewGuid(),
                                PasswordSalt = saltKey,
                                Password = sha1PasswordHash,
                                UserName = objitem.UserName,
                                LoweredUserName = obj.Name,
                                Email = obj.Email,
                                Address = obj.Address,
                                AgencyID = obj.ID,
                                Mobile = obj.Phone,
                                IsApproved = true,
                                IsLockedOut = false,
                                CreateDate = date,
                                IsDeleted = false
                            };
                            _daUserDa.Add(user);
                            var dnUsersInRoles = new DN_UsersInRoles
                            {
                                UserId = role.RoleId,
                                AgencyID = obj.ID,
                                DateCreated = date
                            };
                            user.DN_UsersInRoles.Add(dnUsersInRoles);
                        }
                        _daUserDa.Save();
                    }

                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LockAgency(string key, string lstInt)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var obj = _da.GetListByArrId(lstInt);
                    foreach (var item in obj)
                    {
                        item.IsLock = true;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnLockAgency(string key, string lstInt)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var obj = _da.GetListByArrId(lstInt);
                    foreach (var item in obj)
                    {
                        item.IsLock = false;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string listint)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var list = _da.GetListByArrId(listint);
                    foreach (var item in list)
                    {
                        item.IsDelete = true;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public DN_Agency UpdateBase(DN_Agency obj, AgencyItem objItem)
        {
            obj.EnterpriseID = EnterprisesId();
            obj.Name = objItem.Name;
            obj.CreateDate = objItem.CreateDate;
            obj.FullName = objItem.FullName;
            obj.Phone = objItem.Phone;
            obj.Email = objItem.Email;
            obj.Address = objItem.Address;
            obj.Code = objItem.Code;
            obj.IPTimekeep = objItem.IPTimekeep;
            obj.Port = objItem.Port;
            obj.DateEnd = objItem.DateEnd;
            obj.IsShow = objItem.IsShow;
            obj.GroupID = objItem.GroupID;
            obj.DateStart = objItem.DateStart;
            obj.DateEnd = objItem.DateEnd;
            obj.MarketID = objItem.MarketID;
            return obj;
        }
    }
}
