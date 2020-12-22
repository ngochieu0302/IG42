using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FDI.Web.Models
{
    public class TranferMoneyModel
    {
        //
        // GET: /TranferMoneyModel/

        public decimal? Moneytranfer { get; set; }
        public decimal? Moneycurrent { get; set; }
        public int? CustomerID { get; set; }

    }
}
