using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnBarcode.Barcode;

namespace NhapKhoLocal
{
    public partial class Form1 : Form
    {
        private readonly Font _printFont;
        private readonly StreamReader _streamToPrint;
        public static double Kl = 0;
        private  double _klcheck;
        public static string Com = ConfigurationManager.AppSettings["Com"];
        public Form1()
        {
            InitializeComponent();
            GenerateBacode("12345gaa");
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += timer1_Tick;
            var reader = new ArduinoSerialReader(Com);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_klcheck != Kl)
            {
                _klcheck = Kl;
                PrintHelpPage();
            }
        }
        private void PrintHelpPage()
        {
            // Create a WebBrowser instance. 
            var webBrowserForPrinting = new WebBrowser
            {
                DocumentText = "<div><div style=\"height: 115px ; width: 400px \">" +
                "<div style=\"padding: 0px \">" +
                "<div style=\"margin: 0px \"><div style=\"padding: 0px ; margin: 0px ; float: left\">" +
                "<div style=\"text-align: left; display: block; width: 100%; font-size: 11px; margin-top: 4px\">" +
                "<strong style=\"float: left; text-transform: uppercase; margin-right: 3px\">Thịt tươi</strong> <strong style=\"float: left; text-transform: uppercase\"></strong></div></div></div><div >" +
                "<div style=\"padding: 0px ; margin: -2px 0 -3px 10px ; float: left; width: 393px\">" +
                    "<img src=\"C:\\code128.gif\"></div>" +
                "<div style=\"width: calc(100% - 227px);float: left; margin-right: 5px\">" +
                     "<div style=\"padding: 0px ; margin: 0 5px 0 0 ; float: left; width: 100%\">" +
                        "<strong style=\"font-size: 11px; float: left\">Đơn giá:</strong>" +
                        "<strong style=\"font-size: 11px;margin-right: 10px; float: right\"></strong></div>" +
                     "<div style=\"padding: 0px ; margin: 0 5px 0 0 ; float: left; width: 100%\">" +
                        "<strong style=\"font-size: 11px; float: left\">Số lượng:</strong>" +
                          "<strong style=\"font-size: 11px;margin-right: 10px; float: right\"></strong></div>" +
                    "<div  style=\"margin: 0 5px 0 0 ;width: 100%\">" +
                        "<strong style=\"font-size: 11px; float: left\">Thành tiền:</strong><br /></div>" +
                    "<div style=\"margin: 0 5px 0 0 ; width: 100%\">" +
                        "<strong style=\"font-size: 16px; float: left\"></strong></div></div></div><div ><div >" +
                    "<div style=\"margin: 0 15px; width: 100%\"><strong style=\"font-size: 11px; float: left\">NĐG:" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                    "- HSD: " + DateTime.Now.AddDays(3).ToString("dd/MM/yyyy HH:mm") + "</strong></div> <br />" +
                    "<div style=\"margin: 0 15px; width: 100%\"><strong style=\"font-size: 11px; float: left\">IG4.VN</strong><strong style=\"font-size: 11px;margin-right: 35px; float: right\">TOÀN DÂN ĂN THỰC PHẨM SẠCH!</strong></div></div></div></div></div></div>"
            };
            webBrowserForPrinting.DocumentCompleted += PrintDocument;
        }

        private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).Print();
            ((WebBrowser)sender).Dispose();
        }
        private void GenerateBacode(string _data)
        {
            var code128 = new Linear
            {
                Data = _data,
                Type = BarcodeType.CODE128,
                AddCheckSum = true,
                UOM = UnitOfMeasure.PIXEL,
                X = 1,
                Y = 60,
                LeftMargin = 0,
                RightMargin = 0,
                TopMargin = 0,
                BottomMargin = 0,
                Resolution = 72,
                Rotate = Rotate.Rotate0,
                ShowText = true,
                ShowCheckSumChar = true,
                TextFont = new Font("Arial", 14, FontStyle.Bold),
                TextMargin = 6,
                Format = ImageFormat.Gif
            };
            code128.drawBarcode("C:\\code128.gif");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    public class ArduinoSerialReader : IDisposable
    {
        private readonly SerialPort _serialPort;

        public ArduinoSerialReader(string portName)
        {
            try
            {
                _serialPort = new SerialPort(portName);
                _serialPort.Open();
                _serialPort.DataReceived += serialPort_DataReceived;
            }
            catch (Exception)
            {

            }
        }

        void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            try
            {
                var text = _serialPort.ReadLine();
                var leng = text.Length;
                var bien = text.Substring(8, leng - 11);
                double kla;
                double.TryParse(bien, out kla);
                Form1.Kl = kla;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
        }
    }
}
