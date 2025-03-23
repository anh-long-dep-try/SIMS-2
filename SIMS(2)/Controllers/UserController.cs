using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS_2_.Data;
using SIMS_2_.Models;
using System.Linq;
using System.Security.Claims;

namespace SIMS_2_.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            user.RoleId = 1; // Hardcode to Student role

            if (ModelState.IsValid)
            {
                try
                {
                    user.CreatedAt = DateTime.Now;
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();

                    var student = new Student
                    {
                        Name = user.UserName,
                        PersonalInfo = "N/A",
                        EnrollmentDate = DateTime.Now,
                        UserId = user.UserId
                    };
                    _dbContext.Students.Add(student);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("UNIQUE KEY constraint") == true)
                    {
                        ViewBag.Error = "Username or email already exists.";
                    }
                    else
                    {
                        ViewBag.Error = "An error occurred while saving your data. Please try again.";
                    }
                }
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null)
            {
                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.RoleName) // Add the role as a claim
                };

                // Create identity and principal
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user using cookie authentication
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Optionally, store additional info in session (if still needed)
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);

                // Redirect based on role
                switch (user.RoleId)
                {
                    case 1: // Student
                        return RedirectToAction("StudentPage", "Student");
                    case 2: // Faculty
                        return RedirectToAction("FacultyPage", "Faculty");
                    case 3: // Admin
                        return RedirectToAction("ManagerPage", "Admin");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear the session
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}