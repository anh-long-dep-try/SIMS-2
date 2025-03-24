using System.Collections.Generic;
using System.Linq;
using SIMS_2_.Models;

namespace SIMS_2_.Services
{
    public class AuthService
    {
        private static AuthService _instance;
        private List<User> _users;

        private AuthService()
        {
            // Simulated user database
            _users = new List<User>
            {
                new User { UserId = 1, UserName = "student", Password = "student", RoleId = 1, Email = "student@gamil.com" },
                new User { UserId = 4, UserName = "faculty", Password = "faculty", RoleId = 2, Email = "faculty@gmail.com" },
                new User { UserId = 5, UserName = "admin", Password = "adminn", RoleId = 3, Email = "admin@gmail.com" }
            };
        }

        public static AuthService Instance => _instance ??= new AuthService();

        public User Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }
    }
}
