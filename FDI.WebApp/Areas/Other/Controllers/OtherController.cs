using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;

namespace FDI.Web.Areas.Other.Controllers
{
    public class OtherController : Controller
    {
        readonly CityBL _cityBl = new CityBL();
        readonly GoogleMapBL _googleMapBl = new GoogleMapBL();

        #region Werther
        public ActionResult Werther()
        {
            var lstCity = _cityBl.GetAllListByWeather();
            return View(lstCity);
        }

        public static string GetUrlJson(string url)
        {
            var data = new WebClient();
            try
            {
                data.Encoding = Encoding.UTF8;
                var datas = data.DownloadString(url);
                return datas;
            }
            catch (Exception)
            {
                return " ";
            }
        }

        public ActionResult GetWerther(string locationId)
        {
            var url = GetUrlJson("http://api.openweathermap.org/data/2.5/weather?lat=21.034678&lon=105.792084&appid=be8d3e323de722ff78208a7dbb2dcd6f");
            var werther = JsonConvert.DeserializeObject<WertherItem>(url);
            return View(werther);
        }

        #endregion

        #region lấy giá vàng sjc
        public ActionResult Rate()
        {
            var xDoc = new XmlDocument();
            xDoc.Load("http://www.sjc.com.vn/xml/tygiavang.xml");
            // lấy ngày cập nhật
            var nodeDateTime = xDoc.GetElementsByTagName("ratelist");
            var model = new ModelRateCityItem();
            foreach (XmlNode item in nodeDateTime)
            {
                if (item.Attributes == null) continue;
                model.DateUpdated = item.Attributes["updated"].Value;
            }
            // lấy giá vàng của các tỉnh thành
            var listNodes = xDoc.GetElementsByTagName("city");
            var lstRateCity = new List<RateCityItem>();
            foreach (XmlNode item in listNodes)
            {
                var lstRate = new List<RateItem>();
                var rateCityItem = new RateCityItem();
                for (var i = 0; i < item.ChildNodes.Count; i++)
                {
                    var attributes = item.ChildNodes[i].Attributes;
                    if (attributes == null) continue;
                    var rate = new RateItem
                    {
                        Buy = attributes["buy"].Value,
                        Sell = attributes["sell"].Value,
                        Type = attributes["type"].Value
                    };
                    lstRate.Add(rate);
                }
                if (item.Attributes == null) continue;
                rateCityItem.CityName = item.Attributes["name"].Value;
                rateCityItem.ListRateItem = lstRate;
                lstRateCity.Add(rateCityItem);
            }
            model.ListRateCityItem = lstRateCity;
            return View(model);
        }
        #endregion

        #region lấy tỉ giá ngoại tệ
        public ActionResult ExRate()
        {
            //var model = new ModelExrateItem();
            //var lstExrateItem = new List<ExrateItem>();
            //var xDocExrate = new XmlDocument();
            //xDocExrate.Load("http://vietcombank.com.vn/ExchangeRates/ExrateXML.aspx");
            //// lấy ngày cập nhật
            //var nodeDateTime = xDocExrate.GetElementsByTagName("DateTime");
            //foreach (XmlNode item in nodeDateTime)
            //    model.DateUpdated = item.InnerText;
            //// lấy tỉ giá ngoại tệ
            //var nodeExrate = xDocExrate.GetElementsByTagName("Exrate");
            //foreach (XmlNode item in nodeExrate)
            //{
            //    var attributes = item.Attributes;
            //    if (attributes == null) continue;
            //    var exRate = new ExrateItem
            //    {
            //        CurrencyCode = attributes["CurrencyCode"].Value,
            //        CurrencyName = attributes["CurrencyName"].Value,
            //        Buy = attributes["Buy"].Value,
            //        Transfer = attributes["Transfer"].Value,
            //        Sell = attributes["Sell"].Value
            //    };
            //    lstExrateItem.Add(exRate);
            //}
            //model.ListExrateItem = lstExrateItem;
            return View();
        }
        #endregion

