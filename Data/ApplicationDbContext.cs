using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestoranASP.Models;

namespace RestoranASP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Kategorija> Kategorije { get; set; }

        public DbSet<Jelo> Jela {  get; set; }

        public DbSet<Narudzbina> Narudzbine {  get; set; }

        public DbSet<StavkaNarudzbine> StavkeNarudzbina {  get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Jelo>()
                .HasOne(j => j.Kategorija)
                .WithMany(k => k.Jela)
                .HasForeignKey(j => j.KategorijaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StavkaNarudzbine>()
                .HasKey(sn => new { sn.NarudzbinaId, sn.JeloId });

            builder.Entity<StavkaNarudzbine>()
                .HasOne(sn => sn.Narudzbina)
                .WithMany(n => n.Jela)
                .HasForeignKey(sn => sn.NarudzbinaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StavkaNarudzbine>()
                .HasOne(sn => sn.Jelo)
                .WithMany(j => j.Narudzbine)
                .HasForeignKey(sn => sn.JeloId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
