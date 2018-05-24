namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AidatBorc")]
    public partial class AidatBorc
    {
        public int id { get; set; }

        [StringLength(30)]
        public string hesap_kodu { get; set; }

        [Column(TypeName = "money")]
        public decimal? borc_bakiye { get; set; }

        [Column(TypeName = "money")]
        public decimal? aidat { get; set; }

        [Column(TypeName = "money")]
        public decimal? gecikmezammi { get; set; }

        [Column(TypeName = "money")]
        public decimal? buaykiborc { get; set; }

        [Column(TypeName = "money")]
        public decimal? geneltoplam { get; set; }

        [StringLength(4)]
        public string yil { get; set; }

        [StringLength(15)]
        public string ay { get; set; }
    }
}
