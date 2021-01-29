using FDI.Base;
using FDI.DA;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using System.Net;
using System.Web.Script.Serialization;
using FDI.DA.DA;
using FDI.CORE;
using Newtonsoft.Json.Linq;
using ZaloDotNetSDK;

namespace FDI.MvcAPI.Controllers
{
    [CustomerAuthorize]
    public class CustomerAppController : BaseAppApiController
    {
        TokenOtpDA tokenOtpDA = new TokenOtpDA();
        CustomerAppIG4DA customerDA = new CustomerAppIG4DA();
        CustomerBL _customerBl = new CustomerBL();
        OrderAppIG4DA orderDA = new OrderAppIG4DA();
        CustomerAddressAppIG4DA customerAddressDA = new CustomerAddressAppIG4DA();
        readonly WalletCustomerAppIG4DA _walletCustomerDa = new WalletCustomerAppIG4DA("#");
        readonly WalletsAppIG4DA _walletsDa = new WalletsAppIG4DA("#");
        readonly CustomerPolicyAppIG4DA _customerPolicyDa = new CustomerPolicyAppIG4DA("#");
        readonly CashOutWalletAppIG4DA _cashOutWalletDa = new CashOutWalletAppIG4DA("#");
        readonly RewardHistoryAppIG4DA _rewardHistoryDa = new RewardHistoryAppIG4DA("#");
        readonly AgencyDA _agencyDa = new AgencyDA("");

        //public ActionResult CustomerOrther(string lstInt)
        //{
        //    var model = customerDA.GetItemByID()
        //}

