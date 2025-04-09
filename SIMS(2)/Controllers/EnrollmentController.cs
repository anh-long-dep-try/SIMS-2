using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS_2_.Models;
using SIMS_2_.Repositories;

namespace SIMS_2_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(
            IEnrollmentService enrollmentService,
            ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentService.GetAllEnrollments();
            return View(enrollments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int studentId, int courseId, string grade)
        {
            try
            {
                var enrollment = await _enrollmentService.EnrollStudent(studentId, courseId, grade);
                TempData["SuccessMessage"] = "Student enrolled successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling student");
                ModelState.AddModelError("", "An error occurred while enrolling the student.");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollment(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int studentId, int courseId)
        {
            try
            {
                var enrollment = await _enrollmentService.UpdateEnrollment(id, studentId, courseId);
                TempData["SuccessMessage"] = "Enrollment updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating enrollment");
                ModelState.AddModelError("", "An error occurred while updating the enrollment.");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _enrollmentService.DeleteEnrollment(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Enrollment deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Enrollment not found.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting enrollment");
                TempData["ErrorMessage"] = "An error occurred while deleting the enrollment.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CourseEnrollments(int courseId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByCourse(courseId);
            return View(enrollments);
        }

        [HttpGet]
        public async Task<IActionResult> StudentEnrollments(int studentId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByStudent(studentId);
            return View(enrollments);
        }
    }
} 