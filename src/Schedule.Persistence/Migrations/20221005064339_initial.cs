using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schedule.Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CronMask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mask = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronMask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Market",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyTypeId = table.Column<int>(type: "int", nullable: false),
                    MarketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.CheckConstraint("CK_Company", "Number LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' AND CompanyTypeId > 1");
                    table.ForeignKey(
                        name: "FK_Company_CompanyType",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Company_Market",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    CronMaskId = table.Column<int>(type: "int", nullable: false),
                    CompanyTypeId = table.Column<int>(type: "int", nullable: false),
                    MarketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => new { x.CronMaskId, x.MarketId, x.CompanyTypeId });
                    table.ForeignKey(
                        name: "FK_Config_CompanyType",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Config_CronMask",
                        column: x => x.CronMaskId,
                        principalTable: "CronMask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Config_Market",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    CompanyTypeId = table.Column<int>(type: "int", nullable: false),
                    MarketId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => new { x.MarketId, x.CompanyTypeId, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_Notification_CompanyType",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_Market",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CompanyType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "all" },
                    { 2, "small" },
                    { 3, "medium" },
                    { 4, "large" }
                });

            migrationBuilder.InsertData(
                table: "CronMask",
                columns: new[] { "Id", "Mask" },
                values: new object[,]
                {
                    { 1, "0 0 1 * *" },
                    { 2, "0 0 5 * *" },
                    { 3, "0 0 7 * *" },
                    { 4, "0 0 10 * *" },
                    { 5, "0 0 14 * *" },
                    { 6, "0 0 15 * *" },
                    { 7, "0 0 20 * *" },
                    { 8, "0 0 28 * *" }
                });

            migrationBuilder.InsertData(
                table: "Market",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Denmark" },
                    { 2, "Norway" },
                    { 3, "Sweden" },
                    { 4, "Finland" }
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "CompanyTypeId", "MarketId", "Name", "Number" },
                values: new object[,]
                {
                    { new Guid("54142eda-2b7c-43bb-83f4-5dc79dba5988"), 4, 3, "unscheduled company", "1231231234" },
                    { new Guid("aad7a630-af1c-4952-9cb4-44b8b847853b"), 2, 1, "scheduled company", "0123456789" },
                    { new Guid("ffe5ffdd-9a9e-4be4-88ac-b90614b04ce8"), 3, 2, "*all* company", "4564564567" }
                });

            migrationBuilder.InsertData(
                table: "Config",
                columns: new[] { "CompanyTypeId", "CronMaskId", "MarketId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 1, 1, 2 },
                    { 2, 1, 3 },
                    { 3, 1, 3 },
                    { 4, 1, 4 },
                    { 1, 2, 1 },
                    { 1, 2, 2 },
                    { 4, 2, 4 },
                    { 2, 3, 3 },
                    { 3, 3, 3 },
                    { 1, 4, 1 },
                    { 1, 4, 2 },
                    { 4, 4, 4 },
                    { 2, 5, 3 },
                    { 3, 5, 3 },
                    { 1, 6, 1 },
                    { 4, 6, 4 },
                    { 1, 7, 1 },
                    { 1, 7, 2 },
                    { 4, 7, 4 },
                    { 2, 8, 3 },
                    { 3, 8, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyTypeId",
                table: "Company",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_MarketId",
                table: "Company",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "UQ_Company_Name",
                table: "Company",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Company_Number",
                table: "Company",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CompanyType_Name",
                table: "CompanyType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Config_CompanyTypeId",
                table: "Config",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Config_MarketId",
                table: "Config",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "UQ_CronMask_Mask",
                table: "CronMask",
                column: "Mask",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Market_Name",
                table: "Market",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CompanyTypeId",
                table: "Notification",
                column: "CompanyTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "CronMask");

            migrationBuilder.DropTable(
                name: "CompanyType");

            migrationBuilder.DropTable(
                name: "Market");
        }
    }
}
