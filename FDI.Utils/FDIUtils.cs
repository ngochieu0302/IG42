using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Speech.Recognition;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using FDI.Base;
using FDI.Simple;

namespace FDI.Utils
{
    public static class FDIUtils
    {
        public static List<int> StringToListInt(string source)
        {
            try
            {
                var lst = new List<int>();
                if (string.IsNullOrEmpty(source)) return lst;
                var temp = source.Split(',').ToList();
                lst.AddRange(from item in temp where !string.IsNullOrEmpty(item) select int.Parse(item));
                return lst;
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }
        public static List<string> StringToListString(string source)
        {
            try
            {
                var lst = new List<string>();
                if (string.IsNullOrEmpty(source)) return lst;
                lst = source.Split(',').ToList();
                return lst;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
        public static List<Guid> StringToListGuid(string source)
        {
            try
            {
                var lst = new List<Guid>();
                if (string.IsNullOrEmpty(source)) return lst;
                lst = source.Split(',').Select(Guid.Parse).ToList();
                return lst;
            }
            catch (Exception)
            {
                return new List<Guid>();
            }
        }

        public static List<Guid> StringToListIntGuid(string source, out List<int> listint)
        {
            listint = new List<int>();
            try
            {
                var lst = new List<Guid>();
                var lstint = new List<int>();
                if (string.IsNullOrEmpty(source)) return lst;
                foreach (var item in source.Split(','))
                {
                    if (item.All(char.IsDigit)) lstint.Add(int.Parse(item)); else lst.Add(Guid.Parse(item));
                }
                listint = lstint;
                return lst;
            }
            catch (Exception)
            {
                return new List<Guid>();
            }
        }

        public static bool IsAllDigits(string str)
        {
            return str.All(char.IsDigit);
        }

        public static string FormatBytes(int bytes)
        {
            const int scale = 1024;
            var orders = new[] { "GB", "MB", "KB", "Bytes" };
            var max = (long)Math.Pow(scale, orders.Length - 1);
            foreach (var order in orders)
            {
                if (bytes > max) return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);
                max /= scale;
            }
            return "0 Bytes";
        }

        public static string FormatBytes(double bytes)
        {
            const int scale = 1024;
            var orders = new[] { "GB", "MB", "KB", "Bytes" };
            var max = (long)Math.Pow(scale, orders.Length - 1);
            foreach (var order in orders)
            {
                if (bytes > max) return string.Format("{0:##.##} {1}", decimal.Divide((decimal)bytes, max), order);
                max /= scale;
            }
            return "0 Bytes";
        }

        public static string GetYoutubeId(string source)
        {
            if (source.IndexOf("?v=", StringComparison.Ordinal) < 0)
                return string.Empty;
            var start = source.IndexOf("?v=", StringComparison.Ordinal) + 3;
            var end = source.IndexOf("&", StringComparison.Ordinal) < 0 ? source.Length : source.IndexOf("&", StringComparison.Ordinal);
            return source.Substring(start, end - start);
        }

        public static string GetYoutubeLink(string source)
        {
            if (source.IndexOf("?v=") < 0)
                return string.Empty;
            const string temp = "http://www.youtube.com/v/";
            return temp + GetYoutubeId(source);
        }

        public static string GetPercent(int voteCount, int totalVote)
        {
            try
            {
                var value = ((double)voteCount / totalVote);
                return (value.ToString("0.0%") == "NaN") ? "0" : value.ToString("0.0%");
            }
            catch { return string.Empty; }
        }

        public static string GetDataVoteWidthCss(int voteCount, int totalVote)
        {
            try
            {
                var value = ((double)voteCount / totalVote);
                return (value.ToString("0.0%") == "NaN") ? "0" : value.ToString("0.0%").Replace("%", "");
            }
            catch { return string.Empty; }
        }

        public static string GetSubstringName(string name)
        {
            const int lengthName = 30;
            return name.Length < lengthName ? name : name.Substring(0, lengthName);
        }
        public static string MoneyDouble(this double price)
        {
            var str = price.ToString();
            if (str.Contains(","))
            {
                var a = 0;
                var stringpr = str.Split(',');
                if (stringpr.Count() > 1)
                {
                    for (int i = stringpr[1].Length; i > 0; i--)
                    {
                        if (stringpr[1].Substring(i - 1, 1) != "0")
                        {
                            a = i;
                            break;
                        }
                    }
                    var b1 = int.Parse(stringpr[0]);
                    var b2 = string.Format("{0:0,0}", b1);
                    var temp = b2.StartsWith("0") ? int.Parse(b2).ToString() : b2;
                    var sub = stringpr[1].Substring(0, a);
                    if (!string.IsNullOrEmpty(sub)) temp = temp + "." + sub;
                    return temp;
                }
                else
                {
                    var b2 = string.Format("{0:0,0}", price);
                    var temp = b2.StartsWith("0") ? int.Parse(b2).ToString() : b2;
                    return temp;
                }
            }
            else
            {
                var a = 0;
                var stringpr = str.Split('.');
                if (stringpr.Count() > 1)
                {
                    for (int i = stringpr[1].Length; i > 0; i--)
                    {
                        if (stringpr[1].Substring(i - 1, 1) != "0")
                        {
                            a = i;
                            break;
                        }
                    }
                    var b1 = int.Parse(stringpr[0]);
                    var b2 = string.Format("{0:0,0}", b1);
                    var temp = b2.StartsWith("0") ? int.Parse(b2).ToString() : b2;
                    var sub = stringpr[1].Substring(0, a);
                    if (!string.IsNullOrEmpty(sub)) temp = temp + "." + sub;
                    return temp;
                }
                else
                {
                    var b2 = string.Format("{0:0,0}", price);
                    var temp = b2.StartsWith("0") ? int.Parse(b2).ToString() : b2;
                    return temp;
                }
            }
        }
        /// <summary>
        /// Lấy về danh sách ID
        /// </summary>
        /// <param name="arrId"></param>
        /// <returns></returns>
        public static List<int> GetDanhSachIDsQuaFormPost(string arrId)
        {
            var dsId = new List<int>();
            if (!string.IsNullOrEmpty(arrId)) // Nếu không rỗng
            {
                if (arrId.IndexOf(',') > 0) //nếu nhiều hơn 1
                {
                    var tempIDs = arrId.Split(',');
                    dsId.AddRange(tempIDs.Select(idConvert => Convert.ToInt32(idConvert)));
                }
                else dsId.Add(Convert.ToInt32(arrId));
            }
            return dsId;
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                var length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                var sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
               

        
        public static string RemoveSpecialCharacter(string unicode)
        {
            unicode = Regex.Replace(unicode, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–]", "-", RegexOptions.IgnoreCase);
            return unicode;
        }

        

        /// <summary>
        /// Hàm chuyển một chuỗi tiếng việt có dấu thành tiếng việt không dấu (không bỏ dấu cách) - chuyển dấu,->cách
        /// </summary>
        /// <param name="unicode">xâu tiếng việt có dấu</param>
        public static string UnicodeToEngSearch(string unicode)
        {
            unicode = Regex.Replace(unicode, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[đ|Đ]", "d", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ý|ỳ|ỷ|ỹ|ỵ|Ý|Ỳ|Ỷ|Ỹ|Ỵ]", "y", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ ,]", " ", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[, ]", " ", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[,]", " ", RegexOptions.IgnoreCase);
            return unicode;
        }
        public static string NewUnicodeToAscii(string unicode)
        {
            unicode = Regex.Replace(unicode, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[đ|Đ]", "d", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ý|ỳ|ỷ|ỹ|ỵ|Ý|Ỳ|Ỷ|Ỹ|Ỵ]", "y", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–]", "", RegexOptions.IgnoreCase);
            return unicode;
        }
        public static string Slug(string unicode)
        {
            unicode = NewUnicodeToAscii(unicode);
            unicode = unicode.ToLower().Trim();
            unicode = Regex.Replace(unicode, @"\s+", " ");
            unicode = Regex.Replace(unicode, "[\\s]", "-");
            unicode = Regex.Replace(unicode, @"-+", "-");
            return unicode;
        }
        public static string Format(this DateTime date, string format)
        {
            return date.ToString(format);
        }

        /// <summary>
        /// Hàm tạo Mã SHA1 lấy từ hệ thống cũ
        /// Name		Date		    Comment         
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (string.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            var saltAndPassword = string.Concat(password, saltkey);
            var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            return hashedPassword;
        }
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string saltKey)
        {
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(saltKey, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static string Decrypt(string cipherText, string saltKey)
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();
            using (var password = new Rfc2898DeriveBytes(saltKey, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Hàm tạo random 6 ký tự
        /// DongDT 
        /// 04/03/2014
        /// </summary>
        /// <returns></returns>
        static readonly Random Random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const string Charsnew = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static string RandomKey(int size)
        {
            return new string(Enumerable.Repeat(Chars, size).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static string RandomCode(int size)
        {
            var stringChars = new char[size];

            for (var i = 0; i < size; i++)
            {
                stringChars[i] = Charsnew[Random.Next(Charsnew.Length)];
            }
            var finalString = new string(stringChars);
            var key = finalString;
            return key;
        }

        public static string RandomKey(string email)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[3];
            var emailChars = new char[3];
            var random = new Random();
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            email = email.Replace("@", string.Empty);
            email = email.Replace(".", string.Empty);
            for (var e = 0; e < emailChars.Length; e++)
            {
                emailChars[e] = email[random.Next(email.Length)];
            }
            var emailArrayNews = new String(emailChars);
            var pass = emailArrayNews + finalString + string.Format("{0:mm:ss}", DateTime.Now);
            pass = pass.Replace(":", string.Empty);
            return pass;
        }
        public static string RandomOtp(int size)
        {
            var stringdate = DateTime.Now.ToString("ss");
            const string chars = "0123456789";
            var stringChars = new char[size];
            var random = new Random();
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            var key = finalString + stringdate;
            return key;
        }
        public static string CreateRandom(string name)
        {
            var stringdate = DateTime.Now.ToString("ddMMyy");
            const string chars = "0123456789";
            var stringChars = new char[4];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            var key = stringdate + name + finalString;
            return key;
        }

        public static List<DiscountCode> CreateRandomKey(int leng, int count, string firt, string last)
        {
            leng = leng - firt.Length - last.Length;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[leng];
            var random = new Random();
            var list = new List<DiscountCode>();
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < stringChars.Length; j++)
                {
                    stringChars[j] = chars[random.Next(chars.Length)];
                }
                var finalString = new String(stringChars);
                var ob = new DiscountCode
                {
                    Code = firt + finalString + last,
                    IsComplete = false,
                    IsSMS = false,
                    IsDelete = false
                };
                list.Add(ob);
            }
            return list;
        }
        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GetMd5Sum(string str)
        {
            str = str + "NetPOSa@2016";
            var enc = Encoding.Unicode.GetEncoder();
            var unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(unicodeText);
            var sb = new StringBuilder();
            foreach (byte t in result)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
        public static string Md5ByPhp(string password)
        {
            var textBytes = Encoding.Default.GetBytes(password);
            var cryptHandler = new MD5CryptoServiceProvider();
            var hash = cryptHandler.ComputeHash(textBytes);
            var ret = "";
            foreach (var a in hash)
            {
                if (a < 16)
                    ret += "0" + a.ToString("x");
                else
                    ret += a.ToString("x");
            }
            return ret;
        }
        public static string RemoveHtmlTag(string source)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                return Regex.Replace(source, "<.*?>", "");
            }
            return string.Empty;
        }
        public static string RemoveAscii(string source)
        {
            return Regex.Replace(RemoveHtmlTag(source), @"\t*\n*\r*\s*", "");
        }
        public static string GetSmallHightlightsShortDes(string hightlightsDes)
        {
            var result = hightlightsDes;
            if (!string.IsNullOrEmpty(hightlightsDes))
            {
                var regex = new Regex(@"<\s*li[^>]*>(.*?)<\s*/li\s*>", RegexOptions.IgnoreCase);
                var collection = regex.Matches(Regex.Replace(hightlightsDes, @"\t*\n*\r*", ""));
                if (collection.Count > 0)
                {
                    result = "<div class='hightlight-des'><ul>";
                    for (var i = 0; i < collection.Count; i++)
                    {
                        if (i < 3)
                        {
                            result += "<li>" + collection[i].Groups[1].Value + "</li>";
                        }
                    }
                    result += "</ul></div>";
                }
            }

            return result;
        }
        public static string ReplaceSuggestKeyword(string source, string keyword)
        {
            if (string.IsNullOrEmpty(source) == false)
            {
                source = source.ToLower();
                keyword = keyword.ToLower();
                return source.Replace(keyword, "<b>" + keyword + "</b>");
            }
            return string.Empty;
        }
        public static string GetFileExtend(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
        }
        public static string GetFileNameNoneExtent(string fileName)
        {
            return fileName.Substring(0, fileName.LastIndexOf(".", StringComparison.Ordinal));
        }
        public static string GetHtmlAuToTag(string containerName, List<ListItem> valuesSelected)
        {
            var stbBuilder = new StringBuilder();
            stbBuilder.AppendFormat("					<input type=\"hidden\" name=\"{0}\" id=\"{0}\"/>", containerName);
            stbBuilder.AppendFormat("                    <input type=\"text\" name=\"{0}_Input\" id=\"{0}_Input\" value=\"Nhập vào từ khóa để tìm kiếm, nhấn enter để xác nhận !\" onblur=\"if(this.value=='') this.value='Nhập vào từ khóa để tìm kiếm, nhấn enter để xác nhận !';\" onfocus=\"if(this.value=='Nhập vào từ khóa để tìm kiếm, nhấn enter để xác nhận !') this.value='';\" />", containerName);
            stbBuilder.AppendFormat("                    <ul id=\"{0}_Value\" class=\"{0} listValues\">", containerName);

            foreach (var item in valuesSelected)
            {
                stbBuilder.AppendFormat("                            <li id=\"{0}_Value{1}\" name=\"{1}\" key=\"\">", containerName, item.ID);
                stbBuilder.AppendFormat("                                <span>{0}</span><a href=\"javascript:deletevalues('{1}_Value{2}');\">", item.Title, containerName, item.ID);
                stbBuilder.AppendFormat("                                <img border=\"0\" src=\"/Content/Admin/images/gridview/act_filedelete.png\" /></a>");
                stbBuilder.AppendFormat("                            </li>");
            }
            stbBuilder.AppendFormat("                    </ul>");
            return stbBuilder.ToString();
        }
        
        public static string RandomMaKh(string txt, int? count, int length)
        {
            var zero = "00000000000000" + count;
            return txt + zero.Substring(zero.Length - length);
        }

        public static List<Guid> ConvertStringToGuids(string strId)
        {
            var lst = new List<Guid>();
            if (string.IsNullOrEmpty(strId)) return lst;
            lst = strId.Split(',').Select(Guid.Parse).ToList();
            return lst;
        }

        public static int FdiDayOfWeek(this DateTime date)
        {
            var t = (int)date.DayOfWeek;
            if (t == 0) return 7;
            return t;
        }

        public static List<WeekItem> ListWeekByYear(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);
            var currentCulture = CultureInfo.CurrentCulture;
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var result = new List<WeekItem>();
            var start = startDate.AddDays(-startDate.FdiDayOfWeek());
            for (var tm = startDate; tm <= endDate; tm = tm.AddDays(7))
            {
                if (dfi != null)
                {
                    var id = currentCulture.Calendar.GetWeekOfYear(tm, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                    var w = start.AddDays(7 * (id - 1));
                    var mon = Enumerable.Range(1, 7).Select(num => w.AddDays(num)).ToArray();
                    var obj = new WeekItem
                    {
                        ID = id,
                        MonToSun = mon,
                        MonToSunView = mon[0].ToString("dd/MM") + "-" + mon[6].ToString("dd/MM/yyyy")
                    };
                    result.Add(obj);
                }
            }
            return result;
        }

        public static int GetWeekNumber(DateTime date)
        {
            var myCi = new CultureInfo("en-US");
            var myCal = myCi.Calendar;
            var myCwr = myCi.DateTimeFormat.CalendarWeekRule;
            var myFirstDow = myCi.DateTimeFormat.FirstDayOfWeek;
            var numberWeek = myCal.GetWeekOfYear(date, myCwr, myFirstDow);
            var start = new DateTime(date.Year, 1, 1);
            if (start.FdiDayOfWeek() == 7) return numberWeek + 1;
            return numberWeek;
        }

        public static DateTime[] WeekDays(int year, int weekNumber)
        {
            var start = new DateTime(year, 1, 1);
            start = start.AddDays(-start.FdiDayOfWeek());
            start = start.AddDays(7 * (weekNumber - 1));
            return Enumerable.Range(1, 7).Select(num => start.AddDays(num)).ToArray();
        }

        // Hàm đọc số thành chữ
        public static string NumberToWord(this decimal? soTien, string strTail)
        {
            if (soTien == null) return "";
            string[] tien = { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
            int lan, i;
            var ketQua = "";
            var viTri = new int[6];
            if (soTien < 0) return "Số tiền âm !";
            if (soTien == 0) return "Không đồng !";
            decimal so = Math.Abs(soTien ?? 0);
            //Kiểm tra số quá lớn
            if (soTien > 8999999999999999) return "";
            viTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(viTri[5].ToString()) * 1000000000000000;
            viTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(viTri[4].ToString()) * +1000000000000;
            viTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(viTri[3].ToString()) * 1000000000;
            viTri[2] = (int)(so / 1000000);
            viTri[1] = (int)((so % 1000000) / 1000);
            viTri[0] = (int)(so % 1000);
            if (viTri[5] > 0) lan = 5;
            else if (viTri[4] > 0) lan = 4;
            else if (viTri[3] > 0) lan = 3;
            else if (viTri[2] > 0) lan = 2;
            else if (viTri[1] > 0) lan = 1;
            else lan = 0;
            for (i = lan; i >= 0; i--)
            {
                var tmp = ReadNumber(viTri[i]);
                ketQua += tmp;
                if (viTri[i] != 0) ketQua += tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) ketQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (ketQua.Substring(ketQua.Length - 1, 1) == ",") ketQua = ketQua.Substring(0, ketQua.Length - 1);
            ketQua = ketQua.Trim() + strTail;
            return ketQua.Substring(0, 1).ToUpper() + ketQua.Substring(1);
        }
        // Hàm đọc số có 3 chữ số
        private static string ReadNumber(int baso)
        {
            string[] chuSo = { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bẩy", " tám", " chín" };
            var ketQua = "";
            var tram = baso / 100;
            var chuc = (baso % 100) / 10;
            var donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                ketQua += chuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) ketQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                ketQua += chuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) ketQua = ketQua + " linh";
            }
            if (chuc == 1) ketQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1)) ketQua += " mốt";
                    else ketQua += chuSo[donvi];
                    break;
                case 5:
                    if (chuc == 0) ketQua += chuSo[donvi];
                    else ketQua += " lăm";
                    break;
                default:
                    if (donvi != 0) ketQua += chuSo[donvi];
                    break;
            }
            return ketQua;
        }

        public static string AudioToWord()
        {
            var sre = new SpeechRecognitionEngine();
            Grammar gr = new DictationGrammar();
            sre.LoadGrammar(gr);
            sre.SetInputToWaveFile(@"c:\temp\apls.wav");
            sre.BabbleTimeout = new TimeSpan(Int32.MaxValue);
            sre.InitialSilenceTimeout = new TimeSpan(Int32.MaxValue);
            sre.EndSilenceTimeout = new TimeSpan(100000000);
            sre.EndSilenceTimeoutAmbiguous = new TimeSpan(100000000);
            var sb = new StringBuilder();
            while (true)
            {
                try
                {
                    var recText = sre.Recognize();
                    if (recText == null) break;
                    sb.Append(recText.Text);
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            return sb.ToString();
        }
        public static List<KeyValuePair<int, string>> ListtypePromotion()
        {
            var dictonary = new Dictionary<int, string>()
        {
            {1,"Khuyến mãi theo sản phẩm"},
            {2,"Khuyến mãi theo tổng đơn hàng"},
        };
            return dictonary.ToList();
        }
    }
}
