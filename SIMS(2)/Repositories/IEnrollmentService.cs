using SIMS_2_.Models;

namespace SIMS_2_.Repositories
{
    public interface IEnrollmentService
    {
        Task<Enrollment> EnrollStudent(int studentId, int courseId, string grade);
        Task<Enrollment> UpdateEnrollment(int enrollmentId, int studentId, int courseId);
        Task<bool> DeleteEnrollment(int enrollmentId);
        Task<Enrollment> GetEnrollment(int enrollmentId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByCourse(int courseId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudent(int studentId);
        Task<IEnumerable<Enrollment>> GetAllEnrollments();
    }
} 