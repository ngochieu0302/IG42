using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class EnterprisesController : BaseController
    {
        private readonly EnterprisesDA _enterprisesDa;
        private readonly STGroupDA _stGroupDa;
        private readonly BonusTypeDA _bonusTypeDa;
        
        public EnterprisesController()
        {
            _enterprisesDa = new EnterprisesDA("#");
            _stGroupDa = new STGroupDA("#");
            _bonusTypeDa = new BonusTypeDA("#");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelEnterprisesItem
            {
                ListItem = _enterprisesDa.GetListSimpleByRequest(Request),
                PageHtml = _enterprisesDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public string GetCodeEnterprises()
        {
            var code = _enterprisesDa.GetCodeEnterprises();
            return code;
        }

        public ActionResult AjaxView()
        {
            var customerType = _enterprisesDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = customerType;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var enterprises = new DN_Enterprises();
            if (DoAction == ActionType.Edit)
            {
                enterprises = _enterprisesDa.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.ST_Group = _stGroupDa.GetAll();
            ViewBag.BonusType = _bonusTypeDa.GetItemTop();
            ViewData.Model = enterprises;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
        public string CheckDomainDN(string DomainDN, int id)
        {
            var result = _enterprisesDa.CheckUrl(DomainDN,id);
            return result ? "false" : "true";
        }
        public string CheckUrl(string Url, int id)
        {
            var result = _enterprisesDa.CheckUrl(Url, id);
            return result ? "false" : "true";
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var enterprises = new DN_Enterprises();
            List<DN_Enterprises> ltsEnterprises;
            StringBuilder stbMessage;
            var dateStart = Request["_DateStart"];
            var dateEnd = Request["_DateEnd"];
            var pass = Request["PasswordNew"];
            var groupId = Request["GroupId"];
            var images = Request["Value_DefaultImages"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(enterprises);
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            enterprises.DateStart = dateStart.StringToDecimal();
                            enterprises.DateEnd = dateEnd.StringToDecimal();
                        }
                        enterprises.DateCreated = DateTime.Now.TotalSeconds();
                        if (!string.IsNullOrEmpty(pass))
                        {
                            var saltKey = FDIUtils.CreateSaltKey(5);
                            var sha1PasswordHash = FDIUtils.CreatePasswordHash(saltKey, pass);
                            enterprises.PasswordSalt = saltKey;
                            enterprises.Password = sha1PasswordHash;
                        }
                        enterprises.ST_Group = _enterprisesDa.GetListGroupByArrID(FDIUtils.StringToListInt(groupId));
                        enterprises.IsDeleted = false;
                        _enterprisesDa.Add(enterprises);
                        _enterprisesDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = enterprises.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(enterprises.Name))
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
                        enterprises = _enterprisesDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(enterprises);
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            enterprises.DateStart = dateStart.StringToDecimal();
                            enterprises.DateEnd = dateEnd.StringToDecimal();
                        }
                        if (!string.IsNullOrEmpty(pass))
                        {
                            var sha1PasswordHash = FDIUtils.CreatePasswordHash(enterprises.PasswordSalt, pass);
                            enterprises.Password = sha1PasswordHash;
                        }
                        enterprises.ST_Group.Clear();
                        enterprises.ST_Group = _enterprisesDa.GetListGroupByArrID(FDIUtils.StringToListInt(groupId));
                        _enterprisesDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = enterprises.ID.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(enterprises.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsEnterprises = _enterprisesDa.GetListByArrId(Request["itemID"]);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsEnterprises)
                    {
                        item.IsDeleted = true;
                        stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _enterprisesDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsEnterprises = _enterprisesDa.GetListByArrId(Request["itemID"]).Where(o => o.IsShow != null && !o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsEnterprises)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _enterprisesDa.Save();
                    msg.ID = string.Join(",", ltsEnterprises.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsEnterprises = _enterprisesDa.GetListByArrId(Request["itemID"]).Where(o => o.IsShow != null && o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsEnterprises)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _enterprisesDa.Save();
                    msg.ID = string.Join(",", ltsEnterprises.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
