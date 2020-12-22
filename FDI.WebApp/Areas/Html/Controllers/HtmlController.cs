using System;
using System.Linq;
using System.Web.Mvc;
using FDI.Areas.Admin.Controllers;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Areas.Html.Controllers
{
    public class HtmlController : Controller
    {
        //
        // GET: /Html/Html/
        private readonly HtmlSettingDL _da = new HtmlSettingDL();
        private readonly HtmlSettingBL _bl = new HtmlSettingBL();

        public PartialViewResult Index(int ctrId, string url)
        {
            var model = new ModelHtmlMapItem
            {
                CtrId = ctrId,
                CtrUrl = url
            };
            var lst = _bl.GetByKey(ctrId);
            var lang = Fdisystem.LanguageId;
            var item1 = lst.FirstOrDefault(c => c.LanguageId == lang);
            if (item1 != null)
            {
                model.HtmlMapItem = item1;
                return PartialView(model);
            }
            if (!lst.Any())
            {//Chưa có nội dung html của ngôn ngữ nào
                AddHtmlMap(ctrId, AddHtmlContent(ctrId).ID, null);
                model.HtmlMapItem = new HtmlMapItem {IdModule = ctrId, Value = ""};
                return PartialView(model);
            }
            var item2 = lst.FirstOrDefault();
            if (item2.IdCopy == null)
            {//Chưa có nội dung html ngôn ngữ hiện tại, Ngôn ngữ khác ko được copy 
                AddHtmlMap(ctrId, AddHtmlContent(ctrId).ID, null);
                model.HtmlMapItem = new HtmlMapItem {Value = "", IdModule = ctrId};
                return PartialView(model);
            }
            //Chưa có nội dung html ngôn ngữ hiện tại, Ngôn ngữ khác được coppy 
            var item3 = _bl.GetByKey((int)item2.IdCopy).FirstOrDefault(c => c.LanguageId == lang);
            if (item3 != null)
            {
                AddHtmlMap(ctrId, item3.IdHtml, item2.IdCopy);
                model.HtmlMapItem = new HtmlMapItem {Value = item3.Value, IdModule = ctrId};
                return PartialView(model);
            }

            var htmlcontent = AddHtmlContent((int)item2.IdCopy);
            AddHtmlMap(item2.IdCopy, htmlcontent.ID, null);
            AddHtmlMap(ctrId, htmlcontent.ID, item2.IdCopy);
            model.HtmlMapItem = new HtmlMapItem {ID = htmlcontent.ID, Value = "", IdModule = ctrId};
            return PartialView(model);
        }

        public HtmlContent AddHtmlContent(int idModule)
        {
            var html = new HtmlContent { Value = "" };
            _da.Add(html);
            _da.Save();
            return html;
        }

        public HtmlMap AddHtmlMap(int? idModule, int? idHtml, int? idCoppy)
        {
            var html = new HtmlMap
            {
                IdHtml = idHtml,
                IdModule = idModule,
                IdCopy = idCoppy,
                LanguageId = Utility.Getcookie("LanguageId")
            };
            _da.Add(html);
            _da.Save();
            return html;
        }

        public ActionResult EditHtml(int id = 0)
        {
            var model = _da.GetByKey(id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public object Save()
        {
            var msg = new JsonMessage();
            try
            {
                var editorHtml = Request.Unvalidated["editorHtml"];
                var htmlId = Request["htmlId"];
                var item = _da.GetByid(int.Parse(htmlId));
                item.Value = editorHtml;
                _da.Save();
                msg.Erros = false;
                msg.Message = "Cập nhật thành công.";
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Có lỗi xảy ra vui lòng thử lại.";
            }
            return Json(msg);
        }

    }
}
