using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly TestDBContext db;

        public StudentRepository(TestDBContext db)
        {
            this.db = db;
        }

        public void AddStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            db.Students.Add(student);
        }

        public void DeleteStudent(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found.");
            }
            db.Students.Remove(student);
        }

        public List<Student> GetAllStudents()
        {
            return db.Students
                     .Include(s => s.Department)
                     .ToList();
        }

        public Student GetStudentById(int id)
        {
            var student = db.Students
                            .Include(s => s.Department) // load Department
                            .FirstOrDefault(s => s.StudentID == id);

            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found.");
            }
            return student;
        }

        public List<Student> GetStudentsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }

            return db.Students
                     .Include(s => s.Department) // load Department
                     .Where(s => s.FirstName.Contains(name))
                     .ToList();
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = db.Students.Find(student.StudentID);
            if (existingStudent != null)
            {
                var departmentExists = db.Departments.Any(d => d.DepartmentID == student.DepartmentID);
                if (!departmentExists)
                {
                    throw new ArgumentException($"Department with ID {student.DepartmentID} does not exist.");
                }

                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.Email = student.Email;
                existingStudent.Phone = student.Phone;
                existingStudent.DepartmentID = student.DepartmentID;
            }
        }

        public void save()
        {
            db.SaveChanges();
        }
    }   
}
