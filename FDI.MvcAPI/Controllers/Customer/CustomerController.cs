using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA;
using FDI.Simple;
using FDI.DA;
using FDI.Utils;
using System.Linq;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class CustomerController : BaseApiController
    {
        private readonly CustomerDA _da = new CustomerDA();
        private readonly CityDA _cityDA = new CityDA();
        private readonly DNCardDA _cardDa = new DNCardDA();
        public ActionResult GetListByCustomer(string key, int customerId)
        {
            var obj = key != Keyapi ? new List<DNCardItem>() : _cardDa.GetListByCustomer(customerId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(string key)
        {
            var obj = key != Keyapi ? new List<CustomerItem>() : _da.GetList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<CustomerItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByParent(string key, int parentId)
        {
            var obj = key != Keyapi ? new List<CustomerItem>() : _da.GetListByParent(parentId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItems(int agencyId,int type)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCustomerItem()
                : new ModelCustomerItem { ListItem = _da.GetListSimpleByRequest(Request, agencyId, type), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDiscountRequest()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCustomerItem()
                : new ModelCustomerItem { ListItem = _da.GetDiscountRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListTree(string key, int id, int lv, int agencyId)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTree(id, lv, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAutoFashion(string key, string keword, int agencyId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerItem(string key, int id)
        {
            var obj = key != Keyapi ? new CustomerItem() : _da.GetCustomerItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerBySerial(string key, string serial)
        {
            var obj = key != Keyapi ? new CustomerItem() : _da.GetCustomerBySerial(serial);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerItemBySerial(string key, string serial)
        {
            var obj = key != Keyapi ? new CustomerItem() : _da.GetCustomerItemBySerial(serial);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListCity(string key)
        {
            var obj = key != Keyapi ? new List<CityItem>() : _cityDA.GetListSimple();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListDistrict(string key, int cityId)
        {
            var obj = key != Keyapi ? new List<DistrictItem>() : _cityDA.GetListDistrict(cityId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckCardSerial(string key, string sirial)
        {
            if (key == Keyapi)
            {
                var b = _cardDa.CheckCardSerial(sirial);
                return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckUserName(string key, string txt, int id)
        {
            if (key == Keyapi)
            {
                var b = _da.CheckUserName(txt, id);
                return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckParent(string key, string txt)
        {
            if (key == Keyapi)
            {
                var b = _da.CheckParent(txt);
                return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckEmail(string key, string txt, int id)
        {
            if (key == Keyapi)
            {
                var b = _da.CheckEmail(txt, id);
                return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckPhone(string key, string txt, int id)
        {
            if (key == Keyapi)
            {
                var b = _da.CheckPhone(txt, id);
                return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddCard(string key, int id, string card, string code)
        {
            var json = new JsonMessage { Erros = true, Message = "Không có hành động nào thực hiện" };
            try
            {
                var model = _cardDa.GetCardItem(card, code);
                if (model != null)
                {
                    if (!_cardDa.CheckSendCard(model.ID))
                    {
                        _cardDa.Add(new Send_Card
                        {
                            CustomerID = id,
                            CardID = model.ID,
                            DateCreate = ConvertDate.TotalSeconds(DateTime.Now)
                        });
                        _cardDa.Save();
                        json.Erros = false;
                        json.Message = "Thêm thẻ thành công";
                    }
                    else
                    {
                        json.Erros = true;
                        json.Message = "Thẻ đã được sử dụng";
                    }

                }
                else
                {
                    json.Erros = true;
                    json.Message = "Thẻ không tồn tại";
                }
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Add(string key, string json)
        {
            var model = new Base.Customer();
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                UpdateModel(model);
                var birth = Request["Birthday_"];
                var parent = Request["Parent"];
                var serial = Request["CardSerial"];
                var pin = Request["PinCard"];
                var NoteCate = Request["NoteCate"];
                if (!string.IsNullOrEmpty(serial))
                {
                    var carditem = _da.GetCardItem(serial, pin);
                    model.CardID = carditem.ID;
                }
                model.FullName = HttpUtility.UrlDecode(model.FullName);
                model.Address = HttpUtility.UrlDecode(model.Address);
                model.Birthday = birth.StringToDecimal();
                model.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                model.IsDelete = false;
                model.IsActive = true;
                model.PasswordSalt = FDIUtils.CreateSaltKey(5);
                model.PassWord = FDIUtils.CreatePasswordHash(model.PassWord ?? "fdi123456", model.PasswordSalt);
                if (!string.IsNullOrEmpty(NoteCate))
                {
                    var customerCare = new Customer_Care
                    {
                        Note = HttpUtility.UrlDecode(NoteCate),
                        AgencyId = Agencyid()
                    };
                    model.Customer_Care.Add(customerCare);
                }
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                var birth = Request["Birthday_"];
                var serial = Request["CardSerial"];
                var pin = Request["PinCard"];
                var NoteCate = Request["NoteCate"];
                var PassWord = Request["PassWord"];
                model.Birthday = birth.StringToDecimal();
                var phone = model.Phone;
                UpdateModel(model);
                if (!model.CardID.HasValue && !string.IsNullOrEmpty(serial))
                {
                    var carditem = _da.GetCardItem(serial, pin);
                    if (carditem != null)
                        model.CardID = carditem.ID;
                }
                var customerCare = model.Customer_Care.FirstOrDefault();
                if (customerCare != null && !string.IsNullOrEmpty(NoteCate))
                {
                    customerCare.Note = HttpUtility.UrlDecode(NoteCate);
                }
                else if(!string.IsNullOrEmpty(NoteCate))
                {
                    customerCare = new Customer_Care
                    {
                        Note = HttpUtility.UrlDecode(NoteCate),
                        AgencyId = Agencyid()
                    };
                    model.Customer_Care.Add(customerCare);
                }
                if (!string.IsNullOrEmpty(PassWord))
                {
                    model.PassWord = FDIUtils.CreatePasswordHash(PassWord, model.PasswordSalt);
                }
                model.FullName = HttpUtility.UrlDecode(model.FullName);
                model.Address = HttpUtility.UrlDecode(model.Address);
                model.Phone = phone;
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCustomerCare(string key, string json, int agencyId)
        {
            var msg = new JsonMessage(false, "Sở thích cập nhật thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                var NoteCate = Request["NoteCate"];
                var customerCare = model.Customer_Care.Where(v => v.AgencyId == agencyId).FirstOrDefault();
                if (customerCare != null)
                {
                    customerCare.Note = HttpUtility.UrlDecode(NoteCate);
                }
                else
                {
                    customerCare = new Customer_Care
                    {
                        Note = HttpUtility.UrlDecode(NoteCate),
                        AgencyId = Agencyid()
                    };
                    model.Customer_Care.Add(customerCare);
                }
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListByArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsDelete = true;
                    }
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Truy cập thất bại.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
