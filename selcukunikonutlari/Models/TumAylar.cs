namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TumAylar")]
    public partial class TumAylar
    {
        public int id { get; set; }

        [StringLength(15)]
        public string ayadi { get; set; }
    }
}
