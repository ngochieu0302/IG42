using System;
using System.Linq;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ModuleControlDL : BaseDA
    {
        public ModeItem GetModulById(int id)
        {
            try
            {
                var query = from c in FDIDB.ModuleControls
                            where c.Id == id 
                            select new ModeItem
                            {
                                ID = c.Id,
                                Module = c.Module,
                                Action = c.Action,
                                Section = c.Section,
                                Sort = c.Sort ?? 0
                            };
                return query.FirstOrDefault() ?? new ModeItem();
            }
            catch (Exception)
            {
                return new ModeItem();
            }

        }

        public ModuleControl GetItemModule(int id)
        {
            try
            {
                var query = from c in FDIDB.ModuleControls
                            where c.Id == id 
                            select c;
                return query.FirstOrDefault() ?? new ModuleControl();
            }
            catch (Exception)
            {
                return new ModuleControl();
            }
        }

        public int GetCountHtmlMap(int id)
        {
            var query = from c in FDIDB.HtmlMaps
                        where c.IdHtml == id
                        select c;
            return query.Count();
        }

        public HtmlContent GetHtmlContent(int id)
        {
            var query = from c in FDIDB.HtmlContents
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }

        public HtmlMap GetHtmlMap(int ctrId)
        {
            var lang = Utility.Getcookie("LanguageId");
            var query = from c in FDIDB.HtmlMaps
                        where c.IdModule == ctrId && c.LanguageId == lang
                        select c;
            return query.FirstOrDefault();
        }

        public void Add(ModuleControl item)
        {
            FDIDB.ModuleControls.Add(item);
        }

        public void Add(HtmlMap item)
        {
            FDIDB.HtmlMaps.Add(item);
        }

        public void Delete(ModuleControl item)
        {
            FDIDB.ModuleControls.Remove(item);
        }

        public void Delete(HtmlContent item)
        {
            FDIDB.HtmlContents.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
