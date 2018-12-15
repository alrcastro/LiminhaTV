using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LiminhaTV.Migrations
{
    public partial class teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    ChannelId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<int>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    LogoURL = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.ChannelId);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    ChProgramId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Channel = table.Column<string>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.ChProgramId);
                });

            migrationBuilder.CreateTable(
                name: "ListContainer",
                columns: table => new
                {
                    ListContainerID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListContainer", x => x.ListContainerID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "ListContainer");
        }
    }
}
