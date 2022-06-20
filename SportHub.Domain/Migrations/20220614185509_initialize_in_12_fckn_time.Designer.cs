﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportHub.Domain;

#nullable disable

namespace SportHub.Domain.Migrations
{
    [DbContext(typeof(SportHubDBContext))]
<<<<<<<< HEAD:SportHub.Domain/Migrations/20220614185509_initialize_in_12_fckn_time.Designer.cs
    [Migration("20220614185509_initialize_in_12_fckn_time")]
    partial class initialize_in_12_fckn_time
========
    [Migration("20220620170304_initial_n22")]
    partial class initial_n22
>>>>>>>> main:SportHub.Domain/Migrations/20220620170304_initial_n22.Designer.cs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SportHub.Domain.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ContentText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ImageItemId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPublished")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("ReferenceItemId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ImageItemId");

                    b.HasIndex("ReferenceItemId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("SportHub.Domain.Models.DisplayedLanguage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("DisplayedLanguages");
                });

            modelBuilder.Entity("SportHub.Domain.Models.DisplayItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("DisplayLocation")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("ImageItemId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDisplayed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("ImageItemId");

                    b.ToTable("DisplayItems");
                });

            modelBuilder.Entity("SportHub.Domain.Models.ImageItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Alt")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ImageLink")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhotoTitle")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.ToTable("ImageItems");
                });

            modelBuilder.Entity("SportHub.Domain.Models.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("SportHub.Domain.Models.NavigationItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("ParentsItemId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ParentsItemId");

                    b.ToTable("NavigationItems");
                });

            modelBuilder.Entity("SportHub.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("varchar(320)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("char(64)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SportHub.Domain.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.HasAlternateKey("RoleName");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleName = "User"
                        },
                        new
                        {
                            Id = 2,
                            RoleName = "Admin"
                        });
                });

            modelBuilder.Entity("UserUserRole", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserUserRole");
                });

            modelBuilder.Entity("SportHub.Domain.Models.Article", b =>
                {
                    b.HasOne("SportHub.Domain.Models.ImageItem", "ImageItem")
                        .WithMany()
                        .HasForeignKey("ImageItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SportHub.Domain.Models.NavigationItem", "ReferenceItem")
                        .WithMany()
                        .HasForeignKey("ReferenceItemId");

                    b.Navigation("ImageItem");

                    b.Navigation("ReferenceItem");
                });

            modelBuilder.Entity("SportHub.Domain.Models.DisplayItem", b =>
                {
                    b.HasOne("SportHub.Domain.Models.Article", "Article")
                        .WithMany("DisplayItems")
                        .HasForeignKey("ArticleId");

                    b.HasOne("SportHub.Domain.Models.ImageItem", "ImageItem")
                        .WithMany()
                        .HasForeignKey("ImageItemId");

                    b.Navigation("Article");

                    b.Navigation("ImageItem");
                });

            modelBuilder.Entity("SportHub.Domain.Models.NavigationItem", b =>
                {
                    b.HasOne("SportHub.Domain.Models.NavigationItem", "ParentsItem")
                        .WithMany()
                        .HasForeignKey("ParentsItemId");

                    b.Navigation("ParentsItem");
                });

            modelBuilder.Entity("UserUserRole", b =>
                {
                    b.HasOne("SportHub.Domain.Models.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SportHub.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SportHub.Domain.Models.Article", b =>
                {
                    b.Navigation("DisplayItems");
                });
#pragma warning restore 612, 618
        }
    }
}
