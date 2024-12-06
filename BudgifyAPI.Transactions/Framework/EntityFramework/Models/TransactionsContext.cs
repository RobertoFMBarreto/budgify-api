using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Transactions.Framework.EntityFramework.Models;

public partial class TransactionsContext : DbContext
{
    public TransactionsContext()
    {
    }

    public TransactionsContext(DbContextOptions<TransactionsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Reocurring> Reocurrings { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionGroup> TransactionGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=42765;UserId=postgres;Password=budgify;Database=Transactions");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.IdCategory)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_category");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reocurring>(entity =>
        {
            entity.HasKey(e => e.IdReocurring).HasName("reocurring_pkey");

            entity.ToTable("reocurring");

            entity.HasIndex(e => e.IdCategory, "IX_reocurring_id_category");

            entity.HasIndex(e => e.IdSubcategory, "IX_reocurring_id_subcategory");

            entity.Property(e => e.IdReocurring)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_reocurring");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdSubcategory).HasColumnName("id_subcategory");
            entity.Property(e => e.IdWallet).HasColumnName("id_wallet");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsMonthly)
                .HasDefaultValue(false)
                .HasColumnName("is_monthly");
            entity.Property(e => e.IsWeekly)
                .HasDefaultValue(false)
                .HasColumnName("is_weekly");
            entity.Property(e => e.IsYearly)
                .HasDefaultValue(false)
                .HasColumnName("is_yearly");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Reocurrings)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FKCategory");

            entity.HasOne(d => d.IdSubcategoryNavigation).WithMany(p => p.Reocurrings)
                .HasForeignKey(d => d.IdSubcategory)
                .HasConstraintName("FKSubcategory");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.IdSubcategory).HasName("subcategory_pkey");

            entity.ToTable("subcategory");

            entity.HasIndex(e => e.IdCategory, "IX_subcategory_id_category");

            entity.Property(e => e.IdSubcategory)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_subcategory");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Subcategories)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCategory");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.IdTransaction).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.HasIndex(e => e.IdCategory, "IX_transactions_id_category");

            entity.HasIndex(e => e.IdReocurring, "IX_transactions_id_reocurring");

            entity.HasIndex(e => e.IdSubcategory, "IX_transactions_id_subcategory");

            entity.HasIndex(e => e.IdTransactionGroup, "IX_transactions_id_transaction_group");

            entity.Property(e => e.IdTransaction)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_transaction");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Date)
                .HasPrecision(6)
                .HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdReocurring).HasColumnName("id_reocurring");
            entity.Property(e => e.IdSubcategory).HasColumnName("id_subcategory");
            entity.Property(e => e.IdTransactionGroup).HasColumnName("id_transaction_group");
            entity.Property(e => e.IdWallet).HasColumnName("id_wallet");
            entity.Property(e => e.IsPlanned)
                .HasDefaultValue(false)
                .HasColumnName("is_planned");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FkCategory");

            entity.HasOne(d => d.IdReocurringNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdReocurring)
                .HasConstraintName("FKReocurring");

            entity.HasOne(d => d.IdSubcategoryNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdSubcategory)
                .HasConstraintName("FKSubcategory");

            entity.HasOne(d => d.IdTransactionGroupNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdTransactionGroup)
                .HasConstraintName("FKTransactiongroup");
        });

        modelBuilder.Entity<TransactionGroup>(entity =>
        {
            entity.HasKey(e => e.IdTransactionGroup).HasName("transaction_group_pkey");

            entity.ToTable("transaction_group");

            entity.Property(e => e.IdTransactionGroup)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_transaction_group");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasPrecision(6)
                .HasColumnName("end_date");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.PlannedAmount).HasColumnName("planned_amount");
            entity.Property(e => e.StartDate)
                .HasPrecision(6)
                .HasColumnName("start_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
