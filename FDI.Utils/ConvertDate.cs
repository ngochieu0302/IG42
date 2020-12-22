using System;
using System.Globalization;
using FDI.Simple;
using System.Collections.Generic;
using FDI.CORE;

namespace FDI.Utils
{
    public static class ConvertDateItem
    {
        
       

        /// <summary>
        /// dongdt 22/11/2017
        /// </summary>
        /// <param name="dateTime">Ngày</param>
        /// <returns>Danh sách ngày trong tuần</returns>
        public static List<DayItem> ListDayInWeek(DateTime dateTime)
        {
            var listday = new List<DayItem>();
            var sttd = 0;
            var start = dateTime.AddDays(1 - dateTime.Thu());
            for (var i = start; i < start.AddDays(7); i = i.AddDays(1))
            {
                var obj = new DayItem
                {
                    Item = new DateItem { I = sttd++, S = i.TotalSeconds(), E = i.AddDays(1).TotalSeconds() },
                    Date = i,
                    Thu = i.Thu(),
                };
                listday.Add(obj);
            }
            return listday;
        }
        /// <summary>
        /// dongdt 22/11/2017
        /// </summary>
        /// <param name="dateTime">Ngày</param>
        /// <returns>Danh sách ngày trong tháng</returns>
        public static List<DayItem> ListDayInMonth(DateTime dateTime)
        {
            var listday = new List<DayItem>();
            var sttd = 0;
            for (var i = dateTime; i < dateTime.AddMonths(1); i = i.AddDays(1))
            {
                var obj = new DayItem
                {
                    Item = new DateItem { I = sttd++, S = i.TotalSeconds(), E = i.AddDays(1).TotalSeconds() },
                    Date = i,
                    Thu = i.Thu(),
                };
                listday.Add(obj);
            }
            return listday;
        }
        /// <summary>
        /// dongdt 22/11/2017
        /// </summary>
        /// <param name="year">Năm</param>
        /// <returns>Danh sách tháng trong năm</returns>
        public static List<MonthItem> ListMonthinYear(int year)
        {
            var listday = new List<MonthItem>();
            for (var i = 1; i <= 12; i++)
            {

                var date = new DateTime(year, i, 1);
                var obj = new MonthItem
                {
                    Item = new DateItem { I = i, S = date.TotalSeconds(), E = date.AddMonths(1).TotalSeconds() },
                };
                listday.Add(obj);
            }
            return listday;
        }

        public static decimal TotalSecondsMonth(int month = 0, int year = 0)
        {
            var datenow = DateTime.Now;
            var epoch = year == 0 ? (month == 0 ? (new DateTime(datenow.Year, datenow.Month, 1) - FDI.CORE.ConvertDate.DateDefault).TotalSeconds
                : (new DateTime(datenow.Year, month, 1) - FDI.CORE.ConvertDate.DateDefault).TotalSeconds) : (new DateTime(year, month, 1) - FDI.CORE.ConvertDate.DateDefault).TotalSeconds;
            return (int)epoch;
        }
    }
}