﻿using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class GalleryPictureItem : BaseSimple
    {
        public int? AlbumID { get; set; }
        public int? CategoryID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? SourceID { get; set; }
        public string Folder { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string CateName { get; set; }
        public CategoryItem Category { get; set; }
        public int? AgencyID { get; set; }
    }
    public class ModelGalleryPictureItem : BaseModelSimple
    {
        public IEnumerable<GalleryPictureItem> ListItem { get; set; }
    }
}
