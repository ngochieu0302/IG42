using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DocumentsDA : BaseDA
    {
        #region Constructer
        public DocumentsDA()
        {
        }

        public DocumentsDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DocumentsDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DocumentItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Documents
                        orderby c.ID descending 
                        select new DocumentItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name,
                                       Description = c.Description,
                                       CreatedDate = c.CreatedDate,
                                       IsShow = c.IsShow
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
       
        public Document GetById(int id)
        {
            var query = from c in FDIDB.Documents where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<Document> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Documents where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
      
        public List<Category> GetListCategoryByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Categories where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }

        public void Add(Document document)
        {
            FDIDB.Documents.Add(document);
        }

        public void Delete(Document document)
        {
            FDIDB.Documents.Remove(document);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
        
    }
}
