using SIMS_2_.Data;
using SIMS_2_.Models;

namespace SIMS_2_.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _db;

        public CourseRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Course course)
        {
            _db.Courses.Add(course);
            _db.SaveChanges();
        }

        public Course Get(int id)
        {
            return _db.Courses.Find(id);
        }

        public void Update(Course course)
        {
            _db.Courses.Update(course);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = _db.Courses.Find(id);
            if (course != null)
            {
                _db.Courses.Remove(course);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Course> GetAll()
        {
            return _db.Courses.ToList();
        }
    }
}
