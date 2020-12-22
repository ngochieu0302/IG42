using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FDI.CORE
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var tmp = enumValue.GetType()
                .GetMember(enumValue.ToString()).FirstOrDefault();
            if (tmp == null)
            {
                return "";
            }

            return tmp.GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }
        public static string GetCssClass(this Enum enumValue)
        {
            var tmp = enumValue.GetType()
                .GetMember(enumValue.ToString()).FirstOrDefault();
            if (tmp == null)
            {
                return "";
            }

            return tmp.GetCustomAttribute<CssAttribute>()
                .GetValue();
        }
        public static int GetOrder(this Enum enumValue)
        {
            var tmp = enumValue.GetType()
                .GetMember(enumValue.ToString()).FirstOrDefault();
            if (tmp == null)
            {
                return 0;
            }

            return tmp.GetCustomAttribute<OrderAttribute>()
                .GetValue();
        }

    }
}
