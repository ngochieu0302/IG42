using System.Collections.Generic;

namespace FDI.Simple
{
    public class AutoCompleteCommonItem
    {
        public string query { get; set; }
        public List<string> Sussgestions { get; set; }
        public List<SuggestionsItem> suggestions { get; set; }
    }
    
    public class AutoCompleteProduct
    {
        public string query { get; set; }
        public List<string> Sussgestions { get; set; }
        //public List<int> listbed { get; set; }
        public IEnumerable<SuggestionsProduct> suggestions { get; set; }
    }

    public class AutoCompleteRate
    {
        public string query { get; set; }
        public List<string> Sussgestions { get; set; }
        //public List<int> listbed { get; set; }
        public IEnumerable<BaseSuggestions> suggestions { get; set; }
    }

    public class AutoCompleteTMCustomer
    {
        public string query { get; set; }
        public List<string> Sussgestions { get; set; }
        public IEnumerable<SuggestionsTMCustomer> suggestions { get; set; }
    }

    public class AutoCompleteTMNews
    {
        public string query { get; set; }
        public List<string> Sussgestions { get; set; }
        public IEnumerable<SuggestionsTMNews> suggestions { get; set; }
    }
}
