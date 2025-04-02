using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS_2_.Data;
using SIMS_2_.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SIMS_2_.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username and password are required.";
                return View();
            }

            // Find the user in the database
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            // Create the user's claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            // Create the identity and principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = true, // Persist the cookie across browser sessions
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            // Redirect based on role
            if (user.Role.RoleName == "Student")
            {
                return RedirectToAction("StudentPage", "Student");
            }
            else if (user.Role.RoleName == "Admin")
            {
                return RedirectToAction("ManagerPage", "Admin");
            }else if (user.Role.RoleName == "Faculty")
            {
                return RedirectToAction("FacultyPage", "Faculty");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            user.RoleId = 1; // Hardcode to Student role
            Console.WriteLine($"Register POST: UserName={user.UserName}, Email={user.Email}, Password={user.Password}, RoleId={user.RoleId}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("ModelState invalid: " + string.Join(", ", errors));
                ViewBag.Error = "Validation failed: " + string.Join(", ", errors);
                return View(user);
            }

            try
            {
                if (!_dbContext.Database.CanConnect())
                {
                    Console.WriteLine("Database connection failed.");
                    ViewBag.Error = "Cannot connect to the database.";
                    return View(user);
                }

                if (_dbContext.Users.Any(u => u.UserName == user.UserName))
                {
                    Console.WriteLine("Username already exists.");
                    ViewBag.Error = "Username already exists.";
                    return View(user);
                }
                if (_dbContext.Users.Any(u => u.Email == user.Email))
                {
                    Console.WriteLine("Email already exists.");
                    ViewBag.Error = "Email already exists.";
                    return View(user);
                }

                user.CreatedAt = DateTime.Now;
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    _dbContext.Users.Add(user);
                    int userRowsAffected = _dbContext.SaveChanges();
                    Console.WriteLine($"User saved: {userRowsAffected} rows affected, UserId={user.UserId}");

                    var student = new Student
                    {
                        Name = user.UserName,
                        PersonalInfo = "N/A",
                        EnrollmentDate = DateTime.Now,
                        UserId = user.UserId
                    };
                    _dbContext.Students.Add(student);
                    int studentRowsAffected = _dbContext.SaveChanges();
                    Console.WriteLine($"Student saved: {studentRowsAffected} rows affected, StudentId={student.StudentId}");

                    transaction.Commit();
                }

                TempData["Success"] = "Registration successful! Please log in.";
                Console.WriteLine("Registration successful, redirecting to Login");
                return RedirectToAction("Login");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException: {ex.InnerException?.Message}");
                ViewBag.Error = "Database error: " + (ex.InnerException?.Message ?? ex.Message);
                return View(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                ViewBag.Error = "An unexpected error occurred: " + ex.Message;
                return View(user);
            }
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}