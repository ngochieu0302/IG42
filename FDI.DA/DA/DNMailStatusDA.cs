using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNMailStatusDA : BaseDA
    {
        #region Constructer
        public DNMailStatusDA()
        {
        }

        public DNMailStatusDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNMailStatusDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

       

        public DN_StatusEmail GetById(int id)
        {
            var query = from c in FDIDB.DN_StatusEmail where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNMailStatusItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_StatusEmail
                        where o.ID == id
                        select new DNMailStatusItem
                        {
                            ID = o.ID,
                            Status = o.Status,
                        };
            return query.FirstOrDefault();
        }

        public void Add(DN_StatusEmail mailSscStatusEmail)
        {
            FDIDB.DN_StatusEmail.Add(mailSscStatusEmail);
        }

        public void Delete(DN_StatusEmail mailSscStatusEmail)
        {
            FDIDB.DN_StatusEmail.Remove(mailSscStatusEmail);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
