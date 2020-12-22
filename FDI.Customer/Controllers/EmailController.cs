using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Customer.Controllers
{
    public class EmailController : BaseController
    {
        //
        // GET: /Admin/Customer/
        private readonly DNMailSSCAPI _dnMailSscapi = new DNMailSSCAPI();
        private readonly CustomerAPI _customerApi = new CustomerAPI();
        public ActionResult Index(int type = 0)
        {
            var model = new ModelDNMailSSCItem
            {
                Type = type
            };
            switch (type)
            {
                case 1:
                    model.ListItem = _dnMailSscapi.CustomerCountInbox(UserItem.ID, type);
                    break;
                case 2:
                    model.ListItem = _dnMailSscapi.CustomerSentMail(UserItem.ID);
                    break;
                case 3:
                    model.ListItem = _dnMailSscapi.CustomerCountDrafts(UserItem.ID);
                    break;
                case 4:
                    model.ListItem = _dnMailSscapi.CustomerCountSpam(UserItem.ID, 4);
                    break;
                case 5:
                    model.ListItem = _dnMailSscapi.CustomerCountRecycleBin(UserItem.ID);
                    break;
            }

            ViewData.Model = model;
            ViewBag.UserId = UserItem.ID;
            ViewBag.Type = type;
            return View();
        }

        public ActionResult BoxMail()
        {
           var type = Convert.ToInt32(Request.QueryString["type"]);
            var model = new ModelDNMailSSCItem
            {
                Type = type,
                UserId = UserId,
                TotalMailInbox = _dnMailSscapi.CustomerCountInboxNew(UserItem.ID, type).Count,
                TotalMailDrafts = _dnMailSscapi.CustomerCountDrafts(UserItem.ID).Count,
                TotalMailSpam = _dnMailSscapi.CustomerCountSpam(UserItem.ID, 4).Count,
                //TotalMailSpam = _dnMailSscapi.CustomerCountSpamReceive(UserItem.ID, 4).Count,
                //TotalNewsSsc = _newsSscapi.GetAll(UserItem.AgencyID).Count
            };
            return View(model);
        }

        public ActionResult Compose()
        {
            var model = new DNMailSSCItem
            {
                ListCustomerItem = _customerApi.GetList().Take(10)
                //ListDNGroupMailSSCItem = _groupMailSscapi.GetAllByUserId(UserItem.AgencyID, UserId),
                //UserReceiveId = userId
            };

            return View(model);
        }

        public ActionResult DetailsEmail(int id)
        {
            var mailSscItem = _dnMailSscapi.GetItemById(CodeLogin(), id);
            ViewData.Model = mailSscItem;
            try
            {
                _dnMailSscapi.UpdateStatus(CodeLogin(), id);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

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
                mailSscItem.CustomerSendId = UserItem.ID;
                if (mailSscItem.Type == 1)
                    mailSscItem.IsDraft = false;
                else if (mailSscItem.Type == 3)
                    mailSscItem.IsDraft = true;
                mailSscItem.Status = 0;
                mailSscItem.IsDelete = false;
                mailSscItem.IsSpam = false;
                mailSscItem.IsRecycleBin = false;
                mailSscItem.IsImportant = false;
                mailSscItem.ListUserReceiveIds = Request["ListUserReceiveIds"];
                //mailSscItem.Content = Request.Unvalidated.Form["Title"];
                mailSscItem.ListUrlPicture = Request["lstImages"];
                mailSscItem.Content = Request.Unvalidated.Form["ContentE"];
                mailSscItem.CreateDate = date;
                mailSscItem.UpdateDate = date;
                _dnMailSscapi.SetCacheMailCustomer(CodeLogin(), mailSscItem);
                var id = _dnMailSscapi.AddCustomer(CodeLogin());
                if (id > 0)
                {
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = mailSscItem.ID.ToString(),
                        Message = mailSscItem.Type == 1 ? "Thư của bạn đã được gửi đi" : "Thư của bạn đã được lưu vào hộp thư nháp"
                    };
                }
                else
                {
                    msg = new JsonMessage
                    {
                        Erros = true,
                        Message = "Có lỗi xảy ra !"
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

        public ActionResult SendMailAuto()
        {

            var lstUser = _dnMailSscapi.GetListBirthDay();
            return Json(lstUser, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStatus(int id)
        {
            var msg = new JsonMessage();
            try
            {
                _dnMailSscapi.UpdateStatus(CodeLogin(), id);
                msg = new JsonMessage
                {
                    Erros = false,
                    Message = string.Format("Thành công !")
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
                    mailSscItem.CustomerSendId = UserItem.ID;
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
                    _dnMailSscapi.SetCacheMailCustomer(CodeLogin(), mailSscItem);
                    var id = _dnMailSscapi.AddCustomer(CodeLogin());
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
                    json = new JavaScriptSerializer().Serialize(mailSscItem);
                    _dnMailSscapi.SetCacheMailCustomer(CodeLogin(), mailSscItem);
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
            var mailSscItem = new MailType();
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
                        mailSscItem.IsSpam =  type == 1;
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
                    Message = string.Format("Cập nhật thành công !")
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
            var mailSscItem = new MailType();
            try
            {
                mailSscItem.ListID = lstId;
                var json = new JavaScriptSerializer().Serialize(mailSscItem);
                _dnMailSscapi.Delete(CodeLogin(), json);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = mailSscItem.ID.ToString(),
                    Message = string.Format("Đã xóa thư thành công !")
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
            var mailSscItem = new MailType();
            try
            {
                mailSscItem.ListID = lstId;
                var json = new JavaScriptSerializer().Serialize(mailSscItem);
                _dnMailSscapi.ReportDelete(CodeLogin(), json);
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = mailSscItem.ID.ToString(),
                    Message = string.Format("Đã chuyển thư vào thùng rác !")
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
