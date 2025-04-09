using SIMS_2_.Models;

namespace SIMS_2_.Repositories
{
    public interface ICourseManagementService
    {
        Task<Course> CreateCourse(Course course);
        Task<Course> UpdateCourse(Course course);
        Task<bool> DeleteCourse(int courseId);
        Task<Course> GetCourse(int courseId);
        Task<IEnumerable<Course>> GetAllCourses();
    }
} 