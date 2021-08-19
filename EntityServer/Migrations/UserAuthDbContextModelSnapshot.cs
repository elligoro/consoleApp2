﻿// <auto-generated />
using System;
using EntityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityServer.Migrations
{
    [DbContext(typeof(UserAuthDbContext))]
    partial class UserAuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EntityServer.Token", b =>
                {
                    b.Property<Guid>("Tid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aud")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Exp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Sub")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Tid");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("EntityServer.UserCreds", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("UsersCreds");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Password = "CE7A35F64C1EE87D13BA91420D48639506BA8AC2432B7B14858D4651FDD69631",
                            Username = "simon"
                        },
                        new
                        {
                            Id = 2L,
                            Password = "3BAC7D66695F0726CCA3969547ABA728C7DE6E4AD774379D7332DDD4F5E38265",
                            Username = "lake"
                        },
                        new
                        {
                            Id = 3L,
                            Password = "C64AC0577A66787781E7C3391DCD48BB2D74C84BEA6D239A51E9E8B4F034B6E2",
                            Username = "palmer"
                        },
                        new
                        {
                            Id = 4L,
                            Password = "6899ED087724E0D9DFCD2C8208FCEC323295FBA436E02022539AA7D60AA18B73",
                            Username = "george"
                        },
                        new
                        {
                            Id = 5L,
                            Password = "9A16F64870AEEB202F411964046006E294390F81EB740F54FE8F885D2A6D4086",
                            Username = "barak"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}