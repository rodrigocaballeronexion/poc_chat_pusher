using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.API.Migrations
{
    public partial class addingdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelApps",
                columns: table => new
                {
                    AppId = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Secret = table.Column<string>(nullable: false),
                    Cluster = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 164, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelApps", x => x.AppId);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    SubscriberId = table.Column<string>(nullable: false),
                    ChatName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.SubscriberId);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    ChannelId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 164, nullable: false),
                    ChannelAppId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ChannelId);
                    table.ForeignKey(
                        name: "FK_Channels_ChannelApps_ChannelAppId",
                        column: x => x.ChannelAppId,
                        principalTable: "ChannelApps",
                        principalColumn: "AppId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChannelSubscribers",
                columns: table => new
                {
                    ChannelId = table.Column<string>(nullable: false),
                    SubscriberId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelSubscribers", x => new { x.ChannelId, x.SubscriberId });
                    table.ForeignKey(
                        name: "FK_ChannelSubscribers_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelSubscribers_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "SubscriberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(nullable: true),
                    ChannelId = table.Column<string>(nullable: true),
                    SenderId = table.Column<string>(nullable: true),
                    When = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Subscribers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Subscribers",
                        principalColumn: "SubscriberId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ChannelAppId",
                table: "Channels",
                column: "ChannelAppId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelSubscribers_SubscriberId",
                table: "ChannelSubscribers",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChannelId",
                table: "Messages",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelSubscribers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "ChannelApps");
        }
    }
}
