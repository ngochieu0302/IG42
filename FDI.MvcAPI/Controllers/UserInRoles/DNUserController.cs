using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;
using System.Linq;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class DNUserController : BaseApiController
    {
        //
        // GET: /DNUser/

        private readonly DNLoginDA _dalogin = new DNLoginDA("#");
        private readonly DNUserDA _da = new DNUserDA("#");
        private readonly CustomerDA _customerDa = new CustomerDA("#");

        public ActionResult ListItems()
        {
            decimal? total;
            var obj = Request["key"] != Keyapi
                ? new ModelDNUserItem()
                : new ModelDNUserItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid(), out total), TotalFixedSalary = total, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindByName(string name)
        {
            decimal? total;
            var obj = Request["key"] != Keyapi
                ? new List<DNUserItem>()
                :  _da.FindUser(name);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListUserNotSId(string key, string code, Guid id, int sid, decimal date)
        {
            var obj = key != Keyapi ? new List<DNUserCalendarItem>() : _da.ListUserNotSId(Agencyid(), date, sid, id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckUserName(string txt, Guid id, string code)
        {
            return Json(_da.CheckUserName(txt, id, Agencyid()), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListCalendar(string key, decimal dates, decimal datee)
        {
            var obj = key != Keyapi ? new List<DNUserCalendarItem>() : _da.GetListCalendar(Agencyid(), dates, datee);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListCalendarUser(string key, decimal dates, decimal datee, Guid userid)
        {
            var obj = key != Keyapi ? new List<DNUserCalendarItem>() : _da.GetListCalendar(Agencyid(), dates, datee, userid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTotalMonth(string key, string code, decimal dates, decimal datee)
        {
            var obj = key != Keyapi ? new List<DNUserCalendarItem>() : _da.GetListTotalMonth(Agencyid(), dates, datee);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserIdByCodeCheckIn(string key, string codecheckin, int agencyid)
        {
            var obj = key != Keyapi ? new DNUserItem() : _da.GetUserIdByCodeCheckIn(codecheckin, agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemModuleById(string key, Guid id)
        {
            var obj = key != Keyapi ? new DNUserItem() : _da.GetItemModuleById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, Guid id)
        {
            var obj = key != Keyapi ? new DNUserItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateModuleActive(string key, Guid userid, string ltrInts)
        {
            var msg = new JsonMessage(false, "Gán tính năng thành công.");
            try
            {
                if (key == Keyapi)
                {
                    _da.UpdateUserActive(userid, ltrInts);
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

        public ActionResult GetAll(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetListAll(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListAllKt(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetListAllKt(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListAllAgency(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetListAllAgency(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAllChoose(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetListAllChoose(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAllSevice(string key, int agencyId, string json)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetListAllSevice(agencyId, json);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNUserSimpleItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRoleItemById(string key, Guid id)
        {
            var obj = key != Keyapi ? new DN_Users() : _da.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DN_Users>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByAgency(string key)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetListByAgency(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListBirthDay(string key)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetListBirthDay(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetScheduleById(string key, Guid id)
        {
            var obj = key != Keyapi ? new DNUserItem() : _da.GetScheduleById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListUserRolesMonth(string key, int agencyId, int month, int year)
        {
            var obj = key != Keyapi ? new List<SalaryMonthDetailItem>() : _da.GetListUserRolesMonth(agencyId, month, year);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUser(string key, string code, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var user = JsonConvert.DeserializeObject<DNUserAddItem>(json);
                    var saltKey = FDIUtils.CreateSaltKey(5);
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(user.PasswordSalt, saltKey);
                    var obj = new DN_Users { UserId = Guid.NewGuid(), PasswordSalt = saltKey, Password = sha1PasswordHash, AgencyID = Agencyid(), CreateDate = ConvertDate.TotalSeconds(DateTime.Now), IsDeleted = false };
                    UpdateBase(obj, user);
                    _da.Add(obj);
                    _da.Save();
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

        public ActionResult Update(string key, string code, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var user = JsonConvert.DeserializeObject<DNUserAddItem>(json);
                    var obj = _da.GetById(user.UserId);
                    if (!string.IsNullOrEmpty(user.PasswordSalt) && user.PasswordSalt.Length > 5)
                    {
                        obj.Password = FDIUtils.CreatePasswordHash(user.PasswordSalt, obj.PasswordSalt);
                    }
                    UpdateBase(obj, user);
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

        public ActionResult Delete(string key, string code, string listint)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var list = _da.GetListByArrId(listint);
                    foreach (var item in list)
                    {
                        item.IsDeleted = true;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được Xóa.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePass(string key, string code, string passold, string passnew)
        {
            var msg = new JsonMessage(false, "Thay đổi mật khẩu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var userlogin = _dalogin.GetUserItemByCode(code);
                    if (userlogin != null)
                    {
                        var user = _da.GetById(userlogin.UserId);
                        if (user != null && FDIUtils.CreatePasswordHash(passold, user.PasswordSalt) == user.Password)
                        {
                            user.Password = FDIUtils.CreatePasswordHash(passnew, user.PasswordSalt);
                            _da.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Mật khẩu chưa được thay đôi.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCusPass(string key, string code, string passold, string passnew)
        {
            try
            {
                if (key == Keyapi)
                {
                    var userlogin = _dalogin.GetCustomerByCode(code);
                    if (userlogin != null)
                    {
                        var user = _customerDa.GetById(userlogin.ID);
                        if (user != null && FDIUtils.CreatePasswordHash(passold, user.PasswordSalt) == user.PassWord)
                        {
                            user.PassWord = FDIUtils.CreatePasswordHash(passnew, user.PasswordSalt);
                            _customerDa.Save();
                        }
                        else
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public DN_Users UpdateBase(DN_Users dnUsers, DNUserAddItem userItem)
        {
            dnUsers.UserId = userItem.UserId;
            dnUsers.CustomerID = userItem.CustomerID;
            dnUsers.UserName = userItem.UserName;
            dnUsers.Mobile = userItem.Mobile;
            dnUsers.StartDate = userItem.StartDate;
            dnUsers.BirthDay = userItem.BirthDay;
            dnUsers.LoweredUserName = userItem.LoweredUserName;
            dnUsers.FixedSalary = userItem.FixedSalary;
            dnUsers.Email = userItem.Email;
            dnUsers.Gender = userItem.Gender;
            dnUsers.Address = userItem.Address;
            dnUsers.IsLockedOut = userItem.IsLockedOut;
            dnUsers.IsService = userItem.IsService;
            dnUsers.IsApproved = userItem.IsApproved;
            dnUsers.IsAgency = userItem.IsAgency;
            dnUsers.CodeCheckIn = userItem.CodeCheckIn;
            dnUsers.Comment = userItem.Comment;
            return dnUsers;
        }

        public ActionResult ShowHide(string key, Guid userId, bool isOut)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetById(userId);
                    model.IsLockedOut = isOut;
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

        public ActionResult AddModuleUser(string key, string listInt, Guid userId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lstActive = _da.GetListActive();
                    var lst = FDIUtils.StringToListInt(listInt);
                    var model = _da.GetById(userId);
                    model.DN_Module.Clear();
                    model.DN_Module = _da.GetListModulebyListInt(lst);
                    while (model.DN_User_ModuleActive.Count > 0)
                    {
                        var item = model.DN_User_ModuleActive.FirstOrDefault();
                        _da.Delete(item);
                    }
                    model.DN_User_ModuleActive.Clear();
                    foreach (var activeitem in model.DN_Module.SelectMany(item => lstActive.Select(active => new DN_User_ModuleActive
                    {
                        ModuleId = item.ID,
                        UserId = model.UserId,
                        ActiveId = active.ID,
                        Active = true,
                        Check = 1,
                        AgencyId = Agencyid()
                    })))
                    {
                        model.DN_User_ModuleActive.Add(activeitem);
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
    }
}
