using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
   public class DebtAPI : BaseAPI
    {
       public ModelDebtItem GetListByRequest(int agencyId, string url)
       {
           url = string.IsNullOrEmpty(url) ? "?" : url;
           var urlJson = string.Format("{0}Debt/GetListByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
           return GetObjJson<ModelDebtItem>(urlJson);
       }
       public DebtItem GetItemByID(int id)
       {
           var urlJson = string.Format("{0}Debt/GetItemByID?key={1}&id={2}", Domain, Keyapi, id);
           return GetObjJson<DebtItem>(urlJson);
       }
       public JsonMessage Add(string json, Guid userid)
       {
           var urlJson = string.Format("{0}Debt/Add?key={1}&{2}&userid={3}", Domain, Keyapi, json, userid);
           return GetObjJson<JsonMessage>(urlJson);
       }
       public JsonMessage Delete(string lstArrId)
       {
           var urlJson = string.Format("{0}Debt/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
           return GetObjJson<JsonMessage>(urlJson);
       }
       public JsonMessage Update(string json)
       {
           var urlJson = string.Format("{0}Debt/Update?key={1}&{2}", Domain, Keyapi, json);
           return GetObjJson<JsonMessage>(urlJson);
       }
    }
}
