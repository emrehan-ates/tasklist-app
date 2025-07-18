

using Microsoft.EntityFrameworkCore;
using TaskApi.Helpers;
using TaskApi.Models;
using BCrypt.Net;
using AutoMapper;

namespace TaskApi.Services
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _map;

        public UserService(AppDbContext context, IMapper map)
        {
            _context = context;
            _map = map;
        }

        public async Task<User?> ValidateUser(string email, string password)
        {
            try
            {
                var user = await _context.Users.FirstAsync(u => u.user_email == email);
                if (!BCrypt.Net.BCrypt.Verify(password, user.user_password))
                {
                    throw new Exception();
                }
                return user;
               
            }
            catch (Exception ex)
            {
                
                return null;
            }
            
    

            
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.user_email == email);
        }

        public async Task<bool> AddUser(UserDTO newuser)
        {
            var user = _map.Map<User>(newuser);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("Doesnt exist :D");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetProfile(int id)
        {

            var temp = await _context.Users.AsNoTracking().FirstAsync(u => u.user_id == id) ?? throw new Exception("Is this supposed to happen?");

            return temp;
        }
    }
}