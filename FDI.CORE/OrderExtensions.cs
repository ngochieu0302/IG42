using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.CORE
{
    public class OrderExtensions
    {
        public static int GetHourByOrderType(OrderType type)
        {
            switch (type)
            {
              
                case OrderType.TOMORROW:
                    return 0;
                case OrderType.BEFORE12H:
                    return 12;
                case OrderType.BEFORE17H:
                    return 17;
                case OrderType.TWOHOURS:
                    return DateTime.Now.Hour + 2;
            }
            return 1;
        }
        public static int GetDayByOrderType(OrderType type)
        {
            switch (type)
            {
                case OrderType.BEFORE12H:
                case OrderType.BEFORE17H:
                case OrderType.TWOHOURS:
                    return 0;
                case OrderType.TOMORROW:
                    return 1;
            }
            throw new Exception("Order type not support");

        }

    }
}
