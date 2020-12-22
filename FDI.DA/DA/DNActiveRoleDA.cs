using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNActiveRoleDA : BaseDA
    {
        #region Constructer
        public DNActiveRoleDA()
        {
        }

        public DNActiveRoleDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNActiveRoleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<ActiveRoleItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Active
                        orderby c.ID
                        select new ActiveRoleItem
                                   {
                                       ID = c.ID,
                                       NameActive = c.NameActive,
                                       Ord = c.Ord
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<ActiveRoleItem> GetByAll()
        {
            var query = from c in FDIDB.DN_Active
                        select new ActiveRoleItem
                        {
                            ID = c.ID,
                            NameActive = c.NameActive,
                            Ord = c.Ord
                        };
            return query.ToList();
        }

        public List<Role_ModuleActiveItem> GetListRolsActive()
        {
            var query = from c in FDIDB.Role_ModuleActive
                        select new Role_ModuleActiveItem
                        {
                            ID = c.ID,
                            RoleId = c.RoleId,
                            Active = c.Active
                        };
            return query.ToList();
        }

        public DN_Active GetById(int id)
        {
            var query = from c in FDIDB.DN_Active
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <returns>Bản ghi</returns>
        public ActiveRoleItem GetRoleItemById(int id)
        {
            var query = from c in FDIDB.DN_Active
                        where c.ID == id
                        select new ActiveRoleItem
                        {
                            ID = c.ID,
                            NameActive = c.NameActive,
                            Ord = c.Ord
                        };
            var result = query.FirstOrDefault();
            return result;
        }

        public List<ActiveRoleItem> GetAll()
        {
            var query = from c in FDIDB.DN_Active
                        select new ActiveRoleItem
                                   {
                                       ID = c.ID,
                                       NameActive = c.NameActive,
                                       Ord = c.Ord
                                   };
            return query.ToList();
        }

        public List<DN_Active> GetListActiveRole()
        {
            var query = from c in FDIDB.DN_Active
                        select c;
            return query.ToList();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<DN_Active> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DN_Active where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="activeRole">bản ghi cần thêm</param>
        public void Add(DN_Active activeRole)
        {
            FDIDB.DN_Active.Add(activeRole);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        public void Delete(DN_Active activeRole)
        {
            FDIDB.DN_Active.Remove(activeRole);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
