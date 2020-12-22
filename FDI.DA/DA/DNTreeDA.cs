using System;
using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNTreeDA : BaseDA
    {
        public DNTreeDA()
        {
        }
        public DNTreeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNTreeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNTreeItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_Tree
                        where o.AgencyID == agencyId && o.IsDelete == false
                        orderby o.ID descending
                        select new DNTreeItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Level = o.Level,
                            UserID = o.DN_UsersInRoles.UserId,
                            UserName = o.DN_UsersInRoles.DN_Users.UserName,
                            FullName = o.DN_UsersInRoles.DN_Users.LoweredUserName,
                            ListID = "," + o.ListID + ",",
                            ParentID = o.ParentID,
                            UserInRoleID = o.UserInRoleID
                        };
            return query.ToList();
        }
        public List<DNTreeItem> GetListParent(int agencyId)
        {
            var query = from o in FDIDB.GetListTree(agencyId)
                        select new DNTreeItem
                        {
                            ID = o.Id,
                            Level = o.Level,
                            Name = o.Symbol + o.NameRoot,
                            UserName = o.UserName
                        };
            return query.ToList();
        }
        public List<DNTreeItem> GetList(int agencyId)
        {
            var query = from o in FDIDB.DN_Tree
                        where o.AgencyID == agencyId && o.IsDelete == false
                        orderby o.ID descending
                        select new DNTreeItem
                        {
                            ID = o.ID,
                            Level = o.Level,
                            ListID = o.ListID,
                            ParentID = o.ParentID,
                            UserInRoleID = o.UserInRoleID
                        };
            return query.ToList();
        }
        public List<TreeViewItem> GetListTree(int agencyId)
        {
            var query = from c in FDIDB.DN_Tree
                        where c.Level > 0 && c.AgencyID == agencyId && c.IsDelete == false
                        orderby c.Level
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            GuiId = c.DN_UsersInRoles.UserId,
                            IsShow = true,
                            RolesName = c.DN_UsersInRoles.DN_Department.Name,
                            UserName = c.DN_UsersInRoles.DN_Users.UserName,
                            FullName = c.DN_UsersInRoles.DN_Users.LoweredUserName,
                            Name = c.Name,
                            ParentId = c.ParentID,
                        };
            return query.ToList();
        }
        public List<TreeViewItem> GetListTree(int treeId, int agencyId)
        {
            var item = GetDNTreeItem(treeId);
            var lstInt = FDIUtils.StringToListInt(item.ListID);
            var id = item.ID.ToString();
            var query = from c in FDIDB.DN_Tree
                        where c.Level > 0 && c.AgencyID == agencyId && c.IsDelete == false
                        && ((2 - c.Level) < 3 && lstInt.Contains(c.ID) || c.ID == item.ID || c.ListID.Contains(id))
                        orderby c.Level
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            FullName = c.DN_UsersInRoles.DN_Users.LoweredUserName,
                            UserName = c.DN_UsersInRoles.DN_Users.UserName,
                        };
            return query.ToList();
        }
        public List<DNTreeItem> GetListTreeItem(Guid guid)
        {
            var query = from o in FDIDB.DN_Tree
                        where o.DN_UsersInRoles.UserId == guid
                        orderby o.ID descending
                        select new DNTreeItem
                        {
                            ID = o.ID,
                            Level = o.Level,
                            ListID = o.ListID,
                            Name = o.Name
                        };
            return query.ToList();
        }
        public DNTreeItem GetDNTreeItem(Guid guid)
        {
            var query = from o in FDIDB.DN_Tree
                        where o.DN_UsersInRoles.UserId == guid
                        orderby o.ID descending
                        select new DNTreeItem
                        {
                            ID = o.ID,
                            Level = o.Level,
                            ListID = o.ListID,
                            ParentID = o.ParentID,
                            Name = o.Name,
                            UserName = o.UserName,
                            RoleId = o.RoleId,
                            DepartmentId = o.DepartmentId,
                            UserInRoleID = o.UserInRoleID
                        };
            return query.FirstOrDefault();
        }
        public DNTreeItem GetDNTreeItem(int id)
        {
            var query = from o in FDIDB.DN_Tree
                        where o.ID == id
                        orderby o.ID descending
                        select new DNTreeItem
                        {
                            ID = o.ID,
                            Level = o.Level,
                            ListID = o.ListID,
                            ParentID = o.ParentID,
                            Name = o.Name,
                            UserName = o.UserName,
                            RoleId = o.RoleId,
                            DepartmentId = o.DepartmentId,
                            UserInRoleID = o.UserInRoleID
                        };
            return query.FirstOrDefault();
        }
        public DN_Tree GetById(int id)
        {
            var query = from o in FDIDB.DN_Tree where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<DN_Tree> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.DN_Tree where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(DN_Tree item)
        {
            FDIDB.DN_Tree.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
