using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;

namespace FDI.DA.DA.AppSales
{
    public class DocumentsAppDA:BaseDA
    {
        #region Constructer
        public DocumentsAppDA()
        {
        }

        public DocumentsAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DocumentsAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DocumentItem> GetListDocByAgencyApp(int agencyId)
        {
            var query = from c in FDIDB.Documents
                        where c.AgencyId == agencyId && c.IsShow == true && c.Status == true
                        orderby c.CreatedDate descending 
                select new DocumentItem
                {
                    ID = c.ID,
                    Code = c.Code,
                    DateStart = c.DateStart,
                    DateEnd = c.DateEnd,
                    Value = c.Value,
                    Deposit = c.Deposit,
                    FileUrl = ""
                };
            return query.ToList();
        }
        public DocumentItem GetDocFileByIDApp(int id)
        {
            var query = from c in FDIDB.Documents
                        where c.ID == id && c.IsShow == true && c.Status == true
                        select new DocumentItem
                        {
                            FileUrl = ""
                        };
            return query.FirstOrDefault();
        }
    }
}
