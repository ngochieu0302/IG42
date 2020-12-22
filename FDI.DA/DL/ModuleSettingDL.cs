using System.Collections.Generic;
using System.Linq;
using FDI.Base;

namespace FDI.DA
{
    public class ModuleSettingDl : BaseDA
    {

        public IEnumerable<ModuleSetting> GetAll()
        {
            var query = from c in FDIDB.ModuleSettings
                        where c.LanguageId == LanguageId
                        select c;
            return query.ToList();
        }

        public ModuleSetting GetByKey(int moduleId)
        {
            var query = from c in FDIDB.ModuleSettings where c.ModuleId == moduleId && c.LanguageId == LanguageId select c;
            return query.FirstOrDefault();
        }

        public ModuleControl GetModuleControl(int id)
        {
            var query = from c in FDIDB.ModuleControls where c.Id == id select c;
            return query.FirstOrDefault();
        }

        public List<HtmlMap> GetHtmlMapById(int idctr)
        {
            var query = from c in FDIDB.HtmlMaps where c.IdModule == idctr select c;
            return query.ToList();
        }

        public IEnumerable<ModulePage> GetListSysPage()
        {
            var query = from c in FDIDB.ModulePages where c.Id > 1 select c;
            return query.ToList();
        }

        public void Add(ModuleSetting htmlSetting)
        {
            FDIDB.ModuleSettings.Add(htmlSetting);
        }

        public void Add(ModuleControl obj)
        {
            FDIDB.ModuleControls.Add(obj);
        }

        public void Add(HtmlMap obj)
        {
            FDIDB.HtmlMaps.Add(obj);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}