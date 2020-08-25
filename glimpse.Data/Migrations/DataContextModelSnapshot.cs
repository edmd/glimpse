﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using glimpse.Data;

namespace glimpse.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("glimpse.Entities.Header", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsRequestHeader")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RequestResponseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestResponseId");

                    b.ToTable("Headers");
                });

            modelBuilder.Entity("glimpse.Entities.HttpResponseEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("ElapsedTime")
                        .HasColumnType("time");

                    b.Property<Guid?>("RequestResponseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ResponseType")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestResponseId");

                    b.ToTable("HttpResponseEvents");
                });

            modelBuilder.Entity("glimpse.Entities.RequestResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AcceptableResponseTimeMs")
                        .HasColumnType("int");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Interval")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResponseStatus")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RequestResponses");
                });

            modelBuilder.Entity("glimpse.Entities.Header", b =>
                {
                    b.HasOne("glimpse.Entities.RequestResponse", "RequestResponse")
                        .WithMany("Headers")
                        .HasForeignKey("RequestResponseId");
                });

            modelBuilder.Entity("glimpse.Entities.HttpResponseEvent", b =>
                {
                    b.HasOne("glimpse.Entities.RequestResponse", "RequestResponse")
                        .WithMany()
                        .HasForeignKey("RequestResponseId");
                });
#pragma warning restore 612, 618
        }
    }
}
