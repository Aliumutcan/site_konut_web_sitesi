namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Iletisim")]
    public partial class Iletisim
    {
        public int id { get; set; }

        [StringLength(100)]
        public string adsoyad { get; set; }

        [StringLength(150)]
        public string email { get; set; }

        [StringLength(15)]
        public string tel { get; set; }

        [StringLength(600)]
        public string mesaj { get; set; }

        public bool? durum { get; set; }
    }
}
