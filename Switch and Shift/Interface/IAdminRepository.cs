using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switch_and_Shift.Interface
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByEmailAndPasswordAsync(string email, string password);

        Task<List<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(int? id);
        Task AddAdminAsync(Admin admin);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(Users user);
        bool UserExists(int id);

    }
}
