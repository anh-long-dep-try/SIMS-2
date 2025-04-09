using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS_2_.Data;
using SIMS_2_.Models;

namespace SIMS_2_.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class FacultyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FacultyController> _logger;

        public FacultyController(AppDbContext context, ILogger<FacultyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Faculty Dashboard
        public async Task<IActionResult> FacultyPage()
        {
            var facultyId = int.Parse(User.FindFirst("FacultyId")?.Value ?? "0");

            // Get all enrollments for courses taught by this faculty
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.Course.FacultyId == facultyId)
                .ToListAsync();

            // Create the view model
            var viewModel = new FacultyPageViewModel
            {
                Enrollments = enrollments
            };

            return View(viewModel);
        }

        // GET: Edit Grade
        [HttpGet]
        public async Task<IActionResult> EditGrade(int enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            // Verify faculty authorization
            var facultyId = int.Parse(User.FindFirst("FacultyId")?.Value ?? "0");
            if (enrollment.Course.FacultyId != facultyId)
            {
                return Unauthorized("You are not authorized to edit this grade");
            }

            return View(enrollment);
        }

        // POST: Edit Grade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(int enrollmentId, string grade)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            // Verify faculty authorization
            var facultyId = int.Parse(User.FindFirst("FacultyId")?.Value ?? "0");
            if (enrollment.Course.FacultyId != facultyId)
            {
                return Unauthorized("You are not authorized to edit this grade");
            }

            enrollment.Grade = grade;
            _context.Update(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Grade updated successfully!";
            return RedirectToAction("FacultyPage");
        }
    }
}
