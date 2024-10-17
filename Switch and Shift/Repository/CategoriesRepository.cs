using Microsoft.EntityFrameworkCore;
using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Switch_and_Shift.Repository
{
    public class CategoriesRepository : ICategories
    {
        private readonly ApplicationDbContext _context;

        public CategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categories>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categories> GetCategoryByIdAsync(int? id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.categories_id == id);
        }

        public async Task AddCategoryAsync(Categories category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Categories category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.categories_id == id);
        }
    }
}
