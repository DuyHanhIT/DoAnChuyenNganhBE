namespace WebApiShoesStoreDACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("reply")]
    public partial class reply
    {
        [Key]
        public int repid { get; set; }

        [StringLength(1)]
        public string content { get; set; }

        [Column(TypeName = "text")]
        public string image { get; set; }

        public DateTime? createdate { get; set; }

        public int cmtid { get; set; }

        public int accountid { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }

        public virtual comment comment { get; set; }

        public virtual comment comment1 { get; set; }
    }
}
