using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS_2_.Models;
using SIMS_2_.Repositories;

namespace SIMS_2_.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;

        public StudentController(
            IStudentRepository studentRepository,
            ICourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public IActionResult StudentPage()
        {
            // Get the logged-in user's ID from claims
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                TempData["Error"] = "Unable to identify the user. Please log in again.";
                return RedirectToAction("Login", "User");
            }

            // Fetch the student associated with the user
            var student = _studentRepository.GetAll()
                .Include(s => s.User)
                .FirstOrDefault(s => s.UserId == userId);
            if (student == null)
            {
                TempData["Error"] = "Student profile not found.";
                return NotFound();
            }

            // Fetch the student's enrollments with course details
            var enrollments = _studentRepository.GetAll()
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Faculty)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.AcademicCalendar)
                .FirstOrDefault(s => s.StudentId == student.StudentId)?
                .Enrollments
                .ToList() ?? new List<Enrollment>();

            // Fetch available courses (courses the student is not enrolled in)
            var allCourses = _courseRepository.GetAll().ToList();
            var enrolledCourseIds = enrollments.Select(e => e.CourseId).ToList();
            var availableCourses = allCourses
                .Where(c => !enrolledCourseIds.Contains(c.CourseId))
                .ToList();

            // Create the view model
            var viewModel = new StudentPageViewModel
            {
                Student = student,
                Enrollments = enrollments,
                AvailableCourses = availableCourses,
                Schedule = enrollments // Reusing enrollments for the schedule tab
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Enroll(int courseId)
        {
            // Get the logged-in user's ID from claims
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                TempData["Error"] = "Unable to identify the user. Please log in again.";
                return RedirectToAction("Login", "User");
            }

            // Fetch the student associated with the user
            var student = _studentRepository.GetAll()
                .FirstOrDefault(s => s.UserId == userId);
            if (student == null)
            {
                TempData["Error"] = "Student profile not found.";
                return RedirectToAction("StudentPage");
            }

            // Fetch the course
            var course = _courseRepository.Get(courseId);
            if (course == null)
            {
                TempData["Error"] = "The selected course does not exist.";
                return RedirectToAction("StudentPage");
            }

            // Check if the student is already enrolled in the course
            var existingEnrollment = _studentRepository.GetAll()
                .Include(s => s.Enrollments)
                .FirstOrDefault(s => s.StudentId == student.StudentId)?
                .Enrollments
                .FirstOrDefault(e => e.CourseId == courseId);
            if (existingEnrollment != null)
            {
                TempData["Error"] = "You are already enrolled in this course.";
                return RedirectToAction("StudentPage");
            }

            // Create a new enrollment
            var enrollment = new Enrollment
            {
                StudentId = student.StudentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                Grade = null
            };

            try
            {
                _studentRepository.AddEnrollment(enrollment);
                TempData["Success"] = $"Successfully enrolled in {course.CourseName}!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to enroll in the course: {ex.Message}";
            }

            return RedirectToAction("StudentPage");
        }
    }
}