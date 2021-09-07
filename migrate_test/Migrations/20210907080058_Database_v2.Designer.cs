﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using migrate_test.Models;

namespace migrate_test.Migrations
{
    [DbContext(typeof(LDMContext))]
    [Migration("20210907080058_Database_v2")]
    partial class Database_v2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("migrate_test.Models.Image", b =>
                {
                    b.Property<string>("ImageID")
                        .HasColumnType("TEXT")
                        .HasColumnName("IMAGE_ID");

                    b.Property<string>("ImageCode")
                        .HasColumnType("TEXT")
                        .HasColumnName("IMAGE_CODE");

                    b.Property<int>("ImageNO")
                        .HasColumnType("INTEGER")
                        .HasColumnName("IMAGE_NO");

                    b.Property<string>("ImageScheme")
                        .HasColumnType("TEXT")
                        .HasColumnName("IMAGE_SCHEMA");

                    b.Property<string>("OriginalFilename")
                        .HasColumnType("TEXT")
                        .HasColumnName("ORIGINAL_FILENAME");

                    b.Property<int>("SampleID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ImageID");

                    b.HasIndex("SampleID");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("migrate_test.Models.Sample", b =>
                {
                    b.Property<int>("SampleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("SAMPLE_ID");

                    b.Property<string>("DatasetID")
                        .HasColumnType("TEXT")
                        .HasColumnName("DATASET_ID");

                    b.Property<int>("ImageCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("IMAGE_COUNT");

                    b.Property<string>("Metadata")
                        .HasColumnType("TEXT")
                        .HasColumnName("METADATA");

                    b.Property<int>("SampleType")
                        .HasColumnType("INTEGER")
                        .HasColumnName("SAMPLE_TYPE");

                    b.HasKey("SampleID");

                    b.ToTable("Sample");
                });

            modelBuilder.Entity("migrate_test.Models.Image", b =>
                {
                    b.HasOne("migrate_test.Models.Sample", null)
                        .WithMany("Images")
                        .HasForeignKey("SampleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("migrate_test.Models.Sample", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
