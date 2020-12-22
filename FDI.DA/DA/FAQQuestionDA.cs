using System.Linq;
using FDI.Base;
using FDI.Simple;
using System.Collections.Generic;
using System.Web;

namespace FDI.DA
{
    public partial class FAQQuestionDA : BaseDA
    {
        #region Constructer
        public FAQQuestionDA()
        {
        }

        public FAQQuestionDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public FAQQuestionDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<FAQQuestionItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.FAQ_Question
                        orderby c.Title
                        where c.IsShow == isShow
                        && c.Title.StartsWith(keyword) && c.LanguageId == LanguageId
                        select new FAQQuestionItem
                                   {
                                       ID = c.ID,
                                       Title = c.Title
                                   };
            return query.Take(showLimit).ToList();
        }

        public List<FAQQuestionItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.FAQ_Question
                        where o.Category.IsDeleted==false
                        orderby  o.ID descending 
                        select new FAQQuestionItem
                                   {
                                       ID = o.ID,
                                       Title = o.Title,
                                       TitleAscii = o.TitleAscii,
                                       IsShow = o.IsShow,
                                       CategoryID = o.Category.Id,
                                       Content = o.Content,
                                       Category = new CategoryItem
                                                          {
                                                              ID = o.Category.Id,
                                                              Name = o.Category.Name,
                                                              Slug = o.Category.Slug
                                                          },
                                       DateCreated = o.DateCreated,
                                       Email = o.Email,
                                       Fullname = o.Fullname,
                                       Phone = o.Phone,
                                       TotalAnswers = o.FAQ_Answer.Count()
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        
        public FAQ_Question GetById(int questionId)
        {
            var query = from c in FDIDB.FAQ_Question where c.ID == questionId select c;
            return query.FirstOrDefault();
        }

        public List<FAQ_Question> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.FAQ_Question where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(FAQ_Question faqQuestion)
        {
            FDIDB.FAQ_Question.Add(faqQuestion);
        }

        public void Delete(FAQ_Question faqQuestion)
        {
          //  foreach (var answer in faqQuestion.FAQ_Answer)
            //    FDIDB.FAQ_Answer.Remove(answer); //Xóa câu hỏi
            FDIDB.FAQ_Question.Remove(faqQuestion); // Xóa câu trả lời
        }
       
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
