using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA.DA
{
    public class ModuleControlDA:BaseDA
    {
        #region Constructer
        public ModuleControlDA()
        {
        }

        public ModuleControlDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ModuleControlDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<ModuleControlItem> GetListByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.ModuleControls
                        where o.LanguageID == LanguageId
                        orderby o.Id, o.Sort descending
                        select new ModuleControlItem
                        {
                            ID = o.Id,
                            Action = o.Action,
                            Module = o.Module,
                            PageID = o.PageID,
                            PageName = o.ModulePage.Name,
                            Section = o.Section,
                            Sort = o.Sort
                        };
            var pageId = httpRequest["PageID"];
            if (!string.IsNullOrEmpty(pageId))
            {
                var t = int.Parse(pageId);
                query = query.Where(c => c.PageID == t);

            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public ModuleControlItem GetItemById(int id)
        {
            var query = from o in FDIDB.ModuleControls
                        where o.Id == id
                        select new ModuleControlItem
                        {
                            ID = o.Id,
                            Action = o.Action,
                            Module = o.Module,
                            PageID = o.PageID,
                            PageName = o.ModulePage.Name,
                            Section = o.Section,
                            Sort = o.Sort,
                            Layout = o.ModulePage.Layout
                        };
            return query.FirstOrDefault();
        }
        public ModuleControl GetById(int id)
        {
            var query = from c in FDIDB.ModuleControls where c.Id == id select c;
            return query.FirstOrDefault();
        }
        public List<ModuleControl> GetByArrId(List<int> lst)
        {
            var query = from c in FDIDB.ModuleControls where lst.Contains(c.Id) select c;
            return query.ToList();
        }
        public void Add(ModuleControl item)
        {
            FDIDB.ModuleControls.Add(item);
        }
        public void Delete(ModuleControl item)
        {
            FDIDB.ModuleControls.Remove(item);
        }
    }
}
