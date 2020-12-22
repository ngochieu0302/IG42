using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class GalleryDL : BaseDA
    {
        public List<AdvertisingItem> GetListAdvertising(int id)
        {
            var query = from c in FDIDB.Advertisings
                        where c.PositionID == id && c.IsDeleted == false && c.LanguageId == LanguageId && c.Show == true
                        orderby c.Sort
                        select new AdvertisingItem
                        {
                            Link = c.Link,
                            Name = c.Name,
                            Width = c.Width,
                            Height = c.Height,
                            Content = c.Content,
                            PictureUrl = c.Gallery_Picture.IsDeleted == false ? c.Gallery_Picture.Folder + c.Gallery_Picture.Url : "",
                        };
            return query.ToList();
        }
        public AdvertisingItem GetAdvertisingItem(int id)
        {
            var query = from c in FDIDB.Advertisings
                        where c.PositionID == id && c.IsDeleted == false && c.LanguageId == LanguageId && c.Show == true
                        orderby c.Sort
                        select new AdvertisingItem
                        {
                            Link = c.Link,
                            Name = c.Name,
                            Width = c.Width,
                            Height = c.Height,
                            Content = c.Content,
                            PictureUrl = c.Gallery_Picture.IsDeleted == false ? c.Gallery_Picture.Folder + c.Gallery_Picture.Url : "",
                        };
            return query.FirstOrDefault();
        }
        public List<GalleryPictureItem> GetListPictureByCateId(int id)
        {
            var query = from c in FDIDB.Gallery_Picture
                        where  c.IsDeleted == false && c.LanguageId == LanguageId && c.CategoryID == id
                        orderby c.ID
                        select new GalleryPictureItem
                        {
                            Name = c.Name,
                            Url = c.Url + c.Folder,
                            CateName = c.Category.Name,
                        };
            return query.ToList();
        }
        public List<AdvertisingPositionItem> GetAdvertising()
        {
            var query = from c in FDIDB.Advertising_Position
                        where c.IsDeleted == false
                        orderby c.ID descending
                        select new AdvertisingPositionItem
                        {
                            ID = c.ID,
                            Name = c.Name
                        };
            return query.ToList();
        }
    }
}