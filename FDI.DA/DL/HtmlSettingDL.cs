using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class HtmlSettingDL : BaseDA
    {
        public List<HtmlMapItem> GetList()
        {
            var query = from c in FDIDB.HtmlMaps
                        select new HtmlMapItem
                        {
                            IdHtml = c.IdHtml,
                            IdModule = c.IdModule,
                            IdCopy = c.IdCopy,
                            Value = c.HtmlContent.Value,
                            LanguageId = c.LanguageId
                        };
            return query.ToList();
        }

        public HtmlMapItem GetByKey(int id)
        {
            var query = from c in FDIDB.HtmlMaps
                        where c.IdModule == id && c.LanguageId == LanguageId
                        select new HtmlMapItem
                        {
                            IdHtml = c.IdHtml,
                            IdModule = c.IdModule,
                            Value = c.HtmlContent.Value,
                            LanguageId = c.LanguageId
                        };
            return query.FirstOrDefault();
        }

        public HtmlContent GetByid(int id)
        {
            var query = from c in FDIDB.HtmlContents
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        
        public void Add(HtmlContent htmlSetting)
        {
            FDIDB.HtmlContents.Add(htmlSetting);
        }

        public void Add(HtmlMap htmlModule)
        {
            FDIDB.HtmlMaps.Add(htmlModule);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}