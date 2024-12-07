using System;
using System.Collections.Generic;
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=42765;UserId=postgres;Password=budgify;Database=Wallets");

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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
