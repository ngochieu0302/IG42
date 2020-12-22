using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class STGroupDA : BaseDA
    {
        #region Constructer
        public STGroupDA()
        {
        }

        public STGroupDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public STGroupDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<STGroupItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.ST_Group
                        orderby c.Sort
                        select new STGroupItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<STGroupItem> GetAll()
        {
            var query = from c in FDIDB.ST_Group
                        orderby c.Sort
                        select new STGroupItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                        };
            return query.ToList();
        }

        public List<STGroupItem> GetListByEnterprisesId(int enterprisesId)
        {
            var query = from c in FDIDB.ST_Group
                        where c.DN_Enterprises.Any(m => m.ID == enterprisesId)
                        orderby c.Sort
                        select new STGroupItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                        };
            return query.ToList();
        }

        public ST_Group GetById(int id)
        {
            var query = from c in FDIDB.ST_Group
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        
        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<ST_Group> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.ST_Group where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="stGroup">bản ghi cần thêm</param>
        public void Add(ST_Group stGroup)
        {
            FDIDB.ST_Group.Add(stGroup);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        public void Delete(ST_Group stGroup)
        {
            FDIDB.ST_Group.Remove(stGroup);
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
