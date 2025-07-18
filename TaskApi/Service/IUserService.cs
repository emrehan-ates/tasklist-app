using TaskApi.Models;

namespace TaskApi.Services
{
    public interface IUserService
    {
        Task<User?> ValidateUser(string email, string password);
        Task<User?> GetUserByEmail(string email);
        Task<bool> AddUser(UserDTO newuser);
        Task<bool> DeleteUser(int id);
        Task<User> GetProfile(int id);
    }
}