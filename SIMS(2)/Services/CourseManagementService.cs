using Microsoft.EntityFrameworkCore;
using SIMS_2_.Data;
using SIMS_2_.Models;
using SIMS_2_.Repositories;

namespace SIMS_2_.Services
{
    public class CourseManagementService : ICourseManagementService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CourseManagementService> _logger;

        public CourseManagementService(AppDbContext context, ILogger<CourseManagementService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Course> CreateCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Course created successfully: {course.CourseName}");
                return course;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating course: {ex.Message}");
                throw;
            }
        }

        public async Task<Course> UpdateCourse(Course course)
        {
            try
            {
                var existingCourse = await _context.Courses.FindAsync(course.CourseId);
                if (existingCourse == null)
                {
                    throw new KeyNotFoundException($"Course with ID {course.CourseId} not found");
                }

                existingCourse.CourseName = course.CourseName;
                existingCourse.CourseCode = course.CourseCode;
                existingCourse.Credits = course.Credits;
                existingCourse.FacultyId = course.FacultyId;
                existingCourse.CalendarId = course.CalendarId;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Course updated successfully: {course.CourseName}");
                return existingCourse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating course: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteCourse(int courseId)
        {
            try
            {
                var course = await _context.Courses.FindAsync(courseId);
                if (course == null)
                {
                    return false;
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Course deleted successfully: {courseId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting course: {ex.Message}");
                throw;
            }
        }

        public async Task<Course> GetCourse(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Faculty)
                .Include(c => c.AcademicCalendar)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses
                .Include(c => c.Faculty)
                .Include(c => c.AcademicCalendar)
                .Include(c => c.Enrollments)
                .ToListAsync();
        }
    }
} 