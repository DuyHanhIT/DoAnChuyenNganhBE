namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("product")]
    public partial class product
    {
        public int productid { get; set; }

        [StringLength(200)]
        public string name { get; set; }

        [StringLength(100)]
        public string price { get; set; }

        public int? size { get; set; }

        [Column(TypeName = "ntext")]
        public string description { get; set; }

        public DateTime? createdate { get; set; }

        public int accountid { get; set; }

        public int statusid { get; set; }

        [StringLength(400)]
        public string img1 { get; set; }

        [StringLength(400)]
        public string img2 { get; set; }

        [StringLength(400)]
        public string img3 { get; set; }

        [StringLength(400)]
        public string img4 { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }

        public virtual status status { get; set; }

        public virtual status status1 { get; set; }
    }
}
