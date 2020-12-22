using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AutoCompleteItem : BaseSimple
    {
        public string query { get; set; }
        public List<string> suggestions { get; set; }
        public List<string> data { get; set; }
        public List<string> Link { get; set; }

    }
    public class ModelAutoCompleteItem : BaseModelSimple
    {
        
        public IEnumerable<AutoCompleteItem> ListItem { get; set; }
        
    }
}
