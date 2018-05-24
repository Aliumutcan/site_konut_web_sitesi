namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Daire")]
    public partial class Daire
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Daire()
        {
            Galeri = new HashSet<Galeri>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string blokno { get; set; }

        [StringLength(50)]
        public string daireno { get; set; }

        public byte? kat { get; set; }

        [StringLength(600)]
        public string aciklama { get; set; }

        public double? fiyat { get; set; }

        public byte? dairedurum { get; set; }

        public bool? bosdolu { get; set; }

        [Column(TypeName = "money")]
        public decimal? toplamborc { get; set; }

        public virtual DaireSayibi DaireSayibi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Galeri> Galeri { get; set; }
    }
}
