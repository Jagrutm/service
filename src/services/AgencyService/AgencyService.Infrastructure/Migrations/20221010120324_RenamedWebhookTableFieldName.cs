using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgencyService.Infrastructure.Migrations
{
    public partial class RenamedWebhookTableFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Webhooks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "WebhookId",
                table: "Webhooks",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_Webhooks_AgencyId_WebhookId",
                table: "Webhooks",
                newName: "IX_Webhooks_AgencyId_Type");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Webhooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 10, 17, 33, 24, 497, DateTimeKind.Local).AddTicks(564),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 9, 23, 10, 32, 22, 494, DateTimeKind.Local).AddTicks(6604));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Webhooks",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Webhooks",
                newName: "WebhookId");

            migrationBuilder.RenameIndex(
                name: "IX_Webhooks_AgencyId_Type",
                table: "Webhooks",
                newName: "IX_Webhooks_AgencyId_WebhookId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Webhooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 23, 10, 32, 22, 494, DateTimeKind.Local).AddTicks(6604),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 10, 17, 33, 24, 497, DateTimeKind.Local).AddTicks(564));
        }
    }
}
