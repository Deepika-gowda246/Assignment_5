using Assignment_5_Testing.Models;
using Assignment_5_Testing.Viewmodel;
using Microsoft.Extensions.Hosting;

namespace Assignment_5_Testing.Repository
{
    public interface IStudentRepository
    {
       

        Task<List<StudentViewModel>> GetStudents();

        Task<StudentViewModel> GetStudent(int? StudentId);

        Task<int> AddStudent(Student student);
        Task<int> DeleteStudent(int? StudentId);

        Task UpdateStudent(Student student);

    }
}
