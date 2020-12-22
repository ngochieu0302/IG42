using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNMailSSCController : BaseController
    {                               
        readonly DNMailSSCAPI _dnMailSscapi;
        readonly DNUserAPI _dnUserApi;
        readonly DNGroupMailSSCAPI _groupMailSscapi;
        readonly DNNewsSSCAPI _newsSscapi;
        public DNMailSSCController()
        {
            _dnMailSscapi = new DNMailSSCAPI();
            _dnUserApi = new DNUserAPI();
            _groupMailSscapi =  new DNGroupMailSSCAPI();
            _newsSscapi = new DNNewsSSCAPI();
        }
        public ActionResult Index(int type = 0)
        {
            //SendMailAuto();
            var model = new ModelDNMailSSCItem
            {
                Type = type
            };
            switch (type)
            {
                case 1:
                    model.ListItem = _dnMailSscapi.CountInbox(UserId, type);
                    break;
                case 2:
                    model.ListItem = _dnMailSscapi.SentMail(UserId);
                    break;
                case 3:
                    model.ListItem = _dnMailSscapi.CountDrafts(UserId);
                    break;
                case 4:
                    model.ListItem = _dnMailSscapi.CountSpam(UserId, 4);
                    break;
                case 5:
                    model.ListItem = _dnMailSscapi.CountRecycleBin(UserId);
                    break;
                default:
                    model.ListItem = _dnMailSscapi.GetListSimpleByRequest(UserItem.AgencyID, type, UserId);
                    break;
            }

            ViewData.Model = model;
            ViewBag.UserId = UserId;
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelDNMailSSCItem
            {
                ListItem = _dnMailSscapi.GetListSimpleByRequest(UserItem.AgencyID, (int)MailSsc.Inbox, UserId),
                //PageHtml = _dnMailSscapi.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxView()
        {
            var mailSscItem = _dnMailSscapi.GetItemById(CodeLogin(), ArrId.FirstOrDefault());
            ViewData.Model = mailSscItem;
            return View();
        }

        public ActionResult BoxMail()
        {
            var type = Convert.ToInt32(Request.QueryString["type"]);
            var model = new ModelDNMailSSCItem
            {
                Type = type,
                TotalMailInbox = _dnMailSscapi.CountInboxNew(UserId, type).Count,
                TotalMailDrafts = _dnMailSscapi.CountDrafts(UserId).Count,
                TotalMailSpam = _dnMailSscapi.CountSpam(UserId, 4).Count,
                TotalNewsSsc = _newsSscapi.GetAll(UserItem.AgencyID).Count
            };
            return View(model);
        }


        public ActionResult Compose(string userId)
        {
            var model = new ModelDNMailSSCItem
            {
                ListDNUserItem = _dnUserApi.GetListByAgency(),
                ListDNGroupMailSSCItem = _groupMailSscapi.GetAllByUserId(UserItem.AgencyID, UserId),
                UserReceiveId = userId
            };

            return View(model);
        }

        public ActionResult DetailsEmail(int id)
        {
            var mailSscItem = _dnMailSscapi.GetItemById(CodeLogin(), id);
            ViewData.Model = mailSscItem;
            try
            {
                mailSscItem.ID = id;
                mailSscItem.Status = 1;
                var json = new JavaScriptSerializer().Serialize(mailSscItem);
                _dnMailSscapi.Update(UserItem.AgencyID, json, id);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            return View();
        }

        public ActionResult AjaxForm()
        {
            var mailSscItem = new DNMailSSCItem();
            if (DoAction == ActionType.Edit)
            {
                mailSscItem = _dnMailSscapi.GetItemById(CodeLogin(), ArrId.FirstOrDefault());
            }
            ViewData.Model = mailSscItem;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        public ActionResult SendMail()
        {
            var msg = new JsonMessage();
            var mailSscItem = new DNMailSSCItem();
            try
            {
                var date = ConvertDate.TotalSeconds(DateTime.Now);
                UpdateModel(mailSscItem);
                mailSscItem.UserSendId = UserId;
                if (mailSscItem.Type == 1)
                    mailSscItem.IsDraft = false;
                else if (mailSscItem.Type == 3)
                    mailSscItem.IsDraft = true;
                mailSscItem.Status = 0;
                mailSscItem.IsDelete = false;
                mailSscItem.IsSpam = false;
                mailSscItem.IsRecycleBin = false;
                mailSscItem.IsImportant = false;
                mailSscItem.ListUrlPicture = Request["lstImages"];
                mailSscItem.Content = Request.Unvalidated.Form["ContentE"];
                mailSscItem.CreateDate = date;
                mailSscItem.UpdateDate = date;
                _dnMailSscapi.SetCacheMail(UserItem.AgencyID, mailSscItem);
                _dnMailSscapi.Add(UserItem.AgencyID, CodeLogin());
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = mailSscItem.ID.ToString(),
                    Message = mailSscItem.Type == 1 ? "Thư của bạn đã được gửi đi" : "Thư của bạn đã được lưu vào hộp thư nháp"
                };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailAuto()
        {

            var lstUser = _dnMailSscapi.GetListBirthDay();
            return Json(lstUser, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStatus(int id)
        {
            var msg = new JsonMessage();
            var mailSscItem = new DNMailSSCItem();
            try
            {
                mailSscItem.ID = id;
                mailSscItem.Status = 1;
                var json = new JavaScriptSerializer().Serialize(mailSscItem);
                _dnMailSscapi.Update(UserItem.AgencyID, json, id);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = mailSscItem.ID.ToString(),
                    Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(mailSscItem.Title))
                };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #region lưu thư nháp
        public ActionResult ProcessDraftMailbox(int idMail, string userId, string title, string content, string lstFile)
        {
          var msg = new JsonMessage();
            var mailSscItem = new DNMailSSCItem();
            var json = "";
            try
            {
                // thêm mới thư nháp
                if (idMail == 0)
                {
                    UpdateModel(mailSscItem);
                    mailSscItem.Title = title;
                    mailSscItem.UserSendId = UserId;
                    mailSscItem.ListUserReceiveIds = userId;
                    mailSscItem.Type = 1;
                    mailSscItem.Status = 0;
                    mailSscItem.IsDelete = false;
                    mailSscItem.IsSpam = false;
                    mailSscItem.IsRecycleBin = false;
                    mailSscItem.IsImportant = false;
                    mailSscItem.IsDraft = true;
                    mailSscItem.ListUrlPicture = lstFile;
                    mailSscItem.CreateDate = ConvertDate.TotalSeconds(DateTime.Now);
                    mailSscItem.UpdateDate = ConvertDate.TotalSeconds(DateTime.Now);
                    mailSscItem.Content = Request.Unvalidated[content];
                    _dnMailSscapi.SetCacheMail(UserItem.AgencyID, mailSscItem);
                    var id = _dnMailSscapi.Add(UserItem.AgencyID, CodeLogin());
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = id.ToString(),
                        Message = "Thư của bạn đã được lưu vào hộp thư nháp"
                    };
                }
                else
                {
                    mailSscItem.Title = title;
                    mailSscItem.Content = Request.Unvalidated[content];
                    mailSscItem.UpdateDate = ConvertDate.TotalSeconds(DateTime.Now);
                    UpdateModel(mailSscItem);
                    _dnMailSscapi.SetCacheMail(UserItem.AgencyID, mailSscItem);
                    _dnMailSscapi.UpdateDraftMailbox(CodeLogin(), idMail);
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = idMail.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(mailSscItem.Title))
                    };
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult UpdateType(string lstId, int status, int type)
        {
            var msg = new JsonMessage();
            var mailSscItem = new DNMailSSCItem();
            try
            {
                switch (status)
                {
                    case 1:
                        mailSscItem.IsDraft = false;
                        mailSscItem.IsRecycleBin = false;
                        mailSscItem.IsSpam = type == 1;
                        mailSscItem.IsDelete = false;
                        break;
                    case 2:
                        mailSscItem.IsDraft = false;
                        mailSscItem.IsRecycleBin = false;
                        mailSscItem.IsSpam = type == 1;
                        mailSscItem.IsDelete = false;
                        break;
                    case 3:
                        mailSscItem.IsDraft = false;
                        mailSscItem.IsRecycleBin = false;
                        mailSscItem.IsSpam = type == 1;
                        mailSscItem.IsDelete = false;
                        break;
                    case 4:
                        mailSscItem.IsDraft = false;
                        mailSscItem.IsRecycleBin = false;
                        mailSscItem.IsDelete = false;
                        mailSscItem.IsSpam = type == 1;
                        break;
                    case 5:
                         mailSscItem.IsDraft = false;
                        mailSscItem.IsSpam = false;
                        mailSscItem.IsDelete = false;
                        mailSscItem.IsRecycleBin = type == 1;
                        break;
                }
                mailSscItem.ListID = lstId;
                var json = new JavaScriptSerializer().Serialize(mailSscItem);
                _dnMailSscapi.UpdateType(CodeLogin(), json);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = mailSscItem.ID.ToString(),
                    Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(mailSscItem.Title))
                };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string lstId)
        {
            var msg = new JsonMessage();
            var mailSscItem = new DNMailSSCItem();
            try
            {
                mailSscItem.ListID = lstId;
                var json = new JavaScriptSerializer().Serialize(mailSscItem);
                _dnMailSscapi.Delete(CodeLogin(), json);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = mailSscItem.ID.ToString(),
                    Message = string.Format("Đã xóa thư : <b>{0}</b>", Server.HtmlEncode(mailSscItem.Title))
                };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportDelete(string lstId)
        {
            var msg = new JsonMessage();
            var mailSscItem = new DNMailSSCItem();
            try
            {
                mailSscItem.ListID = lstId;
                var json = new JavaScriptSerializer().Serialize(mailSscItem);
                _dnMailSscapi.ReportDelete(CodeLogin(), json);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = mailSscItem.ID.ToString(),
                    Message = string.Format("Đã chuyển thư vào thùng rác : <b>{0}</b>", Server.HtmlEncode(mailSscItem.Title))
                };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var mailSscItem = new DNMailSSCItem();
            List<DNMailSSCItem> ltsDnMailSscItem;
            var date = Request["DateOff"];
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(mailSscItem);
                        json = new JavaScriptSerializer().Serialize(mailSscItem);
                        _dnMailSscapi.Add(UserItem.AgencyID, CodeLogin());
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = mailSscItem.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(mailSscItem.Title))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        UpdateModel(mailSscItem);
                        json = new JavaScriptSerializer().Serialize(mailSscItem);
                        _dnMailSscapi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = mailSscItem.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(mailSscItem.Title))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsDnMailSscItem = _dnMailSscapi.GetListByArrId(UserItem.AgencyID, lstId);
                    foreach (var item in ltsDnMailSscItem)
                    {
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _dnMailSscapi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = mailSscItem.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsDnMailSscItem.Select(c => c.Title))))
                    };
                    break;
                //case ActionType.Show:
                //    ltsDnMailSscItem = _dnMailSscapi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                //    foreach (var item in ltsDnMailSscItem)
                //    {
                //        item.IsDelete = false;
                //        item.IsShow = true;
                //        UpdateModel(item);
                //        json = new JavaScriptSerializer().Serialize(item);
                //        _dnMailSscapi.Update(UserItem.AgencyID, json, item.ID);
                //    }
                //    msg = new JsonMessage
                //    {
                //        Erros = false,
                //        ID = mailSscItem.ID.ToString(),
                //        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsmailSscItem.Select(c => c.Name))))
                //    };
                //    break;

                //case ActionType.Hide:
                //    ltsmailSscItem = _dnMailSscapi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                //    foreach (var item in ltsmailSscItem)
                //    {
                //        item.IsDelete = false;
                //        item.IsShow = false;
                //        UpdateModel(item);
                //        json = new JavaScriptSerializer().Serialize(item);
                //        _dnMailSscapi.Update(UserItem.AgencyID, json, item.ID);
                //    }
                //    msg = new JsonMessage
                //    {
                //        Erros = false,
                //        ID = mailSscItem.ID.ToString(),
                //        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsmailSscItem.Select(c => c.Name))))
                //    };
                //    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #region upload file
        [HttpPost]
        public ContentResult UploadFiles()
        {
            var fileObj = new List<FileObj>();

            foreach (string file in Request.Files)
            {
                var hpf = Request.Files[file];
                var fileNameRoot = hpf != null ? hpf.FileName : string.Empty;
                if (hpf != null && hpf.ContentLength == 0)
                    continue;

                if (hpf != null)
                {
                    var urlFolder = ConfigData.MailFolder;
                    if (!Directory.Exists(urlFolder))
                        Directory.CreateDirectory(urlFolder);
                    if (fileNameRoot.Length > 1)
                    {
                        var fileLocal = fileNameRoot.Split('.');
                        var fileName = FomatString.Slug(fileLocal[0]) + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + fileLocal[1];
                        var savedFileName = Path.Combine((urlFolder), Path.GetFileName(fileName));
                        hpf.SaveAs(savedFileName);
                        fileObj.Add(new FileObj()
                        {
                            Name = fileName,
                            Size = hpf.ContentLength,
                            Type = hpf.ContentType
                        });
                    }
                }
            }
            return Content("{\"name\":\"" + fileObj[0].Name + "\",\"type\":\"" + fileObj[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", fileObj[0].Size) + "\"}", "application/json");
        }

        #endregion
    }
}
