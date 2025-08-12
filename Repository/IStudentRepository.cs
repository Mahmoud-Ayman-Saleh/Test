using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Repository
{
    public interface IStudentRepository
    {
        public List<Student> GetAllStudents();
        public Student GetStudentById(int id);
        public void DeleteStudent(int id);
        public List<Student> GetStudentsByName(string name);
        public void AddStudent(Student student);
        public void UpdateStudent(Student student);
        public void save();
    }
}