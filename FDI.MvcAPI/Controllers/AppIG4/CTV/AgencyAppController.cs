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
using Microsoft.Ajax.Utilities;
using FDI.CORE;
using Newtonsoft.Json.Linq;
using ZaloDotNetSDK;

namespace FDI.MvcAPI.Controllers
{
    [CustomerAuthorize]
    public class AgencyAppController : BaseAppApiController
    {
        TokenOtpDA tokenOtpDA = new TokenOtpDA();
        CustomerAppIG4DA customerDA = new CustomerAppIG4DA();
        AgencyDA _agencyDa = new AgencyDA("#");
        OrderAppIG4DA orderDA = new OrderAppIG4DA();
        CustomerAddressAppIG4DA customerAddressDA = new CustomerAddressAppIG4DA();
        readonly WalletCustomerAppIG4DA _walletCustomerDa = new WalletCustomerAppIG4DA("#");
        readonly WalletsAppIG4DA _walletsDa = new WalletsAppIG4DA("#");
        readonly CustomerPolicyAppIG4DA _customerPolicyDa = new CustomerPolicyAppIG4DA("#");
        readonly CashOutWalletAppIG4DA _cashOutWalletDa = new CashOutWalletAppIG4DA("#");
        readonly RewardHistoryAppIG4DA _rewardHistoryDa = new RewardHistoryAppIG4DA("#");
        readonly CustomerRewardAppIG4DA _customerRewardDa = new CustomerRewardAppIG4DA();

