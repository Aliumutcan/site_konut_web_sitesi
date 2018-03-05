namespace selcukunikonutlari.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model13")
        {
        }

        public virtual DbSet<AidatBorc> AidatBorc { get; set; }
        public virtual DbSet<BankaBilgileri> BankaBilgileri { get; set; }
        public virtual DbSet<Calisanlar> Calisanlar { get; set; }
        public virtual DbSet<Daire> Daire { get; set; }
        public virtual DbSet<DaireSayibi> DaireSayibi { get; set; }
        public virtual DbSet<Duyuru> Duyuru { get; set; }
        public virtual DbSet<ELMAH_Error> ELMAH_Error { get; set; }
        public virtual DbSet<Galeri> Galeri { get; set; }
        public virtual DbSet<Iletisim> Iletisim { get; set; }
        public virtual DbSet<Onemlitelefonlar> Onemlitelefonlar { get; set; }
        public virtual DbSet<TumAylar> TumAylar { get; set; }
        public virtual DbSet<Yonetici> Yonetici { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AidatBorc>()
                .Property(e => e.borc_bakiye)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AidatBorc>()
                .Property(e => e.aidat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AidatBorc>()
                .Property(e => e.gecikmezammi)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AidatBorc>()
                .Property(e => e.buaykiborc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AidatBorc>()
                .Property(e => e.geneltoplam)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Daire>()
                .Property(e => e.toplamborc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Daire>()
                .HasOptional(e => e.DaireSayibi)
                .WithRequired(e => e.Daire);

            modelBuilder.Entity<DaireSayibi>()
                .HasMany(e => e.Galeri)
                .WithOptional(e => e.DaireSayibi)
                .HasForeignKey(e => e.yk);

            modelBuilder.Entity<Duyuru>()
                .Property(e => e.icerik)
                .IsUnicode(false);
        }
    }
}
