using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNMailSSCController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly DNMailSSCDA _dnMailSscda = new DNMailSSCDA();
        private readonly DNMailStatusDA _dnMailStatusDa = new DNMailStatusDA();
        
        private readonly DNUserDA _dnUserBl = new DNUserDA("#");
        private readonly DNFileMailDA _mailBl = new DNFileMailDA();
        private readonly DNGroupMailSSCDA _groupMailSscbl = new DNGroupMailSSCDA();

        public ActionResult GetListSimpleByRequest(int type, Guid userId)
        {
            var obj = Request["key"] != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.GetListSimpleByRequest(Request, type, Agencyid(), userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Mail_SSC() : _dnMailSscda.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNMailSSCItem() : _dnMailSscda.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountInboxNew(string key, int type, Guid userId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CountInboxNew(type, Agencyid(), userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountInbox(string key, int type, Guid userId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CountInbox(type, Agencyid(), userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountDrafts(string key, Guid userId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CountDrafts(Agencyid(), userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SentMail(string key, Guid userId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.SentMail(Agencyid(), userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountSpam(string key, int type, Guid userId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CountSpam(type, Agencyid(), userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountRecycleBin(string key, Guid userId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CountRecycleBin(userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region khách hàng
        public ActionResult CustomerCountInboxNew(string key, int type, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountInboxNew(type, Agencyid(), customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCountInbox(string key, int type, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountInbox(type, Agencyid(), customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCountDrafts(string key, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountDrafts(Agencyid(), customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerSentMail(string key, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerSentMail(Agencyid(), customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomerCountSpam(string key, int type, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountSpam(type, Agencyid(), customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCountSpamSend(string key, int type, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountSpamSend(type, Agencyid(), customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCountSpamReceive(string key, int type, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountSpamReceive(type, Agencyid(), customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCountRecycleBin(string key, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountRecycleBin(customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCountRecycleBinSend(string key, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountRecycleBinSend(customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCountRecycleBinReceive(string key, int customerId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.CustomerCountRecycleBinReceive(customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetListDelete(string key, Guid userId, int type)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.GetListDelete(Agencyid(), userId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListBirthDay(string key)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _dnUserBl.GetListBirthDay(1);
            //foreach (var dnUserItem in obj)
            //{
            //    Utility.SendEmail("noreply@fditech.vn", "fditech@123", dnUserItem.Email, "Chúc bạn " + dnUserItem.LoweredUserName + " sinh nhật vui vẻ ", "Chúc mừng sinh nhật bạn");
            //} 
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNMailSSCItem>() : _dnMailSscda.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string code)
        {
            if (key == Keyapi)
            {
                string url = "Utility/GetContentEmail?key=" + code;
                var urlJson = string.Format("{0}", UrlG + url);
                var email = Utility.GetObjJson<DNMailSSCItem>(urlJson);
                email.AgencyID = Agencyid();
                if (email.ListUserReceiveIds != null)
                {
                    // insert file đính kèm
                    var lstImage = email.ListUrlPicture.Split(',');
                    var listfile = lstImage.Select(item => new DN_File_Mail
                    {
                        IsShow = true,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                        Url = item,
                        Name = item,
                        Folder = "/Uploads/Mail/",
                        AgencyId = Agencyid()
                    }).ToList();
                    foreach (var item in listfile)
                    {
                        _mailBl.Add(item);
                    }
                    _mailBl.Save();
                    var strId = string.Join(",", listfile.Select(m => m.ID));
                    var list = new List<DN_Mail_SSC>();
                    _mailBl.Save();
                    List<int> listid;
                    var lstId = FDIUtils.StringToListIntGuid(email.ListUserReceiveIds, out listid);
                    var group = _groupMailSscbl.GetListByArrId(listid);
                    if (group.Any())
                    {
                        foreach (var item in group)
                        {
                            foreach (var user in item.ListDNUserItem)
                            {
                                var obj = new DN_Mail_SSC();
                                email.UserReceiveId = user.UserId;
                                UpdateBase(obj, email);
                                list.Add(obj);
                            }
                        }
                    }

                    foreach (var guid in lstId)
                    {
                        var obj = new DN_Mail_SSC();
                        email.UserReceiveId = guid;
                        UpdateBase(obj, email);
                        list.Add(obj);
                    }

                    var check = true;
                    foreach (var item in list)
                    {
                        if (check)
                        {
                            check = false;
                            var lstFileMail = _dnMailSscda.GetFileMailArrId(strId);
                            foreach (var itemf in lstFileMail)
                                item.DN_File_Mail.Add(itemf);
                        }

                        _dnMailSscda.Add(item);
                    }
                    _dnMailSscda.Save();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCustomer(string key, string code)
        {
            if (key == Keyapi)
            {
               
                string url = "Utility/GetContentEmail?key=" + code;
                var urlJson = string.Format("{0}", UrlCustomer + url);
                var email = Utility.GetObjJson<DNMailSSCItem>(urlJson);
                email.AgencyID = Agencyid();
                if (email.ListUserReceiveIds != null)
                {
                    // insert file đính kèm
                    var lstImage = email.ListUrlPicture.Split(',');
                    var listfile = lstImage.Select(item => new DN_File_Mail
                    {
                        IsShow = true,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                        Url = item,
                        Name = item,
                        Folder = "/Uploads/Mail/",
                        AgencyId = Agencyid()
                    }).ToList();
                    foreach (var item in listfile)
                    {
                        _mailBl.Add(item);
                    }
                    _mailBl.Save();

                    var strId = string.Join(",", listfile.Select(m => m.ID));
                    var list = new List<DN_Mail_SSC>();
                    _mailBl.Save();

                    var lstId = FDIUtils.StringToListInt(email.ListUserReceiveIds);
                    // người gửi
                    
                    foreach (var item in lstId)
                    {
                        var obj1 = new DN_Mail_SSC();
                        email.CustomerReceiveId = item;
                        email.StatusEmail = true;
                        UpdateBase(obj1, email);
                        list.Add(obj1);
                        _dnMailSscda.Add(obj1);
                     
                        var obj2 = new DN_Mail_SSC();
                        email.CustomerReceiveId = item;
                        email.StatusEmail = false;
                        UpdateBase(obj2, email);
                        list.Add(obj2);
                        _dnMailSscda.Add(obj2);
                    }

                    var check1 = true;
                    foreach (var item in list)
                    {
                        if (check1)
                        {
                            check1 = false;
                            var lstFileMail = _dnMailSscda.GetFileMailArrId(strId);
                            foreach (var itemf in lstFileMail)
                                item.DN_File_Mail.Add(itemf);
                        }
                        _dnMailSscda.Add(item);
                    }

                    _dnMailSscda.Save();

                    // bảng status
                    foreach (var item in list)
                    {
                        var objStatus1 = new DN_StatusEmail
                        {
                            CustomerId = item.StatusEmail == true ? item.CustomerSendId : item.CustomerReceiveId,
                            Status = item.StatusEmail,
                            MailId = item.ID
                        };
                        _dnMailStatusDa.Add(objStatus1);
                    }
                    _dnMailStatusDa.Save();
                
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var obj = _dnMailSscda.GetById(id);
                obj.Status = 1;
                _dnMailSscda.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStatus(string key,  int id)
        {
            if (key == Keyapi)
            {
                var obj = _dnMailSscda.GetById(id);
                obj.Status = 1;
                _dnMailSscda.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDraftMailbox(string key, string code, int id)
        {
            if (key == Keyapi)
            {
                string url = "Utility/GetContentEmail?key=" + code;
                var urlJson = string.Format("{0}", UrlCustomer + url);
                var email = Utility.GetObjJson<DNMailSSCItem>(urlJson);
                var obj = _dnMailSscda.GetById(id);
                obj.Title = email.Title;
                obj.Content = email.Content;
                _dnMailSscda.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateType(string key, string json)
        {
            if (key == Keyapi)
            {
                var dnMailSsc = JsonConvert.DeserializeObject<MailType>(json);
                var ltsArrId = FDIUtils.StringToListInt(dnMailSsc.ListID);
                foreach (var id in ltsArrId)
                {
                    var obj = _dnMailSscda.GetById(id);
                    obj.IsSpam = dnMailSsc.IsSpam;
                    obj.IsRecycleBin = dnMailSsc.IsRecycleBin;
                    obj.IsDraft = dnMailSsc.IsDraft;
                    obj.IsDelete = dnMailSsc.IsDelete;
                    obj.UpdateDate = ConvertDate.TotalSeconds(DateTime.Now);
                }
                _dnMailSscda.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, string json)
        {
            if (key == Keyapi)
            {
                var dnMailSsc = JsonConvert.DeserializeObject<MailType>(json);
                var ltsArrId = FDIUtils.StringToListInt(dnMailSsc.ListID);
                foreach (var id in ltsArrId)
                {
                    var obj = _dnMailSscda.GetById(id);
                    obj.IsDelete = true;
                }
                _dnMailSscda.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportDelete(string key, string json)
        {
            if (key == Keyapi)
            {
                var dnMailSsc = JsonConvert.DeserializeObject<DNMailSSCItem>(json);
                var ltsArrId = FDIUtils.StringToListInt(dnMailSsc.ListID);
                foreach (var id in ltsArrId)
                {
                    var obj = _dnMailSscda.GetById(id);
                    obj.IsRecycleBin = true;
                }
                _dnMailSscda.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Delete(string key, List<int> listint)
        //{
        //    if (key == Keyapi)
        //    {
        //        var list = _dnMailSscda.GetListByArrId(listint);
        //        foreach (var item in list)
        //        {
        //            _dnMailSscda.Delete(item);
        //        }
        //        _dnMailSscda.Save();
        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}

        public DN_Mail_SSC UpdateBase(DN_Mail_SSC mailSsc, DNMailSSCItem mailSscItem)
        {
            mailSsc.AgencyID = mailSscItem.AgencyID;
            mailSsc.Title = mailSscItem.Title;
            mailSsc.Content = mailSscItem.Content;
            mailSsc.CreateDate = mailSscItem.CreateDate;
            mailSsc.UpdateDate = mailSscItem.UpdateDate;
            mailSsc.CustomerSendId = mailSscItem.CustomerSendId;
            mailSsc.CustomerReceiveId = mailSscItem.CustomerReceiveId;
            mailSsc.Status = mailSscItem.Status;
            mailSsc.Type = mailSscItem.Type;
            mailSsc.StatusEmail = mailSscItem.StatusEmail;
            mailSsc.UserSendId = mailSscItem.UserSendId;
            mailSsc.UserReceiveId = mailSscItem.UserReceiveId;
            mailSsc.IsSpam = mailSscItem.IsSpam;
            mailSsc.IsDraft = mailSscItem.IsDraft;
            mailSsc.IsRecycleBin = mailSscItem.IsRecycleBin;
            mailSsc.IsDelete = mailSscItem.IsDelete;
            mailSsc.IsImportant = mailSscItem.IsImportant;
            return mailSsc;
        }
    }
}
