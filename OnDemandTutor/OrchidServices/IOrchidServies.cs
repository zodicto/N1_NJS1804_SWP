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
        public Student AddStudent(Student student);
        void AddStudent(User user);
        Task DeleteUser(string id);
        public IQueryable<User> GetAllUsers();


        public Student GetStudent(string id);


        public User GetUserById(string id);


        public Student LoginStudent(string username, string password);


        public bool RegisterStudent(string username, string password, string firstName, string lastName, string gmail, string birthdate, bool gender);
        Task SaveChangesAsync();
        void UpdateUser(User user);
    }
}
