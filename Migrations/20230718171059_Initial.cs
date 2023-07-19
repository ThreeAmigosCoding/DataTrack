using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataTrack.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnalogInput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Driver = table.Column<string>(type: "longtext", nullable: false),
                    IOAddress = table.Column<string>(type: "longtext", nullable: false),
                    ScanTime = table.Column<int>(type: "int", nullable: false),
                    ScanOn = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LowLimit = table.Column<double>(type: "double", nullable: false),
                    HighLimit = table.Column<double>(type: "double", nullable: false),
                    Unit = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogInput", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnalogOutput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    IOAddress = table.Column<string>(type: "longtext", nullable: false),
                    LowLimit = table.Column<double>(type: "double", nullable: false),
                    HighLimit = table.Column<double>(type: "double", nullable: false),
                    Unit = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogOutput", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DigitalInput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Driver = table.Column<string>(type: "longtext", nullable: false),
                    IOAddress = table.Column<string>(type: "longtext", nullable: false),
                    ScanTime = table.Column<int>(type: "int", nullable: false),
                    ScanOn = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalInput", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DigitalOutput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    IOAddress = table.Column<string>(type: "longtext", nullable: false),
                    InitialValue = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalOutput", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Alarm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EdgeValue = table.Column<double>(type: "double", nullable: false),
                    Unit = table.Column<string>(type: "longtext", nullable: false),
                    AnalogInputId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alarm_AnalogInput_AnalogInputId",
                        column: x => x.AnalogInputId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    AnalogInputId = table.Column<Guid>(type: "char(36)", nullable: true),
                    DigitalInputId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AnalogInput_AnalogInputId",
                        column: x => x.AnalogInputId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_DigitalInput_DigitalInputId",
                        column: x => x.DigitalInputId,
                        principalTable: "DigitalInput",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Alarm_AnalogInputId",
                table: "Alarm",
                column: "AnalogInputId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AnalogInputId",
                table: "Users",
                column: "AnalogInputId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DigitalInputId",
                table: "Users",
                column: "DigitalInputId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarm");

            migrationBuilder.DropTable(
                name: "AnalogOutput");

            migrationBuilder.DropTable(
                name: "DigitalOutput");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AnalogInput");

            migrationBuilder.DropTable(
                name: "DigitalInput");
        }
    }
}
