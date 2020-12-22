using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class Gallery_VideoDA : BaseDA
    {
        #region Constructer
        public Gallery_VideoDA()
        {
        }

        public Gallery_VideoDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public Gallery_VideoDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<VideoItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Gallery_Video
                        orderby c.Name
                        where c.IsShow == isShow
                        && c.Name.StartsWith(keyword) && !c.IsDeleted.Value
                        select new VideoItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name
                                   };
            return query.Take(showLimit).ToList();
        }

        public List<VideoItem> GetListSimpleByRequest(HttpRequestBase httpRequest, List<int> ltsIdNotInclude)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Gallery_Video
                        where !ltsIdNotInclude.Contains(o.ID) && !o.IsDeleted.Value
                        select new VideoItem
                                   {

                                       ID = o.ID,
                                       Name = o.Name,
                                       IsShow = o.IsShow.Value,
                                       Description = o.Description,
                                       DateCreated = o.DateCreated.Value,
                                       Url = o.Url,
                                       UrlYoutube = o.UrlYoutube,
                                       PictureId = o.PictureID,
                                       GalleryPicture = new GalleryPictureItem
                                                             {
                                                                 Name = o.Gallery_Picture.Name,
                                                                 Url = o.Gallery_Picture.Url
                                                             },
                                       CategoryId = o.CategoryID,
                                      
                                   };
            if (Request.CategoryID > 0)
                query = query.Where(o => o.CategoryId == Request.CategoryID);

            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();

        }

        public VideoItem GetItemById(int id)
        {
            var query = from o in FDIDB.Gallery_Video
                        orderby o.Name
                        where o.ID == id
                        select new VideoItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            IsShow = o.IsShow.Value,
                            Description = o.Description,
                            DateCreated = o.DateCreated.Value,
                            Url = o.Url,
                            UrlYoutube = o.UrlYoutube,
                            PictureId = o.PictureID,
                            UrlPicture = o.Gallery_Picture.Folder + o.Gallery_Picture.Url,
                            GalleryPicture = new GalleryPictureItem
                            {
                                Name = o.Gallery_Picture.Name,
                                Url = o.Gallery_Picture.Url
                            },
                            CategoryId = o.CategoryID,
                        };
            return query.FirstOrDefault();
        }

        public List<VideoItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Gallery_Video
                        where o.IsDeleted == false
                        orderby o.ID descending
                        select new VideoItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name,
                                       IsShow = o.IsShow.Value,
                                       Description = o.Description,
                                       DateCreated = o.DateCreated.Value,
                                       Url = o.Url,
                                       UrlYoutube = o.UrlYoutube,
                                       PictureId = o.PictureID,
                                       CategoryName = o.Category.Name,
                                       GalleryPicture = new GalleryPictureItem
                                       {
                                           Name = o.Gallery_Picture.Name,
                                           Url = o.Gallery_Picture.Url
                                       },
                                       CategoryId = o.CategoryID,
                                   };
            if (Request.CategoryID > 0)
                query = query.Where(o => o.CategoryId == Request.CategoryID);

            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
       
        public Gallery_Video GetById(int id)
        {
            var query = from c in FDIDB.Gallery_Video where c.ID == id select c;
            return query.FirstOrDefault();
        }
        
        public List<Gallery_Video> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Gallery_Video where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(Gallery_Video galleryVideo)
        {
            FDIDB.Gallery_Video.Add(galleryVideo);
        }

        public void Delete(Gallery_Video galleryVideo)
        {
            FDIDB.Gallery_Video.Remove(galleryVideo);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
