using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCycle.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOAuthAndEmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // OAuth provider IDs
            migrationBuilder.AddColumn<string>(
                name: "GoogleId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MicrosoftId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            // Email verification
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            // Password reset
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            // Phone verification
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "GoogleId", table: "Users");
            migrationBuilder.DropColumn(name: "FacebookId", table: "Users");
            migrationBuilder.DropColumn(name: "MicrosoftId", table: "Users");
            migrationBuilder.DropColumn(name: "IsEmailVerified", table: "Users");
            migrationBuilder.DropColumn(name: "EmailVerificationToken", table: "Users");
            migrationBuilder.DropColumn(name: "EmailVerificationTokenExpiry", table: "Users");
            migrationBuilder.DropColumn(name: "PasswordResetToken", table: "Users");
            migrationBuilder.DropColumn(name: "PasswordResetTokenExpiry", table: "Users");
            migrationBuilder.DropColumn(name: "Phone", table: "Users");
            migrationBuilder.DropColumn(name: "PhoneVerified", table: "Users");
        }
    }
}
