using System;
using System.Linq;
using System.Web.Mvc;
using FDI.Areas.Admin.Controllers;
using FDI.DA;
using FDI.Base;
using FDI.Utils;

namespace FDI.Areas.Setting.Controllers
{
    public class SettingController : Controller
    {
        readonly ModuleSettingDl _dl = new ModuleSettingDl();
        readonly ModuleSettingDl _moduleSetting = new ModuleSettingDl();

        #region Copy Module Html

        public PartialViewResult ModuleCopy(int idModule)
        {
            var model = _dl.GetListSysPage();
            ViewBag.key = idModule;
            return PartialView(model);
        }

        public JsonResult SaveModuleCopy(int idModule)
        {
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Thêm mới thành công."
            };
            try
            {
                var idPage = Request["sl-sysPage"];
                var model = _dl.GetModuleControl(idModule);
                var item = new ModuleControl
                {
                    PageID = int.Parse(idPage),
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

        #endregion

        public JsonResult SaveSetting()
        {
            var msg = new JsonMessage();
            var value = Request["value"];
            var ctrId = Request["ctrId"];
            try
            {
                var model = _moduleSetting.GetByKey(int.Parse(ctrId));
                if (model != null)
                {
                    model.Value = value;
                }
                else
                {
                    var obj = new ModuleSetting
                    {
                        ModuleId = int.Parse(ctrId),
                        Value = value,
                        LanguageId = Fdisystem.LanguageId
                    };
                    _moduleSetting.Add(obj);
                }
                _moduleSetting.Save();
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

        public PartialViewResult AjaxTreeCategorySelect(int type1, int type2)
        {
            //var ltsCategory = _dl.GetAllListSimple().Where(m => m.Type == type1 || m.Type == type2).ToList();
            //var ltsValues = FDIUtils.StringToListInt(Request["ValuesSelected"]);
            //var stbHtml = new StringBuilder();
            //_dl.BuildTreeViewCheckBox(ltsCategory, 1, true, ltsValues, ref stbHtml);
            //var model = new ModelCategoryItem
            //{
            //    Container = Request["Container"],
            //    SelectMutil = Convert.ToBoolean(Request["SelectMutil"]),
            //    PageHtml = stbHtml.ToString()
            //};
            //ViewData.Model = model;
            return PartialView();
        }
    }
}
