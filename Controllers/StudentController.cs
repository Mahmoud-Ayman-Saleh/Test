using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.Repository;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // TestDBContext db;
        // public StudentController(TestDBContext db)
        // {
        //     this.db = db;
        // }

        IStudentRepository st;

        public StudentController(IStudentRepository st)
        {
            this.st = st;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var students = st.GetAllStudents();
            
            return Ok(students);
        }
        [HttpGet("{id:int}")]

        public IActionResult GetById(int id)
        {
            var student = st.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            // Map to DTO if needed
            var studentDto = new DTO.StudentDto
            {
                StudentID = student.StudentID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                DepartmentName = student.Department.DepartmentName
            };
            return Ok(studentDto);
        }
        [HttpDelete]
        public IActionResult delete(int id)
        {
            st.DeleteStudent(id);
            st.save();
            return NoContent();
        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            var students = st.GetStudentsByName(name);
            if (students.Count == 0)
            {
                return NotFound();
            }
            return Ok(students);
        }

        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (student == null) return BadRequest("Student cannot be null");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            st.AddStudent(student);
            st.save();
            return CreatedAtAction("GetById", new { id = student.StudentID }, student);
        }

        [HttpPut]
        public IActionResult Update(Student student)
        {
            if (student == null) return BadRequest("Student cannot be null");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            st.UpdateStudent(student);
            st.save();
            return NoContent();
        }
    }
}
