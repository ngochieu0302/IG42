using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class BankItem : BaseSimple
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
    }

    public class ModelBankItem : BaseModelSimple
    {
        public IEnumerable<BankItem> ListItem { get; set; }
    }
}
