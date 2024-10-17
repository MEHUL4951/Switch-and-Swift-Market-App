using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switch_and_Shift.Interface
{
    public interface ICategories
    {
        Task<List<Categories>> GetAllCategoriesAsync();
        Task<Categories> GetCategoryByIdAsync(int? id);
        Task AddCategoryAsync(Categories category);
        Task UpdateCategoryAsync(Categories category);
        Task DeleteCategoryAsync(int id);
        Task<bool> CategoryExistsAsync(int id);

    }
}
