using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Switch_and_Shift.Models;
using Microsoft.AspNetCore.Http;
using Switch_and_Shift.Models;
using Switch_and_Shift.Interface;

namespace Switch_and_Shift.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductRepository productRepository;
        private readonly ICategories categoryRepository;


        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment,IProductRepository productRepository, ICategories categoryRepository)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;

        }

        // GET: PRODUCTS, if there is any problem in running index, uncomment this
        /*        [HttpGet]
                public async Task<IActionResult> Index()
                {                   
                    return View(await _context.PRODUCTS.ToListAsync());
                }*/

        [HttpGet]
        public async Task<IActionResult> Index(string productSearch, string districtSearch, int? upperRange, int? lowerRange)
        {
            int currentUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            ViewData["GetProductDetails"] = productSearch;
            ViewData["GetDistrictDetails"] = districtSearch;
            ViewData["GetUpperRange"] = upperRange;
            ViewData["GetLowerRange"] = lowerRange;

            var products = await productRepository.SearchProductsAsync(productSearch, districtSearch, upperRange, lowerRange, currentUserId);
            return View(products);
        }


        [HttpGet]
        public IActionResult Clear(string productSearch, string districtSearch, int? upperRange, int? lowerRange)
        {
            ViewData["GetProductDetails"] = "";
            ViewData["GetDistrictDetails"] = "";
            ViewData["GetUpperRange"] = null;
            ViewData["GetLowerRange"] = null;
            return RedirectToAction("Index", "Products");
        }


        [HttpGet]
        public IActionResult ClearForMyProducts(string myProductBrand, string myProductCategory)
        {
            ViewData["GetMyProductBrand"] = "";
            ViewData["GetMyProductCategory"] = "";
            myProductBrand = "";
            myProductCategory = "";
            return RedirectToAction("MyProducts", "Products");
        }


        // GET: PRODUCTS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await productRepository.GetProductByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpGet]
        public async Task<IActionResult> soldOut(int? id)
        {

            var Product = await productRepository.GetProductByIdAsync(id); 
            ViewBag.reportBrand = Product.product_brand;
            ViewBag.reportPrice = Product.product_price;
            ViewBag.reportModel = Product.product_model;
            ViewBag.reportImageLocation = Product.image_name;


             await productRepository.DeleteProductAsync(Product);
         
            ViewBag.reportSellerEmail = HttpContext.Session.GetString("Email");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> soldOut(Report report)
        {   

                   
            if (report.buyer_email == HttpContext.Session.GetString("Email"))
            {
                ViewBag.NotificationOfReport = "You cannot give your own email";
                ViewBag.reportBrand = report.product_brand;
                ViewBag.reportPrice = report.product_price;
                ViewBag.reportModel = report.product_model;
                ViewBag.reportImageLocation = report.image_name;
                ViewBag.reportSellerEmail = HttpContext.Session.GetString("Email");
                return View();
            }

            if (_context.Users.Any(x => x.Email == report.buyer_email))
            {
                report.buyer_name = _context.Users.FirstOrDefault(m => m.Email == report.buyer_email).FirstName.ToString() +
                    " " + _context.Users.FirstOrDefault(m => m.Email == report.buyer_email).LastName.ToString();
                report.seller_name = _context.Users.FirstOrDefault(m => m.Email == HttpContext.Session.GetString("Email")).FirstName.ToString() +
                    " " + _context.Users.FirstOrDefault(m => m.Email == HttpContext.Session.GetString("Email")).LastName.ToString();
                _context.Report.Add(report);
                _context.SaveChanges();
                return RedirectToAction("Welcome", "Home");
            }
            else
            {
                ViewBag.NotificationOfReport = "Invalid Email, User Doesn't exist";
                ViewBag.reportBrand = report.product_brand;
                ViewBag.reportPrice = report.product_price;
                ViewBag.reportModel = report.product_model;
                ViewBag.reportImageLocation = report.image_name;
                ViewBag.reportSellerEmail = HttpContext.Session.GetString("Email");
                return View();
            }


        }



        // GET: PRODUCTS/Create
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            List<SelectListItem> selectlist = new List<SelectListItem>();
            foreach (Categories cat in categories)
            {
                selectlist.Add(new SelectListItem { Text = cat.categories_name, Value = cat.categories_id.ToString() });
            }
            ViewBag.categoryListItem = selectlist;
            return View();
        }

        // POST: PRODUCTS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product_ID,product_category,product_price,product_brand,product_model,product_details,product_warranty,product_usage,product_condition,UserId,ImageFile")] Products ProductsModel)
        {

            if (ModelState.IsValid)
            {
                int categoryId = Convert.ToInt32(ProductsModel.product_category);
                ProductsModel.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                var user = _context.Users.FirstOrDefault(m => m.UserId == ProductsModel.UserId); if (user != null && !string.IsNullOrEmpty(user.District))
                {
                    ProductsModel.user_location = user.District.ToString();
                }
                else
                {
                    // Handle the case when the user is null or the District is null/empty
                    ModelState.AddModelError("", "User or user location (District) not found.");
                    return View(ProductsModel); // Return to the view with an error message
                }
                ProductsModel.post_date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                Categories category = await categoryRepository.GetCategoryByIdAsync(categoryId);
                ProductsModel.product_category = category.categories_name.ToString();
                string wwwrootPath = _hostEnvironment.WebRootPath;
                string FileName = Path.GetFileNameWithoutExtension(ProductsModel.ImageFile.FileName);
                string extension = Path.GetExtension(ProductsModel.ImageFile.FileName);
                ProductsModel.image_name = FileName = FileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwrootPath + "/Image/" + FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ProductsModel.ImageFile.CopyToAsync(fileStream);
                }
                await productRepository.AddProductAsync(ProductsModel);
                return RedirectToAction("MyProducts", "Products");
            }
            var categories =await categoryRepository.GetAllCategoriesAsync();
            List<SelectListItem> selectlist = new List<SelectListItem>();
            foreach (Categories cat in categories)
            {
                selectlist.Add(new SelectListItem { Text = cat.categories_name, Value = cat.categories_id.ToString() });
            }
            ViewBag.categoryListItem = selectlist;
            return View(ProductsModel);
        }


        [HttpGet]
        public async Task<IActionResult> MyProducts(string myProductBrand, string myProductCategory)
        {
            int currentUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            ViewData["GetMyProductBrand"] = myProductBrand;
            ViewData["GetMyProductCategory"] = myProductCategory;

            var products = await productRepository.GetFilteredProductsAsync(currentUserId, myProductBrand, myProductCategory);

            return View(products);
        }

        // GET: PRODUCTS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await productRepository.GetProductByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        // POST: PRODUCTS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Product_ID,product_category,product_price,product_brand,product_model,product_details,product_warranty,product_usage,product_condition,UserId,image_name,post_date,user_location,ImageFile")] Products Product)
        {
            if (id != Product.Product_ID)
            {
                return NotFound();
            }

            try
            {
                if (Product.ImageFile != null)
                {
                    var ProductToDelete = await productRepository.GetProductByIdAsync(Product.Product_ID);
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", ProductToDelete.image_name);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    string wwwrootPath = _hostEnvironment.WebRootPath;
                    string FileName = Path.GetFileNameWithoutExtension(Product.ImageFile.FileName);
                    string extension = Path.GetExtension(Product.ImageFile.FileName);
                    Product.image_name = FileName = FileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwrootPath + "/Image/" + FileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Product.ImageFile.CopyToAsync(fileStream);
                    }
                }

                await productRepository.UpdateProductAsync(Product);
        
            }
            catch (DbUpdateConcurrencyException)
            {
                bool productExist = productRepository.ProductExists(Product.Product_ID); 
                if (!productExist)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Welcome", "Home");

        }

        // GET: PRODUCTS/Delete/5

        // POST: PRODUCTS/Delete/5

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Product = await productRepository.GetProductByIdAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", Product.image_name);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            await productRepository.DeleteProductAsync(Product);
            return RedirectToAction(nameof(Index));
        }

        private bool PRODUCTSExists(int id)
        {
            return productRepository.ProductExists(id);
        }
    }
}
