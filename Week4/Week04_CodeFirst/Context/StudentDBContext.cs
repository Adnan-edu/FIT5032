using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Week04_CodeFirst.Models;

namespace Week04_CodeFirst.Context
{
    public class StudentDBContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Unit> Units { get; set; }
    }
}