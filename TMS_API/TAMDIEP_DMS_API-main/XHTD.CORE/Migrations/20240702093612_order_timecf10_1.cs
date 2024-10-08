using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XHTD.CORE.Migrations
{
    /// <inheritdoc />
    public partial class order_timecf10_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Confirm10",
                table: "tblStoreOrderOperating",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeConfirm10",
                table: "tblStoreOrderOperating",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirm10",
                table: "tblStoreOrderOperating");

            migrationBuilder.DropColumn(
                name: "TimeConfirm10",
                table: "tblStoreOrderOperating");
        }
    }
}
