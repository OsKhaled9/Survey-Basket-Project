using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledColumnToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e3c4709-bfee-4c3e-afe3-862af02b18c8",
                column: "IsDisabled",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");
        }
    }
}
