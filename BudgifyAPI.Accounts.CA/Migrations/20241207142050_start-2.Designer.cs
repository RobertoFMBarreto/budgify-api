﻿// <auto-generated />
using System;
using BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgifyAPI.Accounts.CA.Migrations
{
    [DbContext(typeof(AccountsContext))]
    [Migration("20241207142050_start-2")]
    partial class start2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models.User", b =>
                {
                    b.Property<bool>("AllowWalletWatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("allow_wallet_watch");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<int>("Genre")
                        .HasColumnType("integer")
                        .HasColumnName("genre");

                    b.Property<Guid>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id_user")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid?>("IdUserGroup")
                        .HasColumnType("uuid")
                        .HasColumnName("id_user_group");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_active");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_admin");

                    b.Property<bool>("IsManager")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_manager");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("BudgifyAPI.Accounts.CA.Framework.EntityFramework.Models.UserGroup", b =>
                {
                    b.Property<Guid>("IdUserGroup")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id_user_group")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.ToTable("user_group", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
