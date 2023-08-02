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
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    IOAddress = table.Column<string>(type: "longtext", nullable: false),
                    LowerBound = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "double", nullable: false),
                    UpperBound = table.Column<int>(type: "int", nullable: false),
                    IsDigital = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Driver = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DigitalInput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    Admin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RegisteredById = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_RegisteredById",
                        column: x => x.RegisteredById,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                name: "AnalogInputRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AnalogInputId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Value = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogInputRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalogInputRecord_AnalogInput_AnalogInputId",
                        column: x => x.AnalogInputId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DigitalInputRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DigitalInputId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Value = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalInputRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalInputRecord_DigitalInput_DigitalInputId",
                        column: x => x.DigitalInputId,
                        principalTable: "DigitalInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersAnalogInputs",
                columns: table => new
                {
                    AnalogInputsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UsersId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAnalogInputs", x => new { x.AnalogInputsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UsersAnalogInputs_AnalogInput_AnalogInputsId",
                        column: x => x.AnalogInputsId,
                        principalTable: "AnalogInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersAnalogInputs_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersDigitalInputs",
                columns: table => new
                {
                    DigitalInputsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UsersId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDigitalInputs", x => new { x.DigitalInputsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UsersDigitalInputs_DigitalInput_DigitalInputsId",
                        column: x => x.DigitalInputsId,
                        principalTable: "DigitalInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersDigitalInputs_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlarmRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    AlarmId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Value = table.Column<double>(type: "double", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlarmRecords_Alarm_AlarmId",
                        column: x => x.AlarmId,
                        principalTable: "Alarm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Alarm_AnalogInputId",
                table: "Alarm",
                column: "AnalogInputId");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmRecords_AlarmId",
                table: "AlarmRecords",
                column: "AlarmId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalogInputRecord_AnalogInputId",
                table: "AnalogInputRecord",
                column: "AnalogInputId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalInputRecord_DigitalInputId",
                table: "DigitalInputRecord",
                column: "DigitalInputId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RegisteredById",
                table: "Users",
                column: "RegisteredById");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAnalogInputs_UsersId",
                table: "UsersAnalogInputs",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDigitalInputs_UsersId",
                table: "UsersDigitalInputs",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlarmRecords");

            migrationBuilder.DropTable(
                name: "AnalogInputRecord");

            migrationBuilder.DropTable(
                name: "AnalogOutput");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "DigitalInputRecord");

            migrationBuilder.DropTable(
                name: "DigitalOutput");

            migrationBuilder.DropTable(
                name: "UsersAnalogInputs");

            migrationBuilder.DropTable(
                name: "UsersDigitalInputs");

            migrationBuilder.DropTable(
                name: "Alarm");

            migrationBuilder.DropTable(
                name: "DigitalInput");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AnalogInput");
        }
    }
}
