using SIMS_2_.Models;

namespace SIMS_2_.Repositories
{
    public interface IStudentRepository
    {
        void Add(Student student);
        Student Get(int id);
        void Update(Student student);
        void Delete(int id);
        IEnumerable<Student> GetAll();
    }
}
