using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class DocumentFilesItem : BaseSimple
    {
        public string Name { get; set; }
        public int? DocumentId { get; set; }
        public int? Loai { get; set; }
        public string Folder { get; set; }
        public string FileUrl { get; set; }
        public double? FileSize { get; set; }
        public int? Icon { get; set; }
        public bool? Status { get; set; }
        public decimal? DateCreated { get; set; }
        public string TypeFile { get; set; }
        public string UrlDocument { get; set; }
    }

    public class ModelDocumentFilesItem : BaseModelSimple
    {
        public IEnumerable<DocumentFilesItem> ListItem { get; set; }
    }
}
