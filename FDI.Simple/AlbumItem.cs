using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AlbumItem :BaseSimple
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public int? ImagesID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsShow { get; set; }
        public bool IsVideo { get; set; }
        public bool? IsDeleted { get; set; }
        public int TotalPictures { get; set; }
        public virtual PictureItem Gallery_Picture { get; set; }
        public virtual IEnumerable<CategoryItem> ListCategoryItem { get; set; }
    }
    public class ModelAlbumItem : BaseModelSimple
    {
        public IEnumerable<AlbumItem> ListItem { get; set; }
        public string ValuesSelected { get; set; }
        public bool SelectMutil { get; set; }
        public int Type { get; set; }
    }
    public class AlbumVideoItem : BaseSimple
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public int? ImagesID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsShow { get; set; }
        public bool IsVideo { get; set; }
        public bool? IsDeleted { get; set; }
        public int TotalPictures { get; set; }
        public virtual PictureItem Gallery_Picture { get; set; }
        public virtual IEnumerable<CategoryItem> ListCategoryItem { get; set; }
    }
    public class ModelAlbumVideoItem : BaseModelSimple
    {
        public IEnumerable<AlbumItem> ListItem { get; set; }
        public string ValuesSelected { get; set; }
        public bool SelectMutil { get; set; }
        public int Type { get; set; }
    }
}
