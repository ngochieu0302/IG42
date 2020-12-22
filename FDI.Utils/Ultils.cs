using System;

namespace FDI.Utils
{
    public class Ultils
    {
        public static DateTime DateDefault = new DateTime(2019, 1, 1);
        public static string CodeLogin(DateTime dateTime)
        {
            var code = (dateTime - DateDefault).TotalMilliseconds;
            return code.ToString().Replace(".","").Replace(",","");
        }
    }
}
