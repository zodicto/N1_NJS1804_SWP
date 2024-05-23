using OrchidBusinessObjects;
using OrchidDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidRepositories
{
    public class OrchidRepository : IOrchidRepositories
    {
        private readonly OrchidDAO orchidDAOs = null;

        public OrchidRepository()
        {
            if (orchidDAOs == null)
            {
                orchidDAOs = new OrchidDAO();
            }
        }

        public Student AddStudent(Student student)
        {
            return orchidDAOs.AddStudent(student);
        }

        public List<Student> GetStudent()
        {
            return orchidDAOs.GetStudent();
        }

        public Student GetStudent(int id)
        {
            return orchidDAOs.GetStudent(id);
        }
    }
}
