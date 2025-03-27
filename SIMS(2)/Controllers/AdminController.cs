using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS_2_.Data;
using SIMS_2_.Models;

namespace SIMS_2_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AppDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Manager Page with course list
        public async Task<IActionResult> ManagerPage()
        {
            var courses = await _context.Courses
                .Include(c => c.Faculty)
                .Include(c => c.AcademicCalendar)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .ToListAsync();
            return View(courses);
        }

        // GET: Create Course
        [HttpGet]
        public async Task<IActionResult> CreateCourse()
        {
            var faculties = await _context.Faculties.ToListAsync();
            var calendars = await _context.AcademicCalendars.ToListAsync();

            // Check if dropdowns have data; if not, provide feedback
            if (!faculties.Any() || !calendars.Any())
            {
                TempData["ErrorMessage"] = "Cannot create a course because there are no faculties or academic calendars available.";
                return RedirectToAction(nameof(ManagerPage));
            }

            ViewBag.Faculties = faculties;
            ViewBag.Calendars = calendars;

            return View(new Course());
        }

        // POST: Create Course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse([Bind("CourseName,CourseCode,Credits,FacultyId,CalendarId")] Course course)
        {
            var faculties = await _context.Faculties.ToListAsync();
            var calendars = await _context.AcademicCalendars.ToListAsync();
            ViewBag.Faculties = faculties;
            ViewBag.Calendars = calendars;


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ViewBag.ValidationErrors = errors;
                _logger.LogWarning($"Course creation failed. Errors: {string.Join(", ", errors)}");
                return View(course);
            }

            try
            {
                // Ensure related entities exist
                var faculty = await _context.Faculties.FindAsync(course.FacultyId);
                var calendar = await _context.AcademicCalendars.FindAsync(course.CalendarId);

                if (faculty == null || calendar == null)
                {
                    ModelState.AddModelError("", "Invalid Faculty or Academic Calendar selection.");
                    return View(course);
                }

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Course created successfully: {course.CourseName}");
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction(nameof(ManagerPage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating course: {ex.Message}");
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                return View(course);
            }
        }

        // GET: Edit Course
        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Calendars = await _context.AcademicCalendars.ToListAsync();
            return View(course);
        }

        // POST: Edit Course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, [Bind("CourseId,CourseName,CourseCode,Credits,FacultyId,CalendarId")] Course course)
        {
            if (id != course.CourseId)
            {
                _logger.LogWarning($"Course ID mismatch. Passed ID: {id}, Course ID: {course.CourseId}");
                return BadRequest();
            }

            var faculties = await _context.Faculties.ToListAsync();
            var calendars = await _context.AcademicCalendars.ToListAsync();
            ViewBag.Faculties = faculties;
            ViewBag.Calendars = calendars;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ViewBag.ValidationErrors = errors;
                _logger.LogWarning($"Course edit failed. Errors: {string.Join(", ", errors)}");
                return View(course);
            }

            try
            {
                // Ensure the course exists
                var existingCourse = await _context.Courses.FindAsync(id);
                if (existingCourse == null)
                {
                    return NotFound();
                }

                // Ensure related entities exist
                var faculty = await _context.Faculties.FindAsync(course.FacultyId);
                var calendar = await _context.AcademicCalendars.FindAsync(course.CalendarId);

                if (faculty == null || calendar == null)
                {
                    ModelState.AddModelError("", "Invalid Faculty or Academic Calendar selection.");
                    return View(course);
                }

                // Update the existing course
                existingCourse.CourseName = course.CourseName;
                existingCourse.CourseCode = course.CourseCode;
                existingCourse.Credits = course.Credits;
                existingCourse.FacultyId = course.FacultyId;
                existingCourse.CalendarId = course.CalendarId;

                _context.Update(existingCourse);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Course updated successfully: {course.CourseName}");
                TempData["SuccessMessage"] = "Course updated successfully!";
                return RedirectToAction(nameof(ManagerPage));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, $"Concurrency error updating course: {ex.Message}");
                if (!CourseExists(course.CourseId))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating course: {ex.Message}");
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                return View(course);
            }
        }

        // POST: Delete Course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManagerPage));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        // GET: Add Student to Course
        [HttpGet]
        public async Task<IActionResult> AddStudentToCourse(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.CourseId = courseId;
            ViewBag.Students = await _context.Students.ToListAsync();
            return View();
        }

        // POST: Add Student to Course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudentToCourse(int courseId, int studentId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            var student = await _context.Students.FindAsync(studentId);

            if (course == null || student == null)
            {
                return NotFound();
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = studentId,
                EnrollmentDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student added to course successfully!";
            return RedirectToAction(nameof(ManagerPage));
        }
        // GET: Enroll Student in Course
        [HttpGet]
        public async Task<IActionResult> EnrollStudent()
        {
            var students = await _context.Students.ToListAsync();
            var courses = await _context.Courses.ToListAsync();

            // Check if dropdowns have data; if not, provide feedback
            if (!students.Any() || !courses.Any())
            {
                TempData["ErrorMessage"] = "Cannot enroll a student because there are no students or courses available.";
                return RedirectToAction(nameof(ManagerPage));
            }

            ViewBag.Students = students;
            ViewBag.Courses = courses;

            return View();
        }

        // POST: Enroll Student in Course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollStudent(int courseId, int studentId, string grade)
        {
            var course = await _context.Courses.FindAsync(courseId);
            var student = await _context.Students.FindAsync(studentId);

            if (course == null || student == null)
            {
                return NotFound();
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = studentId,
                EnrollmentDate = DateTime.Now,
                Grade = grade 
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student enrolled in course successfully!";
            return RedirectToAction(nameof(ManagerPage));
        }



        // GET: Edit Student Enrollment
        [HttpGet]
        public async Task<IActionResult> EditStudentEnrollment(int enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            ViewBag.Students = await _context.Students.ToListAsync();
            return View(enrollment);
        }

        // POST: Edit Student Enrollment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudentEnrollment(int enrollmentId, int studentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            var student = await _context.Students.FindAsync(studentId);

            if (enrollment == null || student == null)
            {
                return NotFound();
            }

            enrollment.StudentId = studentId;
            _context.Update(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student enrollment updated successfully!";
            return RedirectToAction(nameof(ManagerPage));
        }

        // POST: Delete Student Enrollment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudentEnrollment(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Student enrollment deleted successfully!";
            return RedirectToAction(nameof(ManagerPage));
        }

    }
}
