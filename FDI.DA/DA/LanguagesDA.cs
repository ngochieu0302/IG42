using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class LanguagesDA : BaseDA
    {
        #region Constructer
        public LanguagesDA()
        {
        }

        public LanguagesDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public LanguagesDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<LanguagesItem> GetListSimpleAll(bool isShow)
        {
            var query = from c in FDIDB.Languages
                        where (c.IsShow == isShow)
                        orderby c.Name
                        select new LanguagesItem
                        {
                            ID = c.Id,
                            Code = c.Code.Trim(),
                            Name = c.Name.Trim(),
                            FallbackCulture = c.FallbackCulture,
                            CreatedDate = c.CreatedDate,
                            IsShow = c.IsShow,
                            Icon = c.Gallery_Picture.Folder + c.Gallery_Picture.Url
                        };
            return query.ToList();
        }

        public List<LanguagesItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Languages
                        orderby o.Name 
                        select new LanguagesItem
                                   {
                                       ID = o.Id,
                                       Name = o.Name.Trim(),
                                       Code = o.Code,
                                       FallbackCulture = o.FallbackCulture,
                                       CreatedDate = o.CreatedDate,
                                       IsShow = o.IsShow
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public Language GetById(int id)
        {
            var query = from c in FDIDB.Languages where c.Id == id select c;
            return query.FirstOrDefault();
        }

        public List<Language> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Languages where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }

        public void Add(Language language)
        {
            FDIDB.Languages.Add(language);
        }

        public void Delete(Language language)
        {
            FDIDB.Languages.Remove(language);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
