namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("selcukun_unikonutlari.Mesajlar")]
    public partial class Mesajlar
    {
        public int id { get; set; }

        [Column(TypeName = "text")]
        public string mesaj { get; set; }

        public DateTime? tarih { get; set; }

        public int? yoneticiID { get; set; }

        public bool? durum { get; set; }
    }
}
