using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeReporting.Core.DTOs;
using TimeReporting.Core.Entities;

namespace TimeReporting.Core.Interface
{
    public interface IUserService
    {
        public List<User> GetFullUsers();
        public List<DTOUser> GetUsers();

        public User GetFullUser(string id);
        public DTOUser GetUser(string id);

        public Task<bool> AddUserAsync(User user);
        public Task<bool> UpdateUserAsync(string id, User user);
        public Task<bool> DeleteUserAsync(string id);
        //public bool DeleteUser(User user);
        
    }
}
