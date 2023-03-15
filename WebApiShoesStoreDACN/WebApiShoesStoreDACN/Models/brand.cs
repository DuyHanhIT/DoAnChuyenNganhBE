namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("brand")]
    public partial class brand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public brand()
        {
            shoes = new HashSet<shoes>();
            shoes1 = new HashSet<shoes>();
        }

        public int brandid { get; set; }

        [StringLength(100)]
        public string brandname { get; set; }

        [Column(TypeName = "ntext")]
        public string information { get; set; }

        [Column(TypeName = "text")]
        public string logo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<shoes> shoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<shoes> shoes1 { get; set; }
    }
}
