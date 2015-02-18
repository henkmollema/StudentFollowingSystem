using System;

namespace StudentFollowingSystem.Services
{
    public class PasswordGenerator
    {
        public static string CreateRandomPassword()
        {
            string password = "";
            const int length = 8;
            const string chars = "abcdefghijkmnpqrstuvwxyz";
            var rand = new Random();
            for (int i = 0; i < length; i++)
            {
                password += chars[rand.Next(0, chars.Length)];
            }

            return password;
        }
    }
}
