namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class orderdetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? orderid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? shoeid { get; set; }

        public int? quantity { get; set; }

        public int? size { get; set; }

        public decimal? price { get; set; }

        public decimal? total { get; set; }

        public virtual shoes sho { get; set; }

        public virtual shoes sho1 { get; set; }
    }
}
