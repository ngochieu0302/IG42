using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ModulePageDA : BaseDA
    {
        public ModulePageDA()
        {
        }

        public ModulePageDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ModulePageDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<SysPageItem> GetChildByParentId(int parentId = 1, int moduleType = 0, int root = 1)
        {
            var query = from c in FDIDB.SysPage_GetChildByParent(parentId, moduleType, root, Utility.AgencyId)
                         select new SysPageItem
                         {
                             ID = c.Id,
                             ParentId = c.ParentId,
                             Layout = c.Layout,
                             Level = c.Level,
                             Name = c.Symbol + c.Name
                         };
            return query.ToList();
        }

        public List<TreeViewItem> GetListTree()
        {
            var query = from c in FDIDB.ModulePages
                        where c.Id > 1
                        orderby c.Level, c.Sort
                        select new TreeViewItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            ParentId = c.ParentId,
                            IsShow = true,
                            Count = FDIDB.ModulePages.Count(m => m.ParentId == c.Id)
                        };
            return query.ToList();
        }
        public SysPageItem GetSysPageItem(int id)
        {
            var query = from c in FDIDB.ModulePages
                        where c.Id == id && c.IsDelete == false
                        select new SysPageItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Level = c.Level

                        };
            return query.FirstOrDefault();
        }
        public List<SysPageItem> GetAllListSimpleByParentId(int parentId)
        {
            var query = from c in FDIDB.ModulePages
                        where c.Id > 1 && c.ParentId == parentId
                        select new SysPageItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            Type = c.Type,
                            CreateDate = c.CreateDate,
                            Key = c.Key,
                            Layout = c.Layout
                        };
            return query.ToList();
        }

        public List<SysPageItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.ModulePages
                        where c.Id > 1
                        orderby c.Name
                        where c.Name.StartsWith(keyword)
                        select new SysPageItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Sort = c.Sort,
                            ParentId = c.ParentId,
                            Type = c.Type,
                            CreateDate = c.CreateDate,
                            Key = c.Key,
                            Layout = c.Layout
                        };
            return query.Take(showLimit).ToList();
        }

        public bool CheckTitleAsciiExits(string key, int id)
        {
            var query = (from c in FDIDB.ModulePages
                         where c.Key == key && c.Id != id
                         select c).Count();
            return query > 0;
        }

        public SysPageItem GetBykey(string key)
        {
            var query = from c in FDIDB.ModulePages
                        where c.Key == key
                        select new SysPageItem
                        {
                            ID = c.Id,
                            Layout = c.Layout,
                            SeoDescription = c.SEODescription,
                            SeoKeyword = c.SEOKeyword,
                            SeoTitle = c.SEOTitle,
                            FeUrl = c.FeUrl,
                            LstModeItems = c.ModuleControls.Select(u => new ModeItem
                            {
                                ID = u.Id,
                                Action = u.Action,
                                Module = u.Module,
                                Area = u.Module,
                                Section = u.Section,
                                Sort = u.Sort ?? 0
                            })
                        };
            return query.FirstOrDefault();
        }

        public SysPageItem GetById(int id)
        {
            var query = from c in FDIDB.ModulePages
                        where c.Id == id
                        select new SysPageItem
                        {
                            ID = c.Id,
                            Layout = c.Layout,
                            SeoDescription = c.SEODescription,
                            SeoKeyword = c.SEOKeyword,
                            SeoTitle = c.SEOTitle,
                            FeUrl = c.FeUrl,
                            LstModeItems = c.ModuleControls.OrderBy(u => u.Sort).Select(u => new ModeItem
                            {
                                ID = u.Id,
                                Action = u.Action,
                                Module = u.Module,
                                Area = u.Module,
                                Section = u.Section,
                                Sort = u.Sort ?? 0
                            })
                        };
            return query.FirstOrDefault();
        }

        public ModulePage Get(int id)
        {
            var query = from c in FDIDB.ModulePages where c.Id == id select c;
            return query.FirstOrDefault();
        }

        public List<ModulePage> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.ModulePages where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }

        public void Add(ModulePage item)
        {
            FDIDB.ModulePages.Add(item);
        }

        public void Delete(ModulePage item)
        {
            FDIDB.ModulePages.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
