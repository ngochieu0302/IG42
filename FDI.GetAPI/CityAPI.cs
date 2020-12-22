using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class CityAPI : BaseAPI
    {
        public CityItem GetByid(int id)
        {
            var urlJson = string.Format("{0}TMCity/GetByid?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CityItem>(urlJson);
        }

        public CityItem GetDistrictId(int cityId)
        {
            var urlJson = string.Format("{0}TMCity/GetDistrictId?key={1}&cityId={2}", Domain, Keyapi, cityId);
            return GetObjJson<CityItem>(urlJson);
        }

        public List<CityItem> GetAll()
        {
            var urlJson = string.Format("{0}TMCity/GetAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<CityItem>>(urlJson);
        }
        public List<CityItem> GetAllListByGoogleMap()
        {
            var urlJson = string.Format("{0}TMCity/GetAllListByGoogleMap?key={1}", Domain, Keyapi);
            return GetObjJson<List<CityItem>>(urlJson);
        }

        public DistrictItem GetListGoogleMapByDistrictID(int districtID)
        {
            var urlJson = string.Format("{0}TMCity/GetListGoogleMapByDistrictID?key={1}", Domain, Keyapi);
            return GetObjJson<DistrictItem>(urlJson);
        }
    }
}
