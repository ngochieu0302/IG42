using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNGroupMailSSCDA : BaseDA
    {
        #region Constructer
        public DNGroupMailSSCDA()
        {
        }

        public DNGroupMailSSCDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNGroupMailSSCDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNGroupMailSSCItem> GetListSimpleByRequest(HttpRequestBase httpRequest,  int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_GroupEmail
                        where o.AgencyID == agencyId 
                        orderby o.ID descending
                        select new DNGroupMailSSCItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Slug = o.Slug,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public DN_GroupEmail GetById(int id)
        {
            var query = from c in FDIDB.DN_GroupEmail where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNGroupMailSSCItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_GroupEmail
                        where o.ID == id
                        select new DNGroupMailSSCItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Slug = o.Slug,
                            IsShow = o.IsShow,
                            ListDNUserItem = o.DN_Users.Where(m => m.IsApproved == true && m.IsOut == false && m.IsLockedOut == false).Select(u => new DNUserItem
                            {
                                UserId = u.UserId,
                                UserName = u.UserName,
                               
                            }),
                        };
            return query.FirstOrDefault();
        }

        public List<DNGroupMailSSCItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_GroupEmail
                        where o.AgencyID == agencyid 
                        select new DNGroupMailSSCItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Slug = o.Slug
                        };
            return query.ToList();
        }

        public List<DNGroupMailSSCItem> GetAllByUserId(int agencyid, Guid userId)
        {
            var query = from o in FDIDB.DN_GroupEmail
                        where o.AgencyID == agencyid && o.DN_Users.Any(m => m.UserId == userId)
                        select new DNGroupMailSSCItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Slug = o.Slug
                        };
            return query.ToList();
        }


        public List<DNGroupMailSSCItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_GroupEmail
                        where  ltsArrId.Contains(o.ID)
                        select new DNGroupMailSSCItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Slug = o.Slug
                        };
            return query.ToList();
        }

        public List<DNGroupMailSSCItem> GetListByArrId(List<int> ltsArrId)
        {
            var query = from o in FDIDB.DN_GroupEmail
                        where ltsArrId.Contains(o.ID)
                        select new DNGroupMailSSCItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Slug = o.Slug
                        };
            return query.ToList();
        }

        public List<DN_Users> GetUserArrId(string lstId)
        {
            var ltsArrId = FDIUtils.ConvertStringToGuids(lstId);
            var query = from o in FDIDB.DN_Users
                        where ltsArrId.Contains(o.UserId)
                        select o;
            return query.ToList();
        }

        public void Add(DN_GroupEmail mailSsc)
        {
            FDIDB.DN_GroupEmail.Add(mailSsc);
        }

        public void Delete(DN_GroupEmail mailSsc)
        {
            FDIDB.DN_GroupEmail.Remove(mailSsc);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
