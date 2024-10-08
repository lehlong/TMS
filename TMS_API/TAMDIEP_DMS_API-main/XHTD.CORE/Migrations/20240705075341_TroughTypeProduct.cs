using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XHTD.CORE.Migrations
{
    /// <inheritdoc />
    public partial class TroughTypeProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tblTroughTypeProduct_TroughCode",
                table: "tblTroughTypeProduct",
                column: "TroughCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblTroughTypeProduct_TypeProduct",
                table: "tblTroughTypeProduct",
                column: "TypeProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_tblTroughTypeProduct_tblTrough_TroughCode",
                table: "tblTroughTypeProduct",
                column: "TroughCode",
                principalTable: "tblTrough",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblTroughTypeProduct_tblTypeProduct_TypeProduct",
                table: "tblTroughTypeProduct",
                column: "TypeProduct",
                principalTable: "tblTypeProduct",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTroughTypeProduct_tblTrough_TroughCode",
                table: "tblTroughTypeProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_tblTroughTypeProduct_tblTypeProduct_TypeProduct",
                table: "tblTroughTypeProduct");

            migrationBuilder.DropIndex(
                name: "IX_tblTroughTypeProduct_TroughCode",
                table: "tblTroughTypeProduct");

            migrationBuilder.DropIndex(
                name: "IX_tblTroughTypeProduct_TypeProduct",
                table: "tblTroughTypeProduct");
        }
    }
}
