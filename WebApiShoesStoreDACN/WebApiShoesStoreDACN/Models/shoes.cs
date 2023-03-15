namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class shoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public shoes()
        {
            comments = new HashSet<comment>();
            comments1 = new HashSet<comment>();
            monthlyrevenuedetails = new HashSet<monthlyrevenuedetail>();
            monthlyrevenuedetails1 = new HashSet<monthlyrevenuedetail>();
            ratings = new HashSet<rating>();
            ratings1 = new HashSet<rating>();
            favorites = new HashSet<favorite>();
            favorites1 = new HashSet<favorite>();
            orderdetails = new HashSet<orderdetail>();
            orderdetails1 = new HashSet<orderdetail>();
            saleDetails = new HashSet<saleDetail>();
            saleDetails1 = new HashSet<saleDetail>();
            sizetables = new HashSet<sizetable>();
            sizetables1 = new HashSet<sizetable>();
        }

        [Key]
        public int shoeid { get; set; }

        public int? brandid { get; set; }

        public int? categoryid { get; set; }

        [StringLength(100)]
        public string shoename { get; set; }

        public decimal? price { get; set; }
        public decimal? rate { get; set; }

        [Column(TypeName = "ntext")]
        public string description { get; set; }

        public int? stock { get; set; }

        public int? purchased { get; set; }

        public bool? shoenew { get; set; }

        public DateTime? createdate { get; set; }

        public DateTime? dateupdate { get; set; }

        public int? updateby { get; set; }

        public bool active { get; set; }

        public int? createby { get; set; }

        [StringLength(400)]
        public string image1 { get; set; }

        [StringLength(400)]
        public string image2 { get; set; }

        [StringLength(400)]
        public string image3 { get; set; }

        [StringLength(400)]
        public string image4 { get; set; }

        public virtual brand brand { get; set; }

        public virtual brand brand1 { get; set; }

        public virtual category category { get; set; }

        public virtual category category1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<monthlyrevenuedetail> monthlyrevenuedetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<monthlyrevenuedetail> monthlyrevenuedetails1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rating> ratings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rating> ratings1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<favorite> favorites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<favorite> favorites1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orderdetail> orderdetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orderdetail> orderdetails1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saleDetail> saleDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saleDetail> saleDetails1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sizetable> sizetables { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sizetable> sizetables1 { get; set; }
    }
}
