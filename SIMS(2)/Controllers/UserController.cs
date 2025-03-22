using Microsoft.AspNetCore.Mvc;
using SIMS_2_.Models;
using SIMS_2_.Repositories;
using SIMS_2_.Services;

namespace SIMS_2_.Controllers
{
    public class UserController : Controller
    {
        
        private readonly IStudentRepository _studentRepository;

        public UserController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

   
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var authService = AuthService.Instance;
            if (authService.Authenticate(username, password))
            {
                var role = authService.GetUserRole(username);
                HttpContext.Session.SetString("UserRole", role);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Invalid username or password";
            return View();
        }
    }
}
