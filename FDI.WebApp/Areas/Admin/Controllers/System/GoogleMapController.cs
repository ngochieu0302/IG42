using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class GoogleMapController : BaseController
    {
        
        private readonly GoogleMapDA _googleMapDa;
        private readonly CityDA _systemCityDa;
        private readonly DistrictDA _systemDistrictDa;

        public GoogleMapController()
        {
            _googleMapDa = new GoogleMapDA("#");
            _systemCityDa = new CityDA("#");
            _systemDistrictDa = new DistrictDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ListItems()
        {
            var model = new ModelGoogleMapItem
            {
                ListGoogleMapItem = _googleMapDa.GetListSimpleByRequest(Request),
                PageHtml = _googleMapDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var googleMap = new GoogleMap();
            if (DoAction == ActionType.Edit)
            {
                googleMap = _googleMapDa.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.LstCity = _systemCityDa.GetAll();
            ViewBag.LstDistrict = _systemDistrictDa.GetAllListSimple();
            ViewData.Model = googleMap;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult GetDistrictItemByCityIdt(int cityID)
        {
            var city = _systemCityDa.GetItemById(cityID);
            var lstDistrict = city.ListDistrictItem;
            return Json(lstDistrict, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var googleMap = new GoogleMap();
            List<GoogleMap> lstGoogleMaps;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(googleMap);
                        googleMap.LanguageId = Fdisystem.LanguageId;
                        if (!string.IsNullOrEmpty(Request["DistrictID"]))
                            googleMap.DistrictID = Convert.ToInt32(Request["DistrictID"]);
                        _googleMapDa.Add(googleMap);
                        _googleMapDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = googleMap.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(googleMap.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        googleMap = _googleMapDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(googleMap);
                        if (!string.IsNullOrEmpty(Request["DistrictID"]))
                            googleMap.DistrictID = Convert.ToInt32(Request["DistrictID"]);
                        _googleMapDa.Save();

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = googleMap.ID.ToString(),
                            Message = string.Format("Đã cập nhật: <b>{0}</b>", Server.HtmlEncode(googleMap.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    lstGoogleMaps = _googleMapDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lstGoogleMaps)
                    {
                        _googleMapDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));

                    }
                    msg.ID = string.Join(",", ArrId);
                    _googleMapDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    lstGoogleMaps = _googleMapDa.GetListByArrId(ArrId).Where(o => o.IsShow != null && !o.IsShow.Value).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in lstGoogleMaps)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _googleMapDa.Save();
                    msg.ID = string.Join(",", lstGoogleMaps.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    lstGoogleMaps = _googleMapDa.GetListByArrId(ArrId).Where(o => o.IsShow != null && o.IsShow.Value).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in lstGoogleMaps)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _googleMapDa.Save();
                    msg.ID = string.Join(",", lstGoogleMaps.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
