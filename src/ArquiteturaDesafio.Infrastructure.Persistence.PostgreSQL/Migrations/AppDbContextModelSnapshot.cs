﻿// <auto-generated />
using System;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ArquiteturaDesafio.Core.Domain.Entities.DailyBalance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TransactionCount")
                        .HasColumnType("integer")
                        .HasColumnName("transaction_count");

                    b.HasKey("Id");

                    b.ToTable("daily_balances", (string)null);
                });

            modelBuilder.Entity("ArquiteturaDesafio.Core.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("description");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("ArquiteturaDesafio.Core.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("ArquiteturaDesafio.Core.Domain.Entities.DailyBalance", b =>
                {
                    b.OwnsOne("ArquiteturaDesafio.Core.Domain.ValueObjects.Balance", "FinalBalance", b1 =>
                        {
                            b1.Property<Guid>("DailyBalanceId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("numeric(18,2)")
                                .HasColumnName("final_balance");

                            b1.HasKey("DailyBalanceId");

                            b1.ToTable("daily_balances");

                            b1.WithOwner()
                                .HasForeignKey("DailyBalanceId");
                        });

                    b.OwnsOne("ArquiteturaDesafio.Core.Domain.ValueObjects.Balance", "InitialBalance", b1 =>
                        {
                            b1.Property<Guid>("DailyBalanceId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("numeric(18,2)")
                                .HasColumnName("initial_balance");

                            b1.HasKey("DailyBalanceId");

                            b1.ToTable("daily_balances");

                            b1.WithOwner()
                                .HasForeignKey("DailyBalanceId");
                        });

                    b.OwnsOne("ArquiteturaDesafio.Core.Domain.ValueObjects.Money", "TotalCredits", b1 =>
                        {
                            b1.Property<Guid>("DailyBalanceId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("numeric(18,2)")
                                .HasColumnName("total_credits");

                            b1.HasKey("DailyBalanceId");

                            b1.ToTable("daily_balances");

                            b1.WithOwner()
                                .HasForeignKey("DailyBalanceId");
                        });

                    b.OwnsOne("ArquiteturaDesafio.Core.Domain.ValueObjects.Money", "TotalDebits", b1 =>
                        {
                            b1.Property<Guid>("DailyBalanceId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("numeric(18,2)")
                                .HasColumnName("total_debits");

                            b1.HasKey("DailyBalanceId");

                            b1.ToTable("daily_balances");

                            b1.WithOwner()
                                .HasForeignKey("DailyBalanceId");
                        });

                    b.Navigation("FinalBalance")
                        .IsRequired();

                    b.Navigation("InitialBalance")
                        .IsRequired();

                    b.Navigation("TotalCredits")
                        .IsRequired();

                    b.Navigation("TotalDebits")
                        .IsRequired();
                });

            modelBuilder.Entity("ArquiteturaDesafio.Core.Domain.Entities.Transaction", b =>
                {
                    b.OwnsOne("ArquiteturaDesafio.Core.Domain.ValueObjects.Money", "Amount", b1 =>
                        {
                            b1.Property<Guid>("TransactionId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("numeric(18,2)")
                                .HasColumnName("amount");

                            b1.HasKey("TransactionId");

                            b1.ToTable("transactions");

                            b1.WithOwner()
                                .HasForeignKey("TransactionId");
                        });

                    b.Navigation("Amount")
                        .IsRequired();
                });

            modelBuilder.Entity("ArquiteturaDesafio.Core.Domain.Entities.User", b =>
                {
                    b.OwnsOne("ArquiteturaDesafio.Core.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("City");

                            b1.Property<int>("Number")
                                .HasColumnType("integer");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
                                .HasColumnName("Street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.OwnsOne("ArquiteturaDesafio.Core.Domain.ValueObjects.Geolocation", "Geolocation", b2 =>
                                {
                                    b2.Property<Guid>("AddressUserId")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("Latitude")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("Latitude");

                                    b2.Property<string>("Longitude")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("Longitude");

                                    b2.HasKey("AddressUserId");

                                    b2.ToTable("Users");

                                    b2.WithOwner()
                                        .HasForeignKey("AddressUserId");
                                });

                            b1.Navigation("Geolocation")
                                .IsRequired();
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
