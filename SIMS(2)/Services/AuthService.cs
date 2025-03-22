namespace SIMS_2_.Services
{
    public class AuthService
    {
        private static AuthService _instance;
        private static readonly object _lock = new object();

        private AuthService() { }

        public static AuthService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new AuthService();
                    }
                }
                return _instance;
            }
        }

        public bool Authenticate(string username, string password)
        {
            // Simulate authentication logic (replace with actual database check)
            // For demo, assume "admin" user with password "admin123"
            return username == "admin" && password == "admin123";
        }

        public string GetUserRole(string username)
        {
            // Simulate role retrieval (replace with actual database query)
            // For demo, return "Admin" for "admin" user
            if (username == "admin") return "Admin";
            return "Student"; // Default role
        }
    }
}
