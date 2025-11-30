using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataTypeForIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }
    }
}
