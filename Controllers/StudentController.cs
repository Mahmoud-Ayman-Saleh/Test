using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.Repository;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository st;
        private readonly TestDBContext db;

        public StudentController(IStudentRepository st, TestDBContext db)
        {
            this.st = st;
            this.db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var students = st.GetAllStudents();

            var studentDtos = students.Select(s => new DTO.StudentDto
            {
                StudentID = s.StudentID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Phone = s.Phone,
                DepartmentID = s.DepartmentID ?? 0,
                DepartmentName = s.Department?.DepartmentName ?? "No Department"
            }).ToList();

            return Ok(studentDtos);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var student = st.GetStudentById(id);
            if (student == null) return NotFound();

            var studentDto = new DTO.StudentDto
            {
                StudentID = student.StudentID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                DepartmentID = student.DepartmentID ?? 0,
                DepartmentName = student.Department?.DepartmentName ?? "No Department"
            };
            return Ok(studentDto);
        }

        [HttpGet("byname/{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            var students = st.GetStudentsByName(name);
            if (students.Count == 0) return NotFound();

            var studentDtos = students.Select(s => new DTO.StudentDto
            {
                StudentID = s.StudentID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Phone = s.Phone,
                DepartmentID = s.DepartmentID ?? 0,
                DepartmentName = s.Department?.DepartmentName ?? "No Department"
            }).ToList();

            return Ok(studentDtos);
        }

        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (student == null) return BadRequest("Student cannot be null");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var departmentExists = db.Departments.Any(d => d.DepartmentID == student.DepartmentID);
            if (!departmentExists)
            {
                return BadRequest($"Department with ID {student.DepartmentID} does not exist.");
            }

            st.AddStudent(student);
            st.save();
            return CreatedAtAction("GetById", new { id = student.StudentID }, student);
        }

        [HttpPut]
        public IActionResult Update(Student student)
        {
            if (student == null) return BadRequest("Student cannot be null");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var departmentExists = db.Departments.Any(d => d.DepartmentID == student.DepartmentID);
            if (!departmentExists)
            {
                return BadRequest($"Department with ID {student.DepartmentID} does not exist.");
            }

            st.UpdateStudent(student);
            st.save();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            st.DeleteStudent(id);
            st.save();
            return NoContent();
        }
    }
}
