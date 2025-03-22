using SIMS_2_.Models;

namespace SIMS_2_.Repositories
{
    public interface ICourseRepository
    {
        void Add(Course course);
        Course Get(int id);
        void Update(Course course);
        void Delete(int id);
        IEnumerable<Course> GetAll();
    }
}
