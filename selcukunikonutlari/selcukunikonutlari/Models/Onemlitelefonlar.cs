namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Onemlitelefonlar")]
    public partial class Onemlitelefonlar
    {
        public int id { get; set; }

        [StringLength(100)]
        public string baslik { get; set; }

        [StringLength(100)]
        public string adsoyad { get; set; }

        [StringLength(15)]
        public string tel { get; set; }

        [StringLength(15)]
        public string istel { get; set; }

        [StringLength(100)]
        public string email { get; set; }
    }
}
