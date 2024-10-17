using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _context;

    public AdminRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Admin> GetAdminByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(a => a.Admin_Email == email && a.Admin_Password == password);
    }

    public async Task<List<Users>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Users> GetUserByIdAsync(int? id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == id);
    }


    public async Task AddAdminAsync(Admin admin)
    {
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(Users user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        
    }

    public async Task DeleteUserAsync(Users user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
 

    }

    public bool UserExists(int id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
}
