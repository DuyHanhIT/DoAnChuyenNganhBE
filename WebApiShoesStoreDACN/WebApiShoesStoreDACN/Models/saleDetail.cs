namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class saleDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int saleid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int shoeid { get; set; }

        public decimal? saleprice { get; set; }

        public int? updateby { get; set; }

        /*[Key]
        [Column(Order = 2)]*/
        public bool active { get; set; }

        public virtual sale sale { get; set; }

        public virtual sale sale1 { get; set; }

        public virtual shoes sho { get; set; }

        public virtual shoes sho1 { get; set; }
    }
}
