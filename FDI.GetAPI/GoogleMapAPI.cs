using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class GoogleMapAPI : BaseAPI
    {
        public List<GoogleMapItem> GetListGoogleMapsByDistrictId(string url, int districtId)
        {
            var urlJson = string.Format("{0}GoogleMap/GetListGoogleMapsByDistrictId?key={1}&districtId={2}", url, Keyapi, districtId);
            return GetObjJson<List<GoogleMapItem>>(urlJson);
        }

        public List<GoogleMapItem> GetGoogleMapsItemByCityId(string url, int cityId)
        {
            var urlJson = string.Format("{0}GoogleMap/GetGoogleMapsItemByCityId?key={1}&cityId={2}", url, Keyapi, cityId);
            return GetObjJson<List<GoogleMapItem>>(urlJson);
        }

        public List<GoogleMapItem> GetGoogleMapsItemByDistrictID(string url,int districtId)
        {
            var urlJson = string.Format("{0}GoogleMap/GetGoogleMapsItemByDistrictID?key={1}&districtId={2}", url, Keyapi, districtId);
            return GetObjJson<List<GoogleMapItem>>(urlJson);
        }

        public List<GoogleMapItem> GetAll(string url)
        {
            var urlJson = string.Format("{0}GoogleMap/GetAll?key={1}", url, Keyapi);
            return GetObjJson<List<GoogleMapItem>>(urlJson);
        }
    }
}
