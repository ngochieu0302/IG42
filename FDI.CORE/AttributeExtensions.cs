using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.CORE
{
    public class CssAttribute : Attribute
    {
        public CssAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }

        public string GetValue()
        {
            return Value;
        }
    }

    public class OrderAttribute : Attribute
    {
        public int Order { get; set; }
        public OrderAttribute(int value)
        {
            Order = value;
        }

        public int GetValue()
        {
            return Order;
        }
    }
}
