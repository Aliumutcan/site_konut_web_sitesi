namespace selcukunikonutlari.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DaireSayibi")]
    public partial class DaireSayibi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DaireSayibi()
        {
            Galeri = new HashSet<Galeri>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int daireId { get; set; }

        [StringLength(40)]
        public string ad { get; set; }

        [StringLength(30)]
        public string soyad { get; set; }

        [StringLength(20)]
        public string tel { get; set; }

        [StringLength(30)]
        public string email { get; set; }

        public int? yetkiId { get; set; }

        public int? durum { get; set; }

        public virtual Daire Daire { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Galeri> Galeri { get; set; }
    }
}
