using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class HtmlDA : BaseDA
    {
        public List<HtmlItem> GetList(int id = 0)
        {
            var query = from c in FDIDB.GetListHtml(id, LanguageId)
                        select new HtmlItem
                        {
                            ID = c.ID,
                            CtrId = c.ctrId,
                            Sort = c.Sort??0,
                            PageId = c.PageID??0,
                            Name = c.Name,
                            Section = c.Section
                        };
            return query.ToList();
        }
        public HtmlItem GetHtmlid(int id)
        {
            var query = from c in FDIDB.HtmlContents
                        where c.ID == id
                        select new HtmlItem
                        {
                            ID = c.ID,
                            Value = c.Value
                        };
            return query.FirstOrDefault();
        }

        public ModuleControl GetItemModule(int id)
        {
            var query = from c in FDIDB.ModuleControls
                        where c.Id == id
                        select c;
            return query.FirstOrDefault();
        }

        public int GetCountHtmlMap(int id)
        {
            var query = from c in FDIDB.HtmlMaps
                        where c.IdHtml == id
                        select c;
            return query.Count();
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

        public void Delete(HtmlContent item)
        {
            FDIDB.HtmlContents.Add(item);
        }

        public void Delete(ModuleControl item)
        {
            FDIDB.ModuleControls.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
