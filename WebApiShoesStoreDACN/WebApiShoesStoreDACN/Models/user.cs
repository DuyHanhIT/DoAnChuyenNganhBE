namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int userid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int accountid { get; set; }

        [StringLength(200)]
        public string firstName { get; set; }

        [StringLength(11)]
        public string phone { get; set; }

        [StringLength(150)]
        public string email { get; set; }

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
