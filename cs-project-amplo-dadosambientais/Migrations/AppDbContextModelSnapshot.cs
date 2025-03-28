﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using cs_project_amplo_dadosambientais.Data;

#nullable disable

namespace cs_project_amplo_dadosambientais.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("cs_project_amplo_dadosambientais.Models.AirQualityModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Data")
                        .HasColumnType("jsonb")
                        .HasColumnName("Data");

                    b.Property<DateTime>("InsertTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Obs")
                        .HasColumnType("text");

                    b.Property<Guid>("StationId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("AirQuality", (string)null);
                });

            modelBuilder.Entity("cs_project_amplo_dadosambientais.Models.StationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("InsertTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Obs")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Station", (string)null);
                });

            modelBuilder.Entity("cs_project_amplo_dadosambientais.Models.AirQualityModel", b =>
                {
                    b.HasOne("cs_project_amplo_dadosambientais.Models.StationModel", "Station")
                        .WithMany()
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });
#pragma warning restore 612, 618
        }
    }
}
