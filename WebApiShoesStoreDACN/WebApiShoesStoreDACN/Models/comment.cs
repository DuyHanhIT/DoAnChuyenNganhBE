namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public comment()
        {
            replies = new HashSet<reply>();
            replies1 = new HashSet<reply>();
        }

        [Key]
        public int cmtid { get; set; }

        public int? shoeid { get; set; }

        public int? accountid { get; set; }

        [StringLength(200)]
        public string content { get; set; }

        [Column(TypeName = "text")]
        public string image { get; set; }

        public DateTime? createdate { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }

        public virtual shoes sho { get; set; }

        public virtual shoes sho1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reply> replies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reply> replies1 { get; set; }
    }
}
