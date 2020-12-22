using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FDI.CORE
{
   public static class BoolExtensions
    {
        public static bool IsNotDelete(this bool? isDelete)
        {
            return isDelete == null ||  isDelete.Value == false;
        }
    }
}
