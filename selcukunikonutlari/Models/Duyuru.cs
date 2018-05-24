namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Duyuru")]
    public partial class Duyuru
    {
        public int id { get; set; }

        [StringLength(200)]
        public string baslik { get; set; }

        [Column(TypeName = "text")]
        public string icerik { get; set; }

        public DateTime? tarih { get; set; }

        public byte? durum { get; set; }
    }
}
