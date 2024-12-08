using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;

public partial class WalletsContext : DbContext
{
    public WalletsContext()
    {
    }

    public WalletsContext(DbContextOptions<WalletsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Encoding.UTF8.GetString( Convert.FromBase64String( Environment.GetEnvironmentVariable(
            "ConnectionString__budgify_db"))));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.IdWallet).HasName("pk_wallets");

            entity.ToTable("wallet");

            entity.Property(e => e.IdWallet)
                .ValueGeneratedNever()
                .HasColumnName("id_wallet");
            entity.Property(e => e.AgreementDays).HasColumnName("agreement_days");
            entity.Property(e => e.IdAccount)
                .HasColumnType("character varying")
                .HasColumnName("id_account");
            entity.Property(e => e.IdRequisition)
                .HasMaxLength(255)
                .HasColumnName("id_requisition");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.StoreInCloud)
                .HasDefaultValue(true)
                .HasColumnName("store_in_cloud");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
