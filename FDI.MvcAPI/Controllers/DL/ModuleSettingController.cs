using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class ModuleSettingController : BaseApiController
    {
        readonly ModuleSettingDl _dl = new ModuleSettingDl();

        public JsonResult GetAll()
        {
            var obj = Request["key"] != Keyapi
                ? new List<ModuleSettingItem>() : _dl.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByKey(int moduleId)
        {
            var obj = Request["key"] != Keyapi
               ? new ModuleSettingItem() : _dl.GetByKey(moduleId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModuleControl(int id)
        {
            var obj = Request["key"] != Keyapi
               ? new ModuleControlItem() : _dl.GetModuleControl(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHtmlMapById(int idctr)
        {
            var obj = Request["key"] != Keyapi
               ? new List<HtmlMapItem>() : _dl.GetHtmlMapById(idctr);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListSysPage()
        {
            var obj = Request["key"] != Keyapi
               ? new List<ModulePageItem>() : _dl.GetListSysPage();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveModuleCopy(int idModule, int idPage)
        {
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Thêm mới thành công."
            };
            try
            {
                var model = _dl.GetModuleControl(idModule);
                var item = new ModuleControl
                {
                    PageID = idPage,
                    Action = model.Action,
                    Module = model.Module,
                    Section = model.Section,
                    Sort = model.Sort
                };
                _dl.Add(item);
                _dl.Save();
                if (model.Action.ToLower().Equals(("ModuleHTML").ToLower()))
                {
                    var html = _dl.GetHtmlMapById(idModule).FirstOrDefault();
                    var htmlnew = new HtmlMap
                    {
                        IdHtml = html.IdHtml,
                        IdModule = item.Id
                    };
                    _dl.Add(htmlnew);
                    _dl.Save();
                }
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Có lỗi xảy ra";
            }
            return Json(msg);
        }
        public JsonResult SaveSetting(string value, int ctrId)
        {
            var msg = new JsonMessage();
            try
            {
                var model = _dl.GetItemSetting(ctrId);
                if (model != null)
                {
                    model.Value = value;
                    model.AgencyID = Agencyid();
                }
                else
                {
                    var obj = new ModuleSetting
                    {
                        ModuleId = ctrId,
                        Value = value,
                        LanguageId = "vi",
                        AgencyID = Agencyid()
                    };
                    _dl.Add(obj);
                }
              
                _dl.Save();
                msg.Erros = false;
                msg.Message = "Thêm mới thành công";
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