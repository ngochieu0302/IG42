using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NhapKho.Web.Models
{
    public class Barcodecs 
    {
        //
        // GET: /Barcodecs/

        public string GenerateBarcode()
        {
            try
            {
                var charPool = "1-2-3-4-5-6-7-8-9-0".Split('-');
                var rs = new StringBuilder();
                var length = 10;
                var rnd = new Random();
                while (length-- > 0)
                {
                    var index = (int)(rnd.NextDouble() * charPool.Length);
                    if (charPool[index] != "-")
                    {
                        rs.Append(charPool[index]);
                        charPool[index] = "-";
                    }
                    else
                        length++;
                }
                return rs.ToString();
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
            }
            return "";
        }

        //31 December 2012 Prapti

        public byte[] GetBarcodeImage(string strbarcode, string file = "", bool showcode = false)
        {
            var barcode = new BarCode39();
            try
            {
                const int barSize = 35;
                var fontFile = HttpContext.Current.Server.MapPath("~/Content/fonts/FREE3OF9.TTF");
                return (barcode.Code39(strbarcode, barSize, showcode, file, fontFile));
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
            }
            return null;
        }

    }
}
