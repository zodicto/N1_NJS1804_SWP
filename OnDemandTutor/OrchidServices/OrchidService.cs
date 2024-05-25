using OrchidBusinessObjects;
using OrchidRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidServices
{
    public class OrchidService : IOrchidServies
    {
        private readonly IOrchidRepositories orchidRepositories = null;
        public OrchidService()
        {
            if (orchidRepositories == null)
            {
                orchidRepositories = new OrchidRepository();
            }
        }
        public Student AddStudent(Student student)
        {
            return orchidRepositories.AddStudent(student);

        }

        public List<Student> GetStudent()
        {
            return orchidRepositories.GetStudent();
        }

        public Student GetStudent(int id)
        {
            return orchidRepositories.GetStudent(id);
        }
    }
}
