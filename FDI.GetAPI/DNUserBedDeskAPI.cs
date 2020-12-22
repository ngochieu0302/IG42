using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNUserBedDeskAPI : BaseAPI
    {
        public DNUserBedDeskItem GetById(int id)
        {
            var urlJson = string.Format("{0}DNUserBedDesk/GetById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNUserBedDeskItem>(urlJson);
        }

        public int UpdateBedId(int bedid, int id)
        {
            var urlJson = string.Format("{0}DNUserBedDesk/UpdateBedId?key={1}&bedid={2}&id={3}", Domain, Keyapi, bedid, id);
            return GetObjJson<int>(urlJson);
        }
        
        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNUserBedDesk/Add?key={1}&json={2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(int id)
        {
            var urlJson = string.Format("{0}DNUserBedDesk/Delete?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<int>(urlJson);
        }
        
    }
}
