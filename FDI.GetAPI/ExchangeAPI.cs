using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ExchangeAPI : BaseAPI
    {
        public List<DNExchangeItem> GetListByNow(int agencyid)
        {
            var urlJson = string.Format("{0}DNWeekly/GetListByNow?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<DNExchangeItem>>(urlJson);
        }
        public int Add(int agencyid,string bedid, string bedexid, string end)
        {
            var urlJson = string.Format("{0}DNExchange/Add?key={1}&agencyId={2}&bedid={3}&name={4}&end={5}", Domain, Keyapi,agencyid, bedid, bedexid, end);
            return GetObjJson<int>(urlJson);
        }
    }
}
