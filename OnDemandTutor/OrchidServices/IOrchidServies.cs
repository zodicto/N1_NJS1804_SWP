using OrchidBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidServices
{
    public interface IOrchidServies
    {
        public List<Student> GetStudent();
        public Student GetStudent(int id);
        public Student AddStudent(Student student);
        Task GetStudentsAsync();
    }
}
