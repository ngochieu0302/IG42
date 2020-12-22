using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhapKho.Web.Models
{
     [Serializable]
    public class JsonMessage
    {
        public bool Erros { get; set; }
        public string Message { get; set; }
        public string ID { get; set; }
        public int MsgID { get; set; }
        public int Type { get; set; }

        public JsonMessage()
        {
            Erros = false;
            Message = string.Empty;
            ID = string.Empty;
            Type = 0;
            MsgID = 0;
        }

        public JsonMessage(bool erros, string message)
        {
            Erros = erros;
            Message = message;
        }

    }
}
