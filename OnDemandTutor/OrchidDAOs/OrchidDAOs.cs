using OrchidBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidDAOs
{
    public class OrchidDAO
    {
        private readonly DbMiniCapStoneContext dbCotext = null;
        public OrchidDAO()
        {
            if (dbCotext == null)
            {
                dbCotext = new DbMiniCapStoneContext();
            }
        }
        public List<Student> GetStudent()
        {
            return dbCotext.Students.ToList();
        }
        public Student GetStudent(int id)
        {
            return dbCotext.Students.FirstOrDefault(m => m.Id.Equals(id));
        }
        public Student AddStudent(Student student)
        {
            dbCotext.Students.Add(student);
            dbCotext.SaveChanges();
            return student;
        }
        
      
    }
}
