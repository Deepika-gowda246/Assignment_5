using Assignment_5_Testing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestStudent.Test
{
    public class DummyDataDBInitializer
    {
        public DummyDataDBInitializer()
        {
        }

        public void Seed(StudentTestContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Students.AddRange(
                new Student() { Name = "Deepika", Address = "CKM", Class = "1puc" },
                new Student() { Name = "Disha", Address = "ckm", Class = "2puc" },
                new Student() { Name = "Kartik", Address = "DWD", Class = "1puc" },
                new Student() { Name = "Shashi", Address = "mysore", Class = "2puc" }
            );

            
            context.SaveChanges();
        }

    }
}
