using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Repository
{
    public class StudentRepository : IStudentRepository
    {
        TestDBContext db;

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
            return db.Students.ToList();
        }

        public Student GetStudentById(int id)
        {
            var student = db.Students.Find(id);
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
            return db.Students.Where(s => s.FirstName.Contains(name)).ToList();
        }

        public void UpdateStudent(Student student)
        {
            db.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void save()
        {
            db.SaveChanges();
        }
    }   
}