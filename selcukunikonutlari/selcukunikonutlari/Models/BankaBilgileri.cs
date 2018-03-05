namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BankaBilgileri")]
    public partial class BankaBilgileri
    {
        public int id { get; set; }

        [StringLength(50)]
        public string bankaadi { get; set; }

        [StringLength(100)]
        public string iban { get; set; }

        [StringLength(50)]
        public string hesapno { get; set; }

        [StringLength(50)]
        public string subeno { get; set; }
    }
}