        public ActionResult GetProfile()
        {
            var obj = _agencyDa.GetItemById(CustomerId);
            return Json(new BaseResponse<AgencyItem> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        //#region Wallets customer

        //public ActionResult AddWallets(WalletCustomerAppIG4Item data)
        //{
        //    try
        //    {
        //        var wallet = new WalletCustomer
        //        {
        //            CustomerID = data.CustomerID,
        //            TotalPrice = data.Totalprice ?? 0,
        //            DateCreate = DateTime.Now.TotalSeconds(),
        //            IsDelete = false,
        //            Note = data.Note,
        //            IsActive = true,
        //            Transaction_no = data.TransactionNo,
        //        };
        //        _walletCustomerDa.Add(wallet);
        //        _walletCustomerDa.Save();
        //        UpdateLevelCustomer(data.CustomerID ?? 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new JsonMessage(404, ex.ToString()), JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        //}


        //[AllowAnonymous]
        //public ActionResult GetListWalletsHistory(int customerId, int type, int page, int take)
        //{
        //    var list = _walletCustomerDa.GetListWalletCustomerbyId(customerId);
        //    var listall = list.Select(item => new WalletsAppAppIG4Item
        //    {
        //        ID = item.ID,
        //        Name = item.TypeWalet == 1 ? "Giao dịch nạp tiền vào G-Store" : "Đơn bán thành công cho khách hàng",
        //        Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
        //        Price = item.Totalprice,
        //        Type = 1
        //    })
        //        .ToList();
        //    var listcash = _cashOutWalletDa.GetListbyCustomer(customerId);
        //    listall.AddRange(listcash.Select(item => new WalletsAppAppIG4Item
        //    {
        //        ID = item.ID,
        //        Name = item.TypeCash == 1 ? "Giao dịch mua hàng từ G-Store" : item.TypeCash == 2 ? "Giao dịch mua gói dịch vụ từ G-Store" : item.Query,
        //        Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
        //        Price = item.Total,
        //        Type = 2
        //    }));
        //    listall = listall.Where(c => (c.Type == type || type == 0)).OrderByDescending(c => c.ID).ToList();
        //    listall = listall.Skip((page - 1) * take).Take(take).ToList();
        //    return Json(new BaseResponse<List<WalletsAppAppIG4Item>>() { Code = 200, Data = listall, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        //}
        //[AllowAnonymous]
        //public ActionResult GetTotalWallets(int customerId)
        //{
        //    decimal? totalw = 0;
        //    var total = _walletsDa.GetWalletsItemById(customerId);
        //    if (total != null)
        //    {
        //        totalw = total.Wallets;
        //    }
        //    return Json(new BaseResponse<decimal>() { Code = 200, Data = totalw ?? 0, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        //}
        //[AllowAnonymous]
        //public ActionResult GetListWalletsRewardHistory(int customerId, int type, int page, int take)
        //{
        //    var list = _walletCustomerDa.GetListWalletReward(customerId);
        //    var listall = list.Select(item => new WalletsAppAppIG4Item
        //    {
        //        ID = item.ID,
        //        Name = item.Type == (int)Reward.Dep ? "Thưởng nạp ví chính" : "Thưởng " + item.BonustypeName,
        //        Price = item.Price,
        //        Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
        //        Type = 1
        //    })
        //        .ToList();
        //    var listrecive = _walletCustomerDa.GetListWalletRecive(customerId);
        //    listall.AddRange(listrecive.Select(item => new WalletsAppAppIG4Item
        //    {
        //        ID = item.ID,
        //        Name = item.Query,
        //        Price = item.Price,
        //        Date = item.DateCreate.DecimalToString("dd/MM/yyyy hh:mm:ss"),
        //        Type = 2
        //    }));
        //    listall = listall.Where(c => (type == 0 || c.Type == type)).OrderByDescending(c => c.ID).ToList();
        //    listall = listall.Skip((page - 1) * page).Take(take).ToList();
        //    return Json(new BaseResponse<List<WalletsAppAppIG4Item>>() { Code = 200, Data = listall, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetTotalWalletsReward(int customerId)
        //{
        //    decimal? totalw = 0;
        //    var total = _walletsDa.GetWalletsRewardByCusId(customerId);
        //    if (total != null)
        //    {
        //        totalw = total.Total;
        //    }
        //    return Json(new BaseResponse<decimal>() { Code = 200, Data = totalw ?? 0, Erros = false, Message = "" }, JsonRequestBehavior.AllowGet);
        //}
        //#endregion
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
        public async Task<ActionResult> CheckPhoneRegister(string phone)
        {
            try
            {
                if (!_agencyDa.CheckExitsByPhone(phone))
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
        public async Task<ActionResult> ValidateToken(string token, string phone, string tokenDevice)
        {
            if (!tokenOtpDA.ValidateToken(token, phone, (int)TokenOtpType.Authen))
            {
                return Json(new JsonMessage(1000, "Thông tin đăng nhập không hợp lệ"), JsonRequestBehavior.AllowGet);
            }
            tokenOtpDA.UpdateIsUsed(token, phone);
            await tokenOtpDA.SaveAsync();
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
            return Json(new BaseResponse<CustomerAppIG4Item>() { Code = 200, Erros = false, Message = "", Data = new CustomerAppIG4Item() { Token = tokenResponse, RefreshToken = refreshToken,ID = customer.ID,IsPrestige = customer.IsFdi} }, JsonRequestBehavior.AllowGet);
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
            var customer = _agencyDa.GetById(data.ID);
            if (customer == null)
            {
                return Json(new JsonMessage(1000, "Not found"));
            }

            if (!string.IsNullOrEmpty(data.Mobile) && customerDA.CheckExitsByPhone(data.ID, data.Mobile))
            {
                return Json(new JsonMessage(1001, "Số điện thoại đã tồn tại"));
            }

            if (!string.IsNullOrEmpty(data.Email) && customerDA.CheckExitsByEmail(data.ID, data.Email))
            {
                return Json(new JsonMessage(1002, "Email đã tồn tại"));
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
            customerDA.Save();
            var obj = _agencyDa.GetItemByIdApp(data.ID);
            return Json(new BaseResponse<CustomerAppIG4Item> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        public  ActionResult UpdateAcountRegisterStep2(CustomerAppIG4Item data)
        {
            var customer = _agencyDa.GetById(data.ID);
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
            customerDA.Save();
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




    }
}
