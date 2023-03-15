namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sizetable")]
    public partial class sizetable
    {
        [Key]
        [Column(Order = 0)]
        public int stid { get; set; }

        public int s38 { get; set; }

        public int? s39 { get; set; }

        public int? s40 { get; set; }

        public int? s41 { get; set; }

        public int? s42 { get; set; }

        public int? s43 { get; set; }

        public int? s44 { get; set; }

        public int? s45 { get; set; }

        public int? s46 { get; set; }

        public int? s47 { get; set; }

        public int? s48 { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? shoeid { get; set; }

        public virtual shoes sho { get; set; }

        public virtual shoes sho1 { get; set; }
    }
}
