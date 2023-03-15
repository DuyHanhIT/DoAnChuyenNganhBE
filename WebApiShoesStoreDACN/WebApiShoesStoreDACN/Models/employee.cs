namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("employee")]
    public partial class employee
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int epid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? accountid { get; set; }

        [StringLength(200)]
        public string firstName { get; set; }

        public DateTime? birthday { get; set; }

        public bool? gender { get; set; }

        [StringLength(11)]
        public string phone { get; set; }

        [StringLength(200)]
        public string lastName { get; set; }

        [StringLength(200)]
        public string address { get; set; }

        [StringLength(400)]
        public string avatar { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }
    }
}
