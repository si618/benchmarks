﻿// <auto-generated />
using System;
using Benchmarks.Database.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Benchmarks.Database.SqlServer.Migrations
{
    [DbContext(typeof(SqlServerDbContext))]
    [Migration("20240311045217_Current")]
    partial class Current
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Benchmarks.Core.ClusteredIndex", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateTimeUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("Decimal")
                        .HasColumnType("decimal(20, 5)");

                    b.Property<long>("LongInteger")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("ClusteredIndexes");
                });

            modelBuilder.Entity("Benchmarks.Core.NonClusteredIndex", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateTimeUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("Decimal")
                        .HasColumnType("decimal(20, 5)");

                    b.Property<long>("LongInteger")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.ToTable("NonClusteredIndexes");
                });
#pragma warning restore 612, 618
        }
    }
}
