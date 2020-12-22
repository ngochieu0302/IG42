using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNFileMailDA : BaseDA
    {
        #region Constructer
        public DNFileMailDA()
        {
        }

        public DNFileMailDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNFileMailDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNFileMailItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_File_Mail
                        where o.IsDeleted == false && o.AgencyId == agencyId
                        orderby o.ID descending
                        select new DNFileMailItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Url = o.Url,
                            DateCreated = o.DateCreated,
                            Folder = o.Folder,
                            IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_File_Mail GetById(int id)
        {
            var query = from c in FDIDB.DN_File_Mail where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNFileMailItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_File_Mail
                        where o.ID == id
                        select new DNFileMailItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Url = o.Url,
                            DateCreated = o.DateCreated,
                            Folder = o.Folder,
                            IsShow = o.IsShow
                        };
            return query.FirstOrDefault();
        }

        public List<DNFileMailItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_File_Mail
                        where o.IsShow == true && o.IsDeleted == false && o.AgencyId == agencyid
                        select new DNFileMailItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Url = o.Url,
                            DateCreated = o.DateCreated,
                            Folder = o.Folder,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }


        public List<DNFileMailItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_File_Mail
                        where o.IsDeleted == false && ltsArrId.Contains(o.ID)
                        select new DNFileMailItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Url = o.Url,
                            DateCreated = o.DateCreated,
                            Folder = o.Folder,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public void Add(DN_File_Mail dayOff)
        {
            FDIDB.DN_File_Mail.Add(dayOff);
        }

        public void Delete(DN_File_Mail dayOff)
        {
            FDIDB.DN_File_Mail.Remove(dayOff);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
