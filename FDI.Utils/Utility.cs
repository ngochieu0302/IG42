using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using FDI.CORE;
using FDI.Simple;
using Newtonsoft.Json;

namespace FDI.Utils
{
    public static class Utility
    {
        public static DateTime DateDefault = new DateTime(2016, 1, 1);
        public static string Domafdi = "http://fditech.vn";
        //public static string DomaApi = "http://gabapi.fditech.vn/";
        public static string DomaApi = "http://localhost:13655/";
        public static string ApiSv = "http://localhost:13655/";
        //public static string DomaApi = "http://gabapi.fditech.vn/";
        //public static string DomaApi = "http://api.ig4.vn/";
        //public static string ApiSv = "http://api.ig4.vn/";
        //public static string _d = "http://localhost:2160/";
        //public static string _dimg = "http://img.ig4.vn/";
        //public static string _dimg = "http://localhost:2160/";
        public static string _d = "http://phanmembanhang.fditech.vn/";
        public static string _dimg = "http://phanmembanhang.fditech.vn/";
        public static string _dvideo = "http://img.ig4.vn/";
        public static int _day = 0;
        public static int AgencyId = 1006;
        private static int _m = -1;
        public static int S = 0;
        public static string TitleEmail;
        public static string ContentEmail;

        public static int GetIntForm(string key)
        {
            try
            {
                return int.Parse(HttpContext.Current.Request.Form[key]);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //var m = distanceBetween2Points(21.029151, 105.787846, 21.029038, 105.788372);
        //var m1 = distanceBetween2Points(20.946786, 105.744546, 21.004453, 105.821892);
        public static double DistanceBetween2Points(double la1, double lo1, double la2, double lo2)
        {
            double dLat = (la2 - la1) * (Math.PI / 180);
            double dLon = (lo2 - lo1) * (Math.PI / 180);
            double la1ToRad = la1 * (Math.PI / 180);
            double la2ToRad = la2 * (Math.PI / 180);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(la1ToRad) * Math.Cos(la2ToRad) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = 6371000 * c;
            return d;
        }
        public static string ImageLogo(this string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName)) return "";
            return WebConfig.ImageLogo + pictureName;
        }
        public static string UploadPicture(this string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName)) return "";
            return WebConfig.UrlUploadsImage + pictureName;
        }
        public static string UploadMailPicture(this string pictureName)
        {
            return (string.IsNullOrEmpty(pictureName)) ? "" : WebConfig.UrlUploadsMailImage + pictureName;
        }
        public static string Picture(this string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName)) return "";
            var list = pictureName.Split('/');
            pictureName = pictureName.Replace(list[0], "");
            return WebConfig.UrlImage + pictureName;
        }
        public static string TempPicture(this string pictureName)
        {
            return WebConfig.TempImage + pictureName;
        }
        public static bool CheckChamTraLoiComment(DateTime ngayGuiComment, DateTime? ngayTraLoi)
        {
            var time = ngayTraLoi.HasValue ? (ngayTraLoi.Value - ngayGuiComment).TotalSeconds : (DateTime.Now - ngayGuiComment).TotalSeconds;
            return time > 900;
        }

        public static string GetPublicIp()
        {
            return GetUrlJson(Domafdi + "/json/IP");
        }
        public static string GetIPnew()
        {
            var context = HttpContext.Current;
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0) return addresses[0];
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public static void SendEmail(string emailSend, string emailSendPwd, string emailReceive, string subject, string content)
        {
            const int portServer = 587;
            const string smtpServer = "smtp.gmail.com";
            try
            {
                var mailMessage = new MailMessage();
                var smtpServerClient = new SmtpClient(smtpServer);
                mailMessage.From = new MailAddress(emailReceive);
                mailMessage.To.Add(emailReceive);
                //mailMessage.Bcc.Add("khanh060992@gmail.com");
                mailMessage.Subject = subject;
                mailMessage.Body = content;
                mailMessage.IsBodyHtml = true;
                smtpServerClient.Port = portServer;
                smtpServerClient.Credentials = new NetworkCredential(emailSend, emailSendPwd);
                smtpServerClient.EnableSsl = true;
                smtpServerClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static string TrimSpecialCharacterPhone(string numberPhone)
        {
            var returnStringNumber = numberPhone.Select((t, i) => numberPhone.ElementAt(i)).Where(c => c != '-').Aggregate("", (current, c) => current + c);
            return returnStringNumber;
        }
        public static string GetLang()
        {
            var url = HttpContext.Current.Request.RawUrl;
            return url.Contains("/en/") ? "en" : "vi";
        }
        public static string Getcookie(string name)
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            var cookie = HttpContext.Current.Request.Cookies[name];
            return cookie != null ? cookie.Value : null;
        }
        public static void Setcookie(string value, string name, int time)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Value = value;
                cookie.Expires = DateTime.Now.AddDays(time);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                var newcookies = new HttpCookie(name) { Value = value, Expires = DateTime.Now.AddDays(time) };
                HttpContext.Current.Response.Cookies.Add(newcookies);
            }
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
        public static T GetObjJson<T>(string url) where T : new()
        {
            var data = new WebClient();
            try
            {
                data.Encoding = Encoding.UTF8;
                var datas = data.DownloadString(url);
                return JsonConvert.DeserializeObject<T>(datas);
            }
            catch (Exception)
            {
                return new T();
            }
        }

        public static decimal MonthTotalSeconds(int month, int year, out decimal datee)
        {
            var datenow = new DateTime(year, month, 1);
            var epoch = (int)(datenow - DateDefault).TotalSeconds;
            datee = (int)(datenow.AddMonths(1) - DateDefault).TotalSeconds;
            return epoch;
        }
        public static string TimeAgo(DateTime dateTime)
        {
            string result;
            var timeSpan = DateTime.Now.Subtract(dateTime);
            if (timeSpan <= TimeSpan.FromSeconds(60)) result = string.Format("{0} giây trước", timeSpan.Seconds);
            else if (timeSpan <= TimeSpan.FromMinutes(60)) result = string.Format("khoảng {0} phút trước", timeSpan.Minutes);
            else if (timeSpan <= TimeSpan.FromHours(24)) result = string.Format("khoảng {0} giờ trước", timeSpan.Hours);
            else if (timeSpan <= TimeSpan.FromDays(30)) result = string.Format("khoảng {0} ngày trước", timeSpan.Days);
            else if (timeSpan <= TimeSpan.FromDays(365)) result = string.Format("khoảng {0} tháng trước", timeSpan.Days / 30);
            else result = string.Format("khoảng {0} năm trước", timeSpan.Days / 365);
            return result;
        }
        public static List<FilesItem> UploadDocument(string fileNameLocal)
        {
            var lst = new List<FilesItem>();
            try
            {
                if (!string.IsNullOrEmpty(fileNameLocal))
                {
                    var arrDocument = fileNameLocal.Split(',');
                    foreach (var item in arrDocument)
                    {
                        var folder = DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day + "\\";
                        var folderinsert = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                        var urlFolder = ConfigData.DocumentFolder + folder;
                        if (!Directory.Exists(urlFolder))
                            Directory.CreateDirectory(urlFolder);
                        if (item.Length > 1)
                        {
                            var fileLocal = item.Split('.');
                            var fileName = fileLocal[0] +"." + fileLocal[1];
                            File.Copy(ConfigData.TempFolder + fileName, urlFolder + fileName);
                            var fileItem = new FilesItem
                            {
                                Folder = folderinsert,
                                FileUrl = fileName,
                                DateCreated = DateTime.Now.TotalSeconds(),
                                TypeFile = fileLocal[1],
                                Status = true,
                                Name = fileName
                            };
                            lst.Add(fileItem);
                        }
                    }
                    return lst;
                }
                return new List<FilesItem>();
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                return new List<FilesItem>();
            }
        }
    }
}