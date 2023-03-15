namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class monthlyrevenuedetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int shoeid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string month { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string year { get; set; }

        public int? sellnumber { get; set; }

        public decimal? turnover { get; set; }

        public virtual monthlyrevenue monthlyrevenue { get; set; }

        public virtual monthlyrevenue monthlyrevenue1 { get; set; }

        public virtual shoes sho { get; set; }

        public virtual shoes sho1 { get; set; }
    }
}
