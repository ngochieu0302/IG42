﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FDI.Utils
{
    public class ConvertUtil
    {
        public static int ToInt32(object obj)
        {
            int retVal;
            try
            {
                retVal = Convert.ToInt32(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static List<Guid> LsGuiId(string lguiid)
        {
            var list = new List<Guid>();
            if (!string.IsNullOrEmpty(lguiid))
            {
                if (lguiid.Contains(","))
                {
                    return lguiid.Trim().Split(',').Select(Guid.Parse).ToList();
                }
                list.Add(Guid.Parse(lguiid));
            }
            return list;
        }

        public static string StringToBoolString(string obj)
        {
            var retVal = "0";
            if (obj.ToLower() == "true")
                retVal = "1";
            return retVal;
        }
        public static int ToInt32(object obj, int defaultValue)
        {
            var retVal = defaultValue;
            if (obj == null || obj == DBNull.Value)
                return retVal;
            try
            {
                retVal = Convert.ToInt32(obj);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static double ToDouble(object obj)
        {
            double retVal;

            try
            {
                retVal = Convert.ToDouble(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static long ToLong(object obj)
        {
            long retVal;

            try
            {
                retVal = Convert.ToInt64(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static decimal? ToDecimal(object obj)
        {
            decimal? retVal;

            try
            {
                retVal = Convert.ToDecimal(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            double retVal = 0;

            if (obj == null || obj == DBNull.Value)
                return retVal;

            try
            {
                retVal = Convert.ToDouble(obj);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static string ToString(object obj)
        {
            string retVal;

            try
            {
                retVal = Convert.ToString(obj);
            }
            catch
            {
                retVal = String.Empty;
            }

            return retVal;
        }

        public static string ToString(object obj, string defaultValue)
        {
            var retVal = String.Empty;

            if (obj == null || obj == DBNull.Value)
                return retVal;

            try
            {
                retVal = Convert.ToString(obj);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        //public static DateTime ToDate(object obj)
        //{
        //    var retVal = DateTime.Now;
        //    var strArr = obj.ToString().Split(' ');
        //    var lenStrArr = strArr.Length;
        //    try
        //    {
        //        if (lenStrArr >= 1)
        //        {
        //            var str = strArr[0];
        //            var strTemp = str.Split('/');
        //            if (strTemp.Length == 3)
        //            {
        //                var t = string.Empty;
        //                if (lenStrArr == 2)
        //                {
        //                    t = strArr[1];
        //                }
        //                var input = string.Format("{0}-{1}-{2} {3}", strTemp[2], strTemp[1], strTemp[0], t);
        //                retVal = Convert.ToDateTime(input);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        retVal = DateTime.Now;
        //    }
        //    return retVal;
        //}

        public static bool CheckDateTime()
        {
            try
            {

                Convert.ToDateTime("25-12-2015");
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public static bool CheckDateTimeddmmyyyy()
        {
            try
            {
                Convert.ToDateTime("25-12-2015");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static DateTime ToDateTime(string obj)
        {

            var retVal = DateTime.Now;
            var strArr = obj.Split(' ');
            var lenStrArr = strArr.Length;
            try
            {
                if (CheckDateTime())
                {
                    return Convert.ToDateTime(obj);
                }

                if (lenStrArr >= 1)
                {
                    var str = strArr[0];
                    var strTemp = str.Split('/');
                    if (strTemp.Length == 3)
                    {
                        var t = string.Empty;
                        if (lenStrArr == 2) t = strArr[1];
                        var input = CheckDateTimeddmmyyyy() ? string.Format("{0}-{1}-{2} {3}", strTemp[2], strTemp[1], strTemp[0], t) : string.Format("{0}-{1}-{2} {3}", strTemp[2], strTemp[0], strTemp[1], t);
                        retVal = Convert.ToDateTime(input);
                    }
                    else
                    {
                        strTemp = str.Split('-');
                        if (strTemp.Length == 3)
                        {
                            var t = string.Empty;
                            if (lenStrArr == 2) t = strArr[1];
                            var input = string.Format("{0}-{1}-{2} {3}", strTemp[2], strTemp[1], strTemp[0], t);
                            retVal = Convert.ToDateTime(input);
                        }
                    }
                }
            }
            catch
            {
                if (lenStrArr >= 1)
                {
                    var str = strArr[0];
                    var strTemp = str.Split('/');
                    if (strTemp.Length == 3)
                    {
                        var t = string.Empty;
                        if (lenStrArr == 2)
                        {
                            t = strArr[1];
                        }
                        var input = string.Format("{0}-{2}-{1} {3}", strTemp[2], strTemp[1], strTemp[0], t);

                        retVal = Convert.ToDateTime(input);
                    }
                    else retVal = Convert.ToDateTime(str);
                }
            }
            return retVal;
        }

        private static readonly StringComparison _stringComparison;
        private static string Replace(string original, string pattern, string replacement)
        {
            if (_stringComparison == StringComparison.Ordinal)
            {
                return original.Replace(pattern, replacement);
            }
            int position0, position1;
            var count = position0 = 0;
            var inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = original.IndexOf(pattern, position0, _stringComparison)) != -1)
            {
                for (var i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                foreach (var t in replacement)
                    chars[count++] = t;
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (var i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

    }
}
