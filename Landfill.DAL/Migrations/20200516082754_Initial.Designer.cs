﻿// <auto-generated />
using System;
using Landfill.DAL.Implementation.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Landfill.DAL.Implementation.Migrations
{
    [DbContext(typeof(LandfillContext))]
    [Migration("20200516082754_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Landfill.Entities.Announcement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContentId")
                        .HasColumnType("int");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValiUntil")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ContentId")
                        .IsUnique();

                    b.ToTable("announcements");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            ContentId = 4,
                            Header = "Short header",
                            ValiUntil = new DateTime(2020, 5, 26, 11, 27, 54, 396, DateTimeKind.Local).AddTicks(5318)
                        },
                        new
                        {
                            Id = 5,
                            ContentId = 5,
                            Header = "Long header",
                            ValiUntil = new DateTime(2020, 6, 5, 11, 27, 54, 401, DateTimeKind.Local).AddTicks(4310)
                        },
                        new
                        {
                            Id = 6,
                            ContentId = 6,
                            Header = "New header",
                            ValiUntil = new DateTime(2020, 6, 15, 11, 27, 54, 401, DateTimeKind.Local).AddTicks(4371)
                        });
                });

            modelBuilder.Entity("Landfill.Entities.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("contents");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContentType = "FAQ",
                            State = "Published"
                        },
                        new
                        {
                            Id = 2,
                            ContentType = "FAQ",
                            State = "Modified"
                        },
                        new
                        {
                            Id = 3,
                            ContentType = "FAQ",
                            State = "Deleted"
                        },
                        new
                        {
                            Id = 4,
                            ContentType = "Announcement",
                            State = "Deleted"
                        },
                        new
                        {
                            Id = 5,
                            ContentType = "Announcement",
                            State = "Modified"
                        },
                        new
                        {
                            Id = 6,
                            ContentType = "Announcement",
                            State = "Published"
                        });
                });

            modelBuilder.Entity("Landfill.Entities.ContentTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContentId")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContentId");

                    b.ToTable("contentTranslations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContentId = 1,
                            Language = "UA",
                            Text = "Питання 1"
                        },
                        new
                        {
                            Id = 2,
                            ContentId = 2,
                            Language = "UA",
                            Text = "Питання 2"
                        },
                        new
                        {
                            Id = 3,
                            ContentId = 3,
                            Language = "UA",
                            Text = "Питання 3"
                        },
                        new
                        {
                            Id = 4,
                            ContentId = 1,
                            Language = "EN",
                            Text = "Qwestion 1"
                        },
                        new
                        {
                            Id = 5,
                            ContentId = 2,
                            Language = "EN",
                            Text = "Qwestion 2"
                        },
                        new
                        {
                            Id = 6,
                            ContentId = 3,
                            Language = "EN",
                            Text = "Qwestion 3"
                        },
                        new
                        {
                            Id = 7,
                            ContentId = 4,
                            Language = "UA",
                            Text = "Оголошення 1"
                        },
                        new
                        {
                            Id = 8,
                            ContentId = 5,
                            Language = "UA",
                            Text = "Оголошення 2"
                        },
                        new
                        {
                            Id = 9,
                            ContentId = 6,
                            Language = "UA",
                            Text = "Оголошення 3"
                        },
                        new
                        {
                            Id = 10,
                            ContentId = 4,
                            Language = "EN",
                            Text = "Annnouncement 1"
                        },
                        new
                        {
                            Id = 11,
                            ContentId = 5,
                            Language = "EN",
                            Text = "Annnouncement 2"
                        },
                        new
                        {
                            Id = 12,
                            ContentId = 6,
                            Language = "EN",
                            Text = "Annnouncement 3"
                        });
                });

            modelBuilder.Entity("Landfill.Entities.FAQ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContentId")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContentId")
                        .IsUnique();

                    b.ToTable("faqs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContentId = 1,
                            Tag = "First tag"
                        },
                        new
                        {
                            Id = 2,
                            ContentId = 2,
                            Tag = "Second tag"
                        },
                        new
                        {
                            Id = 3,
                            ContentId = 3,
                            Tag = "Third tag"
                        });
                });

            modelBuilder.Entity("Landfill.Entities.Announcement", b =>
                {
                    b.HasOne("Landfill.Entities.Content", "Content")
                        .WithOne("Announcement")
                        .HasForeignKey("Landfill.Entities.Announcement", "ContentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Landfill.Entities.ContentTranslation", b =>
                {
                    b.HasOne("Landfill.Entities.Content", "Content")
                        .WithMany("Translations")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Landfill.Entities.FAQ", b =>
                {
                    b.HasOne("Landfill.Entities.Content", "Content")
                        .WithOne("Faq")
                        .HasForeignKey("Landfill.Entities.FAQ", "ContentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
