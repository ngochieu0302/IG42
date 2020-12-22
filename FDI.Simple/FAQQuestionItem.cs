using System;
using System.Collections.Generic;

namespace FDI.Simple
{
     [Serializable]
    public class FAQQuestionItem : BaseSimple
    {
        public string Title { get; set; }
        public string TitleAscii { get; set; }
        public string Fullname { get; set; }
         public string Address { get; set; }
         public string FIleattached { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryAscii { get; set; }
        public string CategoryName { get; set; }
        public int TotalAnswers { get; set; }
        public virtual FAQAnswerItem AnswerItem { get; set; }
        public virtual IEnumerable<FAQAnswerItem> FAQ_Answer { get; set; }
        public virtual IEnumerable<ProductItem> Shop_Product { get; set; }
        public virtual CategoryItem Category { get; set; }
    }
     public class ModelFAQQuestionItem : BaseModelSimple
     {
         public IEnumerable<FAQQuestionItem> ListItem { get; set; }
         public FAQQuestionItem Item { get; set; }
         public List<CategoryItem> ListCate { get; set; }
         public FAQAnswerItem AnswerItem { get; set; }
         public CategoryItem CategoryItem { get; set; } 
     }
}
