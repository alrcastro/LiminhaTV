using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LiminhaTV.Migrations
{
    public partial class program : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Program");
        }
    }
}
