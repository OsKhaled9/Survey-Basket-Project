using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44bf0349-4864-40ee-92bd-7fb43c4af571", "f12619e1-1d13-4012-8ce3-562aa5bdf03b", false, false, "Admin", "ADMIN" },
                    { "a12ee1e9-2555-4e2f-8aa9-aa4a9a94d05d", "43de3594-fe83-4e63-bcf5-da5857229bb0", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0e3c4709-bfee-4c3e-afe3-862af02b18c8", 0, "013d96f6-02f3-4caf-bffb-236f7c7603d5", "admin@survey-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEDpo+gw0TS4X1EIYVBGrTuebRwo4nrn0aILem8WA5J1AXfhABl1t0HXIdpLPWVKm2w==", null, false, "0DB5937AF4A54DA18CC7BF3AA6A92B82", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 2, "permissions", "polls:add", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 3, "permissions", "polls:update", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 4, "permissions", "polls:delete", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 5, "permissions", "questions:read", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 6, "permissions", "questions:add", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 7, "permissions", "questions:update", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 8, "permissions", "users:read", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 9, "permissions", "users:add", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 10, "permissions", "users:update", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 11, "permissions", "roles:read", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 12, "permissions", "roles:add", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 13, "permissions", "roles:update", "44bf0349-4864-40ee-92bd-7fb43c4af571" },
                    { 14, "permissions", "results:read", "44bf0349-4864-40ee-92bd-7fb43c4af571" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "44bf0349-4864-40ee-92bd-7fb43c4af571", "0e3c4709-bfee-4c3e-afe3-862af02b18c8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a12ee1e9-2555-4e2f-8aa9-aa4a9a94d05d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "44bf0349-4864-40ee-92bd-7fb43c4af571", "0e3c4709-bfee-4c3e-afe3-862af02b18c8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44bf0349-4864-40ee-92bd-7fb43c4af571");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e3c4709-bfee-4c3e-afe3-862af02b18c8");
        }
    }
}
