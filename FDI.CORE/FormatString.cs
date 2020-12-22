using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.CORE
{
    public static class FormatString
    {
        public static string Money(this decimal? price)
        {
            return (price ?? 0).MoneyBTC();
        }
        public static string Money(this decimal price)
        {
            return price.MoneyBTC();
        }

        public static string Money(this int? price)
        {
            return price == null ? "0" : string.Format("{0:0,0}", price);
        }
        public static string Money(this int price)
        {
            return string.Format("{0:0,0}", price);
        }
        public static string MoneyBTC(this decimal price)
        {
            var str = price.ToString();
            if (str.Contains(","))
            {
                var a = 0;
                var stringpr = str.Split(',');
                if (stringpr.Length > 1)
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
                if (stringpr.Length > 1)
                {
                    for (int i = stringpr[1].Length; i > 0; i--)
                    {
                        if (stringpr[1].Substring(i - 1, 1) != "0")
                        {
                            a = i;
                            break;
                        }
                    }
                    var b1 = decimal.Parse(stringpr[0]);
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
        public static string Quantity(this decimal? price, string fomat = "0:0.##")
        {
            return price == null ? "0" : string.Format("{" + fomat + "}", price);
        }
        public static string Quantity(this decimal price, string fomat = "0:0.##")
        {
            return string.Format("{" + fomat + "}", price);
        }
    }
}
