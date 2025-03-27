using SIMS_2_.Data;
using SIMS_2_.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            return _db.Students
                .Include(s => s.User)
                .Include(s => s.Enrollments)
                .FirstOrDefault(s => s.StudentId == id);
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

        public IQueryable<Student> GetAll()
        {
            return _db.Students.AsQueryable();
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            _db.Enrollments.Add(enrollment);
            _db.SaveChanges();
        }
    }
}