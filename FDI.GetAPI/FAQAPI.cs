using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class FaqAPI : BaseAPI
    {
        public FAQQuestionItem GetQuestionById(string url, int id = 0)
        {
            var urlJson = string.Format("{0}Faq/GetQuestionById?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<FAQQuestionItem>(urlJson);
        }

        public FAQAnswerItem GetAnswerByQuestionId(string url, int id = 0)
        {
            var urlJson = string.Format("{0}Faq/GetAnswerByQuestionId?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<FAQAnswerItem>(urlJson);
        }

        public List<FAQQuestionItem> GetAllQuestion(string url)
        {
            var urlJson = string.Format("{0}Faq/GetListFaqQuestionItem?key={1}", url, Keyapi);
            return GetObjJson<List<FAQQuestionItem>>(urlJson);
        }
    }
}
