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
    public class DNNewsSSCController : BaseController
    {
        private readonly DNNewsSSCDA _dnNewsSscda;
        private readonly AgencyDA _agencyDa;
        public DNNewsSSCController()
        {
            _dnNewsSscda = new DNNewsSSCDA("#");
            _agencyDa = new AgencyDA("#");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelDNNewsSSCItem
            {
                ListItem = _dnNewsSscda.GetListSimpleByRequest(Request),
                PageHtml = _dnNewsSscda.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var customerType = _dnNewsSscda.GetById(ArrId.FirstOrDefault());
            ViewData.Model = customerType;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var dnNewsSsc = new DN_NewsSSC();
            if (DoAction == ActionType.Edit)
            {
                dnNewsSsc = _dnNewsSscda.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.AgencyID = _agencyDa.GetAll();
            ViewData.Model = dnNewsSsc;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var dnNewsSsc = new DN_NewsSSC();
            List<DN_NewsSSC> ltsdnNewsSsc;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(dnNewsSsc);
                        dnNewsSsc.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        _dnNewsSscda.Add(dnNewsSsc);
                        _dnNewsSscda.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnNewsSsc.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(dnNewsSsc.Title))
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
                        dnNewsSsc = _dnNewsSscda.GetById(ArrId.FirstOrDefault());
                        dnNewsSsc.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        UpdateModel(dnNewsSsc);
                        _dnNewsSscda.Save();

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnNewsSsc.ID.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(dnNewsSsc.Title))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                //case ActionType.Delete:
                //    ltsdnNewsSsc = _dnNewsSscda.GetListByArrId(ArrId.FirstOrDefault());
                //    stbMessage = new StringBuilder();
                //    foreach (var item in ltsdnNewsSsc)
                //    {
                //        _dnNewsSscda.Delete(item);
                //        stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));

                //    }
                //    msg.ID = string.Join(",", ArrId);
                //    _dnNewsSscda.Save();
                //    msg.Message = stbMessage.ToString();
                //    break;
                //case ActionType.Show:
                //    ltsdnNewsSsc = _dnNewsSscda.GetListByArrId(ArrId).Where(o => o.IsShow != null && !o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                //    stbMessage = new StringBuilder();
                //    foreach (var item in ltsdnNewsSsc)
                //    {
                //        item.IsShow = true;
                //        stbMessage.AppendFormat("Đã hiển thị chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                //    }
                //    _dnNewsSscda.Save();
                //    msg.ID = string.Join(",", ltsdnNewsSsc.Select(o => o.ID));
                //    msg.Message = stbMessage.ToString();
                //    break;

                //case ActionType.Hide:
                //    ltsdnNewsSsc = _dnNewsSscda.GetListByArrId(ArrId).Where(o => o.IsShow != null && o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng được hiển thị
                //    stbMessage = new StringBuilder();
                //    foreach (var item in ltsdnNewsSsc)
                //    {
                //        item.IsShow = false;
                //        stbMessage.AppendFormat("Đã ẩn chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                //    }
                //    _dnNewsSscda.Save();
                //    msg.ID = string.Join(",", ltsdnNewsSsc.Select(o => o.ID));
                //    msg.Message = stbMessage.ToString();
                //    break;
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
