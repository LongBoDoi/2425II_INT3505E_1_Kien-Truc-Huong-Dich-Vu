﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PastebinBackend.Data;

#nullable disable

namespace PastebinBackend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("PastebinBackend.Models.Analytic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("PasteId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ViewDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ViewsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PasteId");

                    b.ToTable("Analytics");
                });

            modelBuilder.Entity("PastebinBackend.Models.Paste", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Exposure")
                        .HasColumnType("int");

                    b.Property<string>("PasteKey")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PasteKey")
                        .IsUnique();

                    b.ToTable("Pastes");
                });

            modelBuilder.Entity("PastebinBackend.Models.Analytic", b =>
                {
                    b.HasOne("PastebinBackend.Models.Paste", "Paste")
                        .WithMany()
                        .HasForeignKey("PasteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paste");
                });
#pragma warning restore 612, 618
        }
    }
}
