using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class setDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Log");

            migrationBuilder.CreateTable(
                name: "EntityToInspects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityToInspects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLog",
                schema: "Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueryString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnerExceptionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnerExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inspectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionVisits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityToInspectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionVisits_EntityToInspects_EntityToInspectId",
                        column: x => x.EntityToInspectId,
                        principalTable: "EntityToInspects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InspectionVisits_Inspectors_InspectorId",
                        column: x => x.InspectorId,
                        principalTable: "Inspectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Violations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionVisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Violations_InspectionVisits_InspectionVisitId",
                        column: x => x.InspectionVisitId,
                        principalTable: "InspectionVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionVisits_EntityToInspectId",
                table: "InspectionVisits",
                column: "EntityToInspectId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionVisits_InspectorId",
                table: "InspectionVisits",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_InspectionVisitId",
                table: "Violations",
                column: "InspectionVisitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionLog",
                schema: "Log");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.DropTable(
                name: "InspectionVisits");

            migrationBuilder.DropTable(
                name: "EntityToInspects");

            migrationBuilder.DropTable(
                name: "Inspectors");
        }
    }
}
