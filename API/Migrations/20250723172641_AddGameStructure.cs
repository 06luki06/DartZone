using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace At.luki0606.DartZone.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGameStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartScore = table.Column<int>(type: "integer", nullable: false),
                    CurrentScore = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HasFinished = table.Column<bool>(type: "boolean", nullable: false),
                    HasStarted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Throws",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Throws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Throws_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Darts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ThrowId = table.Column<Guid>(type: "uuid", nullable: false),
                    Multiplier = table.Column<int>(type: "integer", nullable: false),
                    Field = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Darts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Darts_Throws_ThrowId",
                        column: x => x.ThrowId,
                        principalTable: "Throws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Darts_ThrowId",
                table: "Darts",
                column: "ThrowId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                table: "Games",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Throws_GameId",
                table: "Throws",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Darts");

            migrationBuilder.DropTable(
                name: "Throws");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");
        }
    }
}
