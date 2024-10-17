using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switch_and_Shift.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(int userId);
        Task AddUserAsync(Users user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(int userId);
        Task<Users> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(int userId);
    }
}
