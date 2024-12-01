using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;

public partial class UserContext : DbContext
{
    public UserContext()
    {
    }

    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseNpgsql( Environment.GetEnvironmentVariable(
        //    "ConnectionStrings_budgify-db"));
        optionsBuilder.UseNpgsql( getConnectionString());
    }
    private static string getConnectionString()
    {
        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");

        var config = new ConfigurationBuilder().AddJsonFile("appsettings" + (String.IsNullOrWhiteSpace(environmentName) ? "" : "." + environmentName) + ".json", false).Build();
        return config.GetConnectionString("budgify-db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.IdUser)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_user");
            entity.Property(e => e.AllowWalletWatch)
                .HasDefaultValue(true)
                .HasColumnName("allow_wallet_watch");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Genre).HasColumnName("genre");
            entity.Property(e => e.IdUserGroup).HasColumnName("id_user_group");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .HasColumnName("is_admin");
            entity.Property(e => e.IsManager)
                .HasDefaultValue(false)
                .HasColumnName("is_manager");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdToken).HasName("user_refresh_token_pkey");

            entity.ToTable("user_refresh_token");

            entity.HasIndex(e => e.Device, "device_unique_refresh").IsUnique();

            entity.Property(e => e.IdToken)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_token");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_date");
            entity.Property(e => e.Device).HasColumnName("device");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.LastUsage)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_usage");
            entity.Property(e => e.Token).HasColumnName("token");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserRefreshTokens)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_refresh_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
