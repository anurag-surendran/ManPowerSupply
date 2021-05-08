using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManPowerSupplyAPI.Migrations
{
    public partial class IncomeAndExpenditureReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReceiptAndPaymentId",
                table: "EmployeePayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReceiptAndPaymentId",
                table: "CustomerPayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReceiptAndPaymentId",
                table: "Attendance",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountHead",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AccountGroupId = table.Column<long>(type: "bigint", nullable: false),
                    IsDualAccount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountHead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountHead_AccountGroup_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReceiptAndPayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountHeadId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<long>(type: "bigint", maxLength: 18, nullable: false),
                    Particular = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptAndPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptAndPayment_AccountHead_AccountHeadId",
                        column: x => x.AccountHeadId,
                        principalTable: "AccountHead",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AccountGroup",
                columns: new[] { "Id", "AccountType", "Description", "IsDeleted", "LastUpdatedBy", "LastUpdatedDate", "Name" },
                values: new object[,]
                {
                    { 1L, 3, null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Other Income" },
                    { 2L, 4, null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Other Expense" },
                    { 3L, 2, null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Capital" },
                    { 4L, 1, null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bank" }
                });

            migrationBuilder.InsertData(
                table: "AccountHead",
                columns: new[] { "Id", "AccountGroupId", "Description", "IsDeleted", "LastUpdatedBy", "LastUpdatedDate", "Name" },
                values: new object[] { 2L, 1L, null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cutomer Receipt" });

            migrationBuilder.InsertData(
                table: "AccountHead",
                columns: new[] { "Id", "AccountGroupId", "Description", "IsDeleted", "LastUpdatedBy", "LastUpdatedDate", "Name" },
                values: new object[] { 1L, 2L, null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Company TA" });

            migrationBuilder.InsertData(
                table: "AccountHead",
                columns: new[] { "Id", "AccountGroupId", "Description", "IsDeleted", "LastUpdatedBy", "LastUpdatedDate", "Name" },
                values: new object[] { 3L, 2L, null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee Payment" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayment_ReceiptAndPaymentId",
                table: "EmployeePayment",
                column: "ReceiptAndPaymentId",
                unique: true,
                filter: "[ReceiptAndPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayment_ReceiptAndPaymentId",
                table: "CustomerPayment",
                column: "ReceiptAndPaymentId",
                unique: true,
                filter: "[ReceiptAndPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_ReceiptAndPaymentId",
                table: "Attendance",
                column: "ReceiptAndPaymentId",
                unique: true,
                filter: "[ReceiptAndPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroup_Name",
                table: "AccountGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountHead_AccountGroupId",
                table: "AccountHead",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptAndPayment_AccountHeadId",
                table: "ReceiptAndPayment",
                column: "AccountHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_ReceiptAndPayment_ReceiptAndPaymentId",
                table: "Attendance",
                column: "ReceiptAndPaymentId",
                principalTable: "ReceiptAndPayment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPayment_ReceiptAndPayment_ReceiptAndPaymentId",
                table: "CustomerPayment",
                column: "ReceiptAndPaymentId",
                principalTable: "ReceiptAndPayment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeePayment_ReceiptAndPayment_ReceiptAndPaymentId",
                table: "EmployeePayment",
                column: "ReceiptAndPaymentId",
                principalTable: "ReceiptAndPayment",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_ReceiptAndPayment_ReceiptAndPaymentId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPayment_ReceiptAndPayment_ReceiptAndPaymentId",
                table: "CustomerPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeePayment_ReceiptAndPayment_ReceiptAndPaymentId",
                table: "EmployeePayment");

            migrationBuilder.DropTable(
                name: "ReceiptAndPayment");

            migrationBuilder.DropTable(
                name: "AccountHead");

            migrationBuilder.DropTable(
                name: "AccountGroup");

            migrationBuilder.DropIndex(
                name: "IX_EmployeePayment_ReceiptAndPaymentId",
                table: "EmployeePayment");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPayment_ReceiptAndPaymentId",
                table: "CustomerPayment");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_ReceiptAndPaymentId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "ReceiptAndPaymentId",
                table: "EmployeePayment");

            migrationBuilder.DropColumn(
                name: "ReceiptAndPaymentId",
                table: "CustomerPayment");

            migrationBuilder.DropColumn(
                name: "ReceiptAndPaymentId",
                table: "Attendance");
        }
    }
}
