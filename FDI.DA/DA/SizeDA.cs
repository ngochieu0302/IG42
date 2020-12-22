using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;


namespace FDI.DA
{
    public class SizeDA : BaseDA
    {
        #region Constructer
        public SizeDA()
        {
        }

        public SizeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public SizeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<SizeItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Sizes
                        orderby c.Name
                        select new SizeItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name.Trim()
                                   };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="isShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<SizeItem> GetListSimpleAll(bool isShow)
        {
            var query = from c in FDIDB.Sizes
                        where (c.IsShow == isShow)
                        orderby c.Name
                        select new SizeItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name.Trim()
                                   };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<SizeItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in FDIDB.Sizes
                        orderby c.Name
                        where c.Name.StartsWith(keyword)
                        select new SizeItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name.Trim()
                                   };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <param name="isShow"> </param>
        /// <returns></returns>
        public List<SizeItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Sizes
                        orderby c.Name
                        where c.IsShow == isShow
                        && c.Name.StartsWith(keyword)
                        select new SizeItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name.Trim(),
                                       Sort = c.Sort
                                   };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="httpRequest"> </param>
        /// <returns>Danh sách bản ghi</returns>
        public List<SizeItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Sizes
                        where  o.LanguageId == LanguageId
                        select new SizeItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name.Trim(),
                                       IsShow = o.IsShow,
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="ltsArrID"></param>
        /// <returns></returns>
        public List<SizeItem> GetListSimpleByArrID(List<int> ltsArrID)
        {
            var query = from o in FDIDB.Sizes
                        where ltsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new SizeItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name.Trim(),
                                   };
            TotalRecord = query.Count();
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SizeItem GetSystemConfigItemById(int id)
        {
            var query = from o in FDIDB.Sizes
                        where o.ID == id
                        orderby o.ID descending
                        select new SizeItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name.Trim(),
                                   };
            return query.FirstOrDefault();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="id">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Size GetById(int id)
        {
            var query = from c in FDIDB.Sizes where c.ID == id select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Size> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Sizes where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="size">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool CheckExits(Size size)
        {
            var query = from c in FDIDB.Sizes where ((c.Name == size.Name) && (c.ID != size.ID)) select c;
            return query.Any();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="name">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Size GetByName(string name)
        {
            var query = from c in FDIDB.Sizes where ((c.Name == name)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="size"> bản ghi cần thêm</param>
        public void Add(Size size)
        {
            FDIDB.Sizes.Add(size);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="size">Xóa bản ghi</param>
        public void Delete(Size size)
        {
            FDIDB.Sizes.Remove(size);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
