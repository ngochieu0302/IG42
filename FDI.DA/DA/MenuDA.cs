using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Simple;
using FDI.Base;

namespace FDI.DA
{
    public class MenuDA : BaseDA
    {
        
        public MenuDA()
        {

        }

        public MenuDA(string pathPaging)
        {
            PathPaging = pathPaging;

        }

        public MenuDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        //public List<MenusItem> GetChildByParentId(int groupId, int parentId = 1)
        //{
        //    var cate = from c in FDIDB.Menu_GetChildByParentId(groupId, parentId, LanguageId)
        //               select new MenusItem
        //               {
        //                   ID = c.Id,
        //                   ParentId = c.ParentId,
        //                   Name = c.Name
        //               };
        //    var re = cate.ToList();
        //    return re;
        //}

        public List<MenusItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId, int groupId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Menus
                        where c.AgencyID == agencyId && c.GroupId == groupId
                        orderby c.Id descending
                        select new MenusItem
                        {
                            ID = c.Id,
                            Name = c.Name.Trim(),
                            GroupId = c.GroupId,
                            ParentId = c.ParentId,
                            IsLevel = c.IsLevel,
                            IsNewTab = c.IsNewTab,
                            Type = c.Type,
                            Sort = c.Sort,
                            Url = c.Url,
                            Icon = c.Icon,
                            LanguageId = c.LanguageId,
                            CreatedDate = c.CreatedDate,
                            PortalId = c.PortalId,
                            Active = c.Active

                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }



        public List<MenusItem> GetListByParentId(int groupId, int agencyId)
        {
            var cate = from c in FDIDB.Menu_GetList(LanguageId, groupId, agencyId)
                       select new MenusItem
                       {
                           ID = c.Id,
                           ParentId = c.ParentId,
                           Name = c.Name,
                           Url = c.Slug,
                           Type = c.Type,
                           IsLevel = c.IsLevel
                       };
            var re = cate.ToList();
            return re;
        }

        public MenusItem GetItemById(int id)
        {
            var query = from c in FDIDB.Menus
                        orderby c.Name
                        where c.Id == id
                        select new MenusItem
                        {
                            ID = c.Id,
                            Name = c.Name.Trim(),
                            GroupId = c.GroupId,
                            ParentId = c.ParentId,
                            IsLevel = c.IsLevel,
                            IsShow = c.IsShow,
                            IsNewTab = c.IsNewTab,
                            Type = c.Type,
                            Sort = c.Sort,
                            Url = c.Url,
                            Icon = c.Icon,
                            LanguageId = c.LanguageId,
                            CreatedDate = c.CreatedDate,
                            PortalId = c.PortalId,
                            Active = c.Active
                        };
            return query.FirstOrDefault();
        }
        
        public List<MenusItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Menus
                        orderby c.Name
                        where c.LanguageId == LanguageId
                        && c.Name.StartsWith(keyword)
                        select new MenusItem
                                   {
                                       ID = c.Id,
                                       Name = c.Name.Trim(),
                                       GroupId = c.GroupId,
                                       ParentId = c.ParentId,
                                       IsLevel = c.IsLevel,
                                       IsNewTab = c.IsNewTab,
                                       Type = c.Type,
                                       Sort = c.Sort,
                                       Url = c.Url,
                                       Icon = c.Icon,
                                       LanguageId = c.LanguageId,
                                       CreatedDate = c.CreatedDate,
                                       PortalId = c.PortalId,
                                       Active = c.Active
                                   };
            return query.Take(showLimit).ToList();
        }

        public List<TreeViewItem> GetListTree(int groupId, int agencyId)
        {
            var query = from c in FDIDB.Menus
                        where c.IsDeleted == false && c.GroupId == groupId && c.LanguageId == LanguageId && c.AgencyID == agencyId
                        orderby c.IsLevel,c.Sort
                        select new TreeViewItem
                        {
                            ID = c.Id,
                            Name = c.Name.Trim(),
                            GroupId = c.GroupId,
                            ParentId = c.ParentId,
                            IsShow = c.IsShow,
                            Count = FDIDB.Menus.Count(m=>m.ParentId == c.Id)
                        };
            return query.ToList();
        }

        public List<MenusItem> GetAllListSimpleByParentId(int parent, int groupId)
        {
            var query = from c in FDIDB.Menus
                        where c.Id > 1 && c.ParentId == parent && c.GroupId == groupId
                        orderby c.Sort
                        select new MenusItem
                        {
                            ID = c.Id,
                            Name = c.Name.Trim(),
                            GroupId = c.GroupId,
                            ParentId = c.ParentId,
                            PageId = c.PageId,
                            IsLevel = c.IsLevel,
                            IsNewTab = c.IsNewTab,
                            Type = c.Type,
                            Sort = c.Sort,
                            Url = c.Url,
                            Icon = c.Icon,
                            LanguageId = c.LanguageId,
                            CreatedDate = c.CreatedDate,
                            PortalId = c.PortalId,
                            Active = c.Active
                        };
            return query.ToList();
        }

        public Menu GetById(int id)
        {
            var query = from c in FDIDB.Menus where c.Id == id select c;
            return query.FirstOrDefault();
        }

        public List<Menu> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Menus where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }
       
        public void Add(Menu menu)
        {
            FDIDB.Menus.Add(menu);
        }
       
        public void Delete(Menu menu)
        {
            FDIDB.Menus.Remove(menu);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
        
    }
}
