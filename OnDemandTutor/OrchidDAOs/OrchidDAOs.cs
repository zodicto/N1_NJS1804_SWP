using OrchidBusinessObjects;
using System;
using System.Linq;

namespace OrchidDAOs
{
    public class OrchidDAO
    {
        private readonly DbMiniCapStoneContext dbContext = null;

        public OrchidDAO()
        {
            if (dbContext == null)
            {
                dbContext = new DbMiniCapStoneContext();
            }
        }

        // Method to get student by ID
        public Student GetStudent(string id)
        {
            return dbContext.Students.FirstOrDefault(m => m.Id.Equals(id));
        }

        // Method to add a new student (registration)
        public Student AddStudent(Student student)
        {
            dbContext.Students.Add(student);
            dbContext.SaveChanges();
            return student;
        }

        // Method to register a new student
        public bool RegisterStudent(string username, string password, string firstName, string lastName, string gmail, string birthdate, bool gender)
        {
            // Check if the user already exists
            var existingUser = dbContext.Users.FirstOrDefault(u => u.Username == username || u.Gmail == gmail);
            if (existingUser != null)
            {
                return false; // User already exists
            }

            // Create new user
            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Gmail = gmail,
                Birthdate = birthdate,
                Gender = gender,
                Status = "Active"
            };

            dbContext.Users.Add(newUser);

            // Create new student linked to the new user
            var newStudent = new Student
            {
                Id = Guid.NewGuid().ToString(),
                UserId = newUser.Id
            };

            dbContext.Students.Add(newStudent);
            dbContext.SaveChanges();
            return true; // Registration successful
        }

        // Method to login a student
        public Student LoginStudent(string username, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                return dbContext.Students.FirstOrDefault(s => s.UserId == user.Id);
            }
            return null; // Login failed
        }

        // Method to get user by ID
        public User GetUserById(string id)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id.Equals(id));
        }

        // Method to get all users
        public IQueryable<User> GetAllUsers()
        {
            return dbContext.Users;
        }
    }
}
