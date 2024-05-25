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
        private readonly OrchidDAO orchidDAO = null;
        public Student AddStudent(Student student)
        {
            return orchidDAO.AddStudent(student);
        }
        public OrchidRepository()
        {
            orchidDAO = new OrchidDAO();
        }

        public IQueryable<User> GetAllUsers()
        {
            return orchidDAO.GetAllUsers();
        }

        public Student GetStudent(string id)
        {
            return orchidDAO.GetStudent(id);
        }

        public User GetUserById(string id)
        {
            return orchidDAO.GetUserById(id);
        }

        public Student LoginStudent(string username, string password)
        {
            return orchidDAO.LoginStudent(username, password);
        }

        public bool RegisterStudent(string username, string password, string firstName, string lastName, string gmail, string birthdate, bool gender)
        {
            return orchidDAO.RegisterStudent(username, password, firstName, lastName, gmail, birthdate, gender);
        }
    }
}
