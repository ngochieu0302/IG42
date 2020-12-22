using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNLoginAPI : BaseAPI
    {
        public DNUserItem GetUserItemByCode(string code)
        {
            var urlJson = string.Format("{0}DNLogin/GetUserItemByCode?key={1}&code={2}", Domain, Keyapi, code);
            var key = string.Format("DNLoginAPIGetUserItemByCode_{0}", code);
            return GetCacheNow<DNUserItem>(key, urlJson, ConfigCache.TimeExpire360);
        }
        public bool Logout(string code)
        {
            var urlJson = string.Format("{0}DNLogin/Logout?key={1}&code={2}", Domain, Keyapi, code);
            GetObjJson<int>(urlJson);
            var key = string.Format("DNLoginAPIGetUserItemByCode_{0}", code);
            if (Cache.KeyExistsCache(key)) Cache.DeleteCache(key);
            return true;
        }

        public DNUserItem Login(string code, string username, string pass, bool ischeck, string domain)
        {
            var urlJson = string.Format("{0}DNLogin/Login?key={1}&code={2}&username={3}&pass={4}&ischeck={5}&domain={6}", Domain, Keyapi, code, username, pass, ischeck, domain);
            var key = string.Format("DNLoginAPIGetUserItemByCode_{0}", code);
            return GetCacheNow<DNUserItem>(key, urlJson, ConfigCache.TimeExpire360);
        }
        public CustomerItem GetCustomerByCode(string code)
        {
            var urlJson = string.Format("{0}DNLogin/GetCustomerByCode?key={1}&code={2}", Domain, Keyapi, code);
            return GetObjJson<CustomerItem>(urlJson);
        }
        public CustomerItem CustomerLogin(string code, string username, string pass, bool ischeck)
        {
            var urlJson = string.Format("{0}DNLogin/CustomerLogin?key={1}&code={2}&username={3}&pass={4}&ischeck={5}", Domain, Keyapi, code, username, pass, ischeck);
            return GetObjJson<CustomerItem>(urlJson);
        }
    }
}