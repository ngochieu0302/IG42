using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DocumentFilesDA : BaseDA
    {
        #region Constructer
        public DocumentFilesDA()
        {
        }

        public DocumentFilesDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DocumentFilesDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<DocumentFilesItem> GetListSimpleAll(int agencyId)
        {
            var query = from c in FDIDB.DocumentFiles
                        orderby c.ID
                        select new DocumentFilesItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name,
                                       FileUrl = c.FileUrl,
                                       DateCreated = c.DateCreated,
                                   };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<DocumentFilesItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in FDIDB.DocumentFiles
                        orderby c.Name
                        where c.Name.StartsWith(keyword) 
                        select new DocumentFilesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            FileUrl = c.FileUrl,
                            DateCreated = c.DateCreated,
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
        public List<DocumentFilesItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.DocumentFiles
                        orderby c.Name
                        where  c.Name.StartsWith(keyword)
                        select new DocumentFilesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            FileUrl = c.FileUrl,
                            DateCreated = c.DateCreated,
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="httpRequest"> </param>
        /// <returns>Danh sách bản ghi</returns>
        public List<DocumentFilesItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DocumentFiles
                        select new DocumentFilesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            FileUrl = c.FileUrl,
                            DateCreated = c.DateCreated,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DocumentFilesItem GetItemsByID(int id)
        {
            var query = from c in FDIDB.DocumentFiles
                        where c.ID == id
                        select new DocumentFilesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            FileUrl = c.FileUrl,
                            DateCreated = c.DateCreated,
                        };
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="ltsArrID"></param>
        /// <returns></returns>
        public List<DocumentFilesItem> GetListSimpleByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DocumentFiles
                        where ltsArrID.Contains(c.ID)
                        orderby c.ID descending
                        select new DocumentFilesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            FileUrl = c.FileUrl,
                            DateCreated = c.DateCreated,
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        public List<DocumentFile> GetListFileByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DocumentFiles where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentFilesItem GetSystemConfigItemById(int id)
        {
            var query = from c in FDIDB.DocumentFiles
                        where c.ID == id
                        orderby c.ID descending
                        select new DocumentFilesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            FileUrl = c.FileUrl,
                            DateCreated = c.DateCreated,
                        };
            return query.FirstOrDefault();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="id">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public DocumentFile GetById(int id)
        {
            var query = from c in FDIDB.DocumentFiles where c.ID == id select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<DocumentFile> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DocumentFiles where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="systemConfig">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool CheckExits(DocumentFile document)
        {
            var query = from c in FDIDB.DocumentFiles where ((c.Name == document.Name) && (c.ID != document.ID)) select c;
            return query.Any();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="name">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public DocumentFile GetByName(string name)
        {
            var query = from c in FDIDB.DocumentFiles where ((c.Name == name)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Language"> bản ghi cần thêm</param>
        public void Add(DocumentFile document)
        {
            FDIDB.DocumentFiles.Add(document);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="systemConfig">Xóa bản ghi</param>
        public void Delete(DocumentFile document)
        {
            FDIDB.DocumentFiles.Remove(document);
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
