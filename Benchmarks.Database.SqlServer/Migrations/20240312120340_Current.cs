using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Benchmarks.Database.SqlServer.Migrations
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                    DateTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LongInteger = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusteredIndexes", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "NonClusteredIndexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                    DateTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LongInteger = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonClusteredIndexes", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "SimpleEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                    DateTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
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
