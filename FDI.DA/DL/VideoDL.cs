using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class VideoDL : BaseDA
    {
        public List<VideoItem> GetList(int page)
        {
            var query = from n in FDIDB.Gallery_Video
                        where n.IsDeleted == false && n.IsShow == true && n.IsHome == true
                        orderby n.ID descending
                        select new VideoItem
                        {
                            ID = n.ID,
                            Url = n.Url,
                            UrlPicture = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Name = n.Name,
                            DateCreated = n.DateCreated
                        };            
            return query.ToList();
        }

        public List<VideoItem> GetList(int cateId,int page, ref int total)
        {
            var query = from n in FDIDB.Gallery_Video
                        where n.IsDeleted == false && n.IsShow == true && n.CategoryID == cateId
                        orderby n.ID descending
                        select new VideoItem
                        {
                            ID = n.ID,
                            Url = n.Url,
                            UrlPicture = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Name = n.Name,
                            DateCreated = n.DateCreated
                        };
            query = query.Paging(page, 9, ref total);
            return query.ToList();
        }
        public VideoItem GetById(int id)
        {
            var query = from n in FDIDB.Gallery_Video
                        where n.IsDeleted == false && n.ID == id
                        orderby n.ID descending
                        select new VideoItem
                        {
                            ID = n.ID,
                            Url = n.Url,
                            UrlPicture = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Name = n.Name,
                            Description = n.Description,
                            DateCreated = n.DateCreated
                        };
            return query.FirstOrDefault();
        }
        public List<VideoItem> GetList()
        {
            var query = from c in FDIDB.Gallery_Video
                        where c.IsDeleted == false && c.IsShow == true
                        select new VideoItem
                        {
                            ID = c.ID,
                            Url = c.Url,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Name = c.Name
                        };

            return query.ToList();
        }
        public List<VideoItem> GetVideoOther(int id)
        {
            var query = from c in FDIDB.Gallery_Video
                        where c.IsDeleted == false && c.IsShow == true && c.ID != id
                        orderby c.ID descending
                        select new VideoItem
                        {
                            ID = c.ID,
                            Url = c.Url,
                            DateCreated = c.DateCreated,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Name = c.Name
                        };
            return query.Take(6).ToList();
        }
        public VideoItem GetVideoHot()
        {
            var query = from c in FDIDB.Gallery_Video
                        where c.IsDeleted == false && c.IsShow == true && c.IsHome == true
                        orderby c.ID descending
                        select new VideoItem
                        {
                            ID = c.ID,
                            Url = c.Url,
                            Description = c.Description,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Name = c.Name
                        };
            return query.FirstOrDefault();
        }

    }
}