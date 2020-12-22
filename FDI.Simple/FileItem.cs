using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class FileItem : BaseSimple
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string TypeIcon { get; set; }
        public DateTime Created { get; set; }
        public byte[] Data { get; set; }
        public string DataSize { get; set; }
        public int TotalNews { get; set; }
        public int TotalAlbum { get; set; }
        public int TotalAnswer { get; set; }
        public int TotalProduct { get; set; }
        public int TotalGuide { get; set; }
    }
    public class ModelFileItem : BaseModelSimple
    {
        public IEnumerable<FileItem> ListItem { get; set; }
    }
    public class FilesItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? DocumentId { get; set; }
        public string Folder { get; set; }
        public string FileUrl { get; set; }
        public double? FileSize { get; set; }
        public string Icon { get; set; }
        public bool? Status { get; set; }
        public decimal? DateCreated { get; set; }
        public string TypeFile { get; set; }
    }
}
