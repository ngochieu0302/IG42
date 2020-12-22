using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class FileObj
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string NameRoot { get; set; }
        public string Forder { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public bool Error { get; set; }
    }
    public class FileUploadItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class ModelFileObj : BaseSimple
    {
        public List<FileObj> ListItem { get; set; }
        public List<CategoryItem> ListCategoryItem { get; set; }
        public int Type { get; set; }
        public int CategoryId { get; set; }
        public string Action { get; set; }

    }
}
