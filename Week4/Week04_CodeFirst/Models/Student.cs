using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Week04_CodeFirst.Models
{
    public class Student
    {
        public int Id { get; set; }
        public String FirstName { get; set; }

        public String LasttName { get; set; }

        public virtual List<Unit> Units { get; set; }
    }
}