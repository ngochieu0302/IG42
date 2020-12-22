using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        //
        // GET: /Admin/Customer/
        private readonly CustomerDA _da;
        private readonly DNCardDA _cardDa = new DNCardDA();
        private readonly CityDA _systemCityDa;

        public CustomerController()
        {
            _da = new CustomerDA("#");
            _systemCityDa = new CityDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var listcustome = _da.GetListAdminByRequest(Request);
            var model = new ModelCustomerItem
            {
                ListItem = listcustome,
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var customerType = _da.GetCustomerItem(ArrId.FirstOrDefault());
            var model = customerType;
            return View(model);
        }
        public ActionResult AjaxHistory()
        {
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new CustomerItem
            {
                IsActive = true
            };
            ViewBag.ListCity = _systemCityDa.GetListSimple();
            if (DoAction == ActionType.Edit)
            {
                model = _da.GetCustomerItem(ArrId.FirstOrDefault());
                ViewBag.ListDistrict = _systemCityDa.GetListDistrict(model.CityID ?? 0);
            }
            ViewBag.Action = DoAction;
            return View(model);
        }

        public ActionResult District(int cityId, int districtId = 0)
        {
            ViewBag.districtId = districtId;
            var model = _systemCityDa.GetListDistrict(cityId);
            return PartialView(model);
        }
        public ActionResult GetDistrict(int cityId)
        {
            var model = _systemCityDa.GetListDistrict(cityId);
            return Json(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var model = new Customer();
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            //List<Customer> ltsCustomerItems;
            //StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
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
                        model.Birthday = ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(birth));
                        model.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        model.IsDelete = false;
                        model.IsActive = true;
                        model.PasswordSalt = FDIUtils.CreateSaltKey(5);
                        model.PassWord = FDIUtils.CreatePasswordHash(model.PassWord ?? "ssc123456", model.PasswordSalt);
                        if (!string.IsNullOrEmpty(NoteCate))
                        {
                            var customerCare = new Customer_Care
                            {
                                Note = NoteCate,
                                AgencyId = AgencyId
                            };
                            model.Customer_Care.Add(customerCare);
                        }
                        _da.Add(model);
                        _da.Save();
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Thêm mới thất bại.";
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        model = _da.GetById(ArrId.FirstOrDefault());
                        var birth = Request["Birthday_"];
                        var serial = Request["CardSerial"];
                        var pin = Request["PinCard"];                       
                        model.Birthday = ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(birth));
                        var phone = model.Phone;
                        UpdateModel(model);
                        if (!model.CardID.HasValue && !string.IsNullOrEmpty(serial))
                        {
                            var carditem = _da.GetCardItem(serial, pin);
                            if (carditem != null)
                                model.CardID = carditem.ID;
                            
                        }
                        if (!string.IsNullOrEmpty(model.PassWord))
                        {                            
                            model.PassWord = FDIUtils.CreatePasswordHash(model.PassWord, model.PasswordSalt);
                        }
                        model.Phone = phone;
                        _da.Save();
                        msg.Message = "Cập nhật dữ liệu thành công";
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật";
                        Log2File.LogExceptionToFile(ex);
                    }
                    break;

                case ActionType.Delete:
                    try
                    {

                        var lst = _da.GetListByArrId(ArrId);
                        foreach (var item in lst)
                        {
                            item.IsDelete = true;
                        }
                        _da.Save();
                        msg.Message = "Xóa dữ liệu thành công";
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được xóa";
                        Log2File.LogExceptionToFile(ex);
                    }                    
                    break;
                default:
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult ResetPassword(int id)
        {

            var msg = new JsonMessage();
            var customer = _da.GetById(id);
            var pass = FDIUtils.RandomString(4).ToLower();
            const string stringRegexCheckEmailOrWith = @"^(([^<>()[\]\\.,;:\s@\""]+"
                                                       + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                                       + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                                       + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                                       + @"[a-zA-Z]{2,}))$";
            if (!string.IsNullOrEmpty(customer.Email) && Regex.IsMatch(customer.Email, stringRegexCheckEmailOrWith))
            {
                _da.Save();
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = customer.ID.ToString(),
                    Message = string.Format("Đã Reset lại password: <b>{0}</b>", Server.HtmlEncode(customer.FullName))
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(customer.Phone))
            {
                _da.Save();
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = customer.ID.ToString(),
                    Message = string.Format("Đã Reset lại password: <b>{0}</b>", Server.HtmlEncode(customer.FullName))
                };

                //FDIUtils.SendSMSToCustomer(customer.UserName, customer.Mobile, customer.Mobile, pass);
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Email khách hàng không đúng và Sđt không có!";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }        
        public string CheckByUserName(string UserName, int id)
        {
            var result = _da.CheckUserName(UserName.Trim(), id);
            return result ? "false" : "true";
        }
        public string CheckByEmail(string Email, int id)
        {
            var result = _da.CheckEmail(Email.Trim(), id);
            return result ? "false" : "true";
        }
        public string CheckByPhone(string Phone, int id)
        {
            var result = _da.CheckPhone(Phone.Trim(), id);
            return result ? "false" : "true";
        }
        public string CheckCardSerial(string CardSerial)
        {
            var result = _cardDa.CheckCardSerial(CardSerial);
            return result ? "true" : "false";
        }
        public string CheckByParent(string Parent)
        {
            var result = _da.CheckParent(Parent);
            return result == false ? "false" : "true";
        }
        public ActionResult Auto(int agencyId)
        {
            var query = Request["query"];
            var ltsResults = _da.GetListAuto(query, 10, agencyId);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
    }
}
