using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Switch_and_Shift.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger,IProductRepository productRepository , IUserRepository userRepository )
        {
            _context = context;
            _logger = logger;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {   
            HttpContext.Session.Clear();
            return View(await productRepository.GetAllProductsAsync());
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

      

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Welcome()
        {

            int currentUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            if (currentUserId == 0)
            {
                return RedirectToAction("Index", "HOME");
            }
            var productQuery = from x in _context.Products select x;
            var loc = _context.Users.FirstOrDefault(m => m.UserId == currentUserId).District.ToString();
            productQuery = productQuery.Where(x => x.user_location == loc && x.UserId != currentUserId);
            productQuery = productQuery.Where(x => x.UserId != currentUserId &&
                _context.Users.FirstOrDefault(m => m.UserId == x.UserId).report_show == 1);
            ViewBag.userLocationTitle = loc;
            return View(await productQuery.AsNoTracking().ToListAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
