using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeReporting.Data.Migrations
{
    /// <inheritdoc />
    public partial class TIME_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leaveRequestsList_usersList_UserId1",
                table: "leaveRequestsList");

            migrationBuilder.DropForeignKey(
                name: "FK_workLogsList_usersList_UserId",
                table: "workLogsList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workLogsList",
                table: "workLogsList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usersList",
                table: "usersList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_leaveRequestsList",
                table: "leaveRequestsList");

            migrationBuilder.DropIndex(
                name: "IX_leaveRequestsList_UserId1",
                table: "leaveRequestsList");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "leaveRequestsList");

            migrationBuilder.RenameTable(
                name: "workLogsList",
                newName: "WorkLogs");

            migrationBuilder.RenameTable(
                name: "usersList",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "leaveRequestsList",
                newName: "LeaveRequests");

            migrationBuilder.RenameIndex(
                name: "IX_workLogsList_UserId",
                table: "WorkLogs",
                newName: "IX_WorkLogs_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LeaveRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkLogs",
                table: "WorkLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveRequests",
                table: "LeaveRequests",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Type", "UserId" },
                values: new object[] { 1, "329" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_UserId",
                table: "LeaveRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Users_UserId",
                table: "LeaveRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogs_Users_UserId",
                table: "WorkLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Users_UserId",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogs_Users_UserId",
                table: "WorkLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkLogs",
                table: "WorkLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveRequests",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_UserId",
                table: "LeaveRequests");

            migrationBuilder.RenameTable(
                name: "WorkLogs",
                newName: "workLogsList");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "usersList");

            migrationBuilder.RenameTable(
                name: "LeaveRequests",
                newName: "leaveRequestsList");

            migrationBuilder.RenameIndex(
                name: "IX_WorkLogs_UserId",
                table: "workLogsList",
                newName: "IX_workLogsList_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "leaveRequestsList",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "leaveRequestsList",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_workLogsList",
                table: "workLogsList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usersList",
                table: "usersList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_leaveRequestsList",
                table: "leaveRequestsList",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "leaveRequestsList",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Type", "UserId", "UserId1" },
                values: new object[] { 0, 329, null });

            migrationBuilder.CreateIndex(
                name: "IX_leaveRequestsList_UserId1",
                table: "leaveRequestsList",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_leaveRequestsList_usersList_UserId1",
                table: "leaveRequestsList",
                column: "UserId1",
                principalTable: "usersList",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_workLogsList_usersList_UserId",
                table: "workLogsList",
                column: "UserId",
                principalTable: "usersList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
