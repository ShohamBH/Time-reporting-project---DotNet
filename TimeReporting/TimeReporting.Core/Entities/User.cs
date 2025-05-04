using System;

namespace TimeReporting.Core.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public DateTime Birthdate { get; set; }
        public Role Role { get; set; } // Enum - Admin או Employee

        public User(string id, string firstName, string lastName, string email, string phone, string password, string city, DateTime birthdate, Role role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Password = password;
            City = city;
            Birthdate = birthdate;
            Role = role;
        }

        public User() { }
    }
}