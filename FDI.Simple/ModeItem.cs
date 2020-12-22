namespace FDI.Simple
{
    public class ModeItem:BaseSimple
    {
        public string Module { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Section { get; set; }
        public string Area { get; set; }
        public string ReturnType { get; set; }
        public int Sort { get; set; }
    }
}
