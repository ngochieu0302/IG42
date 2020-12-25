using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AutoCompleteAppIg4Item : BaseSimple
    {
        public string query { get; set; }
        public List<string> suggestions { get; set; }
        public List<string> data { get; set; }
        public List<string> Link { get; set; }

    }
    public class ModelAutoCompleteAppIg4Item : BaseModelSimple
    {
        
        public IEnumerable<AutoCompleteAppIg4Item> ListItem { get; set; }
        
    }
    public class AutoCompleteCustomerAppIg4Item : BaseSimple
    {
        public string query { get; set; }
        public List<SuggestionsProduct> suggestions { get; set; }
        public List<string> data { get; set; }
        public List<string> Link { get; set; }

    }
    public class AutoCompleteProductAppIg4
    {
        public string query { get; set; }
        public List<string> Sussgestions { get; set; }
        //public List<int> listbed { get; set; }
        public IEnumerable<SuggestionsProduct> suggestions { get; set; }
    }
    public class Select2Model
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
