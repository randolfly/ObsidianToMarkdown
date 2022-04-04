﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ObsidianToMarkdown.Context;

#nullable disable

namespace ObsidianToMarkdown.Migrations
{
    [DbContext(typeof(ObsidianFileInfoContext))]
    [Migration("20220404112246_InitMigrate")]
    partial class InitMigrate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("ObsidianToMarkdown.Context.ObsidianFileInfo", b =>
                {
                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sha256")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Path");

                    b.ToTable("ObsidianFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