        public ActionResult GetProfile()
        {
            var obj = customerDA.GetItemByID(CustomerId);
            return Json(new BaseResponse<CustomerAppIG4Item> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        #region Wallets customer

        public ActionResult AddWallets(WalletCustomerAppIG4Item data)
        {
            try
            {
                var wallet = new WalletCustomer
                {
                    CustomerID = data.CustomerID,
                    TotalPrice = data.Totalprice ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    IsDelete = false,
                    Note = data.Note,
                    IsActive = true,
                    Transaction_no = data.TransactionNo,
                };
                _walletCustomerDa.Add(wallet);
                _walletCustomerDa.Save();
                UpdateLevelCustomer(data.CustomerID ?? 0);
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage(404, ex.ToString()), JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        }


        [System.Web.Http.AllowAnonymous]
        public ActionResult GetListWalletsHistory(int customerId, int type, int page, int take)
        {
            var list = _walletCustomerDa.GetListWalletCustomerbyId(customerId);
            var listall = list.Select(item => new WalletsAppAppIG4Item
            {
                ID = item.ID,
                Name = item.TypeWalet == 1 ? "Giao dịch nạp tiền vào G-Store" : "Đơn bán thành công cho khách hàng",
                Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
                Price = item.Totalprice,
                Type = 1
            })
                .ToList();
            var listcash = _cashOutWalletDa.GetListbyCustomer(customerId);
            listall.AddRange(listcash.Select(item => new WalletsAppAppIG4Item
            {
                ID = item.ID,
                Name = item.TypeCash == 1 ? "Giao dịch mua hàng từ G-Store" : item.TypeCash == 2 ? "Giao dịch mua gói dịch vụ từ G-Store" : item.Query,
                Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
                Price = item.Total,
                Type = 2
            }));
            listall = listall.Where(c => (c.Type == type || type == 0)).OrderByDescending(c => c.ID).ToList();
            listall = listall.Skip((page - 1) * take).Take(take).ToList();
            return Json(new BaseResponse<List<WalletsAppAppIG4Item>>() { Code = 200, Data = listall, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetTotalWallets(int customerId)
        {
            decimal? totalw = 0;
            var total = _walletsDa.GetWalletsItemById(customerId);
            if (total != null)
            {
                totalw = total.Wallets;
            }
            return Json(new BaseResponse<decimal>() { Code = 200, Data = totalw ?? 0, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetListWalletsRewardHistory(int customerId, int type, int page, int take)
        {
            var list = _walletCustomerDa.GetListWalletReward(customerId);
            var listall = list.Select(item => new WalletsAppAppIG4Item
            {
                ID = item.ID,
                Name = item.Type == (int)Reward.Dep ? "Thưởng nạp ví chính" : "Thưởng " + item.BonustypeName,
                Price = item.Price,
                Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
                Type = 1
            })
                .ToList();
            var listrecive = _walletCustomerDa.GetListWalletRecive(customerId);
            listall.AddRange(listrecive.Select(item => new WalletsAppAppIG4Item
            {
                ID = item.ID,
                Name = item.Query,
                Price = item.Price,
                Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
                Type = 2
            }));
            listall = listall.Where(c => (type == 0 || c.Type == type)).OrderByDescending(c => c.ID).ToList();
            listall = listall.Skip((page - 1) * page).Take(take).ToList();
            return Json(new BaseResponse<List<WalletsAppAppIG4Item>>() { Code = 200, Data = listall, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalWalletsReward(int customerId)
        {
            decimal? totalw = 0;
            var total = _walletsDa.GetWalletsRewardByCusId(customerId);
            if (total != null)
            {
                totalw = total.Total;
            }
            return Json(new BaseResponse<decimal>() { Code = 200, Data = totalw ?? 0, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [AllowAnonymous]
        public async Task<ActionResult> Login(string phone)
        {
            try
            {
                if (!customerDA.CheckExitsByPhone(phone, Type))
                {
                    customerDA.Add(new Base.Customer()
                    {
                        Mobile = phone,
                        IsDelete = false,
                        IsPrestige = false,
                        // type = 1 la khach hang
                        Type = 1,
                        DateCreated = DateTime.Now.TotalSeconds()
                    });
                    customerDA.Save();
                }
                //var otp = FDIUtils.RandomOtp(4);
                var otp = "123456";
                //var otppost = new PostOtpLoginAppIG4()
                //{
                //    msisdn = phone.Remove(0, 1).Insert(0, "84"),
                //    brandname = "G-STORE",
                //    msgbody = "IG4: Ma xac minh cua ban la " + otp,
                //    user = "G-STORE",
                //    pass = "GSTORE123",
                //    charset = "8"
                //};
                //var url = "http://123.31.20.167:8383/restservice/";
                //var result = await PostDataAsync<List<ResultotpAppIG4>>(url, otppost);
                //if (result.FirstOrDefault()?.Result.code == "200")
                //{
                tokenOtpDA.Add(new TokenOtp()
                {
                    ObjectId = phone,
                    Token = otp,
                    IsDeleted = false,
                    IsUsed = false,
                    TypeToken = (int)TokenOtpType.Authen,
                    DateCreated = DateTime.Now,
                });
                tokenOtpDA.Save();
                //}
                //else
                //{
                //    return Json(new JsonMessage(-1, "Gửi mã OTP thất bại"), JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(-2, e.ToString()), JsonRequestBehavior.AllowGet);

            }

            return Json(new JsonMessage(200, "Ok"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateAgencyQR(string phone)
        {
            try
            {
                var agency = _agencyDa.GetByPhone(phone);
                var cus = customerDA.GetById(CustomerId);
                cus.AgencyID = agency.ID;
                customerDA.Save();
            }
            catch (Exception e)
            {
                return Json(new JsonMessage { Code = 404, Message = e.ToString() });
            }

            return Json(new JsonMessage { Code = 200, Message = "" });
        }
        [System.Web.Mvc.HttpPost]
        [AllowAnonymous]

        public ActionResult FacebookCallback(string accesstoken, string token)
        {
            try
            {
                var fb = new FacebookClient();
                if (!string.IsNullOrEmpty(accesstoken))
                {
                    fb.AccessToken = accesstoken;
                    dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                    var cus = new Base.Customer
                    {
                        Email = me.email,
                        UserName = me.email,
                        FullName = me.first_name + me.middle_name + me.last_name,
                        DateCreated = DateTime.Now.TotalSeconds(),
                        IsActive = true,
                        IsDelete = false,
                        idUserFacebook = me.id,
                        TokenDevice = token,
                    };
                    //dynamic pic = fb.Get(me.id+"/picture");

                    InsertCustomerFacebook(cus);
                    var customer = customerDA.GetByidUserFacebook(cus.idUserFacebook);
                    var key = Guid.NewGuid();
                    IAuthContainerModel model = new JWTContainerModel()
                    {
                        Claims = new Claim[]
                        {
                            new Claim(type: "Phone", value: customer.Mobile ?? ""),
                            new Claim(type: "Type", value: "Token"),
                            new Claim(type: "ID",value: customer.ID.ToString()),
                        },
                        ExpireMinutes = 10,
                    };
                    IAuthContainerModel modelRefreshToken = new JWTContainerModel()
                    {
                        Claims = new Claim[]
                        {
                            new Claim(type: "Phone", value: customer.Mobile ?? ""),
                            new Claim(type: "Type", value: "RefreshToken"),
                            new Claim(type: "key", value: key.ToString()),
                            new Claim(type: "ID",value: customer.ID.ToString()),
                        },
                        ExpireMinutes = 60 * 24 * 30,
                    };
                    var tokenResponse = JWTService.Instance.GenerateToken(model: model);
                    var refreshToken = JWTService.Instance.GenerateToken(model: modelRefreshToken);
                    customerDA.InsertToken(data: new TokenRefresh() { GuidId = key });
                    customerDA.Save();
                    return Json(data: new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshToken, ID = customer.ID } }, behavior: JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult GoogleCallback(string accesstoken, string token)
        {
            Userclass userinfo = new Userclass();
            try
            {
                string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accesstoken + "";
                WebRequest request = WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();

                userinfo = js.Deserialize<Userclass>(responseFromServer);

                var cus = new Base.Customer
                {
                    Email = userinfo.email,
                    UserName = userinfo.email,
                    FullName = userinfo.family_name + " " + userinfo.given_name + " " + userinfo.name,
                    DateCreated = DateTime.Now.TotalSeconds(),
                    IsActive = true,
                    IsDelete = false,
                    idUserGoogle = userinfo.id,
                    AvatarUrl = userinfo.picture,
                    TokenDevice = token,
                };

                InsertCustomerGoogle(cus);
                var customer = customerDA.GetByidUserGoogle(cus.idUserGoogle);
                var key = Guid.NewGuid();
                IAuthContainerModel model = new JWTContainerModel()
                {
                    Claims = new Claim[]
                    {
                        new Claim(type: "Phone", value: customer.Mobile ?? ""),
                        new Claim(type: "Type", value: "Token"),
                        new Claim(type: "ID",value: customer.ID.ToString()),
                    },
                    ExpireMinutes = 10,
                };
                IAuthContainerModel modelRefreshToken = new JWTContainerModel()
                {
                    Claims = new Claim[]
                    {
                        new Claim(type: "Phone", value: customer.Mobile ?? ""),
                        new Claim(type: "Type", value: "RefreshToken"),
                        new Claim(type: "key", value: key.ToString()),
                        new Claim(type: "ID",value: customer.ID.ToString()),
                    },
                    ExpireMinutes = 60 * 24 * 30,
                };
                var tokenResponse = JWTService.Instance.GenerateToken(model: model);
                var refreshToken = JWTService.Instance.GenerateToken(model: modelRefreshToken);
                customerDA.InsertToken(data: new TokenRefresh() { GuidId = key });
                customerDA.Save();
                return Json(data: new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshToken } }, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Redirect("/");
            }

            return Redirect("/");
        }
        [AllowAnonymous]
        [System.Web.Mvc.HttpPost]
        public ActionResult ZaloCallback(string accesstoken)
        {
            try
            {
                var appId = 3722523456944291775;
                var appSecret = "MU1RP7QQ6k8ndjhPNqdj";

                if (!string.IsNullOrEmpty(accesstoken))
                {
                    ZaloAppInfo appInfo = new ZaloAppInfo(appId, appSecret, "callbackUrl");
                    ZaloAppClient appClient = new ZaloAppClient(appInfo);
                    JObject me = appClient.getProfile(accesstoken, "fields=a,name,id,birthday,gender,phone,picture");
                    var output = JsonConvert.SerializeObject(me);
                    ZaloCustomerItem deserializedProduct = JsonConvert.DeserializeObject<ZaloCustomerItem>(output);
                    if (string.IsNullOrEmpty(deserializedProduct.error))
                    {
                        var cus = new Base.Customer
                        {
                            UserName = deserializedProduct.name,
                            FullName = deserializedProduct.name,
                            DateCreated = DateTime.Now.TotalSeconds(),
                            IsActive = true,
                            IsDelete = false,
                            idUserZalo = deserializedProduct.id,
                            AvatarUrl = deserializedProduct.picture.data.url,
                        };

                        InsertCustomerZalo(cus);
                        var customer = customerDA.GetbyidUserZalo(cus.idUserZalo);
                        var key = Guid.NewGuid();
                        IAuthContainerModel model = new JWTContainerModel()
                        {
                            Claims = new Claim[]
                            {
                                new Claim(type: "Phone", value: customer.Mobile ?? ""),
                                new Claim(type: "Type", value: "Token"),
                                new Claim(type: "ID",value: customer.ID.ToString()),
                            },
                            ExpireMinutes = 10,
                        };
                        IAuthContainerModel modelRefreshToken = new JWTContainerModel()
                        {
                            Claims = new Claim[]
                            {
                                new Claim(type: "Phone", value: customer.Mobile ?? ""),
                                new Claim(type: "Type", value: "RefreshToken"),
                                new Claim(type: "key", value: key.ToString()),
                                new Claim(type: "ID",value: customer.ID.ToString()),
                            },
                            ExpireMinutes = 60 * 24 * 30,
                        };
                        var tokenResponse = JWTService.Instance.GenerateToken(model: model);
                        var refreshToken = JWTService.Instance.GenerateToken(model: modelRefreshToken);
                        customerDA.InsertToken(data: new TokenRefresh() { GuidId = key });
                        customerDA.Save();
                        return Json(data: new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshToken, ID = customer.ID } }, behavior: JsonRequestBehavior.AllowGet);
                    }
                    return Json(data: new { Code = deserializedProduct.error, Erros = true, Message = "Có lỗi xảy ra vui lòng xem lại mã lỗi" }, behavior: JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
            return Redirect("/");
        }

        private void InsertCustomerFacebook(Base.Customer cus)
        {
            var obj = customerDA.GetByidUserFacebook(cus.idUserFacebook);
            if (obj == null)
            {
                customerDA.Add(cus);
                customerDA.Save();
            }
        }
        private void InsertCustomerGoogle(Base.Customer cus)
        {
            var obj = customerDA.GetByidUserGoogle(cus.idUserGoogle);
            if (obj == null)
            {
                customerDA.Add(cus);
                customerDA.Save();
            }
        }
        private void InsertCustomerZalo(Base.Customer cus)
        {
            var obj = customerDA.GetbyidUserZalo(cus.idUserZalo);
            if (obj == null)
            {
                customerDA.Add(cus);
                customerDA.Save();
            }
            else
            {
                obj = cus;
                customerDA.Save();
            }
        }
        [AllowAnonymous]
        public async Task<ActionResult> ValidateToken(string token, string phone, string tokenDevice)
        {
            if (!tokenOtpDA.ValidateToken(token, phone, (int)TokenOtpType.Authen))
            {
                return Json(new JsonMessage(1000, "Thông tin đăng nhập không hợp lệ"), JsonRequestBehavior.AllowGet);
            }
            tokenOtpDA.UpdateIsUsed(token, phone);
            await tokenOtpDA.SaveAsync();
            var customer = customerDA.GetByPhone(phone, Type);
            var key = Guid.NewGuid();
            IAuthContainerModel model = new JWTContainerModel()
            {
                Claims = new Claim[]
                       {
                            new Claim("Phone", customer.Mobile),
                            new Claim("Type", "Token"),
                            new Claim("ID",customer.ID.ToString()),
                       },
                ExpireMinutes = 10,
            };
            IAuthContainerModel modelRefreshToken = new JWTContainerModel()
            {
                Claims = new Claim[]
                       {
                            new Claim("Phone", customer.Mobile),
                            new Claim("Type", "RefreshToken"),
                            new Claim("key", key.ToString()),
                            new Claim("ID",customer.ID.ToString()),
                       },
                ExpireMinutes = 60 * 24 * 30,
            };
            var tokenResponse = JWTService.Instance.GenerateToken(model);
            var refreshToken = JWTService.Instance.GenerateToken(modelRefreshToken);
            customerDA.InsertToken(new TokenRefresh() { GuidId = key });
            customer.TokenDevice = tokenDevice;
            customerDA.Save();
            return Json(new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshToken, ID = customer.ID, Fullname = customer.FullName  } }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> RefreshToken(string refreshToken)
        {
            if (!JWTService.Instance.IsTokenValid(refreshToken))
            {
                return Json(new JsonMessage(1000, "invalid token"));
            }
            var tmp = JWTService.Instance.GetTokenClaims(refreshToken).FirstOrDefault(m => m.Type == "Type");

            if (tmp.Value.ToString() != "RefreshToken")
            {
                return Json(new JsonMessage(1000, "invalid token"));
            }
            var phone = JWTService.Instance.GetTokenClaims(refreshToken).FirstOrDefault(m => m.Type == "Phone")?.Value.ToString();
            var id = JWTService.Instance.GetTokenClaims(refreshToken).FirstOrDefault(m => m.Type == "ID")?.Value.ToString();
            var key = Guid.Parse(JWTService.Instance.GetTokenClaims(refreshToken).FirstOrDefault(m => m.Type == "key")?.Value ?? throw new InvalidOperationException());

            var oldToke = customerDA.GetTokenByGuidId(key);
            if (oldToke == null)
            {
                return Json(new JsonMessage(1000, "invalid token"));
            }
            customerDA.DeleteTokenRefresh(oldToke);
            key = Guid.NewGuid();

            IAuthContainerModel model = new JWTContainerModel()
            {
                Claims = new Claim[]
                       {
                            new Claim("Phone", phone),
                            new Claim("Type", "Token"),
                            new Claim("ID",id),
                       },
                ExpireMinutes = 10,
            };

            IAuthContainerModel modelRefreshToken = new JWTContainerModel()
            {
                Claims = new Claim[]
                       {
                            new Claim("Phone", phone),
                            new Claim("key", key.ToString()),
                            new Claim("Type", "RefreshToken"),
                            new Claim("ID",id),
                       },
                ExpireMinutes = 60 * 24 * 30,
            };

            var tokenResponse = JWTService.Instance.GenerateToken(model);
            var refreshTokenResponse = JWTService.Instance.GenerateToken(modelRefreshToken);
            customerDA.InsertToken(new TokenRefresh() { GuidId = key });
            customerDA.Save();

            return Json(new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshTokenResponse } }, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> AddAdress(CustomerAddressAppIG4Item data)
        {
            if (data.Latitude == null || data.Longitude == null)
            {
                return Json(new JsonMessage(1000, "Tọa độ không được để trống"), JsonRequestBehavior.AllowGet);
            }

            if (customerAddressDA.CheckExit(CustomerId, data.Latitude.Value, data.Longitude.Value))
            {
                return Json(new JsonMessage(1000, "Tọa độ đã tồn tại"), JsonRequestBehavior.AllowGet);
            }

            if (data.IsDefault)
            {
                customerAddressDA.ResetDefault(CustomerId);
            }

            var item = new CustomerAddress()
            {
                CustomerId = CustomerId,
                CustomerName = data.CustomerName,
                Address = data.Address,
                DateCreated = DateTime.Now,
                IsDefault = data.IsDefault,
                Phone = data.Phone,
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                IsDelete = false,
                AddressType = data.AddressType
            };
            customerAddressDA.Add(item);
            await customerAddressDA.SaveAsync();

            return Json(new BaseResponse<CustomerAddressAppIG4Item>() { Code = 200, Data = customerAddressDA.GetItemById(item.ID) }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DeleteAdress(int id)
        {

            var address = customerAddressDA.GetById(id, CustomerId);
            if (address == null)
            {
                return Json(new BaseResponse<CustomerAddressAppIG4Item>() { Code = 1000, Message = "Địa chỉ không tồn tại" }, JsonRequestBehavior.AllowGet);

            }
            if (address != null)
            {
                address.IsDelete = true;

            }
            customerAddressDA.Save();

            return Json(new BaseResponse<CustomerAddressAppIG4Item>() { Code = 200 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAdress()
        {
            var lst = customerAddressDA.GetAll(CustomerId);
            return Json(new BaseResponse<List<CustomerAppIG4Item>>() { Code = 200, Data = lst, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> UpdateAddress(CustomerAddressAppIG4Item data)
        {
            if (data.Latitude == null || data.Longitude == null)
            {
                return Json(new JsonMessage(1000, "Tọa độ không được để trống"), JsonRequestBehavior.AllowGet);
            }

            var item = customerAddressDA.GetById(data.ID, CustomerId);

            if (item == null)
            {
                return Json(new JsonMessage(1000, "Địa chỉ không tồn tại"), JsonRequestBehavior.AllowGet);
            }

            if (customerAddressDA.CheckExit(data.ID, CustomerId, data.Latitude.Value, data.Longitude.Value))
            {
                return Json(new JsonMessage(1000, "Tọa độ đã tồn tại"), JsonRequestBehavior.AllowGet);

            }

            if (data.IsDefault)
            {
                customerAddressDA.ResetDefault(CustomerId);
            }

            item.Address = data.Address;
            item.Phone = data.Phone;
            item.Latitude = data.Latitude;
            item.Longitude = data.Longitude;
            item.IsDefault = data.IsDefault;
            item.AddressType = data.AddressType;

            await customerAddressDA.SaveAsync();

            return Json(new BaseResponse<CustomerAddressAppIG4Item>() { Code = 200, Data = data, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);

        }
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> UpdateAcount(CustomerAppIG4Item data)
        {
            var customer = customerDA.GetById(CustomerId);
            if (customer == null)
            {
                return Json(new JsonMessage(1000, "Not found"));
            }
            if (!string.IsNullOrEmpty(data.Mobile) && customerDA.CheckExitsByPhone(CustomerId, data.Mobile))
            {
                return Json(new JsonMessage(1000, "Số điện thoại đã tồn tại"));
            }

            if (!string.IsNullOrEmpty(data.Email) && customerDA.CheckExitsByEmail(CustomerId, data.Email))
            {
                return Json(new JsonMessage(1000, "Email đã tồn tại"));
            }
            customer.Email = data.Email;
            customer.Description = data.Description;
            customer.FullName = data.Fullname;
            customer.Mobile = data.Mobile;
            customer.AgencyID = data.AgencyID ?? 1006;
            var file = Request.Files["fileAvatar"];
            if (file != null)
            {
                var img = await UploadImage(file);
                if (img.Code != 200)
                {
                    return Json(new JsonMessage(1000, "Tải Avatar không thành công"));
                }
                customer.AvatarUrl = img.Data.Folder + img.Data.Url;
            }
            file = Request.Files["fileImgTimeline"];
            if (file != null)
            {
                var img = await UploadImage(file);
                if (img.Code != 200)
                {
                    return Json(new JsonMessage(1000, "Tải ảnh bìa không thành công"));
                }
                customer.ImageTimeline = img.Data.Folder + img.Data.Url;
            }
            customerDA.Save();
            var obj = customerDA.GetItemByID(CustomerId);
            return Json(new BaseResponse<CustomerAppIG4Item> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        private async Task<BaseResponse<GalleryPictureItem>> UploadImage(HttpPostedFileBase file)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    byte[] fileBytes = new byte[file.InputStream.Length + 1]; file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                    content.Add(fileContent);
                    var result = await client.PostAsync("http://imggstore.fditech.vn/home/upload", content);
                    if (result.IsSuccessStatusCode)
                    {
                        var datas = await result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<BaseResponse<GalleryPictureItem>>(datas);
                    }
                    return new BaseResponse<GalleryPictureItem>() { Code = 1000 };
                }
            }
        }
        [AllowAnonymous]
        public ActionResult ListByMap(int km, float la, float lo)
        {
            var lst = _customerBl.ListByMap(km, la, lo);
            return Json(new BaseResponse<List<CustomerAppIG4Item>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetShopPrestige(int page, int pagesize)
        {
            var lst = customerDA.GetPrestige(Latitude, Longitude, page, pagesize);
            return Json(new BaseResponse<List<CustomerAppIG4Item>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetShopPrestigeForMap(double km, double latitude, double longitude, string name)
        {
            var lst = customerDA.GetPrestigeForMap(km, latitude, longitude, name);
            return Json(new BaseResponse<List<CustomerAppIG4Item>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetShopSame(int shopid, double minKm = 0, double maxKm = 0, double latitude = 0, double longitude = 0, int page = 0, int pagesize = 0)
        {
            var lst = customerDA.ShopSame(shopid, minKm, maxKm, latitude, longitude, page, pagesize);
            return Json(new BaseResponse<List<CustomerAppIG4Item>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetListOrderStatus(int cusId, int status, int page, int pagesize)
        {
            var lst = customerDA.GetListOrderCustomer(cusId, status, page, pagesize);
            return Json(new BaseResponse<List<OrderCustomerAppItem>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListPackage()
        {
            var lst = customerDA.GetListPackage();
            return Json(new BaseResponse<List<NewsAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShopPresigeDetail(int id)
        {
            var item = customerDA.ShopPresigeDetail(id);

            if (item == null)
            {
                return Json(new JsonMessage(1000, "Shop không tồn tại"));
            }
            if (item.Latitude.HasValue)
            {
                item.Km = ConvertUtil.distance(Latitude, Longitude, (double)item.Latitude.Value, (double)item.Longitude.Value, 'K');

            }

            //lay category
            item.CategoryItem = customerDA.GetCategoryByShop(id);
            return Json(new BaseResponse<CustomerAppIG4Item>() { Code = 200, Data = item }, JsonRequestBehavior.AllowGet);
        }
        public class Userclass
        {
            public string id
            {
                get;
                set;
            }
            public string name
            {
                get;
                set;
            }
            public string given_name
            {
                get;
                set;
            }
            public string family_name
            {
                get;
                set;
            }
            public string link
            {
                get;
                set;
            }
            public string picture
            {
                get;
                set;
            }
            public string gender
            {
                get;
                set;
            }
            public string locale
            {
                get;
                set;
            }
            public string email { get; set; }
        }
        [AllowAnonymous]
        public ActionResult UpdateStatusCustomer(int orderId, int status, int cusId)
        {
            var data = orderDA.GetById(orderId);
            decimal? totak = 0;
            foreach (var items in data.Shop_Order_Details)
            {
                var k = items.Shop_Product.Category.Profit;
                totak += (items.Shop_Product.Product_Size != null ? items.Shop_Product.Product_Size.Value : 1) * items.Quantity * k * 1000;
            }
            data.Status = status;
            data.Check = 2;
            data.DateUpdateStatus = DateTime.Now.TotalSeconds();
            foreach (var item in data.Shop_Order_Details)
            {
                item.Status = status;
                item.DateUpdateStatus = DateTime.Now.TotalSeconds();
                item.Check = 2;
            }

            var TotalPricegstore = data.Total + data.FeeShip;
            SpliceOrderCustomer(":4000", orderId);
            orderDA.Save();
            if (status == (int)StatusOrder.Complete)
            {
                #region đơn hàng đã giao thành chuyển tiền cho shop
                var cashout = new CashOutWallet
                {
                    CustomerID = 1,
                    TotalPrice = TotalPricegstore ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    OrderID = data.ID,
                    Type = 1,
                    Code = data.Code,
                };
                _cashOutWalletDa.Add(cashout);
                _cashOutWalletDa.Save();
                #endregion

                var config = _walletCustomerDa.GetConfig();
                var shop = customerDA.GetItemByID(data.ShopID ?? 0);

                //var TotalPrice = data.Total - (config.DiscountOrder * data.Total / 100) + data.FeeShip;
                var TotalPrice = (data.Total - totak) + (totak * shop.PercentDiscount / 100) + data.FeeShip;

                var walletcus = new WalletCustomer
                {
                    CustomerID = data.ShopID,
                    TotalPrice = TotalPrice ?? 0,
                    DateCreate = DateTime.Now.TotalSeconds(),
                    IsActive = true,
                    IsDelete = false,
                    Type = 2,
                    Transaction_no = data.Code,
                };
                _walletCustomerDa.Add(walletcus);
                _walletCustomerDa.Save();

                var cus = customerDA.GetItemByID(data.CustomerID ?? 0);
                var sucess = orderDA.GetNotifyById(3);
                var token = cus.tokenDevice;
                Pushnotifycation(sucess.Title, sucess.Content.Replace("{shop}", data.Customer.FullName).Replace("{price}", TotalPrice.Money()).Replace("{code}", data.Code), token, sucess.ID.ToString());

                var shopsucess = orderDA.GetNotifyById(5);
                var tokenshop = shop.tokenDevice;
                Pushnotifycation(shopsucess.Title, shopsucess.Content.Replace("{price}", TotalPrice.Money()).Replace("{code}", data.Code).Replace("{customer}", data.Customer.FullName), tokenshop, shopsucess.ID.ToString());
                var bonusItems = customerDA.ListBonusTypeItems();
                // tính hoa hồng
                var iskg = data.Customer.Type == 2;
                if (!iskg)
                {
                    InsertRewardOrderCustomer(cus, config, totak, data.ID, bonusItems);
                    InsertRewardOrderAgency(shop, config, totak, data.ID, bonusItems);
                }
                else
                {
                    // chiết khấu shop ký gửi

                    //decimal totalpres = data.Shop_Order_Details.Where(detail => detail.IsPrestige == true).Sum(detail => detail.TotalPrice ?? 0);
                    //decimal totalnopres = data.Shop_Order_Details.Where(detail => detail.IsPrestige == false || !detail.IsPrestige.HasValue).Sum(detail => detail.TotalPrice ?? 0);
                    //if (totalpres > 0)
                    //{
                    //    InsertRewardCustomer(cus, totalpres, data.ID, bonusItems, 2, data.ShopID ?? 0);
                    //}
                    //if (totalnopres > 0)
                    //{
                    //    InsertRewardCustomer(cus, totalnopres, data.ID, bonusItems);
                    //}
                }
                // update level KH
                UpdateLevelCustomer(cusId);
            }
            else
            {
                var shop = customerDA.GetItemByID(data.ShopID ?? 0);
                var shopsucess = orderDA.GetNotifyById(10);

                var tokenshop = shop.tokenDevice;
                Pushnotifycation(shopsucess.Title.Replace("{customer}", data.Customer.FullName), shopsucess.Content.Replace("{code}", data.Code), tokenshop, shopsucess.ID.ToString());
            }
            return Json(new BaseResponse<List<ProductItem>> { Code = 200, Message = "Cập nhật trạng thái thành công!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchMoney(string query, int cusId)
        {
            try
            {
                var totalwalet = customerDA.GetItemByID(cusId);
                var config = _walletCustomerDa.GetConfig();
                if (totalwalet.Wallets > 0 && totalwalet.Wallets >= config.PriceSearch)
                {
                    var recive = new ReceiveHistory
                    {
                        CustomerID = cusId,
                        Price = config.PriceSearch,
                        IsActive = true,
                        Query = query,
                        DateCreate = DateTime.Now.TotalSeconds(),
                        Type = 1,
                    };
                    _rewardHistoryDa.Add(recive);
                    _rewardHistoryDa.Save();
                }
                else
                {
                    if (totalwalet.TotalWallets > 0 && totalwalet.TotalWallets >= config.PriceSearch)
                    {
                        var recive = new CashOutWallet()
                        {
                            CustomerID = cusId,
                            TotalPrice = config.PriceSearch,
                            DateCreate = DateTime.Now.TotalSeconds(),
                            Type = 3,
                            Query = query,
                        };
                        _cashOutWalletDa.Add(recive);
                        _cashOutWalletDa.Save();
                    }
                    else
                    {
                        return Json(new BaseResponse<List<ProductItem>> { Code = 1, Message = "Ví tiền của bạn không đủ.!" }, JsonRequestBehavior.AllowGet);

                    }
                }
                return Json(new BaseResponse<List<ProductItem>> { Code = 200, Message = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new BaseResponse<List<ProductItem>> { Code = -1, Message = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> AddComment(RatingAppIG4Item data)
        {
            var comment = customerDA.GetComment(data.ShopId, CustomerId);
            if (comment != null)
            {
                comment.TypeRating = data.TypeRating;
                comment.Title = data.Title;
                comment.Comment = data.Comment;
                comment.DateCreated = DateTime.Now;
                customerDA.Save();
                CaculateRating(data);
                return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
            }
            List<Gallery_Picture> imgs = new List<Gallery_Picture>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var img = await UploadImage(file);
                if (!img.Erros)
                {
                    var picture = new Gallery_Picture
                    {
                        Type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0,

                        Folder = img.Data.Folder,
                        Name = img.Data.Name,
                        DateCreated = DateTime.Now.TotalSeconds(),
                        IsShow = true,
                        Url = img.Data.Url,
                        IsDeleted = false,
                    };
                    imgs.Add(picture);
                }

            }
            var item = new CustomerRating
            {
                ShopId = data.ShopId,
                CustomerId = CustomerId,
                TypeRating = data.TypeRating,
                DateCreated = DateTime.Now,
                Gallery_Picture = imgs,
                Title = data.Title,
                Comment = data.Comment
            };
            customerDA.AddComment(item);
            CaculateRating(data);
            customerDA.Save();
            return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCommentRatings(int id)
        {
            var data = customerDA.GetCommentRatings(id);
            return Json(new BaseResponse<List<RatingAppIG4Item>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }

        private void CaculateRating(RatingAppIG4Item data)
        {
            var rating = customerDA.GetTotalRating(data.ShopId);
            var customer = customerDA.GetById(data.ShopId);
            if (customer != null)
            {
                customer.Ratings = rating.Sum(m => m.Quantity);
                var totalrating = rating.Sum(m => m.Quantity * m.TypeRating);
                if (customer.Ratings != 0) customer.AvgRating = totalrating / customer.Ratings;
                else customer.AvgRating = 0;
                customerDA.Save();
            }
        }
        [AllowAnonymous]
        public ActionResult StaticChartShop(int shopId, int type, DateTime date, int cateId = 0)
        {
            var model = new List<ListOrderShopChartAppIG4Item>();
            if (type == 1)
            {
                model = customerDA.GetStaticChartsShop(date.Year, 0, 0, date, shopId, cateId);
            }
            if (type == 2)
            {
                model = customerDA.GetStaticChartsShop(0, date.Month, 0, date, shopId, cateId);
            }
            if (type == 3)
            {
                model = customerDA.GetStaticChartsShop(0, 0, 1, date, shopId, cateId);
            }
            return Json(new BaseResponse<List<ListOrderShopChartAppIG4Item>>() { Code = 200, Data = model }, JsonRequestBehavior.AllowGet);
        }

    }
}
