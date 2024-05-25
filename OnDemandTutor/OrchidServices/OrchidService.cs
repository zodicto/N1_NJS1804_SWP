using OrchidBusinessObjects;
using OrchidDAOs;
using OrchidRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidServices
{
    public class OrchidService : IOrchidServies// Corrected interface name
    {
        private readonly IOrchidRepositories orchidRepositories = null;

        public OrchidService()
        {
            if (orchidRepositories == null)
            {
                orchidRepositories = new OrchidRepository();
            }
        }

        // Constructor injection


        public Student AddStudent(Student student)
        {
            return orchidRepositories.AddStudent(student);
        }

        public void AddStudent(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAllUsers()
        {
            return orchidRepositories.GetAllUsers();
        }

        public Student GetStudent(string id)
        {
            return orchidRepositories.GetStudent(id);
        }

        public User GetUserById(string id)
        {
            return orchidRepositories.GetUserById(id);
        }

        public Student LoginStudent(string username, string password)
        {
            return orchidRepositories.LoginStudent(username, password);
        }

        public bool RegisterStudent(string username, string password, string firstName, string lastName, string gmail, string birthdate, bool gender)
        {
            return orchidRepositories.RegisterStudent(username, password, firstName, lastName, gmail, birthdate, gender);
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
