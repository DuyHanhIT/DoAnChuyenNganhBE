namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("order")]
    public partial class order
    {
        [Key]
        [Column(Order = 0)]
        /*[DatabaseGenerated(DatabaseGeneratedOption.None)]*/
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]


        public int orderid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? accountid { get; set; }

        /*[Key]*/
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? statusid { get; set; }

        public DateTime createdate { get; set; }

        public DateTime? deliverydate { get; set; }

        [StringLength(200)]
        public string firstName { get; set; }

        [StringLength(11)]
        public string phone { get; set; }

        [StringLength(150)]
        public string email { get; set; }

        [Column(TypeName = "ntext")]
        public string note { get; set; }

        public decimal? total { get; set; }

        public bool? payment { get; set; }

        [Column(TypeName = "text")]
        public string momo { get; set; }

        [StringLength(200)]
        public string lastName { get; set; }

        public string address { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }

        public virtual status status { get; set; }

        public virtual status status1 { get; set; }
    }
}
