namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("searchhistory")]
    public partial class searchhistory
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]

        public int searchid { get; set; }

        [Column(TypeName = "text")]
        public string keyword { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int accountid { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }
    }
}
