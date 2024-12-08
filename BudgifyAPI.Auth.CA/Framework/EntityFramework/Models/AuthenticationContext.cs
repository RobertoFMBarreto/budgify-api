using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Auth.CA.Framework.EntityFramework.Models;

public partial class AuthenticationContext : DbContext
{
    public AuthenticationContext()
    {
    }

    public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Encoding.UTF8.GetString( Convert.FromBase64String( Environment.GetEnvironmentVariable(
            "ConnectionString__budgify_db"))));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
