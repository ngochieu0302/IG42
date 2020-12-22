using System;
using System.Text.RegularExpressions;

namespace FDI.Utils
{
    public static class FomatString
    {
        
        public static string UnicodeToAscii(string unicode)
        {
            unicode = Regex.Replace(unicode, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[đ|Đ]", "d", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[ý|ỳ|ỷ|ỹ|ỵ|Ý|Ỳ|Ỷ|Ỹ|Ỵ]", "y", RegexOptions.IgnoreCase);
            unicode = Regex.Replace(unicode, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–]", "", RegexOptions.IgnoreCase);
            //unicode = Regex.Replace(unicode, "[^A-Za-z0-9-]", "");
            return unicode;
        }
        public static string Slug(string unicode)
        {
            unicode = UnicodeToAscii(unicode);
            unicode = unicode.ToLower().Trim();
            unicode = Regex.Replace(unicode, @"\s+", " ");
            unicode = Regex.Replace(unicode, "[\\s]", "-");
            unicode = Regex.Replace(unicode, @"-+", "-");
            return unicode;
        }

        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) ? value : value.Substring(0, Math.Min(value.Length, maxLength));
        }

        public static string SubString(this string input, int maxLength)
        {
            if (input.Length <= maxLength) return input;
            maxLength -= "...".Length;
            maxLength = input.Length < maxLength ? input.Length : maxLength;
            var isLastSpace = input[maxLength] == ' ';
            var part = input.Substring(0, maxLength);
            if (isLastSpace)
                return part + "...";
            var lastSpaceIndexBeforeMax = part.LastIndexOf(' ');
            if (lastSpaceIndexBeforeMax == -1)
                return part + "...";
            return input.Substring(0, lastSpaceIndexBeforeMax) + "...";
        }
    }
}
