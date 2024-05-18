using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedTackIdandUpdateTrackStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Booking",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                schema: "Booking",
                table: "Items",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Booking",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "Booking",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                schema: "Booking",
                table: "Items",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Booking",
                table: "BookParcels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                schema: "Booking",
                table: "BookParcels",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Booking",
                table: "BookParcels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Booking",
                table: "BookParcels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TracId",
                schema: "Booking",
                table: "BookParcels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "Booking",
                table: "BookParcels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                schema: "Booking",
                table: "BookParcels",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Booking",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "Booking",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Booking",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Booking",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "Booking",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Booking",
                table: "BookParcels");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "Booking",
                table: "BookParcels");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Booking",
                table: "BookParcels");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Booking",
                table: "BookParcels");

            migrationBuilder.DropColumn(
                name: "TracId",
                schema: "Booking",
                table: "BookParcels");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Booking",
                table: "BookParcels");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "Booking",
                table: "BookParcels");
        }
    }
}
