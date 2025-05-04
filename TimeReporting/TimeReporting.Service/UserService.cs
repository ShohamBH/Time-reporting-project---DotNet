using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeReporting.Core.DTOs;
using TimeReporting.Core.Entities;
using TimeReporting.Core.Interface;
using TimeReporting.Data;
using System; // הוספת שימוש ב-Exception

namespace TimeReporting.Service
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public List<User> GetFullUsers()
        {
            return _dataContext.Users.ToList();
        }
        public List<DTOUser> GetUsers()
        {
            return _mapper.Map<List<DTOUser>>(_dataContext.Users.ToList());
        }

        public User GetFullUser(string id)
        {
            return _dataContext.Users.FirstOrDefault(i => i.Id == id);
        }

        public DTOUser GetUser(string id)
        {
            User user = _dataContext.Users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                return _mapper.Map<DTOUser>(user);
            }
            return null;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            if (string.IsNullOrEmpty(user.Id) || user.Id.Length != 9)
            {
                throw new ArgumentException("User ID must be exactly 9 digits.", nameof(user.Id));
            }

            if (_dataContext.Users.Any(u => u.Id == user.Id))
            {
                throw new InvalidOperationException($"User with ID {user.Id} already exists.");
            }

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(string id, User user)
        {
            var existingUser = await _dataContext.Users.FindAsync(id);

            if (existingUser == null)
            {
                return false; // או לזרוק חריגה NotFoundException
            }

            existingUser.City = user.City;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.LastName = user.LastName;
            existingUser.FirstName = user.FirstName;
            existingUser.Role = user.Role;
            existingUser.Birthdate = user.Birthdate;

            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _dataContext.Users.FindAsync(id);

            if (user == null)
            {
                return false; // או לזרוק חריגה NotFoundException
            }

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}