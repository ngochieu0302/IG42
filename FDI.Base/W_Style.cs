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
    
    public partial class W_Style
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public W_Style()
        {
            this.W_Base = new HashSet<W_Base>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> PositionID { get; set; }
        public Nullable<int> FrameID { get; set; }
        public Nullable<int> MarginID { get; set; }
        public Nullable<int> DecorationID { get; set; }
        public Nullable<int> OpacityID { get; set; }
        public Nullable<int> TextStyleID { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<W_Base> W_Base { get; set; }
        public virtual W_Decoration W_Decoration { get; set; }
        public virtual W_Frame W_Frame { get; set; }
        public virtual W_Margin W_Margin { get; set; }
        public virtual W_Opacity W_Opacity { get; set; }
        public virtual W_Position W_Position { get; set; }
        public virtual W_Supplier W_Supplier { get; set; }
        public virtual W_TextStyle W_TextStyle { get; set; }
    }
}