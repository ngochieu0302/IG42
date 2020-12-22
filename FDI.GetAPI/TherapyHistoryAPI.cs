using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class TherapyHistoryAPI:BaseAPI
    {
        public List<TherapyHistoryItem> GetListByCustomerID(int cusId,string phone)
        {
            var urlJson = string.Format("{0}TherapyHistory/GetListByCustomerID?key={1}&cusId={2}&phone={3}", Domain, Keyapi, cusId,phone);
            return GetObjJson<List<TherapyHistoryItem>>(urlJson);
        }
        public JsonMessage Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}TherapyHistory/Add?key={1}&{2}&agencyId={3}", Domain, Keyapi, json, agencyid);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
