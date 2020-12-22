using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhapKho.Web.Models
{
    public class BarCodeModel 
    {
        //
        // GET: /BarCodeModel/

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] BarcodeImage { get; set; }
        public string Barcode { get; set; }
        public string ImageUrl { get; set; }

    }
}
