using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestAssignment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExceptionJournals",
                columns: new[] { "Id", "CreatedAt", "EventId", "ExceptionMessage", "ExceptionType", "QueryString", "RequestBody", "RequestMethod", "RequestPath", "StackTrace" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, "Value cannot be null. (Parameter 'treeName')", "System.ArgumentNullException", "?name=", null, "GET", "/api/tree", "   at TestAssignment.Infrastructure.Services.TreeService.GetTree(String treeName) in TreeService.cs:line 45" },
                    { 2L, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1002L, "Node with the same name already exists in this tree", "TestAssignment.Domain.Exceptions.SecureException", null, "{\"treeName\":\"Company\",\"name\":\"Engineering\",\"parentId\":1}", "POST", "/api/tree/node", "   at TestAssignment.Infrastructure.Services.TreeService.CreateNode(String treeName, String nodeName, Int64 parentId) in TreeService.cs:line 78" },
                    { 3L, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 1003L, "An error occurred while saving the entity changes.", "Microsoft.EntityFrameworkCore.DbUpdateException", null, null, "POST", "/api/journal", "   at Microsoft.EntityFrameworkCore.DbContext.SaveChanges()\n   at TestAssignment.Infrastructure.Services.JournalService.LogException(Exception ex) in JournalService.cs:line 23" },
                    { 4L, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1004L, "Cannot delete a node that has children", "System.InvalidOperationException", null, null, "DELETE", "/api/tree/node/1", "   at TestAssignment.Infrastructure.Services.TreeService.DeleteNode(Int64 nodeId) in TreeService.cs:line 125" },
                    { 5L, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 1005L, "Invalid authentication token", "System.UnauthorizedAccessException", null, null, "POST", "/api/partner/login", "   at TestAssignment.Infrastructure.Services.AuthService.ValidateToken(String token) in AuthService.cs:line 67" }
                });

            migrationBuilder.InsertData(
                table: "TreeNodes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TreeName" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Acme Corporation", null, "Company" },
                    { 10L, new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Electronics", null, "Products" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Code", "CreatedAt", "LastLoginAt" },
                values: new object[,]
                {
                    { 1L, "admin", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, "user1", new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, "user2", new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4L, "testuser", new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "TreeNodes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "TreeName" },
                values: new object[,]
                {
                    { 2L, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Engineering", 1L, "Company" },
                    { 3L, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Sales", 1L, "Company" },
                    { 4L, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), "HR", 1L, "Company" },
                    { 11L, new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Computers", 10L, "Products" },
                    { 12L, new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Mobile Devices", 10L, "Products" },
                    { 5L, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Backend", 2L, "Company" },
                    { 6L, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Frontend", 2L, "Company" },
                    { 7L, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), "DevOps", 2L, "Company" },
                    { 8L, new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), "North America", 3L, "Company" },
                    { 9L, new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Europe", 3L, "Company" },
                    { 13L, new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Laptops", 11L, "Products" },
                    { 14L, new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Desktops", 11L, "Products" },
                    { 15L, new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Smartphones", 12L, "Products" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExceptionJournals",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ExceptionJournals",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ExceptionJournals",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ExceptionJournals",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "ExceptionJournals",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "TreeNodes",
                keyColumn: "Id",
                keyValue: 10L);
        }
    }
}
