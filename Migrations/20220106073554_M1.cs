using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Events.API.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    EventType = table.Column<string>(nullable: true),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "PersonEvents",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    PersonEmail = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEvents", x => new { x.PersonEmail, x.EventId });
                    table.ForeignKey(
                        name: "FK_PersonEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonEvents_People_PersonEmail",
                        column: x => x.PersonEmail,
                        principalTable: "People",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "EndDateTime", "EventType", "StartDateTime", "Title" },
                values: new object[,]
                {
                    { new Guid("11ce940f-0997-463a-b850-6d5a866a07a7"), "Raj's birthday celebration description. Raj's birthday celebration description. Raj's birthday celebration description.", new DateTime(2022, 1, 13, 1, 35, 54, 68, DateTimeKind.Local).AddTicks(2406), "Indoor", new DateTime(2022, 1, 12, 23, 35, 54, 68, DateTimeKind.Local).AddTicks(1914), "Raj's birthday belebration" },
                    { new Guid("21ce940f-0997-463a-b850-6d5a866a07a7"), "David's marriage anniversary celebration. David's marriage anniversary celebration. David's marriage anniversary celebration.", new DateTime(2022, 1, 23, 3, 35, 54, 68, DateTimeKind.Local).AddTicks(2973), "Outdoor", new DateTime(2022, 1, 22, 23, 35, 54, 68, DateTimeKind.Local).AddTicks(2946), "David's marriage anniversary celebration" },
                    { new Guid("31ce940f-0997-463a-b850-6d5a866a07a7"), "Georg's work anniversary celebration. Georg's work anniversary celebration. Georg's work anniversary celebration.", new DateTime(2022, 1, 16, 2, 35, 54, 68, DateTimeKind.Local).AddTicks(3000), "Indoor", new DateTime(2022, 1, 15, 23, 35, 54, 68, DateTimeKind.Local).AddTicks(2996), "Georg's work anniversary celebration" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Email", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { "RajRurhi@events.com", new DateTime(1987, 1, 5, 23, 35, 54, 58, DateTimeKind.Local).AddTicks(5388), "Raj", "Rurhi" },
                    { "DavidJones@events.com", new DateTime(1972, 1, 5, 23, 35, 54, 66, DateTimeKind.Local).AddTicks(3249), "David", "Jones" },
                    { "GeorgeJensen@events.com", new DateTime(1922, 1, 5, 23, 35, 54, 66, DateTimeKind.Local).AddTicks(3323), "Georg", "Jensen" },
                    { "AmitKumar@events.com", new DateTime(1994, 1, 5, 23, 35, 54, 66, DateTimeKind.Local).AddTicks(3332), "Amit", "Kumar" }
                });

            migrationBuilder.InsertData(
                table: "PersonEvents",
                columns: new[] { "PersonEmail", "EventId" },
                values: new object[,]
                {
                    { "RajRurhi@events.com", new Guid("11ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "RajRurhi@events.com", new Guid("31ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "DavidJones@events.com", new Guid("11ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "DavidJones@events.com", new Guid("21ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "GeorgeJensen@events.com", new Guid("11ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "GeorgeJensen@events.com", new Guid("21ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "GeorgeJensen@events.com", new Guid("31ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "AmitKumar@events.com", new Guid("21ce940f-0997-463a-b850-6d5a866a07a7") },
                    { "AmitKumar@events.com", new Guid("31ce940f-0997-463a-b850-6d5a866a07a7") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonEvents_EventId",
                table: "PersonEvents",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonEvents");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
