using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CurrencyInfoLibrary.Models
{
    public partial class CurrencyInfoDBContext : DbContext
    {
        public CurrencyInfoDBContext()
        {
        }

        public CurrencyInfoDBContext(DbContextOptions<CurrencyInfoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<CurrencyRate> CurrencyRate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<CurrencyRate>(entity =>
            {
                entity.Property(e => e.CurrencyDate).HasColumnType("date");

                entity.Property(e => e.Rate).HasColumnType("decimal(10, 5)");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CurrencyRate)
                    .HasForeignKey(d => d.CountryId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
