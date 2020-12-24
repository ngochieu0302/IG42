using System;
using FDI.Simple;

namespace FDI.Utils
{
    [Serializable]
    public class JsonMessage
    {
        public bool Erros { get; set; }
        public string Message { get; set; }
        public string ID { get; set; }
        public int MsgID { get; set; }
        public int Type { get; set; }
        public int Code { get; set; }
        public SaleCodeItem SaleCodeItem { get; set; }

        public int TotalPages
        {
            get
            {
                if (RowPerPage == 0)
                {
                    return 0;
                }
                return (TotalItems % RowPerPage == 0) ? TotalItems / RowPerPage : ((TotalItems - (TotalItems % RowPerPage)) / RowPerPage) + 1;

            }
        }

        public int TotalItems { get; set; }
        public int RowPerPage { get; set; }
        public int Total { get; set; }
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
