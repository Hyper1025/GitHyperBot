using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GitHyperBot.Migrations
{
    public partial class addmigrationTabelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableAccounts",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mute = table.Column<bool>(nullable: false, defaultValue: false),
                    NumberOfWarning = table.Column<uint>(nullable: false),
                    Points = table.Column<uint>(nullable: false),
                    Xp = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableAccounts", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TableGuilds",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoasVindasBool = table.Column<bool>(nullable: false),
                    BoasVindasDescription = table.Column<string>(nullable: true),
                    BoasVindasPvBool = table.Column<bool>(nullable: false),
                    BoasVindasPvDescription = table.Column<string>(nullable: true),
                    BoasVindasPvField1Descri = table.Column<string>(nullable: true),
                    BoasVindasPvField1Title = table.Column<string>(nullable: true),
                    BoasVindasPvField2Descri = table.Column<string>(nullable: true),
                    BoasVindasPvField2Title = table.Column<string>(nullable: true),
                    BoasVindasPvField3Descri = table.Column<string>(nullable: true),
                    BoasVindasPvField3Title = table.Column<string>(nullable: true),
                    BoasVindasPvFooter = table.Column<string>(nullable: true),
                    BoasVindasPvTitle = table.Column<string>(nullable: true),
                    BoasVindasPvUrl = table.Column<string>(nullable: true),
                    BoasVindasTitle = table.Column<string>(nullable: true),
                    BoasVindasUrl = table.Column<string>(nullable: true),
                    IdChatConvites = table.Column<ulong>(nullable: false),
                    IdChatGeral = table.Column<ulong>(nullable: false),
                    IdChatLog = table.Column<ulong>(nullable: false),
                    IdMsgRegistro = table.Column<ulong>(nullable: false),
                    IdMsgRegras = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableGuilds", x => x.GuildId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableAccounts");

            migrationBuilder.DropTable(
                name: "TableGuilds");
        }
    }
}
