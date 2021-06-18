﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovigenCore.Data;

namespace MovigenCore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180910214152_AddIdUserRelationsToTables")]
    partial class AddIdUserRelationsToTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MovigenCore.Models.Cliemae", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Clcon");

                    b.Property<string>("Clcre");

                    b.Property<string>("Cld01");

                    b.Property<string>("Cld02");

                    b.Property<string>("Cld03");

                    b.Property<string>("Cld04");

                    b.Property<string>("Cld05");

                    b.Property<string>("Cld06");

                    b.Property<string>("Cld07");

                    b.Property<string>("Cldir");

                    b.Property<string>("Cldsu");

                    b.Property<string>("Cllpe");

                    b.Property<string>("Clmcr");

                    b.Property<string>("Clnom");

                    b.Property<string>("Clnsu");

                    b.Property<string>("Clobs");

                    b.Property<string>("Clord");

                    b.Property<string>("Clrut");

                    b.Property<string>("Clsuc");

                    b.Property<string>("Clven");

                    b.Property<int>("EmpresaId");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Cliemae");
                });

            modelBuilder.Entity("MovigenCore.Models.Conmae", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cocod");

                    b.Property<string>("Codes");

                    b.Property<int>("EmpresaId");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Conmae");
                });

            modelBuilder.Entity("MovigenCore.Models.Detalle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad");

                    b.Property<string>("Codigo");

                    b.Property<string>("Descripcion");

                    b.Property<double>("Descuento");

                    b.Property<int>("PedidoId");

                    b.Property<double>("Precio");

                    b.Property<double>("Total");

                    b.Property<string>("Unidad");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.ToTable("Detalle");
                });

            modelBuilder.Entity("MovigenCore.Models.Empresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("NombreFantasia")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("RazonSocial")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("Rut")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Empresa");
                });

            modelBuilder.Entity("MovigenCore.Models.Paramet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmpresaId");

                    b.Property<string>("Pacli");

                    b.Property<string>("Paedi");

                    b.Property<string>("Pag01");

                    b.Property<string>("Pag02");

                    b.Property<string>("Pag03");

                    b.Property<string>("Pag04");

                    b.Property<string>("Paiva");

                    b.Property<string>("Panom");

                    b.Property<string>("Paven");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Paramet");
                });

            modelBuilder.Entity("MovigenCore.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ComentarioAdicional");

                    b.Property<string>("CondicionPago");

                    b.Property<int>("EmpresaId");

                    b.Property<bool>("Enviado");

                    b.Property<DateTime>("Fecha");

                    b.Property<string>("RutCliente");

                    b.Property<string>("Sucursal");

                    b.Property<string>("TipoDocumento");

                    b.Property<double>("Total");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.HasIndex("UserId");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("MovigenCore.Models.Prodmae", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descuento");

                    b.Property<int>("EmpresaId");

                    b.Property<string>("Prcantpermit");

                    b.Property<string>("Prcod");

                    b.Property<string>("Prdes");

                    b.Property<string>("Prfam");

                    b.Property<string>("Prflete");

                    b.Property<string>("Prfracc");

                    b.Property<string>("Prila");

                    b.Property<string>("Prstk");

                    b.Property<string>("Prtipoflete");

                    b.Property<string>("Prun");

                    b.Property<string>("Prvl1");

                    b.Property<string>("Prvl2");

                    b.Property<string>("Prvl3");

                    b.Property<string>("Prvl4");

                    b.Property<string>("Prvl5");

                    b.Property<string>("Unidxenv");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Prodmae");
                });

            modelBuilder.Entity("MovigenCore.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<int>("EmpresaId");

                    b.HasIndex("EmpresaId");

                    b.ToTable("ApplicationUser");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovigenCore.Models.Cliemae", b =>
                {
                    b.HasOne("MovigenCore.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovigenCore.Models.Conmae", b =>
                {
                    b.HasOne("MovigenCore.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovigenCore.Models.Detalle", b =>
                {
                    b.HasOne("MovigenCore.Models.Pedido", "Pedido")
                        .WithMany("Detalles")
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovigenCore.Models.Paramet", b =>
                {
                    b.HasOne("MovigenCore.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovigenCore.Models.Pedido", b =>
                {
                    b.HasOne("MovigenCore.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovigenCore.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MovigenCore.Models.Prodmae", b =>
                {
                    b.HasOne("MovigenCore.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovigenCore.Models.ApplicationUser", b =>
                {
                    b.HasOne("MovigenCore.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
