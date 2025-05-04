using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeReporting.Data.Migrations
{
    /// <inheritdoc />
    public partial class TimeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usersList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "leaveRequestsList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaveRequestsList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_leaveRequestsList_usersList_UserId1",
                        column: x => x.UserId1,
                        principalTable: "usersList",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "workLogsList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workLogsList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workLogsList_usersList_UserId",
                        column: x => x.UserId,
                        principalTable: "usersList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "leaveRequestsList",
                columns: new[] { "Id", "EndDate", "StartDate", "Status", "Type", "UserId", "UserId1" },
                values: new object[] { 1, new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 329, null });

            migrationBuilder.InsertData(
                table: "usersList",
                columns: new[] { "Id", "Birthdate", "City", "Email", "FirstName", "LastName", "Password", "Phone", "Role" },
                values: new object[] { "329", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tel Aviv", "shoham@example.com", "Shoham", "Dahan", "123456", "0548502051", 1 });

            migrationBuilder.InsertData(
                table: "workLogsList",
                columns: new[] { "Id", "EntryTime", "ExitTime", "UserId" },
                values: new object[] { 1, new DateTime(2025, 4, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 10, 17, 0, 0, 0, DateTimeKind.Unspecified), "329" });

            migrationBuilder.CreateIndex(
                name: "IX_leaveRequestsList_UserId1",
                table: "leaveRequestsList",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_workLogsList_UserId",
                table: "workLogsList",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leaveRequestsList");

            migrationBuilder.DropTable(
                name: "workLogsList");

            migrationBuilder.DropTable(
                name: "usersList");
        }
    }
}
