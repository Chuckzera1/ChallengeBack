using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanDuplicatesAndAddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clean duplicate CNPJs in Companies table - keep only the first occurrence
            migrationBuilder.Sql(@"
                DELETE FROM ""Companies"" 
                WHERE ""Id"" NOT IN (
                    SELECT MIN(""Id"") 
                    FROM ""Companies"" 
                    GROUP BY ""Cnpj""
                );
            ");

            // Clean duplicate CPFs in Suppliers table - keep only the first occurrence
            migrationBuilder.Sql(@"
                DELETE FROM ""Suppliers"" 
                WHERE ""Id"" NOT IN (
                    SELECT MIN(""Id"") 
                    FROM ""Suppliers"" 
                    WHERE ""Cpf"" IS NOT NULL
                    GROUP BY ""Cpf""
                );
            ");

            // Clean duplicate CNPJs in Suppliers table - keep only the first occurrence
            migrationBuilder.Sql(@"
                DELETE FROM ""Suppliers"" 
                WHERE ""Id"" NOT IN (
                    SELECT MIN(""Id"") 
                    FROM ""Suppliers"" 
                    WHERE ""Cnpj"" IS NOT NULL
                    GROUP BY ""Cnpj""
                );
            ");

            // Clean duplicate RGs in Suppliers table - keep only the first occurrence
            migrationBuilder.Sql(@"
                DELETE FROM ""Suppliers"" 
                WHERE ""Id"" NOT IN (
                    SELECT MIN(""Id"") 
                    FROM ""Suppliers"" 
                    WHERE ""Rg"" IS NOT NULL
                    GROUP BY ""Rg""
                );
            ");

            // Clean duplicate CompanySupplier relationships - keep only the first occurrence
            migrationBuilder.Sql(@"
                DELETE FROM ""CompanySuppliers"" 
                WHERE ""Id"" NOT IN (
                    SELECT MIN(""Id"") 
                    FROM ""CompanySuppliers"" 
                    GROUP BY ""CompanyId"", ""SupplierId""
                );
            ");

            migrationBuilder.DropIndex(
                name: "IX_CompanySuppliers_CompanyId",
                table: "CompanySuppliers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Suppliers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Suppliers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Cnpj",
                table: "Suppliers",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Cpf",
                table: "Suppliers",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Rg",
                table: "Suppliers",
                column: "Rg",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanySuppliers_CompanyId_SupplierId",
                table: "CompanySuppliers",
                columns: new[] { "CompanyId", "SupplierId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Cnpj",
                table: "Companies",
                column: "Cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Cnpj",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Cpf",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Rg",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_CompanySuppliers_CompanyId_SupplierId",
                table: "CompanySuppliers");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Cnpj",
                table: "Companies");


            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Suppliers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Suppliers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySuppliers_CompanyId",
                table: "CompanySuppliers",
                column: "CompanyId");
        }
    }
}
