using SIMS_2_.Models;
using System.Linq;

namespace SIMS_2_.Repositories
{
    public interface IStudentRepository
    {
        void Add(Student student);
        Student Get(int id);
        void Update(Student student);
        void Delete(int id);
        IQueryable<Student> GetAll(); // Changed to IQueryable to support Include
        void AddEnrollment(Enrollment enrollment); // New method to add an enrollment
    }
}