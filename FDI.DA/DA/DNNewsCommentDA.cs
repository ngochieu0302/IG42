using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNNewsCommentDA : BaseDA
    {
        #region Constructer
        public DNNewsCommentDA()
        {
        }

        public DNNewsCommentDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNNewsCommentDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNNewsCommentItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_News_Comment
                        where o.IsShow == true
                        orderby o.ID descending
                        select new DNNewsCommentItem
                        {
                            ID = o.ID,
                            Message = o.Message.Trim(),
                            UserId = o.UserId,
                            DateCreated = o.DateCreated,
                            IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_News_Comment GetById(int id)
        {
            var query = from c in FDIDB.DN_News_Comment where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNNewsCommentItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_News_Comment
                        where o.ID == id
                        select new DNNewsCommentItem
                        {
                            ID = o.ID,
                            Message = o.Message.Trim(),
                            UserId = o.UserId,
                            DateCreated = o.DateCreated,
                            IsShow = o.IsShow
                        };
            return query.FirstOrDefault();
        }

        public List<DNNewsCommentItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_News_Comment
                        where o.IsShow == true && o.AgencyID == agencyid
                        select new DNNewsCommentItem
                        {
                            ID = o.ID,
                            Message = o.Message.Trim(),
                            UserId = o.UserId,
                            DateCreated = o.DateCreated,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }


        public List<DNNewsCommentItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_News_Comment
                        where  ltsArrId.Contains(o.ID)
                        select new DNNewsCommentItem
                        {
                            ID = o.ID,
                            Message = o.Message.Trim(),
                            UserId = o.UserId,
                            DateCreated = o.DateCreated,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public List<DNNewsCommentItem> GetListByParentID(int parentId)
        {
            var query = from o in FDIDB.DN_News_Comment
                        where o.ParentId == parentId
                        orderby o.ID 
                        select new DNNewsCommentItem
                        {
                            ID = o.ID,
                            Message = o.Message.Trim(),
                            UserId = o.UserId,
                            UserName = o.DN_Users.UserName,
                            DateCreated = o.DateCreated,
                            IsShow = o.IsShow
                        };
            return query.OrderBy(m=> m.ID).ToList();
        }


        public void Add(DN_News_Comment newsComment)
        {
            FDIDB.DN_News_Comment.Add(newsComment);
        }

        public void Delete(DN_News_Comment newsComment)
        {
            FDIDB.DN_News_Comment.Remove(newsComment);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
