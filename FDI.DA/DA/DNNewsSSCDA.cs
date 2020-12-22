using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNNewsSSCDA : BaseDA
    {
        #region Constructer
        public DNNewsSSCDA()
        {
        }

        public DNNewsSSCDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNNewsSSCDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNNewsSSCItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_NewsSSC
                        orderby o.ID descending
                        select new DNNewsSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            DateCreated = o.DateCreated,
                            Content = o.Content,
                            IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_NewsSSC GetById(int id)
        {
            var query = from c in FDIDB.DN_NewsSSC where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNNewsSSCItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_NewsSSC
                        where o.ID == id
                        select new DNNewsSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            DateCreated = o.DateCreated,
                            Content = o.Content,
                            IsShow = o.IsShow,
                            ListDNNewsCommentItem = o.DN_News_Comment.Where(m=> m.IsShow == true).Select(m=> new DNNewsCommentItem
                            {
                                ID = m.ID,
                                Message = m.Message,
                                DateCreated = m.DateCreated,
                                UserId = m.UserId,
                                UserName = m.DN_Users.UserName,
                                IsLevel = m.IsLevel,
                                ParentId = m.ParentId
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<DNNewsSSCItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_NewsSSC
                        where o.IsShow == true && o.AgencyID == agencyid
                        select new DNNewsSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            DateCreated = o.DateCreated,
                            Content = o.Content,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }


        public List<DNNewsSSCItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_NewsSSC
                        where  ltsArrId.Contains(o.ID)
                        select new DNNewsSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            DateCreated = o.DateCreated,
                            Content = o.Content,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public void Add(DN_NewsSSC newsSsc)
        {
            FDIDB.DN_NewsSSC.Add(newsSsc);
        }

        public void Delete(DN_NewsSSC newsSsc)
        {
            FDIDB.DN_NewsSSC.Remove(newsSsc);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
