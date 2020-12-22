using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class MenuGroupsDa : BaseDA
    {
        #region Constructer
        public MenuGroupsDa()
        {
        }

        public MenuGroupsDa(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public MenuGroupsDa(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<MenuGroupsItem> GetListSimpleAll(int agencyId)
        {
            var query = from c in FDIDB.MenuGroups
                        where c.AgencyID == agencyId
                        orderby c.Name 
                        select new MenuGroupsItem
                                   {
                                       ID = c.Id,
                                       Name = c.Name.Trim(),
                                       Des = c.Des,
                                       Sort = c.Sort,
                                       IsShow = c.IsShow,
                                       CreatedDate = c.CreatedDate,
                                       UserCreate = c.UserCreate,
                                       UpdatedDate = c.UpdatedDate,
                                       UserUpdate = c.UserUpdate,
                                       PortalId = c.PortalId
                                   };
            return query.ToList();
        }

        public List<MenuGroupsItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.MenuGroups
                        where c.AgencyID == agencyId
                        orderby c.Id descending 
                        select new MenuGroupsItem
                                   {
                                       ID = c.Id,
                                       Name = c.Name.Trim(),
                                       Des = c.Des,
                                       Sort = c.Sort,
                                       IsShow = c.IsShow,
                                       CreatedDate = c.CreatedDate,
                                       UserCreate = c.UserCreate,
                                       UpdatedDate = c.UpdatedDate,
                                       UserUpdate = c.UserUpdate,
                                       PortalId = c.PortalId

                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public MenuGroupsItem GetItemById(int id)
        {
            var query = from c in FDIDB.MenuGroups
                        where c.Id == id
                        orderby c.Name
                        select new MenuGroupsItem
                        {
                            ID = c.Id,
                            Name = c.Name.Trim(),
                            Des = c.Des,
                            Sort = c.Sort,
                            IsShow = c.IsShow,
                            CreatedDate = c.CreatedDate,
                            UserCreate = c.UserCreate,
                            UpdatedDate = c.UpdatedDate,
                            UserUpdate = c.UserUpdate,
                            PortalId = c.PortalId
                        };
            return query.FirstOrDefault();
        }

        public MenuGroup GetById(int id)
        {
            var query = from c in FDIDB.MenuGroups where c.Id == id select c;
            return query.FirstOrDefault();
        }

        public List<MenuGroup> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.MenuGroups where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }

        public void Add(MenuGroup menuGroup)
        {
            FDIDB.MenuGroups.Add(menuGroup);
        }

        public void Delete(MenuGroup menuGroup)
        {
            FDIDB.MenuGroups.Remove(menuGroup);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
