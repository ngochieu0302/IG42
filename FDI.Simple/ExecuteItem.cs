using System;
using System.Data;

namespace FDI.Simple
{
    [Serializable]
    public class ExecuteItem : BaseSimple
    {
        public string ExecuteQuery { get; set; }
        public int DropDownList { get; set; }
    }

    [Serializable]
    public class ExecuteQueryItem : BaseSimple
    {
        public DataTable DataTable { get; set; }
        public ExecuteItem ExecuteItem { get; set; }
        public string Message { get; set; }
        public bool Erros { get; set; }
    }

    
}
