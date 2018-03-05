namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Calisanlar")]
    public partial class Calisanlar
    {
        public int id { get; set; }

        [StringLength(40)]
        public string adi { get; set; }

        [StringLength(30)]
        public string soyadi { get; set; }

        [StringLength(30)]
        public string sorumlubloklar { get; set; }

        [StringLength(15)]
        public string tel { get; set; }

        public bool? sef { get; set; }
    }
}
