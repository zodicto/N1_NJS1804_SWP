using Microsoft.EntityFrameworkCore;
using OrchidBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidRepositories
{
    public interface IOrchidRepositories
    {
        public List<Student> GetStudent();
        public Student GetStudent(int id);
        public Student AddStudent(Student student);
    }
}
