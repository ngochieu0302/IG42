using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class VoteTypeDA : BaseDA
    {
        #region Constructer
        public VoteTypeDA()
        {
        }

        public VoteTypeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public VoteTypeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<VoteTypeItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Vote_Type
                        orderby c.ID 
                        where c.LanguageId == LanguageId && c.IsShow == true
                        select new VoteTypeItem
                                   {
                                       Id = c.ID,
                                       Title = c.Title,
                                       Sort = c.Sort,
                                       Type = c.Type,
                                       IsHome = c.IsHome,
                                       Description = c.Description,
                                       IsShow = c.IsShow,
                                       
                                   };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="isShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<VoteTypeItem> GetListSimpleAll(bool isShow)
        {
            var query = from c in FDIDB.Vote_Type
                        where (c.IsShow == isShow) && c.LanguageId == LanguageId
                        orderby c.ID
                        select new VoteTypeItem
                        {
                            Id = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            Sort = c.Sort,
                            Type = c.Type,
                            IsHome = c.IsHome,
                            IsShow = c.IsShow,
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<VoteTypeItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in FDIDB.Vote_Type
                        orderby c.ID
                        where c.Title.StartsWith(keyword) && c.LanguageId == LanguageId && c.IsShow == true
                        select new VoteTypeItem
                        {
                            Id = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            Sort = c.Sort,
                            Type = c.Type,
                            IsHome = c.IsHome,
                            IsShow = c.IsShow,
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
        public List<VoteTypeItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Vote_Type
                        orderby c.ID
                        where c.IsShow == isShow && c.LanguageId == LanguageId
                        && c.Title.StartsWith(keyword)
                        select new VoteTypeItem
                                   {
                                       Id = c.ID,
                                       Title = c.Title.Trim()
                                   };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="httpRequest"> </param>
        /// <returns>Danh sách bản ghi</returns>
        public List<VoteTypeItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Vote_Type
                        where c.LanguageId == LanguageId && c.IsShow == true
                        select new VoteTypeItem
                        {
                            Id = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            Sort = c.Sort,
                            Type = c.Type,
                            IsHome = c.IsHome,
                            IsShow = c.IsShow,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="ltsArrID"></param>
        /// <returns></returns>
        public List<VoteTypeItem> GetListSimpleByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Vote_Type
                        where ltsArrID.Contains(c.ID) && c.LanguageId == LanguageId && c.IsShow == true
                        orderby c.ID descending
                        select new VoteTypeItem
                        {
                            Id = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            Sort = c.Sort,
                            Type = c.Type,
                            IsHome = c.IsHome,
                            IsShow = c.IsShow,
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VoteTypeItem GetPollItemById(int id)
        {
            var query = from c in FDIDB.Vote_Type
                        where c.ID == id
                        orderby c.ID descending
                        select new VoteTypeItem
                        {
                            Id = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            Sort = c.Sort,
                            Type = c.Type,
                            IsHome = c.IsHome,
                            IsShow = c.IsShow,
                        };
            return query.FirstOrDefault();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="id">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Vote_Type GetById(int id)
        {
            var query = from c in FDIDB.Vote_Type where c.ID == id select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Vote_Type> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Vote_Type where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="systemConfig">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool CheckExits(Vote_Type voteType)
        {
            var query = from c in FDIDB.Vote_Type where ((c.Title == voteType.Title) && (c.ID != voteType.ID)) select c;
            return query.Any();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="name">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Vote_Type GetByName(string name)
        {
            var query = from c in FDIDB.Vote_Type where ((c.Title == name)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="systemConfig"> bản ghi cần thêm</param>
        public void Add(Vote_Type voteType)
        {
            FDIDB.Vote_Type.Add(voteType);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="systemConfig">Xóa bản ghi</param>
        public void Delete(Vote_Type voteType)
        {
            FDIDB.Vote_Type.Remove(voteType);
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
