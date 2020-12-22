using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNEnterprisesAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public int Login(string codee, string username, string domain, string pass, bool isonline)
        {
            var urlJson = string.Format("{0}DNEnterprises/Login?key={1}&codee={2}&username={3}&domain={6}&pass={4}&isonline={5}", _url, Keyapi, codee, username, pass, isonline, domain);
            return GetObjJson<int>(urlJson);
        }

        public int LogOut(string codee)
        {
            var urlJson = string.Format("{0}DNEnterprises/LogOut?key={1}&codee={2}", _url, Keyapi, codee);
            return GetObjJson<int>(urlJson);
        }

        public int UpdatePass(string codee, string pass, string passnew)
        {
            var urlJson = string.Format("{0}DNEnterprises/UpdatePass?key={1}&codee={2}&pass={3}&passnew={4}", _url, Keyapi, codee, pass, passnew);
            return GetObjJson<int>(urlJson);
        }

        public EnterprisesItem GetItemByCodeLogin(string codee)
        {
            var urlJson = string.Format("{0}DNEnterprises/GetItemByCodeLogin?key={1}&codee={2}", _url, Keyapi, codee);
            return GetObjJson<EnterprisesItem>(urlJson);
        }
        public EnterprisesItem GetContent(string domain)
        {
            var urlJson = string.Format("{0}DNEnterprises/GetContent?key={1}&domain={2}", _url, Keyapi, domain);
            var key = string.Format("DNEnterprisesAPIGetContent_{0}", domain);
            return GetCache<EnterprisesItem>(key, urlJson, ConfigCache.TimeExpire360);
        }
        public List<STGroupItem> GetListStGroupById(int enterprisesId)
        {
            var urlJson = string.Format("{0}DNEnterprises/GetListStGroupById?key={1}&enterprisesId={2}", _url, Keyapi, enterprisesId);
            return GetObjJson<List<STGroupItem>>(urlJson);
        }
        public ModelTotalItem GetStaticEnterprise(int enterprisesId)
        {
            var urlJson = string.Format("{0}DNEnterprises/GetStaticEnterprise?key={1}&enterprisesId={2}", _url, Keyapi, enterprisesId);
            return GetObjJson<ModelTotalItem>(urlJson);
        }
    }
}
