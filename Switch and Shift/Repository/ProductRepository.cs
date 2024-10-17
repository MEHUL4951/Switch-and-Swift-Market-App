using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;

namespace Switch_and_Shift.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Products> GetProductByIdAsync(int? id)
        {
           Products product = await  _context.Products.FirstOrDefaultAsync(p => p.Product_ID == id);
            if (product == null)
            {
                return null;
            }
            return product;
        }

        public async Task AddProductAsync(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Products product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Products product)
        {
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Products>> SearchProductsAsync(string productSearch, string districtSearch, int? upperRange, int? lowerRange, int currentUserId)
        {
            var productQuery = _context.Products.AsQueryable();
            productQuery = productQuery.Where(x => x.UserId != currentUserId);

            if (!string.IsNullOrEmpty(productSearch))
            {
                productQuery = productQuery.Where(x => x.product_category.Contains(productSearch));
            }

            if (!string.IsNullOrEmpty(districtSearch))
            {
                productQuery = productQuery.Where(x => x.user_location.Contains(districtSearch));
            }

            if (upperRange.HasValue)
            {
                productQuery = productQuery.Where(x => x.product_price <= upperRange.Value);
            }

            if (lowerRange.HasValue)
            {
                productQuery = productQuery.Where(x => x.product_price >= lowerRange.Value);
            }

            return await productQuery.AsNoTracking().ToListAsync();
        }

        public async Task<List<Products>> GetFilteredProductsAsync(int userId, string brand, string category)
        {
            var productQuery = _context.Products.Where(x => x.UserId == userId);

            if (!string.IsNullOrEmpty(brand))
            {
                productQuery = productQuery.Where(x => x.product_brand.Contains(brand) || x.product_model.Contains(brand));
            }

            if (!string.IsNullOrEmpty(category))
            {
                productQuery = productQuery.Where(x => x.product_category.Contains(category));
            }

            return await productQuery.AsNoTracking().ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Product_ID == id);
        }
    }
}
