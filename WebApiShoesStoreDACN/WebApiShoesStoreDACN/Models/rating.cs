namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rating")]
    public partial class rating
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? shoeid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? accountid { get; set; }

        public decimal? rate { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }

        public virtual shoes sho { get; set; }

        public virtual shoes sho1 { get; set; }
    }
}
