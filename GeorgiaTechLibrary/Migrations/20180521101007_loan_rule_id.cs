using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeorgiaTechLibraryAPI.Migrations
{
    public partial class loan_rule_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_LoanRules_LoanRuleId",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "LoanRuleId",
                table: "Members",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Members_LoanRules_LoanRuleId",
                table: "Members",
                column: "LoanRuleId",
                principalTable: "LoanRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_LoanRules_LoanRuleId",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "LoanRuleId",
                table: "Members",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_LoanRules_LoanRuleId",
                table: "Members",
                column: "LoanRuleId",
                principalTable: "LoanRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
