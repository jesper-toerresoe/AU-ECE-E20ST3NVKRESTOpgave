using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HL7FHIRClient.Migrations
{
    public partial class IntialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPMCompleteSequences",
                columns: table => new
                {
                    BPMCompleteSequenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameOfObject = table.Column<string>(type: "TEXT", nullable: true),
                    DurationInSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BPMCounts = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPMCompleteSequences", x => x.BPMCompleteSequenceId);
                });

            migrationBuilder.CreateTable(
                name: "BPMLocalSampleSequences",
                columns: table => new
                {
                    BPMLocalSampleSequenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SequenceNo = table.Column<long>(type: "INTEGER", nullable: false),
                    NoBPMValues = table.Column<long>(type: "INTEGER", nullable: false),
                    BPMSamples = table.Column<byte[]>(type: "BLOB", nullable: true),
                    BPMCompleteSequenceId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPMLocalSampleSequences", x => x.BPMLocalSampleSequenceId);
                    table.ForeignKey(
                        name: "FK_BPMLocalSampleSequences_BPMCompleteSequences_BPMCompleteSequenceId",
                        column: x => x.BPMCompleteSequenceId,
                        principalTable: "BPMCompleteSequences",
                        principalColumn: "BPMCompleteSequenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BPMLocalSampleSequences_BPMCompleteSequenceId",
                table: "BPMLocalSampleSequences",
                column: "BPMCompleteSequenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPMLocalSampleSequences");

            migrationBuilder.DropTable(
                name: "BPMCompleteSequences");
        }
    }
}
