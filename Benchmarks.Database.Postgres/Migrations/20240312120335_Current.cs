using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Benchmarks.Database.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Current : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClusteredIndexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(42)", maxLength: 42, nullable: false),
                    DateTimeUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LongInteger = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusteredIndexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonClusteredIndexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(42)", maxLength: 42, nullable: false),
                    DateTimeUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LongInteger = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonClusteredIndexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(42)", maxLength: 42, nullable: false),
                    DateTimeUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LongInteger = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleEntities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClusteredIndexes");

            migrationBuilder.DropTable(
                name: "NonClusteredIndexes");

            migrationBuilder.DropTable(
                name: "SimpleEntities");
        }
    }
}
