using System;
using System.Collections.Generic;
using FDI.Base;


namespace FDI.Simple
{
    public class SystemFileItem:BaseSimple
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public int? TypeID { get; set; }
        public DateTime? CreatedDate { get; set; }
         
        public virtual System_FileType SystemFileType { get; set; }
        public virtual IEnumerable<FAQAnswerItem> FAQAnswer { get; set; }
        public virtual IEnumerable<NewsItem> NewsNews { get; set; }
        public virtual IEnumerable<ProductItem> ShopProduct { get; set; }
    }
    public class ModelSystemFileItem : BaseModelSimple
    {
        public IEnumerable<SystemFileItem> ListItem { get; set; }
    }
}
