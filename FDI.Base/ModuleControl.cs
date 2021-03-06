//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class ModuleControl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModuleControl()
        {
            this.HtmlMaps = new HashSet<HtmlMap>();
            this.ModuleSettings = new HashSet<ModuleSetting>();
        }
    
        public int Id { get; set; }
        public Nullable<int> PageID { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Section { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string LanguageID { get; set; }
        public Nullable<int> AgencyID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HtmlMap> HtmlMaps { get; set; }
        public virtual ModulePage ModulePage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleSetting> ModuleSettings { get; set; }
    }
}
