using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIMS_2_.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class FacultyController : Controller
    {
        public IActionResult FacultyPage()
        {
            return View();
        }
    }
}