using Microsoft.EntityFrameworkCore;
using SIMS_2_.Data;
using SIMS_2_.Models;
using SIMS_2_.Repositories;

namespace SIMS_2_.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(AppDbContext context, ILogger<EnrollmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Enrollment> EnrollStudent(int studentId, int courseId, string grade)
        {
            try
            {
                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.Now,
                    Grade = grade
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Student {studentId} enrolled in course {courseId}");
                return enrollment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enrolling student: {ex.Message}");
                throw;
            }
        }

        public async Task<Enrollment> UpdateEnrollment(int enrollmentId, int studentId, int courseId)
        {
            try
            {
                var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
                if (enrollment == null)
                {
                    throw new KeyNotFoundException($"Enrollment with ID {enrollmentId} not found");
                }

                enrollment.StudentId = studentId;
                enrollment.CourseId = courseId;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Enrollment {enrollmentId} updated successfully");
                return enrollment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating enrollment: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteEnrollment(int enrollmentId)
        {
            try
            {
                var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
                if (enrollment == null)
                {
                    return false;
                }

                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Enrollment {enrollmentId} deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting enrollment: {ex.Message}");
                throw;
            }
        }

        public async Task<Enrollment> GetEnrollment(int enrollmentId)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourse(int courseId)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudent(int studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollments()
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }
    }
} 