using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Landfill.DAL.Implementation.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(nullable: false),
                    ValiUntil = table.Column<DateTime>(nullable: false),
                    ContentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_announcements_contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contentTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contentTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contentTranslations_contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "faqs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(nullable: false),
                    ContentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_faqs_contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "contents",
                columns: new[] { "Id", "ContentType", "State" },
                values: new object[,]
                {
                    { 1, "FAQ", "Published" },
                    { 2, "FAQ", "Modified" },
                    { 3, "FAQ", "Deleted" },
                    { 4, "Announcement", "Deleted" },
                    { 5, "Announcement", "Modified" },
                    { 6, "Announcement", "Published" }
                });

            migrationBuilder.InsertData(
                table: "announcements",
                columns: new[] { "Id", "ContentId", "Header", "ValiUntil" },
                values: new object[,]
                {
                    { 6, 6, "New header", new DateTime(2020, 6, 15, 11, 27, 54, 401, DateTimeKind.Local).AddTicks(4371) },
                    { 5, 5, "Long header", new DateTime(2020, 6, 5, 11, 27, 54, 401, DateTimeKind.Local).AddTicks(4310) },
                    { 4, 4, "Short header", new DateTime(2020, 5, 26, 11, 27, 54, 396, DateTimeKind.Local).AddTicks(5318) }
                });

            migrationBuilder.InsertData(
                table: "contentTranslations",
                columns: new[] { "Id", "ContentId", "Language", "Text" },
                values: new object[,]
                {
                    { 1, 1, "UA", "Питання 1" },
                    { 11, 5, "EN", "Annnouncement 2" },
                    { 8, 5, "UA", "Оголошення 2" },
                    { 10, 4, "EN", "Annnouncement 1" },
                    { 7, 4, "UA", "Оголошення 1" },
                    { 12, 6, "EN", "Annnouncement 3" },
                    { 6, 3, "EN", "Qwestion 3" },
                    { 3, 3, "UA", "Питання 3" },
                    { 5, 2, "EN", "Qwestion 2" },
                    { 2, 2, "UA", "Питання 2" },
                    { 4, 1, "EN", "Qwestion 1" },
                    { 9, 6, "UA", "Оголошення 3" }
                });

            migrationBuilder.InsertData(
                table: "faqs",
                columns: new[] { "Id", "ContentId", "Tag" },
                values: new object[,]
                {
                    { 2, 2, "Second tag" },
                    { 1, 1, "First tag" },
                    { 3, 3, "Third tag" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_announcements_ContentId",
                table: "announcements",
                column: "ContentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contentTranslations_ContentId",
                table: "contentTranslations",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_faqs_ContentId",
                table: "faqs",
                column: "ContentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "contentTranslations");

            migrationBuilder.DropTable(
                name: "faqs");

            migrationBuilder.DropTable(
                name: "contents");
        }
    }
}
