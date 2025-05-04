using System;
using TimeReporting.Core.Entities;

namespace TimeReporting.Core.DTOs
{
    public class DTOUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public DateTime Birthdate { get; set; }
        public Role Role { get; set; } // Enum - Admin או Employee
    }
}