        #region thống kê truy cập

        public PartialViewResult CounterStatistics()
        {
            var model = new ModelSystemConfigItem
            {
                
            };
            return PartialView(model);
        }

        #endregion

        #region Tìm điểm giao dịch trên google map

        public ActionResult ATMGoogleMap()
        {
            var model = new ModelCityItem
            {
                ListItem = _cityBl.GetAllListByGoogleMap(),
                ListGoogleMapItem = _googleMapBl.GetAllListSimple()
            };
            return PartialView(model);
        }

        public PartialViewResult Direct(int ctrId, string url)
        {
            var model = new ModelCityItem
            {
                ListItem = _cityBl.GetAllListByGoogleMap(),
                ListGoogleMapItem = _googleMapBl.GetAllListSimple(),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }

        public ActionResult GoogleMap()
        {
            var model = new ModelCityItem
            {
                ListItem = _cityBl.GetAllListByGoogleMap(),
                ListGoogleMapItem = _googleMapBl.GetAllListSimple()
            };
            return View(model);
        }

        public JsonResult GetData(int cityId, int districtId)
        {
            var listGoogleMap = new List<GoogleMapItem>();
            if (cityId == 0 && districtId == 0)
                listGoogleMap = _googleMapBl.GetAllListSimple();
            if (cityId > 0 && districtId == 0)
                listGoogleMap = _googleMapBl.GetGoogleMapsItemByCityId(cityId);
            if (cityId > 0 && districtId > 0)
                listGoogleMap = _googleMapBl.GetGoogleMapsItemByDistrictID(districtId);
            var googleMaps = new List<object>();
            foreach (var item in listGoogleMap)
                googleMaps.Add(
                    new
                    {
                        infowindow = string.Format(item.Name + "<br>" + item.Address + "<br> Tel :" + item.Tel + "<br> Fax: " + item.Fax),
                        id = item.ID,
                        latitude = item.Latitude,
                        longitude = item.Longitude
                    });
            return Json(googleMaps, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictItemByCityId(int cityId)
        {
            var city = _cityBl.GetDistrictItemByCityId(cityId);
            var lstDistrict = city.ListDistrictItem;
            return Json(lstDistrict, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetListGoogleMapByCityId(int cityId)
        {
            var model = _cityBl.GetCityItemById(cityId);
            return PartialView(model);
        }

        public PartialViewResult GetListGoogleMapByDistrictId(int districtId)
        {
            var model = _cityBl.GetListGoogleMapByDistrictID(districtId);
            return PartialView(model);
        }

        public JsonResult GetListGoogleMapByFilter(int cityId, int districtId, int type = 1)
        {
            var num = -1;
            var listGoogleMap = new List<GoogleMapItem>();
            if (cityId == 0 && districtId == 0)
                listGoogleMap = _googleMapBl.GetAllListSimple();
            if (cityId > 0 && districtId == 0)
                listGoogleMap = _googleMapBl.GetGoogleMapsItemByCityId(cityId);
            if (cityId > 0 && districtId > 0)
                listGoogleMap = _googleMapBl.GetGoogleMapsItemByDistrictID(districtId);
            var googleMaps = new List<object>();
            foreach (var item in listGoogleMap.OrderByDescending(m => m.ID))
            {
                num++;
                googleMaps.Add(
                    new
                    {
                        infowindow = string.Format("<div class=\"locationDetail\"><strong>{0}</strong><br><span><i class=\"fa fa-map-marker\"></i>{1}</span><span ><i class=\"fa fa-phone\"></i> Tel :{2}</span><br><span><i class=\"fa fa-fax\"></i> Fax: {3}</span></div>", item.Name, item.Address, item.Tel, item.Fax),
                        id = item.ID,
                        latitude = item.Latitude,
                        longitude = item.Longitude
                    });
            }
            return Json(googleMaps, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}
