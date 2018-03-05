namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Galeri")]
    public partial class Galeri
    {
        public int id { get; set; }

        [StringLength(150)]
        public string resimyol { get; set; }

        [StringLength(150)]
        public string aciklama { get; set; }

        public int? daireId { get; set; }

        public int? calisanId { get; set; }

        public bool? arkaplan { get; set; }

        public bool? slider { get; set; }

        [Column("galeri")]
        public bool? galeri1 { get; set; }

        public bool? vaziyet { get; set; }

        public int? yk { get; set; }

        public virtual Daire Daire { get; set; }

        public virtual DaireSayibi DaireSayibi { get; set; }
    }
}
