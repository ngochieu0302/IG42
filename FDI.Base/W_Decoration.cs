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
    
    public partial class W_Decoration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public W_Decoration()
        {
            this.W_Style = new HashSet<W_Style>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<long> ColorID { get; set; }
        public Nullable<int> RadiusID { get; set; }
        public Nullable<int> BorderID { get; set; }
        public Nullable<int> EffectID { get; set; }
    
        public virtual W_Base W_Base { get; set; }
        public virtual W_Border W_Border { get; set; }
        public virtual W_Effect W_Effect { get; set; }
        public virtual W_Radius W_Radius { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<W_Style> W_Style { get; set; }
    }
}
