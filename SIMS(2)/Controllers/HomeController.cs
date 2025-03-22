using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SIMS_2_.Models;
using SIMS_2_.Repositories;

namespace SIMS_2_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public HomeController(ICourseRepository courseRepository)
        //{
        //    _courseRepository = courseRepository;
        //}

        public IActionResult Index()
        {
            //if (string.IsNullOrEmpty(userRole))
            //{
            //    return RedirectToAction("Login", "User");
            //}

            //ViewBag.UserRole = userRole;
            //var courses = _courseRepository.GetAll();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private readonly ICourseRepository _courseRepository;

        
        
    }
}
