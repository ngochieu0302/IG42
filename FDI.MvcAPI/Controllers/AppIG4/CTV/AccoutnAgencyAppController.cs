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
    public class AccoutnAgencyAppController : BaseAppApiController
    {
        TokenOtpDA tokenOtpDA = new TokenOtpDA();
        CustomerAppIG4DA customerDA = new CustomerAppIG4DA();
        AgencyDA _agencyDa = new AgencyDA("#");
        readonly WalletCustomerAppIG4DA _walletCustomerDa = new WalletCustomerAppIG4DA("#");
        readonly CustomerAppIG4DA _customerApp = new CustomerAppIG4DA("");

        readonly  CustomerRewardDA _customerRewardApp = new CustomerRewardDA();
        public ActionResult GetProfile()
        {
            var obj = _agencyDa.GetItemById(CustomerId);
            return Json(new BaseResponse<AgencyItem> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> Login(string phone)
        {
            try
            {
                //if (!_agencyDa.CheckExitsByPhone(phone))
                //{
                //    _agencyDa.Add(new DN_Agency()
                //    {
                //        Phone = phone,
                //        IsDelete = false,
                //        CreateDate = DateTime.Now.TotalSeconds(),
                //    });
                //    customerDA.Save();
                //}
                var otp = FDIUtils.RandomOtp(4);
                var otppost = new PostOtpLoginAppIG4()
                {
                    msisdn = phone.Remove(0, 1).Insert(0, "84"),
                    brandname = "G-STORE",
                    msgbody = "IG4: Ma xac minh cua ban la " + otp,
                    user = "G-STORE",
                    pass = "GSTORE123",
                    charset = "8"
                };
                var url = "http://123.31.20.167:8383/restservice/";
                var result = await PostDataAsync<List<ResultotpAppIG4>>(url, otppost);
                if (result.FirstOrDefault()?.Result.code == "200")
                {
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
                }
                else
                {
                    return Json(new JsonMessage(-1, "Gửi mã OTP thất bại"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(-2, e.ToString()), JsonRequestBehavior.AllowGet);

            }

            return Json(new JsonMessage(200, "Ok"), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public async Task<ActionResult> CheckPhoneRegister(string phone)
        {
            try
            {
                if (!_agencyDa.CheckExitsByPhone(phone,0))
                {
                    var model = new DN_Agency
                    {
                        Phone = phone,
                        IsDelete = false,
                        CreateDate = DateTime.Now.TotalSeconds(),
                    };
                    _agencyDa.Add(model);
                    customerDA.Save();
                }
                //var otp = FDIUtils.RandomOtp(4);
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
                //    tokenOtpDA.Add(new TokenOtp()
                //    {
                //        ObjectId = phone,
                //        Token = otp,
                //        IsDeleted = false,
                //        IsUsed = false,
                //        TypeToken = (int)TokenOtpType.Authen,
                //        DateCreated = DateTime.Now,
                //    });
                //    tokenOtpDA.Save();
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

        [AllowAnonymous]
        public async Task<ActionResult> ValidateToken(string token, string phone, string tokenDevice)
        {
            //if (!tokenOtpDA.ValidateToken(token, phone, (int)TokenOtpType.Authen))
            //{
            //    return Json(new JsonMessage(1000, "Thông tin đăng nhập không hợp lệ"), JsonRequestBehavior.AllowGet);
            //}
            //tokenOtpDA.UpdateIsUsed(token, phone);
            //await tokenOtpDA.SaveAsync();
            var customer = _agencyDa.GetByPhone(phone);
            var key = Guid.NewGuid();
            IAuthContainerModel model = new JWTContainerModel()
            {
                Claims = new Claim[]
                       {
                            new Claim("Phone", customer.Phone),
                            new Claim("Type", "Token"),
                            new Claim("ID",customer.ID.ToString()),
                       },
                ExpireMinutes = 10,
            };
            IAuthContainerModel modelRefreshToken = new JWTContainerModel()
            {
                Claims = new Claim[]
                       {
                            new Claim("Phone", customer.Phone),
                            new Claim("Type", "RefreshToken"),
                            new Claim("key", key.ToString()),
                            new Claim("ID",customer.ID.ToString()),
                       },
                ExpireMinutes = 60 * 24 * 30,
            };
            var tokenResponse = JWTService.Instance.GenerateToken(model);
            var refreshToken = JWTService.Instance.GenerateToken(modelRefreshToken);
            _agencyDa.InsertToken(new TokenRefresh() { GuidId = key });
            customer.TokenDevice = tokenDevice;
            _agencyDa.Save();
            return Json(new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshToken, ID = customer.ID, IsPrestige = customer.IsFdi,IsVerify = customer.IsVerify,IsBank = customer.IsBank,IsActive = customer.IsActive} }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
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

            var oldToke = _agencyDa.GetTokenByGuidId(key);
            if (oldToke == null)
            {
                return Json(new JsonMessage(1000, "invalid token"));
            }
            _agencyDa.DeleteTokenRefresh(oldToke);
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
            _agencyDa.InsertToken(new TokenRefresh() { GuidId = key });
            _agencyDa.Save();

            return Json(new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshTokenResponse } }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<ActionResult> UpdateAcount(CustomerAppIG4Item data)
        {
            var customer = _agencyDa.GetById(CustomerId);
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
            //customer.Description = data.Description;
            customer.FullName = data.Fullname;
            customer.Address = data.Address;
            //customer.Mobile = data.Mobile;

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
            var obj = _agencyDa.GetItemByIdApp(CustomerId);
            return Json(new BaseResponse<CustomerAppIG4Item> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        
        public async Task<ActionResult> UpdateAcountRegister(CustomerAppIG4Item data)
        {
            var customer = _agencyDa.GetById(CustomerId);
            if (customer == null)
            {
                return Json(new JsonMessage(1000, "Not found"));
            }

            if (!string.IsNullOrEmpty(data.Mobile) && customerDA.CheckExitsByPhone(data.Mobile, CustomerId))
            {
                return Json(new JsonMessage(1001, "Số điện thoại đã tồn tại"));
            }
            customer.Email = data.Email;
            if (data.ParentID != null)
            {
                var item = _agencyDa.GetById(data.ParentID ?? 0);
                if (item != null)
                {
                    if (string.IsNullOrEmpty(item.ListID)) customer.ListID = item.ID.ToString();
                    else customer.ListID = item.ListID + "," + item.ID;
                    customer.ParentID = item.ID;
                    customer.Level = item.Level + 1;
                }
                else customer.Level = 1;
            }
            //customer.Description = data.Description;
            customer.FullName = data.Fullname;
            customer.Address = data.Address;
            //customer.Mobile = data.Mobile;
            var images = new List<Gallery_Picture>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var img = await UploadImage(i);
                if (img.Code != 200)
                {
                    return Json(img);
                }
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
                images.Add(picture);
            }
            if (images.Count == 0)
            {
                return Json(new JsonMessage(1000, "Ảnh xác minh không được để trống."));
            }
            customer.Gallery_Picture = images;
            customer.IsVerify = true;
            _agencyDa.Save();
            var obj = _agencyDa.GetItemByIdApp(CustomerId);
            return Json(new BaseResponse<CustomerAppIG4Item> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult UpdateAcountRegisterStep2(CustomerAppIG4Item data)
        {
            var customer = _agencyDa.GetById(CustomerId);
            if (customer == null)
            {
                return Json(new JsonMessage(1000, "Not found"));
            }

            if (string.IsNullOrEmpty(data.STK))
            {
                return Json(new JsonMessage(-1, "Số tài khoản không được để trống"));
            }

            if (string.IsNullOrEmpty(data.Bankname))
            {
                return Json(new JsonMessage(-2, "Tên ngân hàng không được để trống"));
            }
            customer.BankName = data.Bankname;
            customer.STK = data.STK;
            customer.Branchname = data.Branchname;
            customer.FullnameBank = data.FullnameBank;
            customer.IsFdi = true;
            customer.IsBank = true;
            _agencyDa.Save();
            var obj = _agencyDa.GetItemByIdApp(data.ID);
            return Json(new BaseResponse<CustomerAppIG4Item> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckTranfer(string phone, decimal total)
        {
            try
            {
                var model = _agencyDa.GetByPhone(phone);
                if (model != null)
                {
                    var config = _walletCustomerDa.GetConfig();
                    if (total == config.Price)
                    {
                        model.IsActive = true;
                    }
                    _agencyDa.Save();
                }
                else
                {
                    return Json(new JsonMessage(-1, "Số điện thoại không tồn tại"));
                }
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(404, e.ToString()));

            }
            return Json(new JsonMessage(200, ""));
        }
        [AllowAnonymous]
        private async Task<BaseResponse<GalleryPictureItem>> UploadImage(int i)
        {
            var file = Request.Files[i];
            using (HttpClient client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    byte[] fileBytes = new byte[file.InputStream.Length + 1]; file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                    content.Add(fileContent);
                    var result = await client.PostAsync("http://imgg.wini.vn/home/upload", content);
                    if (result.IsSuccessStatusCode)
                    {
                        var datas = await result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<BaseResponse<GalleryPictureItem>>(datas);
                    }
                    return new BaseResponse<GalleryPictureItem>() { Code = 1000 };
                }
            }
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

        public  ActionResult GetListCustomer(int page, int total)
        {
            var model = _customerApp.GetListCustomerbyAgencyId(CustomerId, page, total);
            return Json(new BaseResponse<List<CustomerAppIG4Item>> {Code = 200, Data = model});
        }
        public  ActionResult GetListAgency(int page, int total)
        {
            var model = _customerApp.GetListAgencyCustomerbyAgencyId(CustomerId, page, total);
            return Json(new BaseResponse<List<CustomerAppIG4Item>> { Code = 200, Data = model });
        }
        public ActionResult GetListSouce(int page, int total)
        {
            var model = _customerApp.GetListSouceCustomerbyAgencyId(CustomerId, page, total);
            return Json(new BaseResponse<List<CustomerAppIG4Item>> { Code = 200, Data = model });
        }

        public ActionResult GetWallet()
        {
            var model = _customerRewardApp.GetWallet(CustomerId);
            return Json(new BaseResponse<CustomerRewardAppIG4Item> {Code = 200, Data = model});
        }

        public ActionResult StaticWalletTotal(decimal? from, decimal? to)
        {

            return Json(new BaseResponse<CustomerRewardAppIG4Item>());
        }

    }
}
