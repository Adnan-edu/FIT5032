using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Week04_CodeFirst.Context;
using Week04_CodeFirst.Models;

namespace Week04_CodeFirst.Repository
{
    public class StudentRepository
    {
        public List<Student> GetStudents()
        {
            StudentDBContext StudentContext = new StudentDBContext();
            return StudentContext.Students.ToList();
        }
    }
}