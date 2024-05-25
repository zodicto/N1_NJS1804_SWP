using Microsoft.EntityFrameworkCore;
using OrchidBusinessObjects;
using OrchidDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidRepositories
{
    public interface IOrchidRepositories
    {
        public Student AddStudent(Student student);


        public IQueryable<User> GetAllUsers();


        public Student GetStudent(string id);


        public User GetUserById(string id);


        public Student LoginStudent(string username, string password);


        public bool RegisterStudent(string username, string password, string firstName, string lastName, string gmail, string birthdate, bool gender);
        

    }
}
