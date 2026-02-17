using System;

namespace EduCycle.Api.Helpers
{
    /// <summary>
    /// Helper class để generate BCrypt password hash
    /// Chạy class này một lần để lấy hash cho "admin@1"
    /// </summary>
    public static class PasswordHashGenerator
    {
        public static void GenerateAdminHash()
        {
            var password = "admin@1";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            
            Console.WriteLine("==================================");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Hash: {hash}");
            Console.WriteLine("==================================");
            Console.WriteLine("\nCopy hash này vào migration file!");
        }

        // Test hash
        public static bool VerifyHash(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
