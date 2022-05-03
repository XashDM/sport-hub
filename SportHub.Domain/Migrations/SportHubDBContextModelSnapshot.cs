﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportHub.Domain;

#nullable disable

namespace SportHub.Domain.Migrations
{
    [DbContext(typeof(SportHubDBContext))]
    partial class SportHubDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SportHub.Domain.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

<<<<<<< HEAD
                    b.Property<string>("Image")
=======
                    b.Property<string>("ContentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageLink")
>>>>>>> fe0d1efdd5152011ffe10601b37c916dd187c0ac
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

<<<<<<< HEAD
                    b.Property<int>("ReferenceItemId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
=======
                    b.Property<int?>("ReferenceItemId")
                        .HasColumnType("int");
>>>>>>> fe0d1efdd5152011ffe10601b37c916dd187c0ac

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReferenceItemId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("SportHub.Domain.Models.NavigationItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

<<<<<<< HEAD
                    b.Property<int?>("ParentsItemId")
=======
                    b.Property<int?>("ParentItemId")
>>>>>>> fe0d1efdd5152011ffe10601b37c916dd187c0ac
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

<<<<<<< HEAD
                    b.HasIndex("ParentsItemId");
=======
                    b.HasIndex("ParentItemId");
>>>>>>> fe0d1efdd5152011ffe10601b37c916dd187c0ac

                    b.ToTable("NavigationItems");
                });

            modelBuilder.Entity("SportHub.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SportHub.Domain.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

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
                    b.HasOne("SportHub.Domain.Models.NavigationItem", "ReferenceItem")
                        .WithMany()
<<<<<<< HEAD
                        .HasForeignKey("ReferenceItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
=======
                        .HasForeignKey("ReferenceItemId");
>>>>>>> fe0d1efdd5152011ffe10601b37c916dd187c0ac

                    b.Navigation("ReferenceItem");
                });

            modelBuilder.Entity("SportHub.Domain.Models.NavigationItem", b =>
                {
<<<<<<< HEAD
                    b.HasOne("SportHub.Domain.Models.NavigationItem", "ParentsItem")
                        .WithMany("Children")
                        .HasForeignKey("ParentsItemId");

                    b.Navigation("ParentsItem");
=======
                    b.HasOne("SportHub.Domain.Models.NavigationItem", "ParentItem")
                        .WithMany()
                        .HasForeignKey("ParentItemId");

                    b.Navigation("ParentItem");
>>>>>>> fe0d1efdd5152011ffe10601b37c916dd187c0ac
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

            modelBuilder.Entity("SportHub.Domain.Models.NavigationItem", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
