using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;

namespace Switch_and_Shift.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategories _categoryRepository;

        public CategoriesController(ApplicationDbContext context,ICategories categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }

        // GET: CATEGORIES
        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepository.GetAllCategoriesAsync());
        }

        // GET: CATEGORIES/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Category =await  _categoryRepository.GetCategoryByIdAsync(id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // GET: CATEGORIES/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CATEGORIES/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("categories_id,categories_name")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                
                await _categoryRepository.AddCategoryAsync(categories); 
                return RedirectToAction(nameof(Index));
            }
            return View(categories);
        }

        // GET: CATEGORIES/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category= await _context.Categories.FindAsync(id);
            var category = await  _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CATEGORIES/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("categories_id,categories_name")] Categories categories)
        {
            if (id != categories.categories_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await  _categoryRepository.UpdateCategoryAsync(categories);                  
                }
                catch (DbUpdateConcurrencyException)
                {
                 
                    if (! await _categoryRepository.CategoryExistsAsync(categories.categories_id)) 
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categories);
        }

        // GET: CATEGORIES/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // POST: CATEGORIES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
          
            var Category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (Category != null)
            {
                await _categoryRepository.DeleteCategoryAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private Task<bool> CategoryExists(int id)
        {
            return  _categoryRepository.CategoryExistsAsync(id);
        }
    }
}
