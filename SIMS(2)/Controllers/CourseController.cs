using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS_2_.Models;
using SIMS_2_.Repositories;

namespace SIMS_2_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseManagementService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(
            ICourseManagementService courseService,
            ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCourses();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseName,CourseCode,Credits,FacultyId,CalendarId")] Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            try
            {
                await _courseService.CreateCourse(course);
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course");
                ModelState.AddModelError("", "An error occurred while creating the course.");
                return View(course);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourse(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,CourseCode,Credits,FacultyId,CalendarId")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(course);
            }

            try
            {
                await _courseService.UpdateCourse(course);
                TempData["SuccessMessage"] = "Course updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course");
                ModelState.AddModelError("", "An error occurred while updating the course.");
                return View(course);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _courseService.DeleteCourse(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Course deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Course not found.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course");
                TempData["ErrorMessage"] = "An error occurred while deleting the course.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 