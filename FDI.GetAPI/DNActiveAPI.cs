using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNActiveAPI : BaseAPI
    {
        private readonly string _url = Domain + "DNActive";
        public List<ActiveRoleItem> GetAll()
        {
            var urlJson = string.Format("{0}/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<ActiveRoleItem>>(urlJson);
        }

        //public FAQAnswerItem GetAnswerByQuestionId(int id = 0)
        //{
        //    var urlJson = string.Format("{0}Faq/GetAnswerByQuestionId?key={1}&id={2}", _url, Keyapi, id);
        //    return GetObjJson<FAQAnswerItem>(urlJson);
        //}

        //public List<FAQQuestionItem> GetAllQuestion()
        //{
        //    var urlJson = string.Format("{0}Faq/GetListFaqQuestionItem?key={1}", _url, Keyapi);
        //    return GetObjJson<List<FAQQuestionItem>>(urlJson);
        //}
    }
}
