namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yonetici")]
    public partial class Yonetici
    {
        public int id { get; set; }

        [StringLength(50)]
        public string kulaniciadi { get; set; }

        [StringLength(50)]
        public string sifre { get; set; }

        public int? yetki { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(20)]
        public string tel { get; set; }

        [StringLength(100)]
        public string adi { get; set; }

        [StringLength(100)]
        public string soyadi { get; set; }

        [StringLength(30)]
        public string hesap_kodu { get; set; }

        public bool? sifre_durum { get; set; }

        [StringLength(100)]
        public string daire { get; set; }
    }
}
