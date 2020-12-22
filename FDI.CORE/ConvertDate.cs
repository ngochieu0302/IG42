using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.CORE
{
    public static class ConvertDate
    {
        /// <summary>
        /// Mốc ngày FDITECH
        /// </summary>
        public static DateTime DateDefault = new DateTime(2019, 1, 1);
        /// <summary>
        /// dongdt 22/11/2017
        /// </summary>
        /// <param name="date">Ngày trong tuần</param>
        /// <param name="weekEnd">Đầu tuần sau</param>
        /// <returns>Ngày đầu tuần</returns>
        public static decimal WeekStart(this DateTime date, out decimal weekEnd)
        {
            var start = date.AddDays(1 - date.Thu());
            weekEnd = start.AddDays(7).TotalSeconds();
            return start.TotalSeconds();
        }

        /// <summary>
        /// dongdt 22/11/2017
        /// </summary>
        /// <param name="date">Ngày trong tháng</param>
        /// <param name="monthEnd">Đầu tháng sau</param>
        /// <returns>Ngày đầu tháng</returns>
        public static decimal MonthStart(this DateTime date, out decimal monthEnd)
        {
            var start = new DateTime(date.Year, date.Month, 1);
            monthEnd = start.AddMonths(1).TotalSeconds();
            return start.TotalSeconds();
        }

        /// <summary>
        /// dongdt 22/11/2017
        /// </summary>
        /// <param name="date">Ngày trong tháng</param>
        /// <param name="yearEnd">Đầu năm sau</param>
        /// <returns>Ngày đầu tháng</returns>
        public static decimal YearStart(this DateTime date, out decimal yearEnd)
        {
            var start = new DateTime(date.Year, 1, 1);
            yearEnd = start.AddYears(1).TotalSeconds();
            return start.TotalSeconds();
        }

        /// <summary>
        /// khanhnq 10/2017
        /// </summary>
        /// <param name="date">yyyy-MM-dd Kiểu định dạng của text type="date"</param>
        /// <returns>DateTime</returns>
        public static DateTime StringToDate(this string date)
        {
            var retVal = DateTime.Today;
            try
            {
                if (!string.IsNullOrEmpty(date)) retVal = Convert.ToDateTime(date);
            }
            catch
            {
                retVal = DateTime.Today;
            }
            return retVal;
        }

        public static decimal StringToDecimal(this string str, int number = 0)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            var date = str.StringToDate();
            if (number != 0) date = date.AddDays(number);
            return date.TotalSeconds();
        }

        public static string DecimalToString(this decimal? totalSeconds, string format)
        {
            return totalSeconds != null ? DateDefault.AddSeconds((double)totalSeconds).ToString(format) : "";
        }
        public static string DecimalToString(this decimal totalSeconds, string format)
        {
            return DateDefault.AddSeconds((double)totalSeconds).ToString(format);
        }

        public static DateTime DecimalToDate(this decimal? totalSeconds)
        {
            return totalSeconds != null ? DateDefault.AddSeconds((double)totalSeconds) : DateTime.Now;
        }
        public static DateTime DecimalToDate(this decimal totalSeconds)
        {
            return  DateDefault.AddSeconds((double)totalSeconds);
        }

        public static decimal TotalSeconds(this DateTime dateTime)
        {
            return (int)(dateTime - DateDefault).TotalSeconds;
        }

        /// <summary>
        /// dongdt 22/11/2017
        /// </summary>
        /// <param name="dateTime">Ngày</param>
        /// <returns>Thứ theo chuẩn FdiTech(1-7)</returns>
        public static int Thu(this DateTime dateTime)
        {
            var thu = (int)dateTime.DayOfWeek;
            return thu == 0 ? 7 : thu;
        }

        public static decimal TotalSecondsMonth(int month = 0, int year = 0)
        {
            var datenow = DateTime.Now;
            var epoch = year == 0 ? (month == 0 ? (new DateTime(datenow.Year, datenow.Month, 1) - DateDefault).TotalSeconds
                : (new DateTime(datenow.Year, month, 1) - DateDefault).TotalSeconds) : (new DateTime(year, month, 1) - DateDefault).TotalSeconds;
            return (int)epoch;
        }

    }
}
