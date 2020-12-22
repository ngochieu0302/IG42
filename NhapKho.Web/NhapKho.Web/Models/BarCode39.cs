using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhapKho.Web.Models
{
    public class BarCode39
    {
        //
        // GET: /BarCode39/

        private const int ItemSepHeight = 3;
        SizeF _titleSize = SizeF.Empty;
        SizeF _barCodeSize = SizeF.Empty;
        SizeF _codeStringSize = SizeF.Empty;
        public BarCode39()
        {
            TitleFont = new Font("Arial", 14);
            CodeStringFont = new Font("Arial", 14);
        }
        #region Barcode Title

        private string Title { get; set; }

        public Font TitleFont { get; set; }

        #endregion
        #region Barcode code string

        private bool ShowCodeString { get; set; }

        public Font CodeStringFont { get; set; }

        #endregion
        #region Barcode Font
        private Font _c39Font;

        private string FontFileName { get; set; }

        private string FontFamilyName { get; set; }

        private float _fontSize = 16;

        private Font Code39Font
        {
            get
            {
                if (_c39Font != null) return _c39Font;
                // Load the barcode font			
                var pfc = new PrivateFontCollection();
                pfc.AddFontFile(FontFileName);
                var family = new FontFamily(FontFamilyName, pfc);
                _c39Font = new Font(family, _fontSize);
                return _c39Font;
            }
        }
        #endregion
        #region Barcode Generation
        public byte[] Code39(string code, int barSize, bool showCodeString, string title, string fontFile)
        {
            // Create stream....
            var ms = new MemoryStream();
            FontFamilyName = "Free 3 of 9";//ConfigurationSettings.AppSettings["BarCodeFontFamily"];
            FontFileName = fontFile;//@"C:\Documents and Settings\narottam.sharma\Desktop\Barcode\WSBarCode\Code39Font\FREE3OF9.TTF";// ConfigurationSettings.AppSettings["BarCodeFontFile"];
            _fontSize = barSize;
            ShowCodeString = showCodeString;
            if (!string.IsNullOrEmpty(title))
                Title = title;
            var objBitmap = GenerateBarcode(code);
            objBitmap.Save(ms, ImageFormat.Png);

            //return bytes....
            return ms.GetBuffer();
        }
        public Bitmap GenerateBarcode(string barCode)
        {
            var bcodeWidth = 0;
            var bcodeHeight = 0;
            // Get the image container...
            var bcodeBitmap = CreateImageContainer(barCode, ref bcodeWidth, ref bcodeHeight);
            var objGraphics = Graphics.FromImage(bcodeBitmap);

            // Fill the background			
            objGraphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bcodeWidth, bcodeHeight));
            var vpos = 0;
            // Draw the barcode
            objGraphics.DrawString(barCode, Code39Font, new SolidBrush(Color.Black), XCentered((int)_barCodeSize.Width, bcodeWidth), vpos);
            //vpos += (((int)_titleSize.Height) + _itemSepHeight);
            // Draw the barcode string
            if (ShowCodeString)
            {
                vpos += (((int)_barCodeSize.Height));
                objGraphics.DrawString(barCode, CodeStringFont, new SolidBrush(Color.Black), XCentered((int)_codeStringSize.Width, bcodeWidth), vpos);
            }
            // Draw the title string
            if (Title != null)
            {
                vpos += (((int)_titleSize.Height) + ItemSepHeight);
                objGraphics.DrawString(Title, TitleFont, new SolidBrush(Color.Black), XCentered((int)_titleSize.Width, bcodeWidth), vpos);

            }
            // return the image...									
            return bcodeBitmap;
        }
        private Bitmap CreateImageContainer(string barCode, ref int bcodeWidth, ref int bcodeHeight)
        {
            // Create a temporary bitmap...
            var tmpBitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            var objGraphics = Graphics.FromImage(tmpBitmap);

            // calculate size of the barcode items...
            if (Title != null)
            {
                _titleSize = objGraphics.MeasureString(Title, TitleFont);
                bcodeWidth = (int)_titleSize.Width;
                bcodeHeight = (int)_titleSize.Height + ItemSepHeight;
            }
            _barCodeSize = objGraphics.MeasureString(barCode, Code39Font);
            bcodeWidth = Max(bcodeWidth, (int)_barCodeSize.Width);
            bcodeHeight += (int)_barCodeSize.Height;
            if (ShowCodeString)
            {
                _codeStringSize = objGraphics.MeasureString(barCode, CodeStringFont);
                bcodeWidth = Max(bcodeWidth, (int)_codeStringSize.Width);
                bcodeHeight += (ItemSepHeight + (int)_codeStringSize.Height);
            }
            // dispose temporary objects...
            objGraphics.Dispose();
            tmpBitmap.Dispose();
            return (new Bitmap(bcodeWidth, bcodeHeight, PixelFormat.Format32bppArgb));
        }

        #endregion
        #region Auxiliary Methods
        private static int Max(int v1, int v2)
        {
            return (v1 > v2 ? v1 : v2);
        }
        private static int XCentered(int localWidth, int globalWidth)
        {
            return ((globalWidth - localWidth) / 2);
        }
        #endregion

    }
}
