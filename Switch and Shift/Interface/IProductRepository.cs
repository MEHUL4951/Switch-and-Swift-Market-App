using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switch_and_Shift.Interface
{
    public interface IProductRepository
    {

        Task<List<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int? id);
        Task AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task DeleteProductAsync(Products product);
        Task<List<Products>> SearchProductsAsync(string productSearch, string districtSearch, int? upperRange, int? lowerRange, int currentUserId);

        Task<List<Products>> GetFilteredProductsAsync(int userId, string brand, string category);
        bool ProductExists(int id);
    }
}
