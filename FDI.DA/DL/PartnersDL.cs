using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class PartnersDL : BaseDA
    {
        public List<PartnerItem> GetList(int page, ref int total)
        {
            var query = from n in FDIDB.Partners
                        where n.IsDeleted == false && n.IsShow == true && n.LanguageId == LanguageId
                        orderby n.ID descending
                        select new PartnerItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            Slug = n.Slug,
                            Description = n.Description ?? "",
                            PictureUrl = n.Gallery_Picture.IsDeleted == false ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            DateCreated = n.DateCreated
                        };
            query = query.Paging(page, 18, ref total);
            return query.ToList();
        }

        public List<PartnerItem> GetListHot(int take)
        {
            var query = from n in FDIDB.Partners
                        where n.IsDeleted == false && n.IsShow == true && n.LanguageId == LanguageId
                        orderby n.ID descending
                        select new PartnerItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            Slug = n.Slug,
                            Description = n.Description,
                            PictureUrl = n.Gallery_Picture.IsDeleted == false ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            DateCreated = n.DateCreated
                        };
            return query.Take(take).ToList();
        }

        public PartnerItem GetById(int id)
        {
            var query = from n in FDIDB.Partners
                        where n.ID == id
                        select new PartnerItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            DateCreated = n.DateCreated,
                            Details = n.Details,
                            Slug = n.Slug,
                            Description = n.Description,
                            
                            LstPictures = n.Gallery_Picture1.Where(u => u.IsShow == true && u.IsDeleted == false).Select(u => new PictureItem
                            {
                                Url = u.Folder + u.Url,
                                Description = u.Description,
                                Name = u.Name
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<PartnerItem> GetPartnerOther(int id)
        {
            var query = from n in FDIDB.Partners
                        where n.IsDeleted == false && n.IsShow == true && n.ID != id && n.LanguageId == LanguageId
                        orderby n.ID descending
                        select new PartnerItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            Slug = n.Slug,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            DateCreated = n.DateCreated,
                            Details = n.Details,
                            Description = n.Description,
                        };
            return query.Take(6).ToList();
        }

        public List<PartnerItem> GetPartnerListnews()
        {
            var query = from n in FDIDB.Partners
                        where n.IsDeleted == false && n.IsShow == true && n.ID != 4 && n.LanguageId == LanguageId
                        orderby n.ID descending
                        select new PartnerItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            Slug = n.Slug,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            
                        };
            return query.Take(6).ToList();
        }

        public List<PartnerItem> GetListPartner(int id)
        {
            var query = from n in FDIDB.Partners
                        where n.ID != id && n.ID != 4 && n.LanguageId == LanguageId
                        orderby n.ID descending
                        select new PartnerItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : ""
                        };
            return query.Take(6).ToList();
        }
    }
}