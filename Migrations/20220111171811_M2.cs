using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Events.API.Migrations
{
    public partial class M2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Events",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("11ce940f-0997-463a-b850-6d5a866a07a7"),
                columns: new[] { "EndDateTime", "StartDateTime" },
                values: new object[] { new DateTime(2022, 1, 18, 11, 18, 10, 333, DateTimeKind.Local).AddTicks(6452), new DateTime(2022, 1, 18, 9, 18, 10, 333, DateTimeKind.Local).AddTicks(5628) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("21ce940f-0997-463a-b850-6d5a866a07a7"),
                columns: new[] { "EndDateTime", "StartDateTime" },
                values: new object[] { new DateTime(2022, 1, 28, 13, 18, 10, 333, DateTimeKind.Local).AddTicks(7102), new DateTime(2022, 1, 28, 9, 18, 10, 333, DateTimeKind.Local).AddTicks(7076) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("31ce940f-0997-463a-b850-6d5a866a07a7"),
                columns: new[] { "EndDateTime", "StartDateTime" },
                values: new object[] { new DateTime(2022, 1, 21, 12, 18, 10, 333, DateTimeKind.Local).AddTicks(7138), new DateTime(2022, 1, 21, 9, 18, 10, 333, DateTimeKind.Local).AddTicks(7133) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "AmitKumar@events.com",
                column: "DateOfBirth",
                value: new DateTime(1994, 1, 11, 9, 18, 10, 331, DateTimeKind.Local).AddTicks(5814));

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "DavidJones@events.com",
                column: "DateOfBirth",
                value: new DateTime(1972, 1, 11, 9, 18, 10, 331, DateTimeKind.Local).AddTicks(5723));

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "GeorgeJensen@events.com",
                column: "DateOfBirth",
                value: new DateTime(1922, 1, 11, 9, 18, 10, 331, DateTimeKind.Local).AddTicks(5804));

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "RajRurhi@events.com",
                column: "DateOfBirth",
                value: new DateTime(1987, 1, 11, 9, 18, 10, 325, DateTimeKind.Local).AddTicks(2342));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("11ce940f-0997-463a-b850-6d5a866a07a7"),
                columns: new[] { "EndDateTime", "StartDateTime" },
                values: new object[] { new DateTime(2022, 1, 13, 1, 35, 54, 68, DateTimeKind.Local).AddTicks(2406), new DateTime(2022, 1, 12, 23, 35, 54, 68, DateTimeKind.Local).AddTicks(1914) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("21ce940f-0997-463a-b850-6d5a866a07a7"),
                columns: new[] { "EndDateTime", "StartDateTime" },
                values: new object[] { new DateTime(2022, 1, 23, 3, 35, 54, 68, DateTimeKind.Local).AddTicks(2973), new DateTime(2022, 1, 22, 23, 35, 54, 68, DateTimeKind.Local).AddTicks(2946) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("31ce940f-0997-463a-b850-6d5a866a07a7"),
                columns: new[] { "EndDateTime", "StartDateTime" },
                values: new object[] { new DateTime(2022, 1, 16, 2, 35, 54, 68, DateTimeKind.Local).AddTicks(3000), new DateTime(2022, 1, 15, 23, 35, 54, 68, DateTimeKind.Local).AddTicks(2996) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "AmitKumar@events.com",
                column: "DateOfBirth",
                value: new DateTime(1994, 1, 5, 23, 35, 54, 66, DateTimeKind.Local).AddTicks(3332));

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "DavidJones@events.com",
                column: "DateOfBirth",
                value: new DateTime(1972, 1, 5, 23, 35, 54, 66, DateTimeKind.Local).AddTicks(3249));

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "GeorgeJensen@events.com",
                column: "DateOfBirth",
                value: new DateTime(1922, 1, 5, 23, 35, 54, 66, DateTimeKind.Local).AddTicks(3323));

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Email",
                keyValue: "RajRurhi@events.com",
                column: "DateOfBirth",
                value: new DateTime(1987, 1, 5, 23, 35, 54, 58, DateTimeKind.Local).AddTicks(5388));
        }
    }
}
