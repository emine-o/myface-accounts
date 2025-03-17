using System.Collections.Generic;

namespace MyFace.Data
{
    public static class PasswordGenerator
    {
        private static readonly IList<string> Passwords = new List<string>
        {
            "5vRsO7Il", "RJxC40Ut", "aKc7WHdz", "$@zZEiFP", "password", "bootcamp", "CSvDTR5@", "m79US11H", "FTKE4W6W", "5XVDXcOd"
        };

        public static string GetPassword(int index)
        {
            return Passwords[index % 10];
        }
        
    }
}