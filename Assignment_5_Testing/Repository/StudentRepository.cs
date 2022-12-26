using Assignment_5_Testing.Models;
using Assignment_5_Testing.Viewmodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Assignment_5_Testing.Repository
{
    public class StudentRepository : IStudentRepository
    {

        StudentTestContext db;
        public StudentRepository(StudentTestContext _db)
        {
            db = _db;
        }

        

        public async Task<List<StudentViewModel>> GetStudents()
        {
            if (db != null)
            {
                return await (from s in db.Students
                              
                              select new StudentViewModel
                              {
                                  StudentId = s.StudentId,
                                  Name = s.Name,
                                  Address = s.Address,
                                  Class = s.Class,
                                  
                              }).ToListAsync();
            }

            return null;
        }

        public async Task<StudentViewModel> GetStudent(int? StudentId)
        {
            if (db != null)
            {
                return await (from s in db.Students.Where(s => s.StudentId == StudentId)
                             
                              select new StudentViewModel
                              {
                                  StudentId = s.StudentId,
                                  Name = s.Name,
                                  Address = s.Address,
                                  Class = s.Class,
                              }).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<int> AddStudent(Student student)
        {
            if (db != null)
            {
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();

                return student.StudentId;
            }

            return 0;
        }

        public async Task<int> DeleteStudent(int? StudentId)
        {
            int result = 0;

            if (db != null)
            {
                //Find the student for specific student id
                var student = await db.Students.FirstOrDefaultAsync(x => x.StudentId == StudentId);

                if (student != null)
                {
                    //Delete that student
                    db.Students.Remove(student);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }


        public async Task UpdateStudent(Student student)
        {
            if (db != null)
            {
                //update  student
                db.Students.Update(student);

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }
    }
}

