namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("monthlyrevenue")]
    public partial class monthlyrevenue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public monthlyrevenue()
        {
            monthlyrevenuedetails = new HashSet<monthlyrevenuedetail>();
            monthlyrevenuedetails1 = new HashSet<monthlyrevenuedetail>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string month { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string year { get; set; }

        public int? sellnumber { get; set; }

        public decimal? turnover { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<monthlyrevenuedetail> monthlyrevenuedetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<monthlyrevenuedetail> monthlyrevenuedetails1 { get; set; }
    }
}
