namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sale()
        {
            saleDetails = new HashSet<saleDetail>();
            saleDetails1 = new HashSet<saleDetail>();
        }

        public int saleid { get; set; }

        [StringLength(150)]
        [Column(TypeName = "ntext")]
        public string salename { get; set; }

        public DateTime? createdate { get; set; }

        public int? createby { get; set; }

        public int? updateby { get; set; }

        public DateTime? startday { get; set; }

        public DateTime? endday { get; set; }

        [Column(TypeName = "ntext")]
        public string content { get; set; }

        public decimal? percent { get; set; }
        public string imgsale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saleDetail> saleDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saleDetail> saleDetails1 { get; set; }
    }
}
