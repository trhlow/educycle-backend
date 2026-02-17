using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCycle.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update password hash cho admin@educycle.com
            // Password mới: admin@1
            migrationBuilder.Sql(
                "UPDATE Users SET PasswordHash = N'$2a$11$vI8aWBnW3fID.ixJCRy.qOhMkGh/.WY6g9E7KO0OLfZNPq9i0jq9y' " +
                "WHERE Id = '00000000-0000-0000-0000-000000000001'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback về hash cũ nếu cần
            migrationBuilder.Sql(
                "UPDATE Users SET PasswordHash = N'$2a$11$TCvAEG19aKWyzD9V1qllquNigqoIj3mT3Ihpviy0HRIP3wQRErAVK' " +
                "WHERE Id = '00000000-0000-0000-0000-000000000001'");
        }
    }
}
