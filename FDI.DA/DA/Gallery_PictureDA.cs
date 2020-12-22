using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class Gallery_PictureDA : BaseDA
    {
        #region Constructer
        public Gallery_PictureDA()
        {
        }

        public Gallery_PictureDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public Gallery_PictureDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        
        public List<PictureItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Gallery_Picture
                        orderby c.Name
                        where c.IsShow == isShow
                        && c.Name.StartsWith(keyword) && !c.IsDeleted.Value
                        select new PictureItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name
                                   };
            return query.Take(showLimit).ToList();
        }

        public IEnumerable<PictureItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Gallery_Picture
                        where o.IsDeleted == false 
                        orderby o.DateCreated descending
                        select new PictureItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name,
                                       IsShow = o.IsShow,
                                       Description = o.Description,
                                       DateCreated = o.DateCreated,
                                       Folder = o.Folder,
                                       Url = o.Folder + o.Url,
                                       CategoryID = o.CategoryID,
                                       //Category = new CategoryItem
                                       //{
                                       //    Name = o.Category.Name,
                                       //    Slug = o.Category.Slug
                                       //}
                                   };

            //if (Request.CategoryID > 0)
            //    query = query.Where(o => o.CategoryID == Request.CategoryID);
            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                var name = FomatString.Slug(Request.Keyword);
                query = query.Where(c => c.Name.Contains(name));
            }
            
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public IEnumerable<PictureItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId, int type = 0)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Gallery_Picture
                        where o.IsDeleted == false  
                        orderby o.ID descending
                        select new PictureItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name,
                                       IsShow = o.IsShow,
                                       Description = o.Description,
                                       DateCreated = o.DateCreated,
                                       Folder = o.Folder,
                                       Url = o.Folder + o.Url,
                                       CategoryID = o.CategoryID,
                                       Category = new CategoryItem
                                       {
                                           Name = o.Category.Name,
                                           Slug = o.Category.Slug
                                       }
                                   };

            if (Request.CategoryID > 0)
                query = query.Where(o => o.CategoryID == Request.CategoryID);
            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                var name = Request.Keyword;
                query = query.Where(c => c.Name.Contains(name));
            }
            
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        
        public Gallery_Picture GetById(int id)
        {
            var query = from c in FDIDB.Gallery_Picture where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public PictureItem GetPictureItem(int id)
        {
            var query = from c in FDIDB.Gallery_Picture
                        orderby c.Name
                        where c.ID == id
                        select new PictureItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            CategoryID = c.CategoryID,
                            Description = c.Description,
                            Folder = c.Folder,
                            Url = c.Folder + c.Url,
                            IsShow = c.IsShow
                        };
            return query.FirstOrDefault();
        }
        
        public List<Gallery_Picture> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.Gallery_Picture where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(Gallery_Picture galleryPicture)
        {
            FDIDB.Gallery_Picture.Add(galleryPicture);
        }       

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
