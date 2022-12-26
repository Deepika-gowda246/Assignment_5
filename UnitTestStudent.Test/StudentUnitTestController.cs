using Assignment_5_Testing.Controllers;
using Assignment_5_Testing.Models;
using Assignment_5_Testing.Repository;
using Assignment_5_Testing.Viewmodel;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestStudent.Test
{
    public class StudentUnitTestController
    {

        private StudentRepository repository;
        public static DbContextOptions<StudentTestContext> dbContextOptions { get; }
        public static string connectionString = "Server=BLR1-LHP-N80939\\SQLEXPRESS;Initial Catalog=StudentTest;MultipleActiveResultSets=true;Trusted_Connection=True;Encrypt=False";

        static StudentUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<StudentTestContext>()
                .UseSqlServer(connectionString)
                .Options;
        }
        public StudentUnitTestController()
        {
            var context = new StudentTestContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            repository = new StudentRepository(context);

        }

        //GET BY ID

        [Fact]
        public async void Task_GetStudentById_Return_OkResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var StudentId = 2;

            //Act  
            var data = await controller.GetStudent(StudentId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetStudentById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var StudentId = 7;

            //Act  
            var data = await controller.GetStudent(StudentId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }


        [Fact]
        public async void Task_GetStudentById_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            int? StudentId = null;

            //Act  
            var data = await controller.GetStudent(StudentId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }




        [Fact]
        public async void Task_GetStudentById_MatchResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            int? StudentId = 1;

            //Act  
            var data = await controller.GetStudent(StudentId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var student = okResult.Value.Should().BeAssignableTo<StudentViewModel>().Subject;

            Assert.Equal("Deepika", student.Name);
            Assert.Equal("CKM", student.Address);
            Assert.Equal("1puc", student.Class);
        }



        // GET ALL 


        [Fact]
        public async void Task_GetStudents_Return_OkResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);

            //Act  
            var data = await controller.GetStudents();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_GetStudents_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);

            //Act  
            var data = controller.GetStudents();
            data = null;

            if (data != null)
                //Assert  
                Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetStudents_MatchResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);

            //Act  
            var data = await controller.GetStudents();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var student = okResult.Value.Should().BeAssignableTo<List<StudentViewModel>>().Subject;

            Assert.Equal("Deepika", student[0].Name);
            Assert.Equal("CKM", student[0].Address);
            Assert.Equal("1puc", student[0].Class);

            Assert.Equal("Disha", student[1].Name);
            Assert.Equal("ckm", student[1].Address);
            Assert.Equal("2puc", student[1].Class);


           


        }



        // ADD NEW STUDENT 

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var student = new Student() { Name = "Ragavi", Address = "Banglore", Class = "2PUC" };

            //Act  
            var data = await controller.AddStudent(student);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Add_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new StudentsController(repository);
           
            Student student = new Student() { StudentId = 1,Name = "", Address = "Banglore", Class = "2PUC" };

            //Act              
            var data = await controller.AddStudent(student);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }




        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var student = new Student() { Name = "Deepika", Address = "CKM", Class = "1puc" };

            //Act  
            var data = await controller.AddStudent(student);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
             

            Assert.Equal(5, okResult.Value);
        
        }


        // UPDATE THE STUDENT


        [Fact]
        public async void Task_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var StudentId = 2;

            //Act  
            var existingPost = await controller.GetStudent(StudentId);
            var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<StudentViewModel>().Subject;

            var student = new Student();
            student.Name = "Chandrika";
            student.Address = result.Address;
            student.Class = result.Class;
            

            var updatedData = await controller.UpdateStudent(student);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var StudentId = 2;

            //Act  
            var existingPost = await controller.GetStudent(StudentId);
            var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<StudentViewModel>().Subject;

            var student = new Student();
            student.StudentId = 1;
            student.Name = "Chandrika";
            student.Address = result.Address;
            student.Class = result.Class;

            var data = await controller.UpdateStudent(student);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }



        [Fact]
        public async void Task_Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var StudentId = 2;

            //Act  
            var existingStudent = await controller.GetStudent(StudentId);
            var okResult = existingStudent.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<StudentViewModel>().Subject;

            var student = new Student();
            student.StudentId = 8;
            student.Name = "Chandrika";
            student.Address = result.Address;
            student.Class = result.Class;

            var data = await controller.UpdateStudent(student);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }




        // DELETE STUDENT

        [Fact]
        public async void Task_Delete_Student_Return_OkResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var StudentId = 2;

            //Act  
            var data = await controller.DeleteStudent(StudentId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Task_Delete_Student_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            var StudentId = 7;

            //Act  
            var data = await controller.DeleteStudent(StudentId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }


        [Fact]
        public async void Task_Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new StudentsController(repository);
            int? StudentId = null;

            //Act  
            var data = await controller.DeleteStudent(StudentId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

    }
}
