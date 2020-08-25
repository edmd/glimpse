using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace glimpse.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    Method = table.Column<string>(nullable: false),
                    RequestBody = table.Column<string>(nullable: true),
                    ResponseStatus = table.Column<int>(nullable: false),
                    ResponseBody = table.Column<string>(nullable: true),
                    AcceptableResponseTimeMs = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Headers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    IsRequestHeader = table.Column<bool>(nullable: false),
                    RequestResponseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headers_RequestResponses_RequestResponseId",
                        column: x => x.RequestResponseId,
                        principalTable: "RequestResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HttpResponseEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ElapsedTime = table.Column<TimeSpan>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    ResponseType = table.Column<int>(nullable: false),
                    RequestResponseId = table.Column<Guid>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpResponseEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HttpResponseEvents_RequestResponses_RequestResponseId",
                        column: x => x.RequestResponseId,
                        principalTable: "RequestResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Headers_RequestResponseId",
                table: "Headers",
                column: "RequestResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_HttpResponseEvents_RequestResponseId",
                table: "HttpResponseEvents",
                column: "RequestResponseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headers");

            migrationBuilder.DropTable(
                name: "HttpResponseEvents");

            migrationBuilder.DropTable(
                name: "RequestResponses");
        }
    }
}
