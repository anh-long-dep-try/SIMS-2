using SIMS_2_.Data;
using SIMS_2_.Models;

namespace SIMS_2_.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _db;

        public StudentRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Student student)
        {
            _db.Students.Add(student);
            _db.SaveChanges();
        }

        public Student Get(int id)
        {
            return _db.Students.Find(id);
        }

        public void Update(Student student)
        {
            _db.Students.Update(student);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var student = _db.Students.Find(id);
            if (student != null)
            {
                _db.Students.Remove(student);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Student> GetAll()
        {
            return _db.Students.ToList();
        }

        
    }
}
