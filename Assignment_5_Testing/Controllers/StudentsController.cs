using Assignment_5_Testing.Models;
using Assignment_5_Testing.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Assignment_5_Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        IStudentRepository studentRepository;
        public StudentsController(IStudentRepository _studentRepository)
        {
            studentRepository = _studentRepository;
        }


        [HttpGet]
        [Route("GetStudents")]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await studentRepository.GetStudents();
                if (students == null)
                {
                    return NotFound();
                }

                return Ok(students);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent(int? StudentId)
        {
            if (StudentId == null)
            {
                return BadRequest();
            }

            try
            {
                var student = await studentRepository.GetStudent(StudentId);

                if (student == null)
                {
                    return NotFound();
                }

                return Ok(student);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] Student model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var StudentId = await studentRepository.AddStudent(model);
                    if (StudentId > 0)
                    {
                        return Ok(StudentId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int? StudentId)
        {
            int result = 0;

            if (StudentId == null)
            {
                return BadRequest();
            }

            try
            {
                result = await studentRepository.DeleteStudent(StudentId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] Student model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await studentRepository.UpdateStudent(model);

                    return Ok();
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }

            return BadRequest();
        }

    }
}
